using System;
using System.Collections.Generic;

namespace PocketDex.Models
{
    public partial class Pokemon
    {
        public Pokemon()
        {
            PokemonAttack = new HashSet<PokemonAttack>();
            PokemonType = new HashSet<PokemonType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public int RegionId { get; set; }
        public string PhotoPath { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<PokemonAttack> PokemonAttack { get; set; }
        public virtual ICollection<PokemonType> PokemonType { get; set; }
    }
}
