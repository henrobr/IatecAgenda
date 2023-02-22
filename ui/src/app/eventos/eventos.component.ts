import { ViewComponent } from './view/view.component';
import { ToastService } from 'angular-toastify';
import { AgendaService } from './../agenda/agenda.service';
import { DashboardComponent } from './../dashboard/dashboard.component';
import { FormControl, FormGroup } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { faCancel, faSave } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  @Input() public idAgenda: number = 0

  constructor(private dsh: DashboardComponent, private agendaService: AgendaService, private toast: ToastService) { }

  ngOnInit() {
    if (this.idAgenda > 0) {
      this.getEventoId(this.idAgenda)
    }


  }

  //icons
  faBan = faCancel
  faSave = faSave

  //form
  @Input() public agendaForm = new FormGroup({ id: new FormControl<number>(0), nome: new FormControl(), data: new FormControl(), hora: new FormControl(), descricao: new FormControl(), local: new FormControl(), participantes: new FormControl(), particular: new FormControl(true), editar: new FormControl<boolean>(false) })

  //
  sending: boolean = false
  erros?: []
  erro: boolean = false

  handleClose() {
    this.dsh.closeModal()
  }

  handleSubmit() {
    //this.usuarioLogado()
    this.erro = false
    this.sending = true
    if (this.agendaForm.value.id! > 0) {
      this.agendaService.update(this.agendaForm?.value, this.agendaForm.value.id!).subscribe({
        next: (res) => {
          if (res.status === 1) {
            this.toast.success(res.message)
            this.dsh.loadEventos()
            //this.vw.handleClose()
            this.dsh.closeModal()
          }
          else {
            this.erro = true
            this.erros = res.errors
            this.sending = false
          }
        },
        error: (e) => {
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

            this.dsh.loadEventos()
            this.dsh.closeModal()
          }
          else {
            this.erro = true
            this.erros = res.errors
            this.sending = false
          }
        },
        error: (e) => {
          this.erro = true
          this.erros?.push.apply([e?.message])
          this.sending = false
        }
      })
    }

  }

  getEventoId(id: number): void {
    this.agendaService.get(id).subscribe({
      next: (res) => {
        if (res.status === 1) {
          this.agendaForm.patchValue(res?.data)
          //this.loading = false
        }
      }
    })
  }

}
