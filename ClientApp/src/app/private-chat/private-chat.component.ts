import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-private-chat',
  templateUrl: './private-chat.component.html',
  styleUrls: ['./private-chat.component.css']
})
export class PrivateChatComponent implements OnInit, OnDestroy {
@Input() toUser = '';

  constructor(public activeModal:NgbActiveModal, public chatService:ChatService){
      }
  ngOnInit(): void {
this.loadPrivateChatMessages();
  }
  ngOnDestroy(): void {
    this.chatService.closePrivateChatMessage(this.toUser)
  }

  sendMessage(content:string){
    this.chatService.sendPrivateMessage(this.toUser,content)
  }

  loadPrivateChatMessages(): void {
    const chatId = this.getPrivateGroupName(this.chatService.name,this.toUser); // Replace with your chat identifier
    this.chatService.getChatMessages(chatId).subscribe(
      (messages) => {
        // Assuming messages is an array, concatenate the arrays.
        this.chatService.privateMessages = [...messages, ...this.chatService.privateMessages];
      },
      (error) => {
        console.error('Error fetching chat messages:', error);
        // Handle the error, show a user-friendly message, or take other appropriate actions.
      }
    );
  }


  getPrivateGroupName(from: string, to: string): string {
    const stringCompare = from.localeCompare(to) < 0;
    return stringCompare ? `${from}-${to}` : `${to}-${from}`;
  }
}
