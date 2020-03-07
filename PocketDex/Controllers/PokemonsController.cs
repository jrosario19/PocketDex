using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PocketDex.Models;
using PocketDex.ViewModels;

namespace PocketDex.Controllers
{
    public class PokemonsController : Controller
    {
        private readonly PokeDexContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PokemonsController(PokeDexContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this._hostingEnvironment = hostingEnvironment;
        }

        // GET: Pokemons
        public async Task<IActionResult> Index()
        {
            var pokeDexContext = _context.Pokemon.Include(p => p.Region).Include(pt => pt.PokemonType).Include(pa=>pa.PokemonAttack);
            ViewBag.TypesVB = _context.Types.ToList();
            ViewBag.AttacksVB = _context.Attack.ToList();
            //_context.PokemonType.Join(_context.Types, pt1 => pt1.TypeId, t => t.Id, (pt1,t)=>new { PokemonTypes = pt1, Types = t });
            return View(await pokeDexContext.ToListAsync());
        }

        // GET: Pokemons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemon = await _context.Pokemon.Include(pt=>pt.PokemonType).Include(pa=>pa.PokemonAttack)
                .Include(p => p.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pokemon == null)
            {
                return NotFound();
            }
            ViewBag.TypesVB = _context.Types.ToList();
            ViewBag.AttacksVB = _context.Attack.ToList();
            return View(pokemon);
        }

