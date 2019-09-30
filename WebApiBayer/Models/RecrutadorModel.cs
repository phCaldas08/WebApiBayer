using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiBayer.Models
{
    public class RecrutadorModel
    {
        public string Nome { get; set; }
        public string Registro { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }

    }
}