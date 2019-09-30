using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiBayer.Models
{
    public class UsuarioModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Tel { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }

    }
}