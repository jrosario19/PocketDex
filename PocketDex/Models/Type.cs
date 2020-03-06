using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PocketDex.Models
{
    public partial class Types
    {
        public Types()
        {
            PokemonType = new HashSet<PokemonType>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public virtual ICollection<PokemonType> PokemonType { get; set; }
    }
}
