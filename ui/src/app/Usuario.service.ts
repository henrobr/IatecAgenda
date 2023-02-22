import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { FuncoesService } from './funcoes.service';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  constructor(private route: Router, private decript: FuncoesService) { }

  logout() {
    localStorage.clear();
    this.route.navigate(['login']);
  }
  get usuarioLogado(): any {
    return this.decript.DecriptarToken(localStorage.getItem('token')!, "nome")
  }
  get obterTokenUsuario(): any {
    return localStorage.getItem('token')
  }
  get logado(): boolean {
    return localStorage.getItem('token') ? true : false;
  }

}
