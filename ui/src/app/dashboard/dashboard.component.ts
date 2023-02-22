import { faCalendarPlus, faSearch, faShare } from '@fortawesome/free-solid-svg-icons';
import { AppComponent } from './../app.component';
import { AgendaService } from './../agenda/agenda.service';
import { ToastService } from 'angular-toastify';
import { UsuarioService } from './../Usuario.service';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef, ModalDirective } from 'ngx-bootstrap/modal';
import { FuncoesService } from './../funcoes.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Component, OnInit, EventEmitter, Output, TemplateRef, Input, ViewChild } from '@angular/core';
import { AgendaProximo } from '../models/AgendaProximo';
import { AgendaHoje } from '../models/AgendaHoje';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  // @ViewChild('childModal', { static: false }) childModal?: ModalDirective;

  title = "Minha Agenda"
  titleModal = "Novo Evento"

  idEvento: number = 0

  feChar() {
    console.log("aqui")
    this.closeModal()
  }

  // hideChildModal(): void {
  //   this.childModal?.hide();
  // }

  //@Input() public fechar() = this.closeModal();

  constructor(private agendaService: AgendaService, private toast: ToastService, private usuarioService: UsuarioService, private route: Router, private funcao: FuncoesService, private modalService: BsModalService) { }

  ngOnInit() {
    this.loadEventos()
  }
  //icons
  faNew = faCalendarPlus
  faSearch = faSearch
  faShare = faShare
  faPlus = faCalendarPlus

  //forms
  agendaSlt = new FormGroup({ idDias: new FormControl<number>(7) })
  formSearch = new FormGroup({ search: new FormControl<string | "">(""), calendario: new FormControl<boolean | false>(false), dtEvento: new FormControl<string | "">("") })
  agendaForm = new FormGroup({ id: new FormControl<number>(0), nome: new FormControl(), data: new FormControl(), hora: new FormControl(), descricao: new FormControl(), local: new FormControl(), participantes: new FormControl(), particular: new FormControl(true), editar: new FormControl<boolean>(false) })

  errorDel: string | undefined
  showForm: boolean = false
  showView: boolean = false
  loading: boolean = false
  reloading: boolean = false
  sending: boolean = false
  sendingDel: boolean = false
  opc: boolean = true

  agendaHoje: AgendaHoje[] | undefined
  agendaProximo?: AgendaProximo[]
  agendaSearchs?: AgendaProximo[]


  //Modal
  modalRef?: BsModalRef

  configModal = {
    backdrop: true,
    ignoreBackdropClick: true
  }

  openModal(template: TemplateRef<any>) {
    //this.modalRef = this.modalService.show(template, Object.assign(this.configModal, { class: 'modal-dialog-centered' }))

    this.modalRef = this.modalService.show(template, this.configModal)
  }
  // public closeModal() {
  //   this.modalRef?.hide()
  // }

  closeModal(modalId?: number) {
    this.modalService.hide(modalId);
  }

  novoEvento(template: TemplateRef<any>) {
    this.idEvento = 0
    this.titleModal = "Novo Evento"
    this.openModal(template)
  }

  viewEvento(template: TemplateRef<any>, id: number) {
    this.idEvento = id
    this.titleModal = "Evento"
    this.openModal(template)
  }

  setOpc(op: boolean) {
    this.opc = op
  }

  diasPrx: number = 7



  usuarioLogado() {
    if (!this.usuarioService.logado) {
      this.toast.warn("Login expirou")
      this.route.navigate(['login'])
    }
  }

  viewDate(dt?: any) {
    return this.funcao.ToDateBr(dt)
  }

  //carrega os eventos 
  loadEventos(): void {
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

  loadProximosEventos() {
    this.usuarioLogado()
    this.reloading = true
    this.diasPrx = !this.agendaSlt?.value.idDias ? 7 : this.agendaSlt?.value.idDias
    this.agendaService.getProximo(this.diasPrx).subscribe({
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
    this.showForm = false
    this.showView = true
    this.loading = true
    this.agendaService.get(id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.loading = false
        }
      }
    })
  }

  getEventoId(id: number): void {
    this.agendaService.get(id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.agendaForm.patchValue(res?.data)
        }
      }
    })
  }

  handleSearch() {
    this.formSearch.value.calendario = this.opc ? false : true
    this.agendaService.getSearch(this.formSearch.value).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.agendaSearchs = res.data
        }
      }
    })
  }

}
