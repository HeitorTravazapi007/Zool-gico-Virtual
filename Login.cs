// Login.cs - CORRIGIDO
using System.ComponentModel.DataAnnotations;

namespace CrundApi
{
    public class Login
    {
        [MaxLength(15)]
        public string cpf { get; set; }

        [MaxLength(20)]
        public string senha { get; set; }
    }
}