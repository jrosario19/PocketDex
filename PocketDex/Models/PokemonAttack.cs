using System;
using System.Collections.Generic;

namespace PocketDex.Models
{
    public partial class PokemonAttack
    {
        public int Id { get; set; }
        public int AttackId { get; set; }
        public int PokemonId { get; set; }

        public virtual Attack Attack { get; set; }
        public virtual Pokemon Pokemon { get; set; }
    }
}
