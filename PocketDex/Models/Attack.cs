using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PocketDex.Models
{
    public partial class Attack
    {
        public Attack()
        {
            PokemonAttack = new HashSet<PokemonAttack>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name ="Nombre")]
        public string Name { get; set; }

        public virtual ICollection<PokemonAttack> PokemonAttack { get; set; }
    }
}
