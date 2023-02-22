namespace api.Models.Views
{
    public class AgendasListView
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public bool Compartilhado { get; set; }
        
    }
}
