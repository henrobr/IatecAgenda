using api.Models.Dbase;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class Repository : IRepository
    {
        private readonly AgendaContext context;

        public Repository(AgendaContext context)
        {
            this.context = context;
        }
        //Geral
        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
        //Agenda
        public async Task<Agendas[]> GetAllAgendasAsync(string idUsuario)
        {
            IQueryable<Agendas> query = context.Agendas.Where(w => w.IdUsuario == idUsuario && w.DtEvento > DateTime.Now.AddDays(-1).Date);

            query = query.AsNoTracking()
                         .OrderBy(o => o.DtEvento);

            return await query.ToArrayAsync();
        }
        public async Task<Agendas[]> GetAllAgendasCompartilhadasAsync(string idUsuario, int dias)
        {
            IQueryable<AgendasCompartilhadas> query = context.AgendasCompartilhadas.Where(w => w.IdUsuario == idUsuario && w.IdAgendaNavigation.DtEvento > DateTime.Now.AddDays(-1).Date && w.IdAgendaNavigation.DtEvento <= DateTime.Now.AddDays(dias).Date);

            query = query.AsNoTracking()
                         .OrderBy(o => o.IdAgendaNavigation.DtEvento);

            return await query.Select(s => new Agendas() { Id = s.IdAgenda, Nome = s.IdAgendaNavigation.Nome, DtEvento = s.IdAgendaNavigation.DtEvento, HrEvento = s.IdAgendaNavigation.HrEvento, Descricao = s.IdAgendaNavigation.Descricao, Local = s.IdAgendaNavigation.Local, Participantes = s.IdAgendaNavigation.Participantes }).ToArrayAsync();
        }
        public async Task<AgendaView> GetAgendaByIdAsync(int idAgenda, string idUsuario)
        {
            AgendaView agnd = new AgendaView();

            IQueryable<Agendas> query = context.Agendas.Include(i => i.IdUsuarioNavigation).Where(w => w.Id == idAgenda && w.IdUsuario == idUsuario);
            IQueryable<AgendasCompartilhadas> queryC = context.AgendasCompartilhadas.Include(i => i.IdAgendaNavigation).ThenInclude(ti => ti.IdUsuarioNavigation)
                                                                                    .Where(w => w.IdAgenda == idAgenda && w.IdUsuario == idUsuario);

            var ag = await query.SingleOrDefaultAsync();
            var agC = await queryC.SingleOrDefaultAsync();

            if (ag == null)
            {
                //verifica se eh evento compartilhado
                if (agC == null)
                    throw new Exception("Evento não encontrada");
                else
                {
                    agnd.Id = agC.IdAgendaNavigation.Id;
                    agnd.Nome = agC.IdAgendaNavigation.Nome;
                    agnd.Data = agC.IdAgendaNavigation.DtEvento.ToString("yyyy-MM-dd");
                    agnd.Hora = agC.IdAgendaNavigation.HrEvento == null ? null : agC.IdAgendaNavigation.HrEvento.GetValueOrDefault().ToString(@"hh\:mm");
                    agnd.Descricao = agC.IdAgendaNavigation.Descricao;
                    agnd.Local = agC.IdAgendaNavigation.Local;
                    agnd.Participantes = agC.IdAgendaNavigation.Participantes;
                    agnd.CriadoPor = agC.IdAgendaNavigation.IdUsuarioNavigation.Nome;
                    agnd.Particular = true;
                    agnd.Editar = false;
                }
            }
            else
            {
                agnd.Id = ag.Id;
                agnd.Nome = ag.Nome;
                agnd.Data = ag.DtEvento.ToString("yyyy-MM-dd");
                agnd.Hora = ag.HrEvento == null ? null : ag.HrEvento.GetValueOrDefault().ToString(@"hh\:mm");
                agnd.Descricao = ag.Descricao;
                agnd.Local = ag.Local;
                agnd.CriadoPor = ag.IdUsuarioNavigation.Nome;
                agnd.Participantes = ag.Participantes;
                agnd.Particular = ag.Particular;
                agnd.Editar = true;
            }

            return agnd;
        }
        public async Task<Agendas> UpdateAgendaByIdAsync(int idAgenda, string idUsuario)
        {
            IQueryable<Agendas> query = context.Agendas.Where(w => w.Id == idAgenda && w.IdUsuario == idUsuario);

            return await query.SingleOrDefaultAsync();
        }
        public async Task<AgendasListView[]> GetAllAgendasProximosAsync(string idUsuario, int dias)
        {
            List<AgendasListView> agnd = new List<AgendasListView>();

            IQueryable<Agendas> query = context.Agendas.Where(w => w.IdUsuario == idUsuario && w.DtEvento > DateTime.Now.Date && w.DtEvento <= DateTime.Now.AddDays(dias).Date);
            IQueryable<AgendasCompartilhadas> queryC = context.AgendasCompartilhadas.Include(i => i.IdAgendaNavigation).ThenInclude(ti => ti.IdUsuarioNavigation)
                                                                                    .Where(w => w.IdUsuario == idUsuario && w.IdAgendaNavigation.DtEvento > DateTime.Now.Date && w.IdAgendaNavigation.DtEvento <= DateTime.Now.AddDays(dias).Date);


            query = query.AsNoTracking().OrderBy(o => o.DtEvento).ThenBy(o => o.HrEvento);
            queryC = queryC.AsNoTracking().OrderBy(o => o.IdAgendaNavigation.DtEvento).ThenBy(o => o.IdAgendaNavigation.HrEvento);
            var ev = await query.ToArrayAsync();
            var evc = await queryC.ToArrayAsync();

            foreach (var e in ev)
                agnd.Add(new AgendasListView() { Id = e.Id, Nome = e.Nome, Data = e.DtEvento, Hora = e.HrEvento == null ? null : e.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = false });

            foreach (var e in evc)
                agnd.Add(new AgendasListView() { Id = e.IdAgendaNavigation.Id, Nome = e.IdAgendaNavigation.Nome, Data = e.IdAgendaNavigation.DtEvento, Hora = e.IdAgendaNavigation.HrEvento == null ? null : e.IdAgendaNavigation.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = true });


            return agnd.OrderBy(o => o.Data).ThenBy(o => o.Hora).ToArray();
        }
        public async Task<AgendasCompartilhadas> GetAgendaCompartilhadaByIdAsync(int idAgenda, string idUsuario)
        {
            IQueryable<AgendasCompartilhadas> query = context.AgendasCompartilhadas.Include(i => i.IdAgendaNavigation).ThenInclude(ti => ti.IdUsuarioNavigation)
                                                                                   .Where(w => w.IdAgenda == idAgenda && w.IdUsuario == idUsuario);

            return await query.SingleOrDefaultAsync();
        }
        public async Task<AgendasListView[]> GetAllEventosAsync(string idUsuario, PaginationFilter filter)
        {
            List<AgendasListView> agnd = new List<AgendasListView>();

            IQueryable<Agendas> query = context.Agendas.Where(w => w.IdUsuario == idUsuario);
            IQueryable<AgendasCompartilhadas> queryC = context.AgendasCompartilhadas.Include(i => i.IdAgendaNavigation).ThenInclude(ti => ti.IdUsuarioNavigation)
                                                                                    .Where(w => w.IdUsuario == idUsuario);

            if (filter.Calendario)
            {
                query = query.Where(w => w.DtEvento == filter.DtEvento.Date);
                queryC = queryC.Where(w => w.IdAgendaNavigation.DtEvento == filter.DtEvento.Date);
            }
            else
            {

                if (!string.IsNullOrEmpty(filter.Search) || !string.IsNullOrWhiteSpace(filter.Search))
                {
                    query = query.Where(w => w.Nome.Contains(filter.Search.Trim()));
                    queryC = queryC.Where(w => w.IdAgendaNavigation.Nome.Contains(filter.Search.Trim()));
                }
            }
            query = query.AsNoTracking().OrderBy(o => o.DtEvento).ThenBy(o => o.HrEvento);
            queryC = queryC.AsNoTracking().OrderBy(o => o.IdAgendaNavigation.DtEvento).ThenBy(o => o.IdAgendaNavigation.HrEvento);
            var ev = await query.ToArrayAsync();
            var evc = await queryC.ToArrayAsync();

            foreach (var e in ev)
                agnd.Add(new AgendasListView() { Id = e.Id, Nome = e.Nome, Data = e.DtEvento, Hora = e.HrEvento == null ? null : e.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = false });

            foreach (var e in evc)
                agnd.Add(new AgendasListView() { Id = e.IdAgendaNavigation.Id, Nome = e.IdAgendaNavigation.Nome, Data = e.IdAgendaNavigation.DtEvento, Hora = e.IdAgendaNavigation.HrEvento == null ? null : e.IdAgendaNavigation.HrEvento.GetValueOrDefault().ToString(@"hh\:mm"), Compartilhado = true });


            return agnd.OrderBy(o => o.Data).ThenBy(o => o.Hora).ToArray();
        }
        public async Task<AgendasListView[]> GetAllEventosCompartilhadosAsync(string idUsuario)
        {
            List<AgendasListView> agnd = new List<AgendasListView>();

            IQueryable<Agendas> query = context.Agendas.Where(w => w.Particular == false && w.IdUsuario != idUsuario && w.DtEvento > DateTime.Now.AddDays(-1).Date);
            IQueryable<AgendasCompartilhadas> queryC = context.AgendasCompartilhadas//.Include(i => i.IdAgendaNavigation).ThenInclude(ti => ti.IdUsuarioNavigation)
                                                                                    .Where(w => w.IdUsuario == idUsuario);
            queryC = queryC.AsNoTracking().OrderBy(o => o.IdAgendaNavigation.DtEvento).ThenBy(o => o.IdAgendaNavigation.HrEvento);
            var evc = await queryC.Select(s => s.IdAgenda).ToListAsync();

            query = query.Where(w => !evc.Contains(w.Id));

            query = query.AsNoTracking().OrderBy(o => o.DtEvento).ThenBy(o => o.HrEvento);
            var ev = await query.ToArrayAsync();


            foreach (var e in ev)
                agnd.Add(new AgendasListView() { Id = e.Id, Nome = e.Nome, Data = e.DtEvento, Hora = e.HrEvento == null ? null : e.HrEvento.GetValueOrDefault().ToString(@"hh\:mm") });

            return agnd.OrderBy(o => o.Data).ThenBy(o => o.Hora).ToArray();
        }
        public async Task<bool> ImportarEventoAgenda(string idUsuario, int idagenda)
        {

            IQueryable<AgendasCompartilhadas> query = context.AgendasCompartilhadas.Where(w => w.IdUsuario == idUsuario && w.IdAgenda == idagenda);
            var r = await query.SingleOrDefaultAsync();

            if (r == null)
            {
                this.Add<AgendasCompartilhadas>(new AgendasCompartilhadas() { IdAgenda = idagenda, IdUsuario = idUsuario });
                return await this.SaveChangesAsync();
            }
            return true;
        }

    }
}
