using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ViTrine.Models
{
    public class Loja
    {
        [Key]
        public Guid LojaId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string UrlFotoLoja { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Nome deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Nome deve conter no máximo 256 caracteres.")]
        public string NomeLoja { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Categoria deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Categoria deve conter no máximo 256 caracteres.")]
        public string CategoriaLoja { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        public string TelefoneLoja { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Cidade deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Cidade deve conter no máximo 256 caracteres.")]
        public string CidadeLoja { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Endereço deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Endereço deve conter no máximo 512 caracteres.")]
        public string EnderecoLoja { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
    }
}
