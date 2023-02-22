import { ToastService } from 'angular-toastify';
import { PerfilService } from './perfil.service';
import { FormGroup, FormControl } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FuncoesService } from './../funcoes.service';
import { faArrowLeft, faCancel, faEdit, faLock, faSave } from '@fortawesome/free-solid-svg-icons';
import { Perfil } from './../models/Perfil';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AppComponent } from './../app.component';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {

  constructor(private usuario: FuncoesService, private modalService: BsModalService, private perfilService: PerfilService, private toast: ToastService) { }

  ngOnInit() {
  }
  //forms
  dadosPerfil: Perfil = { nome: this.usuario.DecriptarToken(localStorage.getItem('token')!, "nome"), username: this.usuario.DecriptarToken(localStorage.getItem('token')!, "name") }
  perfilForm = new FormGroup({ nome: new FormControl<string>(this.dadosPerfil.nome), username: new FormControl<string>(this.dadosPerfil.username) })
  perfilSenha = new FormGroup({ passwordBefore: new FormControl<string>(""), password: new FormControl<string>(""), passwordConfirm: new FormControl<string>("") })

  //
  erros?: []
  erro: boolean = false

  //icons
  faArrowLeft = faArrowLeft
  faEdit = faEdit
  faPass = faLock
  faCancel = faCancel
  faSave = faSave

  modalRef?: BsModalRef

  configModal = {
    backdrop: true,
    ignoreBackdropClick: true
  }

  openModal(template: TemplateRef<any>) {
    //this.modalRef = this.modalService.show(template, Object.assign(this.configModal, { class: 'modal-dialog-centered' }))
    this.erro = false;
    this.erros = [];
    this.modalRef = this.modalService.show(template, this.configModal)
  }
  public closeModal() {
    this.modalRef?.hide()
  }

  handleSubmit() {
    this.erro = false;
    this.perfilService.alterarNome(this.perfilForm.value).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.toast.success(res.message)
          localStorage.setItem("token", res.data.token)
          this.dadosPerfil = { nome: this.usuario.DecriptarToken(localStorage.getItem('token')!, "nome"), username: this.usuario.DecriptarToken(localStorage.getItem('token')!, "name") }
          this.modalRef?.hide()
        }
        else {
          this.erros = res.errors
          this.erro = true
        }
      },
      error: (err) => {
        this.erros?.push.apply([err?.message])
        this.erro = true
      }
    })
    console.log(this.perfilForm.value)
  }

  handleSenha() {
    this.erro = false;
    this.perfilService.alterarSenha(this.perfilSenha.value).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.toast.success(res.message)
          localStorage.setItem("token", res.data.token)
          this.dadosPerfil = { nome: this.usuario.DecriptarToken(localStorage.getItem('token')!, "nome"), username: this.usuario.DecriptarToken(localStorage.getItem('token')!, "name") }
          this.modalRef?.hide()
        }
        else {
          this.erros = res.errors
          this.erro = true
        }
      },
      error: (err) => {
        this.erros?.push.apply([err?.message])
        this.erro = true
      }
    })
    console.log(this.perfilForm.value)
  }

}
