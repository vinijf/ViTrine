using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViTrine.Models
{
    public class Mensagem
    {
        [Key]
        public Guid MensagemId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(1, ErrorMessage = "O campo Texto deve conter no mínimo 1 caracteres.")]
        [MaxLength(512, ErrorMessage = "O campo Texto deve conter no máximo 512 caracteres.")]
        public string TextoMensagem { get; set; }

        [Required]
        public DateTime DataMensagem { get; set; }

        public Guid ChatId { get; set; }

        public virtual Chat Chat { get; set; }
    }
}
