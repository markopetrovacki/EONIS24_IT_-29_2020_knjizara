import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApiService } from './api.service';
import { formatDate } from '@angular/common';
import { Porudzbina } from '../models/porudzbina';
import { HttpClient } from '@angular/common/http';
import { ISession } from '../models/session';

declare const Stripe: any;

@Injectable({
  providedIn: 'root'
})
export class CartService {
  
  public cartItemList : any =[]
  public productList = new BehaviorSubject<any>([]);
  public search = new BehaviorSubject<string>("");

  baseUrl = "http://localhost:5297/api/";

  constructor(private apiService: ApiService, private http: HttpClient) { }
 
  getProducts(){
    return this.productList.asObservable();
  }

  setProduct(product : any){
    this.cartItemList.push(...product);
    this.productList.next(product);
  }
 /*addtoCart(product : any){
    this.cartItemList.push(product);
    this.productList.next(this.cartItemList);
    this.getTotalPrice();
    console.log(this.cartItemList)
  }*/
  getTotalPrice() : number{
    let grandTotal = 0;
    this.cartItemList.map((a:any)=>{
      grandTotal += a.total;
    })
    return grandTotal;
  }
  removeCartItem(product: any){
    this.cartItemList.map((a:any, index:any)=>{
      if(product.id=== a.id){
        this.cartItemList.splice(index,1);
      }
    })
    this.productList.next(this.cartItemList);
  }
  removeAllCart(){
    this.cartItemList = []
    this.productList.next(this.cartItemList);
  }

  addtoCart(product: any) {
    // Provera da li proizvod već postoji u korpi
    const itemIndex = this.cartItemList.findIndex((item: any) => item.id_knjige === product.id_knjige);
    if (itemIndex !== -1) {
      // Ako postoji, povećaj količinu
      this.cartItemList[itemIndex].kolicina++;
      this.cartItemList[itemIndex].total = this.cartItemList[itemIndex].kolicina * this.cartItemList[itemIndex].cena;
    } else {
      // Ako ne postoji, dodaj novi
      product.kolicina = 1;
      product.total = product.cena;
      this.cartItemList.push(product);
    }
    this.productList.next(this.cartItemList);
    this.getTotalPrice();
  }

  updateQuantity(product: any, quantity: number) {
    const itemIndex = this.cartItemList.findIndex((item: any) => item.id_knjige === product.id_knjige);
    if (itemIndex !== -1 && quantity > 0) {
      this.cartItemList[itemIndex].kolicina = quantity;
      this.cartItemList[itemIndex].total = this.cartItemList[itemIndex].kolicina * this.cartItemList[itemIndex].cena;
      this.productList.next(this.cartItemList);
    }
  }

/*
  createOrder(porudzbina: Porudzbina) {


    // Pozivanje API servisa za kreiranje porudžbine
    this.apiService.createPorudzbina(porudzbina).subscribe(
      (response) => {
        console.log('Porudžbina kreirana:', response);
        this.removeAllCart(); // Isprazni korpu nakon kreiranja porudžbine
      },
      (error) => {
        console.error('Greška pri kreiranju porudžbine:', error);
      }
    );
  }*/

  createOrder(porudzbina: any) {
    // Prvo kreiramo glavnu porudžbinu
    this.apiService.createPorudzbina(porudzbina).subscribe(
      (response) => {
        console.log('Porudžbina kreirana:', response);
        const orderId = response.id_porudzbina; // Pretpostavljamo da API vraća ID kreirane porudžbine
        
        if(orderId){
          // Kreiramo zapise za svaku knjigu u porudžbini
          for (let item of this.cartItemList) {
            const porudzbinaKnjiga = {
              id_porudzbina: orderId,
              id_knjige: item.id_knjige,
              kolicina: item.kolicina
            };
            this.apiService.createPorudzbinaKnjiga(porudzbinaKnjiga).subscribe(
              (res) => {
                console.log('Porudžbina knjiga kreirana:', res);
                this.requestSession(orderId);
              },
              (err) => {
                console.error('Greška pri kreiranju porudžbine knjige:', err);
              }
            );
          }
        }
        this.removeAllCart(); // Isprazni korpu nakon kreiranja porudžbine
      },
      (error) => {
        console.error('Greška pri kreiranju porudžbine:', error);
      }
    );
  }
  
  // Metoda za dobijanje ukupne količine knjiga u korpi
  getCartQuantity(id_knjige: any): number {
    let cartItem = this.cartItemList.find((item: any) => item.id_knjige === id_knjige);
    return cartItem ? cartItem.kolicina : 0;
  }
  


  requestSession(id_porudzbina:string){
    this.http.post<ISession>(this.baseUrl + 'create-checkout-session/' + id_porudzbina,{}).subscribe((session)=>{
      this.redirectToCheckout(session.sessionId);
    });
  }

  redirectToCheckout(sessionId: string) {
    const stripe = Stripe('pk_test_51LUQwuG8zN8FyI0EeQYyTW8AtMHauyRYbg7l5DGZ2VyP1p2PoGqTbBIpIRS1Menv6VzVzXwiiO0gpETr4p6DIrOx00kfS7Zwc4');
    stripe.redirectToCheckout({
      sessionId: sessionId
    });
  }
}
