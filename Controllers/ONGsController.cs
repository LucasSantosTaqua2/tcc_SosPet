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
    public class ONGsController : Controller
    {
        private readonly Contexto _context;

        public ONGsController(Contexto context)
        {
            _context = context;
        }

        // GET: ONGs
        public async Task<IActionResult> CentralONGs()
        {
              return _context.ONGsModels != null ? 
                          View(await _context.ONGsModels.ToListAsync()) :
                          Problem("Entity set 'Contexto.ONGsModels'  is null.");
        }

        // GET: ONGs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ONGsModels == null)
            {
                return NotFound();
            }

            var oNGsModel = await _context.ONGsModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oNGsModel == null)
            {
                return NotFound();
            }

            return View(oNGsModel);
        }

        // GET: ONGs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ONGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Endereco,Cidade,Tel,Email,Data,Imagem")] ONGsModel oNGsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oNGsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oNGsModel);
        }

        // GET: ONGs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ONGsModels == null)
            {
                return NotFound();
            }

            var oNGsModel = await _context.ONGsModels.FindAsync(id);
            if (oNGsModel == null)
            {
                return NotFound();
            }
            return View(oNGsModel);
        }

        // POST: ONGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Endereco,Cidade,Tel,Email,Data,Imagem")] ONGsModel oNGsModel)
        {
            if (id != oNGsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oNGsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ONGsModelExists(oNGsModel.Id))
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
            return View(oNGsModel);
        }

        // GET: ONGs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ONGsModels == null)
            {
                return NotFound();
            }

            var oNGsModel = await _context.ONGsModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oNGsModel == null)
            {
                return NotFound();
            }

            return View(oNGsModel);
        }

        // POST: ONGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ONGsModels == null)
            {
                return Problem("Entity set 'Contexto.ONGsModels'  is null.");
            }
            var oNGsModel = await _context.ONGsModels.FindAsync(id);
            if (oNGsModel != null)
            {
                _context.ONGsModels.Remove(oNGsModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ONGsModelExists(int id)
        {
          return (_context.ONGsModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
