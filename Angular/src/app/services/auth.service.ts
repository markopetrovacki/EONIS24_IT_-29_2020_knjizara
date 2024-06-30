import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: string = "http://localhost:5297/api/"
  private userPeyload:any;

  constructor(private http : HttpClient, private router: Router, @Inject(PLATFORM_ID) private platformId: Object) {
    this.userPeyload = this.decodedToken();
   }
/*
  signUp(userObj: any){
    return this.http.post<any>(`${this.baseUrl}knjizara/korisnik`,userObj)
  }

  login(loginObj: any): Observable<any>  {
    return this.http.post<any>(`${this.baseUrl}Auth/login`,loginObj)
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['login'])
  }

  storeToken(tokenValue: string){
    localStorage.setItem('token', tokenValue)
  }

  getToken(){
    return localStorage.getItem('token')
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('token')
  }

  decodedToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log(jwtHelper.decodeToken(token));
    return jwtHelper.decodeToken(token)
  }

  getFullNameFromToken(){
    if(this.userPeyload)
      return this.userPeyload.name;
  }

  getRoleFromToken(){
    if(this.userPeyload)
      return this.userPeyload.role;
  }
*/
signUp(userObj: any) {
  return this.http.post<any>(`${this.baseUrl}knjizara/korisnik`, userObj);
}

login(loginObj: any): Observable<any> {
  return this.http.post<any>(`${this.baseUrl}Auth/login`, loginObj);
}

signOut() {
  if (isPlatformBrowser(this.platformId)) {
    localStorage.clear();
  }
  this.router.navigate(['login']);
}

storeToken(tokenValue: string) {
  if (isPlatformBrowser(this.platformId)) {
    localStorage.setItem('token', tokenValue);
  }
}

getToken() {
  if (isPlatformBrowser(this.platformId)) {
    return localStorage.getItem('token');
  }
  return null;
}

isLoggedIn(): boolean {
  if (isPlatformBrowser(this.platformId)) {
    return !!localStorage.getItem('token');
  }
  return false;
}

decodedToken() {
  if (isPlatformBrowser(this.platformId)) {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken();
    if (token) {
      console.log(jwtHelper.decodeToken(token));
      return jwtHelper.decodeToken(token);
    }
  }
  return null;
}

getFullNameFromToken() {
  if (this.userPeyload) {
    return this.userPeyload.name;
  }
  return null;
}

getRoleFromToken() {
  if (this.userPeyload) {
    return this.userPeyload.role;
  }
  return null;
}

  /*getUserData(token: string) {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any>(`${this.baseUrl}knjizara/korisnik`, { headers });
  }*/
}
