import { ChatService } from './../services/chat.service';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

      userForm:FormGroup =new FormGroup({});

    //state of submission
      submitted=false;
      ApiErrors:string[]=[];

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
                  console.log('Open Chat')

                },
                error: error =>{
                  if(typeof(error.error)!=='object'){
                    this.ApiErrors.push(error.error);
                  }
                }

                })


            }
        }

      }







