using api.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<Usuarios> userManager;
        private readonly IConfiguration configuration;
        private readonly IRepository repository;
        private int st { get; set; } = 1;
        private string msg { get; set; } = "Dados retornados com sucesso";
        private List<ValidationResult> erros = new List<ValidationResult>();
        private List<string> errors = new List<string>();

        public UsuariosController(UserManager<Usuarios> userManager, IConfiguration configuration, IRepository repository)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.repository = repository;
        }
        [HttpPost]
        [Route("Update/Nome")]
        public async Task<IActionResult> UpdateNome(UsuariosForm form)
        {
            LoginOk login = new LoginOk();
            try
            {
                if (form == null)
                    throw new Exception("Erro no envio do formulario");

                form.Nome = form.Nome?.Trim().ToUpper();
                //verifica o model enviado
                erros = new ModelValidation().Result(form);
                //se houver erro
                if (erros.Count > 0)
                    throw new Exception("er");

                var user = await userManager.FindByNameAsync(User.Identity.Name);

                user.Nome = form.Nome;

                var r = await userManager.UpdateAsync(user);

                if (r.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    LoginView usuario = new LoginView();

                    usuario.Id = user.Id;
                    usuario.Nome = user.Nome;
                    usuario.Username = user.UserName;
                    usuario.Role = roles[0].ToString();

                    login.Token = Tokens.GenerateToken(usuario, configuration);

                    st = 1;
                    msg = "Dados gravados com sucesso";
                }
                else
                {
                    foreach (var e in r.Errors)
                        erros.Add(new ValidationResult(e.Description));
                    throw new Exception("er");
                }

            }
            catch (Exception ex)
            {
                st = 0;
                msg = "Erro na solicitação";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    erros.Add(new ValidationResult(excp));

                if (erros.Count > 0)
                    foreach (var e in erros)
                        errors.Add(e.ErrorMessage);

                login = null;
            }
            return new JsonResult(new Response<LoginOk>(st, msg, login, errors));
        }
        [HttpPost]
        [Route("Update/Senha")]
        public async Task<IActionResult> UpdateSenha(UsuariosSenhaForm form)
        {
            try
            {
                if (form == null)
                    throw new Exception("Erro no envio do formulario");

                //verifica o model enviado
                erros = new ModelValidation().Result(form);
                //se houver erro
                if (erros.Count > 0)
                    throw new Exception("er");

                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var r = await userManager.ChangePasswordAsync(user, form.PasswordBefore, form.Password);

                if (r.Succeeded)
                {
                    st = 1;
                    msg = "Dados gravados com sucesso";
                }
                else
                {
                    foreach (var e in r.Errors)
                        erros.Add(new ValidationResult(e.Description));
                    throw new Exception("er");
                }

            }
            catch (Exception ex)
            {
                st = 0;
                msg = "Erro na solicitação";
                string excp = (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                if (excp != "er")
                    erros.Add(new ValidationResult(excp));

                if (erros.Count > 0)
                    foreach (var e in erros)
                        errors.Add(e.ErrorMessage);
            }
            return new JsonResult(new Response<UsuariosSenhaForm>(st, msg, null, errors));
        }
    }
}
