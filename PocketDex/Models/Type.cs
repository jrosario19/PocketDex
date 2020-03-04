using System;
using System.Collections.Generic;

namespace PocketDex.Models
{
    public partial class Types
    {
        public Types()
        {
            PokemonType = new HashSet<PokemonType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PokemonType> PokemonType { get; set; }
    }
}
