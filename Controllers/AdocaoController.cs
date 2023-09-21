using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOSPets.Data;
using SOSPets.Models;

namespace SOSPets.Controllers
{
    public class AdocaoController : Controller
    {
        private readonly Contexto _context;

        public AdocaoController(Contexto context)
        {
            _context = context;
        }

        // GET: Adocao
        public async Task<IActionResult> Index()
        {
            var contexto = _context.AdocaoModel.Include(a => a.Usuario);
            return View(await contexto.ToListAsync());
        }

        // GET: Adocao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdocaoModel == null)
            {
                return NotFound();
            }

            var adocaoModel = await _context.AdocaoModel
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adocaoModel == null)
            {
                return NotFound();
            }

            return View(adocaoModel);
        }

        // GET: Adocao/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Id");
            return View();
        }



        // POST: Adocao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Peso,Porte,Raca,Idade,Cor,Cidade,Data,UsuarioId")] AdocaoModel adocaoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adocaoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Id", adocaoModel.UsuarioId);
            return View(adocaoModel);
        }

        // GET: Adocao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdocaoModel == null)
            {
                return NotFound();
            }

            var adocaoModel = await _context.AdocaoModel.FindAsync(id);
            if (adocaoModel == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Id", adocaoModel.UsuarioId);
            return View(adocaoModel);
        }

        // POST: Adocao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Peso,Porte,Raca,Idade,Cor,Cidade,Data,UsuarioId")] AdocaoModel adocaoModel)
        {
            if (id != adocaoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adocaoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdocaoModelExists(adocaoModel.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Id", adocaoModel.UsuarioId);
            return View(adocaoModel);
        }

        // GET: Adocao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdocaoModel == null)
            {
                return NotFound();
            }

            var adocaoModel = await _context.AdocaoModel
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adocaoModel == null)
            {
                return NotFound();
            }

            return View(adocaoModel);
        }

        // POST: Adocao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdocaoModel == null)
            {
                return Problem("Entity set 'Contexto.AdocaoModel'  is null.");
            }
            var adocaoModel = await _context.AdocaoModel.FindAsync(id);
            if (adocaoModel != null)
            {
                _context.AdocaoModel.Remove(adocaoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdocaoModelExists(int id)
        {
          return (_context.AdocaoModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
