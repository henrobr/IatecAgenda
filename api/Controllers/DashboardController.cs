using api.Data;
using api.Models.Response;
using api.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace api.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly UserManager<Usuarios> userManager;
        private readonly IRepository repository;
        private int st { get; set; } = 1;
        private string msg { get; set; } = "Dados retornados com sucesso";
        private List<ValidationResult> erros = new List<ValidationResult>();
        private List<string> errors = new List<string>();

        public DashboardController(UserManager<Usuarios> userManager, IRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        [HttpGet]
        [Route("{dias?}")]
        public async Task<JsonResult> IndexAsync(int? dias = 7)
        {
            DashboardView dshbd = new DashboardView();
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                //Agendas do Usuario
                var r = await repository.GetAllAgendasAsync(user.Id);
                //Busca agendas compartilhadas e aceitas do usuario
                var c = await repository.GetAllAgendasCompartilhadasAsync(user.Id, dias.GetValueOrDefault());

                //Agendas Hoje
                foreach (var i in r.Where(w => w.DtEvento == DateTime.Now.Date).OrderBy(o => o.DtEvento).OrderBy(o => o.HrEvento).ToList())
                    dshbd.AgendaHoje.Add(new DashboardAgendas() { Id = i.Id, Nome = i.Nome, Data = i.DtEvento, Hora = i.HrEvento == null ? null : i.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = false });
                //Agenda Proximos
                foreach (var i in r.Where(w => w.DtEvento > DateTime.Now.Date && w.DtEvento <= DateTime.Now.AddDays(dias.GetValueOrDefault()).Date).ToList())
                    dshbd.AgendaProximo.Add(new DashboardAgendas() { Id = i.Id, Nome = i.Nome, Data = i.DtEvento, Hora = i.HrEvento == null ? null : i.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = false });

                //Agenda compartilhada hoje
                foreach (var i in c.Where(w => w.DtEvento == DateTime.Now.Date).OrderBy(o => o.DtEvento).OrderBy(o => o.HrEvento).ToList())
                    dshbd.AgendaHoje.Add(new DashboardAgendas() { Id = i.Id, Nome = i.Nome, Data = i.DtEvento, Hora = i.HrEvento == null ? null : i.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = true });
                //Agenda compartilhada Proximos
                foreach (var i in c.Where(w => w.DtEvento > DateTime.Now.Date && w.DtEvento <= DateTime.Now.AddDays(dias.GetValueOrDefault()).Date).ToList())
                    dshbd.AgendaProximo.Add(new DashboardAgendas() { Id = i.Id, Nome = i.Nome, Data = i.DtEvento, Hora = i.HrEvento == null ? null : i.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = true });

                //ordena pelas datas
                dshbd.AgendaHoje = dshbd.AgendaHoje.OrderBy(o => o.Data).ThenBy(o => o.Hora).ToList();
                dshbd.AgendaProximo = dshbd.AgendaProximo.OrderBy(o => o.Data).ThenBy(o => o.Hora).ToList();

            }
            catch (Exception ex)
            {
                st = 0;
                msg = "Erro na solicitação";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    erros.Add(new ValidationResult(excp));

                if (erros.Count > 0)
                    foreach (var e in erros)
                        errors.Add(e.ErrorMessage);
            }
            return new JsonResult(new Response<DashboardView>(st, msg, dshbd, errors));
        }
    }
}
