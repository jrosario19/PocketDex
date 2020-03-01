using System;
using System.Collections.Generic;

namespace PocketDex.Models
{
    public partial class Region
    {
        public Region()
        {
            Pokemon = new HashSet<Pokemon>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClassType { get; set; }

        public virtual ICollection<Pokemon> Pokemon { get; set; }
    }
}
