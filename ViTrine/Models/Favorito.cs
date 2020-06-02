using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ViTrine.Models
{
    public class Favorito
    {
        [Key]
        public Guid FavoritoId { get; set; }

        public Guid LojaId { get; set; }

        public virtual Loja Loja { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
    }
}
