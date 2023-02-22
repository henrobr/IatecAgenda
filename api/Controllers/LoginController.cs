using api.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<Usuarios> userManager;
        private readonly SignInManager<Usuarios> signInManager;
        private readonly AgendaContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment appEnvironment;
        private int st { get; set; } = 0;
        private string msg { get; set; } = "";
        private List<ValidationResult> erros = new List<ValidationResult>();
        private List<string> errors = new List<string>();
        public LoginController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, AgendaContext context, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.appEnvironment = appEnvironment;
        }
        [HttpPost]
        [Route("")]
        public async Task<JsonResult> Index(LoginForm model)
        {
            LoginOk login = new LoginOk();
            try
            {
                //verifica o model enviado
                erros = new ModelValidation().Result(model);
                //Se conter erro
                if (erros.Count > 0)
                    throw new Exception("er");

                //Model ok
                //Busca o usuario
                var user = await userManager.FindByNameAsync(model.Username);

                if (user == null)
                    throw new Exception("Login inválido");

                //Faz o login
                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                string token = "";

                //Se estiver ok
                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    LoginView usuario = new LoginView();

                    usuario.Id = user.Id;
                    usuario.Nome = user.Nome;
                    usuario.Username = user.UserName;
                    usuario.Role = roles[0].ToString();

                    token = Tokens.GenerateToken(usuario, configuration);
                }
                else
                    throw new Exception("Login inválido");

                login.Token = token;

                st = 1;
                msg = "Login realizado com sucesso";
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
        [Route("Novo/Usuario")]
        public async Task<JsonResult> NovoUsuario(UsuariosNovoForm form)
        {
            LoginOk login = new LoginOk();
            try
            {
                //verifica o model enviado
                erros = new ModelValidation().Result(form);
                //Se conter erro
                if (erros.Count > 0)
                    throw new Exception("er");
               
                string nrole = "Administrador";

                var user = new Usuarios
                {
                    UserName = form.Username.Trim().ToLower(),
                    Email = form.Username.Trim().ToLower(),
                    Nome = form.Nome.Trim().ToUpper(),
                    DtInsert = DateTime.Now,
                    EmailConfirmed = true
                };

                var r = await userManager.CreateAsync(user, form.Password);

                // Se o usuário foi criado com sucesso, adiciona ele na role Super
                if (r.Succeeded)
                {
                    //adiciona o usuario na role
                    await userManager.AddToRoleAsync(user, nrole);

                    var roles = await userManager.GetRolesAsync(user);

                    LoginView usuario = new LoginView();

                    usuario.Id = user.Id;
                    usuario.Nome = user.Nome;
                    usuario.Username = user.UserName;
                    usuario.Role = roles[0].ToString();

                    login.Token =  Tokens.GenerateToken(usuario, configuration);
                }
                else
                {
                    foreach (var e in r.Errors)
                        erros.Add(new ValidationResult(e.Description));
                    throw new Exception("er");
                }

                st = 1;
                msg = "Usuário registrado com sucesso";
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


        ////Registrar Novo Usuario Super
        //[HttpPost]
        //[Route("NewSuper")]
        //public async Task<IActionResult> NewSuper()//comentar este objeto depois de criar o usuario
        //{
        //    //Criar Uma Role Super
        //    string nrole = "Super";
        //    IdentityRole identityRole = new IdentityRole
        //    {
        //        Name = nrole
        //    };
        //    // Salva a role na tabela AspNetRole
        //    IdentityResult resultRole = await roleManager.CreateAsync(identityRole);

        //    string Password = "Abc@123";

        //    var user = new Usuarios
        //    {
        //        Id = configuration["Usr"],
        //        UserName = "super",
        //        Email = "rhlobo7@gmail.com",
        //        Nome = "SUPER",
        //        DtInsert = DateTime.Now,
        //        EmailConfirmed = true
        //    };
        //    // Armazena os dados do usuário na tabela AspNetUsers
        //    var result = await userManager.CreateAsync(user, Password);

        //    // Se o usuário foi criado com sucesso, adiciona ele na role Super
        //    if (result.Succeeded)
        //    {
        //        await userManager.AddToRoleAsync(user, nrole);
        //    }

        //    return new JsonResult(new { Status = 1, Message = "Usuário Super foi criado com sucesso" });
        //}

        ////Registrar Novo Usuario Administrador
        //[HttpPost]
        //[Route("NewAdministrador")]
        //public async Task<IActionResult> NewAdministrador()//comentar este objeto depois de criar o usuario
        //{
        //    //Criar Uma Role Administrador
        //    string nrole = "Administrador";
        //    IdentityRole identityRole = new IdentityRole
        //    {
        //        Name = nrole
        //    };
        //    // Salva a role na tabela AspNetRole
        //    IdentityResult resultRole = await roleManager.CreateAsync(identityRole);

        //    string Idnew = Guid.NewGuid().ToString();

        //    string Password = "Abc@123";

        //    var user = new Usuarios
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        UserName = "admim",
        //        Email = "rhlobo7@gmail.com",
        //        Nome = "ADMINISTRADOR DO SISTEMA",
        //        DtInsert = DateTime.Now,
        //        EmailConfirmed = true
        //    };
        //    // Armazena os dados do usuário na tabela AspNetUsers
        //    var result = await userManager.CreateAsync(user, Password);

        //    // Se o usuário foi criado com sucesso, adiciona ele na role Super
        //    if (result.Succeeded)
        //    {
        //        await userManager.AddToRoleAsync(user, nrole);
        //    }

        //    return new JsonResult(new { Status = 1, Message = "Usuário Administrador foi criado com sucesso" });
        //}
    }
}
