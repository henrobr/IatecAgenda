import { UsuarioService } from './../../Usuario.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsuarioNaoAutenticadoGuard implements CanActivate {
  constructor(private usuarioService: UsuarioService, private route: Router) { }
  canActivate() {
    if (this.usuarioService.logado) {
      this.route.navigate(['']);
      return false;
    }
    return true;
  }

}
