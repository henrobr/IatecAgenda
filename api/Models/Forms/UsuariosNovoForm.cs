namespace api.Models.Forms
{
    public class UsuariosNovoForm
    {
        [Required(ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MinLength(2, ErrorMessage = "Nome deve ter ao menos 2 caracteres")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "E-mail inválido")]
        [RegularExpression(@"^[\w-_]+(\.[\w!#$%'*+\/=?\^`{|}]+)*@((([\-\w]+\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "E-mail em formato inválido.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Nova Senha deve ter no minimo 6 caracteres")]
        [StringLength(255, ErrorMessage = "Senha deve ter no minimo 6 caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confimar Senha deve ser igual a Nova Senha")]
        [Compare("Password", ErrorMessage = "Senha e Confirme Senha devem ser iguais")]
        public string PasswordConfirm { get; set; }
    }
}
