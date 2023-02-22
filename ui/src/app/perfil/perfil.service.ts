import { Perfil } from './../models/Perfil';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { Results } from './../models/Results';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PerfilService {
  token = localStorage.getItem("token")
  headers = new HttpHeaders({ 'Authorization': `Bearer ${this.token}` })

  constructor(private http: HttpClient) { }

  alterarNome(data: any): Observable<Results> {
    return this.http.post<Results>(`${environment.urlPrincipal}/Usuarios/Update/Nome`, data, { headers: this.headers })
  }

  alterarSenha(data: any): Observable<Results> {
    return this.http.post<Results>(`${environment.urlPrincipal}/Usuarios/Update/Senha`, data, { headers: this.headers })
  }

}
