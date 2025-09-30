using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CrundApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Crud;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<LoginResponse> ValidarLogin([FromBody] Login login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT COUNT(1) FROM login WHERE cpf = @cpf AND senha = @senha";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@cpf", login.cpf);
                    command.Parameters.AddWithValue("@senha", login.senha);
                    connection.Open();

                    int result = (int)command.ExecuteScalar();

                    if (result > 0)
                    {
                        return Ok(new LoginResponse { success = true, token = "auth-token-" + Guid.NewGuid() });
                    }
                    else
                    {
                        return Ok(new LoginResponse { success = false });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar login");
                return StatusCode(500, new LoginResponse { success = false, message = "Erro interno do servidor" });
            }
        }

        public class LoginResponse
        {
            public bool success { get; set; }
            public string token { get; set; }
            public string message { get; set; }
        }
    }
}