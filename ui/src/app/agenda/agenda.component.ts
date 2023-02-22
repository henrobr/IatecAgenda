import { Router } from '@angular/router';
import { ToastService } from 'angular-toastify';
import { Observable } from 'rxjs';
import { AgendaService } from './agenda.service';
import { Agenda } from './../models/Agenda';
import { Data } from "./../models/Data";
import { AgendaHoje } from "./../models/AgendaHoje";
import { AgendaProximo } from "./../models/AgendaProximo";
import { AgendaPut } from "./../models/AgendaPut";
import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, FormArray } from '@angular/forms'
import { faArrowLeft, faCancel, faChevronLeft, faEdit, faHome, faSave, faSignOutAlt, faTrash, faUserEdit } from '@fortawesome/free-solid-svg-icons';
import { UsuarioService } from '../Usuario.service';
import { FuncoesService } from '../funcoes.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-agenda',
  templateUrl: './agenda.component.html',
  styleUrls: ['./agenda.component.css']
})
export class AgendaComponent implements OnInit {

  constructor(private agendaService: AgendaService, private toast: ToastService, private usuarioService: UsuarioService, private route: Router, private funcao: FuncoesService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadAgendas()
  }

  //Forms
  agendaComp = new FormGroup({ idAgenda: new FormControl<number | null>(null), idUsuario: new FormControl<string | null>(null) })
  agendaForm = new FormGroup({ id: new FormControl(), nome: new FormControl(), data: new FormControl(), hora: new FormControl(), descricao: new FormControl(), local: new FormControl(), participantes: new FormControl(), particular: new FormControl(false), compartilhados: new FormArray<any>([]) })
  agendaSlt = new FormGroup({ idDias: new FormControl(7) })

  //icons
  iconLink = faHome
  faEdit = faEdit
  faTrash = faTrash
  faArrowLeft = faChevronLeft
  faSave = faSave
  faOut = faSignOutAlt
  faUserEdit = faUserEdit
  faCancel = faCancel


  titleLink = ""
  title = "Minha Agenda"

  agendaSelected?: AgendaPut

  showForm: boolean = false
  showView: boolean = false
  loading: boolean = false
  reloading: boolean = false
  sending: boolean = false
  sendingDel: boolean = false
  errorDel: string | undefined

  agenda?: Data
  agendaHoje: AgendaHoje[] | undefined
  agendaProximo?: AgendaProximo[]

  casca: any

  erros?: []
  erro: boolean = false

  delEventoOps?: { id: number, nome: string, data: string, hora?: string }

  modalRef?: BsModalRef

  configModal = {
    backdrop: true,
    ignoreBackdropClick: true
  }

  openModal(template: TemplateRef<any>) {
    //this.modalRef = this.modalService.show(template, Object.assign(this.configModal, { class: 'modal-dialog-centered' }))
    this.modalRef = this.modalService.show(template, this.configModal)
  }

  delEvento(template: TemplateRef<any>, id: number, nome: string, data: string, hora?: string) {
    this.sendingDel = false
    this.delEventoOps = { id: id, nome: nome, data: data, hora: hora }
    this.openModal(template)
  }

  usuarioLogado() {
    if (!this.usuarioService.logado) {
      this.toast.warn("Login expirou")
      this.route.navigate(['login'])
    }
  }

  closeModal() {
    this.modalRef?.hide();
  }

  logOut() {
    this.usuarioService.logout()
  }

  navLink() {
    if (!this.showForm && !this.showView) {
      this.home()
    }
    if (this.showForm && !this.showView && this.agendaForm.value.id > 0) {
      this.return()
    }
    else
      this.voltar()
  }

  novo() {
    this.iconLink = faChevronLeft
    this.titleLink = " Voltar"
    this.title = "Novo Evento"
    this.loading = true
    this.showForm = true
    this.erro = false
    this.agendaSelected = { id: 0, particular: true, editar: false }
    this.agendaForm.patchValue({ id: 0, nome: "", data: null, hora: null, descricao: null, local: null, participantes: null, particular: true })
    this.loading = false
  }

