namespace api.Models.Forms
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Usuário inválido")]
        [MinLength(2, ErrorMessage = "Usuário inválido")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Senha inválida")]
        public string Password { get; set; }
    }
}
