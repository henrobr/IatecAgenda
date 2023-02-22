import { ToastService } from 'angular-toastify';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AgendaService } from './../../agenda/agenda.service';
import { FuncoesService } from './../../funcoes.service';
import { AgendaProximo } from './../../models/AgendaProximo';
import { Component, OnInit, TemplateRef, ViewChildren } from '@angular/core';
import { faArrowUpFromBracket, faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-eventosCompartilhados',
  templateUrl: './eventosCompartilhados.component.html',
  styleUrls: ['./eventosCompartilhados.component.css']
})
export class EventosCompartilhadosComponent implements OnInit {


  constructor(public funcao: FuncoesService, private agendaService: AgendaService, private modalService: BsModalService, private toast: ToastService) { }

  ngOnInit() {
    this.loadEventos()
  }
  //forms arrays
  evntsCompartilhados?: AgendaProximo[]
  impEventoOps?: { id: number, nome: string, data: string, hora?: string }


  //icons
  faUp = faArrowUpFromBracket
  faEye = faEye

  //
  sendingImp: boolean = false
  errorImp: string | undefined

  modalRef?: BsModalRef

  configModal = {
    backdrop: true,
    ignoreBackdropClick: true
  }

  openModal(template: TemplateRef<any>) {
    //this.modalRef = this.modalService.show(template, Object.assign(this.configModal, { class: 'modal-dialog-centered' }))
    this.modalRef = this.modalService.show(template, this.configModal)
  }

  //carrega os eventos Compartilhados
  loadEventos(): void {
    //this.reloading = true
    this.agendaService.getAllCompartilhados().subscribe({
      next: (res) => {
        this.evntsCompartilhados = res?.data
      }
    })
  }

  impEvento(template: TemplateRef<any>, data: any) {
    this.sendingImp = false
    this.impEventoOps = { id: data.id, nome: data.nome, data: this.funcao.ToDateBr(data.data), hora: data.hora }
    this.openModal(template)
  }

  handleImport(id: number) {
    this.agendaService.importarEvento({ idAgenda: id }, id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.toast.success(res.message)
          this.loadEventos();
          this.modalRef?.hide();
        }
        else {

        }
      }
    })
  }

}
