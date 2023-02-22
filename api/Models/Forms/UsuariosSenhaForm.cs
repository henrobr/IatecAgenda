namespace api.Models.Forms
{
    public class UsuariosSenhaForm
    {
        [Required(ErrorMessage = "Senha atual deve ser informada")]
        public string PasswordBefore { get; set; }
        [Required(ErrorMessage = "Nova Senha deve ter no minimo 6 caracteres")]
        [StringLength(255, ErrorMessage = "Senha deve ter no minimo 6 caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confimar Senha deve ser igual a Nova Senha")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

    }
}
