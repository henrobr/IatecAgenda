import { NewComponent } from './login/new/new.component';
import { EventosCompartilhadosComponent } from './eventos/eventosCompartilhados/eventosCompartilhados.component';
import { NgModule } from '@angular/core';
import { HttpClientModule } from "@angular/common/http"
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AgendaComponent } from './agenda/agenda.component';
import { PerfilComponent } from './perfil/perfil.component';
import { NavComponent } from './nav/nav.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { LoginComponent } from './login/login.component';
import { ToastService, AngularToastifyModule } from 'angular-toastify';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EventosComponent } from './eventos/eventos.component';
import { PrincipalComponent } from './compartilhado/principal/principal.component';
import { ViewComponent } from "./eventos/view/view.component";

@NgModule({
   declarations: [
      AppComponent,
      AgendaComponent,
      PerfilComponent,
      NavComponent,
      LoginComponent,
      DashboardComponent,
      EventosComponent,
      PrincipalComponent,
      ViewComponent,
      EventosCompartilhadosComponent,
      NewComponent
   ],
   providers: [
      ToastService
   ],
   bootstrap: [
      AppComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      BsDropdownModule.forRoot(),
      BrowserAnimationsModule,
      FormsModule,
      ReactiveFormsModule,
      ModalModule.forRoot(),
      HttpClientModule,
      FontAwesomeModule,
      AngularToastifyModule
   ]
})
export class AppModule { }
