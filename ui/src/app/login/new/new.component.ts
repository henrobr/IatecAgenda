import { Usuario } from './../../models/Usuario';
import { ToastService } from 'angular-toastify';
import { Router } from '@angular/router';
import { LoginService } from './../login.service';
import { UsuarioService } from './../../Usuario.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-new',
  templateUrl: './new.component.html',
  styleUrls: ['./new.component.css']
})
export class NewComponent implements OnInit {

  constructor(private usuarioService: LoginService, private route: Router, private toast: ToastService) { }

  ngOnInit() {
  }

  //Form
  usuarioForm = new FormGroup<Usuario | any>({ nome: new FormControl<string>(""), username: new FormControl<string>(""), password: new FormControl<string>(""), passwordConfirm: new FormControl<string>("") });

  erro: boolean = false
  erros?: []

  handleNew() {
    this.erro = false
    this.usuarioService.postNovoUsuario(this.usuarioForm.value).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.toast.success(res.message)
          localStorage.clear();
          localStorage.setItem("token", res.data.token)
          window.location.href = "/"
        }
        else {
          this.erro = true
          this.erros = res.errors
        }
      },
      error: (err) => {

        console.log(err.message)
        this.erro = true
        this.erros!.push.apply([err!.message])
        console.log(this.erros)
      }
    })
  }

}
