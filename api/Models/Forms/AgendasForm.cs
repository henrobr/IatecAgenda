namespace api.Models.Forms
{
    public class AgendasForm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Data inválida")]
        public DateTime? Data { get; set; }
        public DateTime? Hora { get; set; }
        public string? Descricao { get; set; }
        public string? Local { get; set; }
        public string? Participantes { get; set; }
        public bool Particular { get; set; } = true;
    }
}
