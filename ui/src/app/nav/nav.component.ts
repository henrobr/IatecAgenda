import { UsuarioService } from './../Usuario.service';
import { Component, OnInit } from '@angular/core';
import { faCalendarPlus, faHome, faSignOutAlt, faUserEdit } from '@fortawesome/free-solid-svg-icons'

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private usuarioService: UsuarioService) { }

  ngOnInit() {
  }


  //icons
  faHome = faHome
  faOut = faSignOutAlt
  faUserEdit = faUserEdit
  faNew = faCalendarPlus
  sair() {
    this.usuarioService.logout()
  }

}
