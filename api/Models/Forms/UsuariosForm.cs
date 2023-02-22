namespace api.Models.Forms
{
    public class UsuariosForm
    {        
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; }
        
    }
}
