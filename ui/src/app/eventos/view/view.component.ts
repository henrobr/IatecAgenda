import { FormControl, FormGroup } from '@angular/forms';
import { ToastService } from 'angular-toastify';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DashboardComponent } from './../../dashboard/dashboard.component';
import { FuncoesService } from './../../funcoes.service';
import { AgendaPut } from './../../models/AgendaPut';
import { AgendaService } from './../../agenda/agenda.service';
import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { faChevronLeft, faEdit, faSave, faSignOutAlt, faTrash, faUserEdit } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-view',
  templateUrl: './view.component.html',
  styleUrls: ['./view.component.css']
})
export class ViewComponent implements OnInit {
  @Input() public id: number = 0
  //@Input() public feChar = this.handleClose()

  constructor(private dsh: DashboardComponent, private agendaService: AgendaService, private funcao: FuncoesService, private modalService: BsModalService, private toast: ToastService) { }

  ngOnInit() {
    if (this.id > 0)
      this.getAgendaView(this.id)
  }
  //vars
  titleModal = "Alterar Evento"
  idEvento: number = 0

  //icons
  faEdit = faEdit
  faTrash = faTrash
  faArrowLeft = faChevronLeft
  faSave = faSave
  faOut = faSignOutAlt
  faUserEdit = faUserEdit


  agendaSelected?: AgendaPut
  delEventoOps?: { id: number, nome: string, data: string, hora?: string }
  agendaForm = new FormGroup({ id: new FormControl<number>(0), nome: new FormControl(), data: new FormControl(), hora: new FormControl(), descricao: new FormControl(), local: new FormControl(), participantes: new FormControl(), particular: new FormControl(true), editar: new FormControl<boolean>(false) })

  loading: boolean = false
  erro: boolean = false
  erros?: []
  sendingDel: boolean = false
  errorDel: string | undefined

  viewDate(dt?: any) {
    return this.funcao.ToDateBr(dt)
  }

  handleClose() {
    this.modalRef?.hide()
    this.dsh.closeModal()
  }

  fecharModal() {
    this.modalRef?.hide()
  }

  modalRef?: BsModalRef

  configModal = {
    backdrop: true,
    ignoreBackdropClick: true
  }

  closeModal(modalId?: number) {
    this.modalService.hide(modalId);
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
  getEventoId(template: TemplateRef<any>, id: number): void {
    //console.log(id)
    //this.dsh.closeModal()
    this.dsh.idEvento = id
    this.dsh.getEventoId(id)
    //this.agendaForm = this.dsh.agendaForm
    this.agendaForm = this.dsh.agendaForm
    this.openModal(template)
  }

  getAgendaView(id: number): void {
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

  handleDelete(id: number) {
    this.sendingDel = true
    this.errorDel = undefined
    this.agendaService.delete({ id: id }, id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.toast.success(res.message)
          this.modalRef?.hide()
          this.dsh.loadEventos()
          this.dsh.closeModal()
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

}
