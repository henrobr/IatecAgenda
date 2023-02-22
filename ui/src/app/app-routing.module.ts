import { NewComponent } from './login/new/new.component';
import { EventosCompartilhadosComponent } from './eventos/eventosCompartilhados/eventosCompartilhados.component';
import { PrincipalComponent } from './compartilhado/principal/principal.component';
import { UsuarioNaoAutenticadoGuard } from './services/guards/usuario-nao-autenticado.guard';
import { UsuarioAutenticadoGuard } from './services/guards/usuario-autenticado.guard';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PerfilComponent } from './perfil/perfil.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: "login", component: LoginComponent, canActivate: [UsuarioNaoAutenticadoGuard] },
  { path: "login/novo/usuario", component: NewComponent, canActivate: [UsuarioNaoAutenticadoGuard] },
  {
    path: "", component: PrincipalComponent,
    children: [
      { path: "", component: DashboardComponent, canActivate: [UsuarioAutenticadoGuard] },
      { path: "eventos/compartilhados", component: EventosCompartilhadosComponent, canActivate: [UsuarioAutenticadoGuard] },
      { path: "perfil", component: PerfilComponent, canActivate: [UsuarioAutenticadoGuard] },
    ],
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
