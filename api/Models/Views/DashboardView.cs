namespace api.Models.Views
{
    public class DashboardView
    {
        public DashboardView()
        {
            AgendaHoje = new List<DashboardAgendas>();
            AgendaProximo = new List<DashboardAgendas>();
        }
        public List<DashboardAgendas> AgendaHoje { get; set; }
        public List<DashboardAgendas> AgendaProximo { get; set; }
    }
    public class DashboardAgendas
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public bool Compartilhado { get; set; }
    }
}
