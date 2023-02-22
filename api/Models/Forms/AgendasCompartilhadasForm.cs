namespace api.Models.Forms
{
    public class AgendasCompartilhadasForm
    {
        [Range(1, double.MaxValue, ErrorMessage = "Evento inválido")]
        public int IdAgenda { get; set; }
    }
}
