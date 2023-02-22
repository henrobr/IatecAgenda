import { Usuario } from './../models/Usuario';
import { Login } from './../models/Login';
import { Results } from './../models/Results';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  postLogin(data: Login): Observable<Results> {
    return this.http.post<Results>(`${environment.urlPrincipal}/Login`, data)
  }

  postNovoUsuario(data: Usuario): Observable<Results> {
    return this.http.post<Results>(`${environment.urlPrincipal}/Login/Novo/Usuario`, data)
  }

}
