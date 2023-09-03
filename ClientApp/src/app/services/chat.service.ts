import { Message } from './../models/message';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PrivateChatComponent } from '../private-chat/private-chat.component';
import { Observable, timestamp } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  name:string='';
  private chatConnection?:HubConnection;
  usersOnline:string[]=[]
  messages:Message[]=[];
  privateMessages:Message[]=[];
  privateMessageInitiated=false;

  constructor(private httpClient : HttpClient ,private modalService:NgbModal) { }

  registerUser(user: User){
    return this.httpClient.post(`${environment.apiUrl}api/chat/register-user`,user,{responseType:'text'});
  }

  createConnection(){
    this.chatConnection=new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}hubs/chat`).withAutomaticReconnect().build();

      this.chatConnection.start().catch(error=>{
        console.log(error);
      });

      this.chatConnection.on('UserConnected',()=>{
        this.addUserConnectionId()
      });

      this.chatConnection.on('usersOnline',(usersOnline)=>{
        this.addUserConnectionId()
        this.usersOnline = [...usersOnline];
      });

      this.chatConnection.on('NewMessage',(newMessage:Message)=>{
        this.messages = [...this.messages,newMessage];

      });
      this.chatConnection.on('OpenPrivateChat',(newMessage:Message)=>{
        this.privateMessages = [...this.privateMessages,newMessage];
        this. privateMessageInitiated=true;
        const modalRef=this.modalService.open(PrivateChatComponent);
        modalRef.componentInstance.toUser=newMessage.from;

      });



      this.chatConnection.on('NewPrivateMessage',(newMessage:Message)=>{
        this.privateMessages = [...this.privateMessages,newMessage];

      });

      this.chatConnection.on('ClosePrivateChat',()=>{
       this.privateMessageInitiated=false;
       //revisit to test chat persisting chat
       this.privateMessages=[];
       this.modalService.dismissAll();

      });



  }

  stopConnection(){
    this.chatConnection?.stop().catch(error=>console.log(error));
  }

  async addUserConnectionId(){
    return this.chatConnection?.invoke("AddUserConnectionId",this.name)
      .catch(error=>console.log(error))

  }

  async sendMessage(content:string){
      const message:Message={
        from:this.name,
        content,

      };

      return this.chatConnection?.invoke('ReceiveMessage',message)
      .catch(err=>console.log(err));
  }

  async sendPrivateMessage(to:string,content:string){
      const message:Message={
        from:this.name,
        to,
        content,
      };

      if(!this.privateMessageInitiated){
        this.privateMessageInitiated=true;
        return this.chatConnection?.invoke('CreatePrivateChat',message).then(()=>{
          this.privateMessages=[...this.privateMessages,message]
        })
        .catch(err=>console.log(err));
    }else{
      return this.chatConnection?.invoke('ReceivePrivateMessage',message)
      .catch(error=>console.log(error))

    }

  }

  async closePrivateChatMessage(otherUser:string){
    return this.chatConnection?.invoke("RemovePrivateChat",this.name,otherUser)
      .catch(error=>console.log(error))

  }



  getChatMessages(chatId: string): Observable<Message[]> {
    return this.httpClient.get<Message[]>(`${environment.apiUrl}api/chat/get-chat-messages?chatId=${chatId}`);
  }

}

