import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Korisnik } from '../models/korisnik';
import { Observable } from 'rxjs';
import { Dobavljac } from '../models/dobavljac';
import { Dostava } from '../models/dostava';
import { Knjiga } from '../models/knjiga';
import { Porudzbina } from '../models/porudzbina';
import { PorudzbinaKnjiga } from '../models/porudzbinaKnjiga';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl: string = 'http://localhost:5297/api/knjizara/';
  constructor(private http: HttpClient) {}

  getUsers() {
    return this.http.get<any>(`${this.baseUrl}korisnik`);
  }
  /*getUserByUsername(username: string){
    return this.http.get<any>(`${this.baseUrl}korisnik/user/`, { params: { username } });
  }*/
  getUserByUsername(username: string) {
    return this.http.get<any>(`${this.baseUrl}korisnik/user`, { params: { username } });
  }
  createUser(korisnik: Korisnik): Observable<Korisnik> {
    return this.http.post<Korisnik>(`${this.baseUrl}korisnik`, korisnik);
  }
  deleteUser(id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}korisnik/${id.toUpperCase()}`);
  }
  updateUser(korisnik: Korisnik): Observable<Korisnik> {
    return this.http.put<Korisnik>(`${this.baseUrl}korisnik`, korisnik);
  }


  getDobavljac() {
    return this.http.get<any>(`${this.baseUrl}dobavljac`);
  }
  createDobavljac(dobavljac: Dobavljac): Observable<Dobavljac> {
    return this.http.post<Dobavljac>(`${this.baseUrl}dobavljac`, dobavljac);
  }
  deleteDobavljac(id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}dobavljac/${id.toUpperCase()}`);
  }
  updateDobavljac(dobavljac: Dobavljac): Observable<Dobavljac> {
    return this.http.put<Dobavljac>(`${this.baseUrl}dobavljac`, dobavljac);
  }


  getDosatava() {
    return this.http.get<any>(`${this.baseUrl}dostava`);
  }
  createDostava(dostava: Dostava): Observable<Dostava> {
    return this.http.post<Dostava>(`${this.baseUrl}dostava`, dostava);
  }
  deleteDostava(id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}dostava/${id.toUpperCase()}`);
  }
  updateDostava(dostava: Dostava): Observable<Dostava> {
    return this.http.put<Dostava>(`${this.baseUrl}dostava`, dostava);
  }


  getKnjiga() {
    return this.http.get<any>(`${this.baseUrl}knjiga`);
  }
  getKnjigaById(id: string) {
    return this.http.get<any>(`${this.baseUrl}knjiga/${id}`);
  }
  createKnjiga(knjiga: Knjiga): Observable<Knjiga> {
    return this.http.post<Knjiga>(`${this.baseUrl}knjiga`, knjiga);
  }
  deleteKnjiga(id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}knjiga/${id}`);
  }
  updateKnjiga(knjiga: Knjiga): Observable<Knjiga> {
    return this.http.put<Knjiga>(`${this.baseUrl}knjiga`, knjiga);
  }


  
  getPorudzbina() {
    return this.http.get<any>(`${this.baseUrl}porudzbina`);
  }
  getPorudzbinaByIdKorisnik(id: string) {
    return this.http.get<any>(`${this.baseUrl}porudzbina/korisnik/${id.toUpperCase()}`);
  }
  createPorudzbina(porudzbina: Porudzbina): Observable<Porudzbina> {
    return this.http.post<Porudzbina>(`${this.baseUrl}porudzbina`, porudzbina);
  }
  deletePorudzbina(id: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}porudzbina/${id}`);
  }
  updatePorudzbina(porudzbina: Porudzbina): Observable<Porudzbina> {
    return this.http.put<Porudzbina>(`${this.baseUrl}porudzbina`, porudzbina);
  }


  getPorudzbinaKnjiga() {
    return this.http.get<any>(`${this.baseUrl}porudzbinaKnjiga`);
  }
  createPorudzbinaKnjiga(porudzbinaKnjiga: PorudzbinaKnjiga): Observable<PorudzbinaKnjiga> {
    return this.http.post<PorudzbinaKnjiga>(`${this.baseUrl}porudzbinaKnjiga`, porudzbinaKnjiga);
  }
  deletePorudzbinaKnjiga(id_knjige: string, id_porudzbina: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}porudzbinaKnjiga/${id_knjige}, ${id_porudzbina}`);
  }
  getKnjigeByPorudzbinaId(id_porudzbina: string): Observable<any[]>  {
    return this.http.get<any>(`${this.baseUrl}porudzbinaKnjiga/porudzbina/${id_porudzbina.toUpperCase()}`);
  }
  deletePorudzbinaKnjigaByPorudzbinaId(id_porudzbina: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}porudzbinaKnjiga/porudzbina/${id_porudzbina}`);
  }
  

}