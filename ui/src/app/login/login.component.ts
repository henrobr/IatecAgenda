import { ToastService } from 'angular-toastify';
import { Login } from './../models/Login';
import { FormGroup, FormControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private loginService: LoginService, private toast: ToastService, private route: Router) { }

  ngOnInit() {
  }

  loginForm = new FormGroup({ username: new FormControl(), password: new FormControl })

  erro: boolean = false
  erros?: []
  sending: boolean = false

  handleLogin(): void {
    console.log(this.loginForm?.value)
    this.erro = false

    this.loginService.postLogin(this.loginForm?.value).subscribe({
      next: (res) => {
        if (res.status) {
          this.toast.success(res.message)
          localStorage.setItem("token", res.data.token)
          window.location.href = "/"
        }
        else {
          this.erro = true
          this.erros = res.errors
        }
      },
      error: (err) => this.toast.error("Falha na comunicação com o servidor de Dados")
    })
  }
}
