﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Genero")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Altura")]
        public string Height { get; set; }
        [Required]
        [Display(Name = "Peso")]
        public string Weight { get; set; }
        [Required]
        [Display(Name = "Región")]
        public int RegionId { get; set; }
        [Required]
        [Display(Name = "Ruta de foto")]
        public string PhotoPath { get; set; }

        public virtual Region Region { get; set; }
        [Required]
        [Display(Name = "Ataques")]
        public virtual ICollection<PokemonAttack> PokemonAttack { get; set; }
        [Required]
        [Display(Name = "Tipos")]
        public virtual ICollection<PokemonType> PokemonType { get; set; }
    }
}
