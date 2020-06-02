using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViTrine.Models
{
    public class Produto
    {
        [Key]
        public Guid ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string UrlFotoProduto { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Nome deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Nome deve conter no máximo 256 caracteres.")]
        public string NomeProduto { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Categoria deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Categoria deve conter no máximo 256 caracteres.")]
        public string CategoriaProduto { get; set; }

        public bool PromocaoProduto { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public double PrecoProduto { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [MinLength(5, ErrorMessage = "O campo Descrição deve conter no mínimo 5 caracteres.")]
        [MaxLength(256, ErrorMessage = "O campo Descrição deve conter no máximo 256 caracteres.")]
        public string DescricaoProduto { get; set; }

        public Guid LojaId { get; set; }

        public virtual Loja Loja { get; set; }
    }
}
