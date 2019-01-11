using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gijgo.Asp.NET.Examples.Models.Entities;
using Gijgo.Asp.NetCore.Data;

namespace Gijgo.AspNetCore.Controllers
{
    public class PlayerTeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayerTeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlayerTeams
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlayerTeams.Include(p => p.Player);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PlayerTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerTeam = await _context.PlayerTeams
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (playerTeam == null)
            {
                return NotFound();
            }

            return View(playerTeam);
        }

        // GET: PlayerTeams/Create
        public IActionResult Create()
        {
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "ID");
            return View();
        }

        // POST: PlayerTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PlayerID,FromYear,ToYear,Team,Apps,Goals")] PlayerTeam playerTeam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "ID", playerTeam.PlayerID);
            return View(playerTeam);
        }

        // GET: PlayerTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerTeam = await _context.PlayerTeams.FindAsync(id);
            if (playerTeam == null)
            {
                return NotFound();
            }
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "ID", playerTeam.PlayerID);
            return View(playerTeam);
        }

        // POST: PlayerTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PlayerID,FromYear,ToYear,Team,Apps,Goals")] PlayerTeam playerTeam)
        {
            if (id != playerTeam.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerTeamExists(playerTeam.ID))
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
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "ID", playerTeam.PlayerID);
            return View(playerTeam);
        }

        // GET: PlayerTeams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerTeam = await _context.PlayerTeams
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (playerTeam == null)
            {
                return NotFound();
            }

            return View(playerTeam);
        }

        // POST: PlayerTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerTeam = await _context.PlayerTeams.FindAsync(id);
            _context.PlayerTeams.Remove(playerTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerTeamExists(int id)
        {
            return _context.PlayerTeams.Any(e => e.ID == id);
        }
    }
}
