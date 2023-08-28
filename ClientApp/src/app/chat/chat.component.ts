import { ChatService } from './../services/chat.service';
import { Component, EventEmitter, OnDestroy, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  @Output() closeChatEmmitter= new EventEmitter();

  constructor(public chatService:ChatService){}
  ngOnDestroy(): void {
    this.chatService.stopConnection();
  }

  ngOnInit():void{
    this.chatService.createConnection();

  }

  navigateToHome(){
    this.closeChatEmmitter.emit();

  }
}
