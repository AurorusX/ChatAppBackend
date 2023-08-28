import { Message } from './../models/message';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  name:string='';
  private chatConnection?:HubConnection;
  usersOnline:string[]=[]
  messages:Message[]=[];

  constructor(private httpClient : HttpClient) { }

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
      })
      this.chatConnection.on('usersOnline',(usersOnline)=>{
        this.addUserConnectionId()
        this.usersOnline = [...usersOnline];
      })

      this.chatConnection.on('NewMessage',(newMessage:Message)=>{
        this.messages = [...this.messages,newMessage];

      })

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
        content
      };

      return this.chatConnection?.invoke('ReceiveMessage',message)
      .catch(err=>console.log(err));
  }

}

