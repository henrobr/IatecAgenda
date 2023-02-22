using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly AgendaContext context;
        private readonly IConfiguration configuration;
        private int st { get; set; } = 0;
        private string message { get; set; } = "";
        public InfoController(AgendaContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetInfo()
        {
            string cnx = "Fail";
            try
            {
                var teste = context.Database.CanConnect();
                if (teste)
                    cnx = "Success";

                st = 1;
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return new JsonResult(new { Status = st, Sistema = configuration["Configs:Sistema"], Versao = configuration["Configs:Versao"], Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), ConexaoDB = cnx, Message = message, Today = DateTime.Now });
        }
    }
}
