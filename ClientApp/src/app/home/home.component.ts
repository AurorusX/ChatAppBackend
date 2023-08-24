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

  constructor(private formbuilder :FormBuilder){}

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

      if (this.userForm.valid){
        console.log(this.userForm.value);
      }
    }



  }




