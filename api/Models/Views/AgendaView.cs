namespace api.Models.Views
{
    public class AgendaView
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Descricao { get; set; }
        public string Local { get; set; }
        public string Participantes { get; set; }
        public bool Particular { get; set; } = true;
        public bool Editar { get; set; } = false;
        public string CriadoPor { get; set; }
    }
}
