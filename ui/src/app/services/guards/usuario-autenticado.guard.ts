import { UsuarioService } from './../../Usuario.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsuarioAutenticadoGuard implements CanActivate {

  constructor(private route: Router, private usuarioService: UsuarioService) { }
  canActivate() {
    if (this.usuarioService.logado) return true

    this.route.navigate(['login'])

    return false
  }
}