using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PocketDex.Models
{
    public partial class Region
    {
        public Region()
        {
            Pokemon = new HashSet<Pokemon>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Clase CSS")]
        public string ClassType { get; set; }

        public virtual ICollection<Pokemon> Pokemon { get; set; }
    }
}
