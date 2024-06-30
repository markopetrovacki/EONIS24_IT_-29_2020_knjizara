import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../../helpers/validateform';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent implements OnInit{

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm!: FormGroup;
  constructor(private fb : FormBuilder, private auth : AuthService, private router: Router){}

  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      ime_korisnika: ['',Validators.required],
      prezime_korisnika: ['',Validators.required],
      adresa_korisnika: ['',Validators.required],
      grad_korisnika:['',Validators.required],
      kontakt_telefon:['',Validators.required],
      status_korisnika:['User',Validators.required],
      username: ['',Validators.required],
      password: ['',Validators.required]
    })
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = "fa-eye") : (this.eyeIcon = "fa-eye-slash");
    this.isText ? (this.type = "text") : (this.type = "password");
  }

  onSignup(){
    if(this.signUpForm.valid){
      //perform login for signup
      this.auth.signUp(this.signUpForm.value)
      .subscribe({
        next:(res=>{
          console.log(res); // Provera strukture objekta res
          alert(res.message)
          this.signUpForm.reset();
          this.router.navigate(['login']);
        }),
        error:(err=>{
          alert("Something when wrong!");
          alert(err?.error.message)
        })
      })

      console.log(this.signUpForm.value)
    }else{
      ValidateForm.validateAllFormFileds(this.signUpForm)
      //logic for thowing error
      alert("Your form is invalid")

    }
  }

}
