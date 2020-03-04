using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PocketDex.Models;

namespace PocketDex.Controllers
{
    public class AttacksController : Controller
    {
        private readonly PokeDexContext _context;

        public AttacksController(PokeDexContext context)
        {
            _context = context;
        }

        // GET: Attacks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attack.ToListAsync());
        }

        // GET: Attacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Attack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attack == null)
            {
                return NotFound();
            }

            return View(attack);
        }

        // GET: Attacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Attack attack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attack);
        }

        // GET: Attacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Attack.FindAsync(id);
            if (attack == null)
            {
                return NotFound();
            }
            return View(attack);
        }

        // POST: Attacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Attack attack)
        {
            if (id != attack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttackExists(attack.Id))
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
            return View(attack);
        }

        // GET: Attacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attack = await _context.Attack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attack == null)
            {
                return NotFound();
            }

            return View(attack);
        }

        // POST: Attacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attack = await _context.Attack.FindAsync(id);
            _context.Attack.Remove(attack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttackExists(int id)
        {
            return _context.Attack.Any(e => e.Id == id);
        }
    }
}
