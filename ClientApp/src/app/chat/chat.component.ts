import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChatService } from './../services/chat.service';
import { Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';
import { PrivateChatComponent } from '../private-chat/private-chat.component';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  @Output() closeChatEmmitter= new EventEmitter();

  constructor(public chatService:ChatService,private modalService:NgbModal){}
  ngOnDestroy(): void {
    this.chatService.stopConnection();
  }

  ngOnInit():void{
    this.chatService.createConnection();
    this.loadChatMessages();

  }

  navigateToHome(){
    this.closeChatEmmitter.emit();

  }

  sendMessage(content:string){
    this.chatService.sendMessage(content);


  }

  openPrivateChat(toUser:string){
    const modalRef=this.modalService.open(PrivateChatComponent);
    modalRef.componentInstance.toUser=toUser;
  }
//Revisit to make sure
  loadChatMessages(): void {
    const chatId = 'AscendantChat';
    this.chatService.getChatMessages(chatId).subscribe(
      (messages) => {
        // Assuming messages is an array, concatenate the arrays.
        this.chatService.messages = [...messages, ...this.chatService.messages];
      },
      (error) => {
        console.error('Error fetching chat messages:', error);
        // Handle the error
      }
    );
  }


}
