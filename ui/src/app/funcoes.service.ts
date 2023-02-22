import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FuncoesService {

  constructor() { }

  DecriptarToken(token: string, tipo: any) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload)[tipo]
  }

  ToDateBr(v: string) {
    return new Date(v.length > 10 ? v : v + "T00:00:01").toLocaleDateString("pt-BR", { timeZone: 'America/Sao_Paulo' })
  }

}
