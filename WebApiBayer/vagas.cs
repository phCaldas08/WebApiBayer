//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiBayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class vagas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vagas()
        {
            this.inscricao = new HashSet<inscricao>();
        }
    
        public int id_vaga { get; set; }
        public string descricao { get; set; }
        public int id_recrutador { get; set; }
        public int duracao { get; set; }
        public System.DateTime data_criacao { get; set; }
        public decimal porcentagem_filtro { get; set; }
        public decimal porcentagem_min { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inscricao> inscricao { get; set; }
        public virtual recrutador recrutador { get; set; }
    }
}
