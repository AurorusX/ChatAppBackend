import { ChatService } from './../services/chat.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

      userForm:FormGroup =new FormGroup({});

    //state of submission
      submitted=false;
      ApiErrors:string[]=[];
      OpenChat=false;

      constructor(private formbuilder :FormBuilder,private chatService:ChatService){}

      ngOnInit():void{
        this.initializeForm();

      }
      initializeForm(){
        this.userForm=this.formbuilder.group({

          name: ['',[Validators.required,Validators.minLength(3),Validators.maxLength(15)]]
        })

      }

        submitForm(){
          this.submitted=true;
          this.ApiErrors=[];

            if (this.userForm.valid){
              this.chatService.registerUser(this.userForm.value).subscribe({
                next:()=>{
                  this.chatService.name=this.userForm.get('name')?.value;
                  this.OpenChat=true;
                  this.userForm.reset();
                  this.submitted=false;
                },
                error: error =>{
                  if(typeof(error.error)!=='object'){
                    this.ApiErrors.push(error.error);
                  }
                }

                })


            }
        }

        closeChat(){
          this.OpenChat=false;
          this.chatService.stopConnection();
        }

      }







