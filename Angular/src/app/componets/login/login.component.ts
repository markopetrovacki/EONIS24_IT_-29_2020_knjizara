import { Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../../helpers/validateform';
import { AuthService } from '../../services/auth.service';
import { NgToastService } from 'ng-angular-popup';
import { UserStoreService } from '../../services/user-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent  implements OnInit{

  loginForm!: FormGroup;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  constructor(
    private fb: FormBuilder, 
    private auth: AuthService, 
    private router : Router,
    private toast: NgToastService,
    private userStore: UserStoreService
  ) {}

  ngOnInit(): void{
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onLogin(){
    if(this.loginForm.valid){

      //console.log(this.loginForm.value)
      //console.log(this.auth.login(this.loginForm.value))
      // Send the obj to database
     
      this.auth.login(this.loginForm.value)
      .subscribe({
        next:(res)=>{
          console.log(res); // Provera strukture objekta res
         alert(res.message);
        
          this.loginForm.reset();
          this.auth.storeToken(res.token);
          const tokenPayload = this.auth.decodedToken();
          this.userStore.setFullNameForStore(tokenPayload.name);
          this.userStore.setRoleFromStore(tokenPayload.role);
          //this.toast.success({ detail: "SUCCESS", summary: res.message, duration: 5000 });
          this.router.navigate(['products']);
          console.log(this.loginForm.value)
        },
        error:(err)=>{
          alert("Something when wrong!");
        // this.toast.error({ detail: "ERROR", summary: "Something when wrong!", duration: 5000});
          alert(err?.error.message)
        }
      })
     
    }else{

      //console.log("Form is not valid")
      //throw the error using toaster and with required fields
      ValidateForm.validateAllFormFileds(this.loginForm);
      alert("Your form is invalid")
    }
  }

}
