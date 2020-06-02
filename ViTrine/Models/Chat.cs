using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ViTrine.Models
{
    public class Chat
    {
        [Key]
        public Guid ChatId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Nome deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Nome deve conter no máximo 256 caracteres.")]
        public string AutorMensagem { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public Guid LojaId { get; set; }

        public virtual Loja Loja { get; set; }
    }
}
