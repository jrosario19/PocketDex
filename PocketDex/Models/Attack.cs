using System;
using System.Collections.Generic;

namespace PocketDex.Models
{
    public partial class Attack
    {
        public Attack()
        {
            PokemonAttack = new HashSet<PokemonAttack>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PokemonAttack> PokemonAttack { get; set; }
    }
}
