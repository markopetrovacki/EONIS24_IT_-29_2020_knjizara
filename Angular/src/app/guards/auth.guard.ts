import { Injectable } from '@angular/core';
import { CanActivateFn, CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { NgToastService } from 'ng-angular-popup';

@Injectable({
  providedIn: 'root'
})
/*
export const authGuard: CanActivateFn = (route, state) => {
  return true;

};*/

export class AuthGuard implements CanActivate {
  
  constructor(private auth : AuthService, private router: Router){

  }
  canActivate():boolean{
    if(this.auth.isLoggedIn()){
      return true;
    }else{
      
      //this.toast.error({detail: "ERROR", summary:"Please login dirst!"});
      this.router.navigate(['login'])
      return false; 
    }
  }
  

};