  viewDate(dt?: any) {
    var pt = dt?.split("-");
    //return pt[2] + "/" + pt[1] + "/" + pt[0]
    return this.funcao.ToDateBr(dt)
  }

  agendaSelect(agenda: Agenda) {
    this.showForm = true
    this.agendaSelected = agenda;
    this.agendaForm.patchValue(agenda)
  }



  loadAgendas(): void {
    this.usuarioLogado()
    this.reloading = true
    this.agendaService.getAll(!this.agendaSlt?.value.idDias ? 7 : this.agendaSlt?.value.idDias).subscribe({
      next: (res) => {
        this.agendaProximo = res?.data?.agendaProximo
        this.agendaHoje = res?.data?.agendaHoje
        this.reloading = false
      }
    })
  }

  loadAgenda() {
    this.usuarioLogado()
    this.reloading = true
    var id = !this.agendaSlt?.value.idDias ? 7 : this.agendaSlt?.value.idDias
    this.agendaService.getProximo(id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.agendaProximo = res?.data?.agendaProximo
          this.reloading = false
        }
      }
    })
  }

  getAgendaView(id: number): void {
    this.usuarioLogado()
    this.iconLink = faChevronLeft
    this.titleLink = " Voltar"
    this.title = "Visualizando Evento"
    this.showForm = false
    this.showView = true
    this.loading = true
    this.agendaService.get(id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.agendaSelected = res?.data
          this.loading = false
        }
      }
    })
  }

  getAgenda(id: number): void {
    this.usuarioLogado()
    this.iconLink = faChevronLeft
    this.titleLink = " Voltar"
    this.title = "Editando Evento"
    this.showForm = true
    this.showView = false
    this.loading = true
    this.agendaService.get(id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.agendaForm.patchValue(res?.data)
          this.loading = false
        }
      }
    })
  }

  handleSubmit() {
    //this.agendaForm?.value.compartilhados?.push({ idAgenda: 0, idUsuario: "35faaeb2-340f-4b63-9ef2-ddc2cefa8117" })
    this.usuarioLogado()
    this.erro = false
    this.sending = true
    if (this.agendaForm.value.id > 0) {
      this.agendaService.update(this.agendaForm?.value, this.agendaForm.value.id).subscribe({
        next: (res) => {
          if (res.status === 1) {
            this.toast.success(res.message)
            this.loadAgendas()
            this.voltar()
          }
          else {
            this.erro = true
            this.erros = res.errors
            this.sending = false
          }
        },
        error: (e) => {
          console.log(e?.message)
          this.erro = true
          this.erros?.push.apply([e?.message])
          this.sending = false
        }
      })
    } else {
      this.agendaService.create(this.agendaForm?.value).subscribe({
        next: (res) => {
          if (res.status === 1) {
            this.toast.success(res.message)
            this.loadAgendas()
            this.voltar()
          }
          else {
            this.erro = true
            this.erros = res.errors
            this.sending = false
          }
        },
        error: (e) => {
          console.log(e?.message)
          this.erro = true
          this.erros?.push.apply([e?.message])
          this.sending = false
        }
      })
    }

  }

  handleDelete(id: number) {
    this.sendingDel = true
    this.errorDel = undefined
    this.agendaService.delete({ id: id }, id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.toast.success(res.message)
          this.modalRef?.hide()
          this.home()
        }
        else {
          this.toast.error(res.errors[0])
          this.sendingDel = false
          this.errorDel = res.errors[0]
        }
      },
      error: (err) => {
        this.toast.error(err?.message)
        this.modalRef?.hide()
      }
    })
  }

  voltar() {
    this.usuarioLogado()
    this.iconLink = faHome
    this.title = "Minha Agenda"
    this.titleLink = ""
    this.erro = false
    this.sending = false
    this.loading = false
    this.showForm = false
    this.showView = false
  }

  home() {
    this.usuarioLogado()
    this.iconLink = faHome
    this.title = "Minha Agenda"
    this.titleLink = ""
    this.erro = false
    this.sending = false
    this.loading = false
    this.showForm = false
    this.showView = false
    this.loadAgendas()
  }

  return() {
    this.usuarioLogado()
    this.getAgendaView(this.agendaForm.value.id)
  }

}
