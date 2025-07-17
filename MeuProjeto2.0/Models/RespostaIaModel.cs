using System;
using System.ComponentModel.DataAnnotations;

namespace MeuProjeto2._0.Models
{
    public class RespostaIaModel
    {
        [Key] // <- CHAVE PRIMÁRIA OBRIGATÓRIA
        public int Id { get; set; }

        [Required]
        public required string TextoOriginal { get; set; }

        [Required]
        public required string XmlGerado { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
