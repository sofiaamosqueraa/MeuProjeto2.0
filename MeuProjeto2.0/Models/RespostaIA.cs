using System;
using System.ComponentModel.DataAnnotations;

namespace MeuProjeto2._0.Models
{
    public class RespostaIA
    {
        [Key]
        public int Id { get; set; }

        public string TextoOriginal { get; set; } = string.Empty;

        public string XmlGerado { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
