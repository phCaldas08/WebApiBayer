using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiBayer.Models
{
    public class StatusResumido
    {
        public object id { get; set; }
        public string id_processo_seletivo { get; set; }
        public int[] id_candidatos { get; set; }
        public int n_candidatos { get => id_candidatos?.Length ?? 0; }
        public decimal porcentagem_compatibilidade_min { get; set; }
    }
}