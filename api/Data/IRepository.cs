using api.Models.Dbase;

namespace api.Data
{
    public interface IRepository
    {
        //Geral
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        //Agenda
        Task<Agendas[]> GetAllAgendasAsync(string idUsuario);
        Task<AgendaView> GetAgendaByIdAsync(int idAgenda, string idUsuario);
        Task<Agendas> UpdateAgendaByIdAsync(int idAgenda, string idUsuario);
        Task<AgendasListView[]> GetAllAgendasProximosAsync(string idUsuario, int dias);
        Task<Agendas[]> GetAllAgendasCompartilhadasAsync(string idUsuario, int dias);
        Task<AgendasCompartilhadas> GetAgendaCompartilhadaByIdAsync(int idAgenda, string idUsuario);
        Task<AgendasListView[]> GetAllEventosAsync(string idUsuario, PaginationFilter filter);
        Task<AgendasListView[]> GetAllEventosCompartilhadosAsync(string idUsuario);
        Task<bool> ImportarEventoAgenda(string idUsuario, int idagenda);

    }
}
