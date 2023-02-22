using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class AgendasController : ControllerBase
    {
        private readonly UserManager<Usuarios> userManager;
        private readonly IRepository repository;
        private int st { get; set; } = 1;
        private string msg { get; set; } = "Dados retornados com sucesso";
        private List<ValidationResult> erros = new List<ValidationResult>();
        private List<string> errors = new List<string>();
        public AgendasController(UserManager<Usuarios> userManager, IRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> New(AgendasForm form)
        {
            try
            {
                if (form == null)
                    throw new Exception("Erro no envio do formulario");

                form.Nome = form.Nome?.Trim();
                //verifica o model enviado
                erros = new ModelValidation().Result(form);
                //se houver erro
                if (erros.Count > 0)
                    throw new Exception("er");

                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var ag = new Agendas()
                {
                    Nome = form.Nome,
                    DtEvento = (DateTime)form.Data,
                    HrEvento = form.Hora == null ? null : TimeSpan.Parse(form.Hora.GetValueOrDefault().ToString("HH:mm:ss")),
                    Descricao = form.Descricao,
                    Local = form.Local,
                    Participantes = form.Participantes,
                    DtInsert = DateTime.Now,
                    IdUsuario = user.Id,
                    DtEdit = DateTime.Now,
                    Particular = form.Particular
                };

                repository.Add<Agendas>(ag);
                if (await repository.SaveChangesAsync())
                    msg = "Dados gravados com sucesso";

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
            return new JsonResult(new Response<AgendasForm>(st, msg, form, errors));
        }
        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, AgendasForm form)
        {
            try
            {
                if (form == null)
                    throw new Exception("Erro no envio do formulario");

                if (id != form.Id)
                    throw new Exception("Erro ID Agenda");

                form.Nome = form.Nome?.Trim();
                //verifica o model enviado
                erros = new ModelValidation().Result(form);
                //se houver erro
                if (erros.Count > 0)
                    throw new Exception("er");

                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var agnd = await repository.UpdateAgendaByIdAsync(id, user.Id);

                if (agnd == null)
                    throw new Exception("Agenda não encontrada.");

                agnd.Nome = form.Nome;
                agnd.DtEvento = (DateTime)form.Data;
                agnd.HrEvento = form.Hora == null ? null : TimeSpan.Parse(form.Hora.GetValueOrDefault().ToString("HH:mm:ss"));
                agnd.Descricao = form.Descricao;
                agnd.Local = form.Local;
                agnd.Participantes = form.Participantes;
                agnd.Particular = form.Particular;
                agnd.DtEdit = DateTime.Now;

                repository.Update<Agendas>(agnd);
                if (await repository.SaveChangesAsync())
                    msg = "Dados gravados com sucesso";

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
            return new JsonResult(new Response<AgendasForm>(st, msg, form, errors));
        }
        [HttpGet]
        [Route("View/{id}")]
        public async Task<IActionResult> View(int id)
        {
            AgendaView agnd = new AgendaView();
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                agnd = await repository.GetAgendaByIdAsync(id, user.Id);

                st = 1;

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
                agnd = null;
            }
            return new JsonResult(new Response<AgendaView>(st, msg, agnd, errors));
        }
        [HttpGet]
        [Route("List/Proximos/{dias}")]
        public async Task<IActionResult> ListProximos(int dias = 7)
        {
            DashboardView dshbd = new DashboardView();
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var ag = await repository.GetAllAgendasProximosAsync(user.Id, dias);

                foreach (var i in ag)
                    dshbd.AgendaProximo.Add(new DashboardAgendas() { Id = i.Id, Nome = i.Nome, Data = i.Data, Hora = i.Hora, Compartilhado = i.Compartilhado });
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
        [HttpPost]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id, AgendasForm form)
        {
            try
            {
                if (form == null)
                    throw new Exception("Erro no envio do formulario");

                if (id != form.Id)
                    throw new Exception("Erro ID Agenda");

                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var agnd = await repository.UpdateAgendaByIdAsync(id, user.Id);

                if (agnd == null)
                {
                    //verifica se agenda eh compartilhada
                    var agndComp = await repository.GetAgendaCompartilhadaByIdAsync(id, user.Id);

                    if (agndComp == null)
                        throw new Exception("Evento não encontrada");
                    else
                        repository.Delete<AgendasCompartilhadas>(agndComp);
                }
                else
                    repository.Delete<Agendas>(agnd);

                if (await repository.SaveChangesAsync())
                    msg = "Evendo excluído com sucesso";

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
            return new JsonResult(new Response<AgendasForm>(st, msg, form, errors));
        }
        [HttpGet]
        [Route("List/Eventos")]
        public async Task<IActionResult> ListEventos([FromQuery] PaginationFilter query)
        {
            List<AgendasListView> evnts = new List<AgendasListView>();
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var ag = await repository.GetAllEventosAsync(user.Id, query);

                evnts = ag.ToList();
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
            return new JsonResult(new Response<List<AgendasListView>>(st, msg, evnts, errors));
        }
        [HttpGet]
        [Route("List/Eventos/Compartilhados")]
        public async Task<IActionResult> ListEventosCompartilhados()
        {
            List<AgendasListView> evnts = new List<AgendasListView>();
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var ag = await repository.GetAllEventosCompartilhadosAsync(user.Id);

                evnts = ag.ToList();
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
            return new JsonResult(new Response<List<AgendasListView>>(st, msg, evnts, errors));
        }
        [HttpPost]
        [Route("Importar/{idagenda}")]
        public async Task<IActionResult> Importar(int idagenda, AgendasCompartilhadasForm form)
        {
            try
            {
                if (form == null)
                    throw new Exception("Erro no envio do formulario");

                if (idagenda != form.IdAgenda)
                    throw new Exception("Erro ID Agenda");
                //verifica o model enviado
                erros = new ModelValidation().Result(form);
                //se houver erro
                if (erros.Count > 0)
                    throw new Exception("er");
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                if (await repository.ImportarEventoAgenda(user.Id, idagenda))
                {
                    st = 1;
                    msg = "Dados gravados com sucesso";
                }
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
            return new JsonResult(new Response<AgendasCompartilhadasForm>(st, msg, null, errors));
        }
    }
}
