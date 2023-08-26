import { ChatService } from './../services/chat.service';
import { Component, EventEmitter, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  @Output() closeChatEmmitter= new EventEmitter();

  constructor(public chatService:ChatService){}

  ngOnInit():void{

  }
  navigateToHome(){
    this.closeChatEmmitter.emit();

  }
}
