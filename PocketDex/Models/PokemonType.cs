using System;
using System.Collections.Generic;

namespace PocketDex.Models
{
    public partial class PokemonType
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int PokemonId { get; set; }

        public virtual Pokemon Pokemon { get; set; }
        public virtual Types Types { get; set; }
    }
}
