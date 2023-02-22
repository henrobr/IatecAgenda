import { Results } from './../models/Results';
import { Data } from "./../models/Data";
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Agenda } from '../models/Agenda';

@Injectable({
  providedIn: 'root'
})
export class AgendaService {
  baseUrl = `${environment.urlPrincipal}`
  token = localStorage.getItem("token")
  headers = new HttpHeaders({ 'Authorization': `Bearer ${this.token}` })


  constructor(private http: HttpClient) { }

  get(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Agendas/View/${id}`, { headers: this.headers })
  }

  getProximo(dias: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Agendas/List/Proximos/${dias}`, { headers: this.headers })
  }

  getAll(dias: number): Observable<Results> {
    return this.http.get<Results>(`${this.baseUrl}/Dashboard/${dias}`, { headers: this.headers })
  }
  create(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Agendas/New`, data, { headers: this.headers });
  }
  update(data: any, id: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/Agendas/Update/${id}`, data, { headers: this.headers });
  }
  delete(data: any, id: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/Agendas/Delete/${id}`, data, { headers: this.headers });
  }
  getSearch(data: any): Observable<Results> {
    return this.http.get<Results>(`${this.baseUrl}/Agendas/List/Eventos?search=${data.search}&Calendario=${data.calendario}&DtEvento=${data.dtEvento}`, { headers: this.headers })
  }
  getAllCompartilhados(): Observable<Results> {
    return this.http.get<Results>(`${this.baseUrl}/Agendas/List/Eventos/Compartilhados`, { headers: this.headers })
  }

  importarEvento(data: any, id: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/Agendas/Importar/${id}`, data, { headers: this.headers });
  }

}
