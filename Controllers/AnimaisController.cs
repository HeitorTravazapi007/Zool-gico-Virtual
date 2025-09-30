using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CrundApi.Controllers // CORREÇÃO: Namespace correto
{
    [ApiController]
    [Route("[controller]")]
    public class AnimaisController : ControllerBase
    {
        private readonly ILogger<AnimaisController> _logger;

        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Crud;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public AnimaisController(ILogger<AnimaisController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAnimal")]
        public IEnumerable<Animal> Get()
        {
            List<Animal> animais = new List<Animal>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // CORREÇÃO: Tabela correta
                string query = "SELECT * FROM animal";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Animal animal = new Animal
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString(),
                        Especie = reader["Especie"].ToString(),
                        Habitate = reader["Habitate"].ToString(),
                        Dieta = reader["Dieta"].ToString(),
                        Peso = Convert.ToDecimal(reader["Peso"]),
                        Curiosidade = reader["Curiosidade"].ToString()
                    };

                    animais.Add(animal);
                }

                reader.Close();
            }

            return animais;
        }

        [HttpGet("{id}", Name = "GetAnimalById")]
        public ActionResult GetAnimalById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM animal WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Animal animal = new Animal
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString(),
                        Especie = reader["Especie"].ToString(),
                        Habitate = reader["Habitate"].ToString(),
                        Dieta = reader["Dieta"].ToString(),
                        Peso = Convert.ToDecimal(reader["Peso"]),
                        Curiosidade = reader["Curiosidade"].ToString()
                    };

                    reader.Close();
                    return Ok(animal);
                }

                reader.Close();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateAnimal([FromBody] Animal animal) // CORREÇÃO: Added [FromBody]
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO animal (Nome, Especie, Habitate, Dieta, Peso, Curiosidade) VALUES (@nome, @especie, @habitate, @dieta, @peso, @curiosidade)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", animal.Nome);
                command.Parameters.AddWithValue("@especie", animal.Especie);
                command.Parameters.AddWithValue("@habitate", animal.Habitate);
                command.Parameters.AddWithValue("@dieta", animal.Dieta);
                command.Parameters.AddWithValue("@peso", animal.Peso);
                command.Parameters.AddWithValue("@curiosidade", animal.Curiosidade);
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Animal criado com sucesso" });
                }
            }

            return BadRequest("Erro ao criar animal");
        }

        [HttpPut("{id}")] // CORREÇÃO: Removido o [HttpPut] duplicado
        public ActionResult UpdateAnimal(int id, [FromBody] Animal animal)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "UPDATE animal SET Nome = @nome, Especie = @especie, Habitate = @habitate, Dieta = @dieta, Peso = @peso, Curiosidade = @curiosidade WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", animal.Nome);
                command.Parameters.AddWithValue("@especie", animal.Especie);
                command.Parameters.AddWithValue("@habitate", animal.Habitate);
                command.Parameters.AddWithValue("@dieta", animal.Dieta);
                command.Parameters.AddWithValue("@peso", animal.Peso);
                command.Parameters.AddWithValue("@curiosidade", animal.Curiosidade);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Animal atualizado com sucesso" });
                }
            }

            return NotFound("Animal não encontrado");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAnimal(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM animal WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok(new { message = "Animal excluído com sucesso" });
                }
            }

            return NotFound("Animal não encontrado");
        }
    }
}