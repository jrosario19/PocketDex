using Microsoft.AspNetCore.Http;
using PocketDex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PocketDex.ViewModels
{
    public class PokemonEditViewModel
    {
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
        [Display(Name = "Foto")]
        public IFormFile PhotoPath { get; set; }
        [Required]
        [Display(Name = "FotoSt")]
        public String PhotoString { get; set; }

        public virtual Region Region { get; set; }
        [Required]
        [Display(Name = "Ataques")]
        public virtual List<int> AttackIds { get; set; }
        [Required]
        [Display(Name = "Tipos")]
        public virtual List<int> TypesIds { get; set; }
    }
}