        // GET: Pokemons/Create
        public IActionResult Create()
        {
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "ClassType");
            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Name");
            ViewData["AttackId"] = new SelectList(_context.Attack, "Id", "Name");
            return View();
        }

        // POST: Pokemons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Gender,Height,Weight,RegionId,PhotoPath,AttackIds,TypesIds")] PokemonCreateViewModel pokemon)
        {
            if (ModelState.IsValid)
            {
                if (pokemon.RegionId==0)
                {
                    ViewBag.ErrorTitle = $"Región no seleccionada para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} debe seleccionar una región a la cual pertenece" +
                        $". Seleccione una región y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                if (pokemon.Gender == null)
                {
                    ViewBag.ErrorTitle = $"Genero no seleccionado para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} debe seleccionar un Genero al cual pertenece" +
                        $". Seleccione una genero y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                if (pokemon.AttackIds.Count>4 || pokemon.AttackIds.Count < 1)
                {
                    ViewBag.ErrorTitle = $"Cantidad requerida de ataques seleccionados para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} solo debe seleccionar entre 1 y 4 ataques"+
                        $". Seleccione y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                if (pokemon.TypesIds.Count > 2 || pokemon.AttackIds.Count < 1)
                {
                    ViewBag.ErrorTitle = $"Cantidad requerida de tipos seleccionados para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} solo debe seleccionar entre 1 y 2 tipos" +
                        $". Seleccione y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                string uniqueFileName = ProcessUploadedFile(pokemon);
                Pokemon pokemonnew = new Pokemon()
                {
                    Name=pokemon.Name,
                    Description=pokemon.Description,
                    Gender= pokemon.Gender,
                    Height=pokemon.Height,
                    Weight=pokemon.Weight,
                    RegionId=pokemon.RegionId,
                    PhotoPath= uniqueFileName

                };
                _context.Add(pokemonnew);
                await _context.SaveChangesAsync();

                foreach (var item in pokemon.AttackIds)
                {
                    PokemonAttack pokemonAttack = new PokemonAttack()
                    {
                        PokemonId = pokemonnew.Id,
                        AttackId = item
                    };
                    _context.Add(pokemonAttack);
                    await _context.SaveChangesAsync();
                }

                foreach (var item in pokemon.TypesIds)
                {
                    PokemonType pokemonType = new PokemonType()
                    {
                        PokemonId = pokemonnew.Id,
                        TypeId = item
                    };
                    _context.Add(pokemonType);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "ClassType", pokemon.RegionId);
            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Name");
            ViewData["AttackId"] = new SelectList(_context.Attack, "Id", "Name");
            return View(pokemon);
        }

        // GET: Pokemons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemon = await _context.Pokemon.FindAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "ClassType", pokemon.RegionId);
            //ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Name");
            //ViewData["AttackId"] = new SelectList(_context.Attack, "Id", "Name");
            List<PokemonType> pokemonTypes = _context.PokemonType.Where(pt => pt.PokemonId == pokemon.Id).ToList();
            List<PokemonAttack> pokemonAttacks = _context.PokemonAttack.Where(pa => pa.PokemonId == pokemon.Id).ToList();
            List<int> typesIds = new List<int>();
            List<int> attackIds = new List<int>();

            foreach (var pokemonType in pokemonTypes)
            {
                typesIds.Add(pokemonType.TypeId);
            }

            foreach (var pokemonAttack in pokemonAttacks)
            {
                attackIds.Add(pokemonAttack.AttackId);
            }

            PokemonEditViewModel pokemonEdit = new PokemonEditViewModel()
            {
                Name=pokemon.Name,
                Description=pokemon.Description,
                Height=pokemon.Height,
                Gender=pokemon.Gender,
                Weight=pokemon.Weight,
                RegionId = pokemon.RegionId,
                PhotoString = pokemon.PhotoPath,
                TypesIds= typesIds,
                AttackIds= attackIds
            };

            
            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Name", pokemonEdit.TypesIds);
            ViewData["AttackId"] = new SelectList(_context.Attack, "Id", "Name", pokemonEdit.AttackIds);
            ViewData["Gender"] = new SelectList(new[] { "Femenino", "Masculino" }, pokemon.Gender);

            return View(pokemonEdit);
        }

        // POST: Pokemons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Gender,Height,Weight,RegionId,PhotoPath,PhotoString,AttackIds,TypesIds")] PokemonEditViewModel pokemon)
        {
            if (id != pokemon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (pokemon.RegionId == 0)
                {
                    ViewBag.ErrorTitle = $"Región no seleccionada para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} debe seleccionar una región a la cual pertenece" +
                        $". Seleccione una región y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                if (pokemon.Gender == null)
                {
                    ViewBag.ErrorTitle = $"Genero no seleccionado para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} debe seleccionar un Genero al cual pertenece" +
                        $". Seleccione una genero y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                if (pokemon.AttackIds.Count > 4 || pokemon.AttackIds.Count < 1)
                {
                    ViewBag.ErrorTitle = $"Cantidad requerida de ataques seleccionados para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} solo debe seleccionar entre 1 y 4 ataques" +
                        $". Seleccione y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                if (pokemon.TypesIds.Count > 2 || pokemon.AttackIds.Count < 1)
                {
                    ViewBag.ErrorTitle = $"Cantidad requerida de tipos seleccionados para crear Pokemon {pokemon.Name}";
                    ViewBag.ErrorMessage = $"Para crear pokemon {pokemon.Name} solo debe seleccionar entre 1 y 2 tipos" +
                        $". Seleccione y luego trate de nuevo" +
                        $".";
                    return View("Error1");
                }
                try
                {
                    List<PokemonType> pokemonTypes = _context.PokemonType.Where(pt => pt.PokemonId == pokemon.Id).ToList();
                    List<PokemonAttack> pokemonAttacks = _context.PokemonAttack.Where(pa => pa.PokemonId == pokemon.Id).ToList();
                    _context.PokemonType.RemoveRange(pokemonTypes);
                    _context.PokemonAttack.RemoveRange(pokemonAttacks);

                    foreach (var item in pokemon.AttackIds)
                    {
                        PokemonAttack pokemonAttack = new PokemonAttack()
                        {
                            PokemonId = pokemon.Id,
                            AttackId = item
                        };
                        _context.PokemonAttack.Add(pokemonAttack);
                        //await _context.SaveChangesAsync();
                    }

                    foreach (var item in pokemon.TypesIds)
                    {
                        PokemonType pokemonType = new PokemonType()
                        {
                            PokemonId = pokemon.Id,
                            TypeId = item
                        };
                        _context.PokemonType.Add(pokemonType);
                        //await _context.SaveChangesAsync();
                    }
                    string uniqueFileName = "";
                    if (pokemon.PhotoPath == null)
                    {
                        uniqueFileName = pokemon.PhotoString;
                    }
                    else
                    {
                        uniqueFileName = ProcessUploadedFileEdit(pokemon);
                    }
                    Pokemon pokemonEdit = new Pokemon()
                    {
                        Id=pokemon.Id,
                        Name = pokemon.Name,
                        Description = pokemon.Description,
                        Height = pokemon.Height,
                        Gender = pokemon.Gender,
                        Weight = pokemon.Weight,
                        RegionId = pokemon.RegionId,
                        PhotoPath = uniqueFileName,
                    };
                        _context.Pokemon.Update(pokemonEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PokemonExists(pokemon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "ClassType", pokemon.RegionId);
            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Name", pokemon.TypesIds);
            ViewData["AttackId"] = new SelectList(_context.Attack, "Id", "Name", pokemon.AttackIds);
            ViewData["Gender"] = new SelectList(new[] { "Femenino", "Masculino" }, pokemon.Gender);
            return View(pokemon);
        }

        // GET: Pokemons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pokemon = await _context.Pokemon
                .Include(p => p.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pokemon == null)
            {
                return NotFound();
            }
            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Name");
            ViewData["AttackId"] = new SelectList(_context.Attack, "Id", "Name");
            return View(pokemon);
        }

        // POST: Pokemons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pokemon = await _context.Pokemon.FindAsync(id);
            try
            {
                
                _context.Pokemon.Remove(pokemon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ViewBag.ErrorTitle = $"{pokemon.Name} pokemon esta en uso";
                ViewBag.ErrorMessage = $"{pokemon.Name} pokemon no puede ser eliminado puesto que esta vinculado con otras entidades" +
                    $". Si quiere eliminarlo favor elimine los elementos relacionados con este pokemon y luego trate de eliminarlo" +
                    $".";
                return View("Error1");
            }
            
        }

        private bool PokemonExists(int id)
        {
            return _context.Pokemon.Any(e => e.Id == id);
        }
        private string ProcessUploadedFile(PokemonCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.PhotoPath != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoPath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PhotoPath.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }
        private string ProcessUploadedFileEdit(PokemonEditViewModel model)
        {
            string uniqueFileName = null;
            if (model.PhotoPath != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoPath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PhotoPath.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }
    }
}
