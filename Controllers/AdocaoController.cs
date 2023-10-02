using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        private string caminhoImagem;

        public AdocaoController(Contexto context, IWebHostEnvironment sistema)
        {
            caminhoImagem = sistema.WebRootPath;
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
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public IActionResult Create()
        {
            /* var claimIdUser = User.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Sid).FirstOrDefault(); 
             ViewData["UsuarioId"] = claimIdUser; */
            ViewData["UsuarioId"] = new SelectList (_context.UsuarioModels, "Id", "Id");
            return View();
        }


        [HttpPost]
        
        public async Task<IActionResult> Create(AdocaoModel adocaoModel, IFormFile imagem)
        {
            string caminhoSalvarImg = caminhoImagem + "\\img\\adocao\\";
            string nomeImg = Guid.NewGuid() + "_" + imagem.FileName;

            long usuarioId = long.Parse(User.FindFirstValue("Sid"));


            if( ! Directory.Exists(caminhoSalvarImg))
            {
                Directory.CreateDirectory(caminhoSalvarImg);
            }

            using (var stream = System.IO.File.Create(caminhoSalvarImg + nomeImg))
            {
                await imagem.CopyToAsync(stream);
            }

            adocaoModel.Imagem = nomeImg;

            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Id");

            _context.AdocaoModel.Add(adocaoModel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

            /* try
             {
                 _context.AdocaoModel.Add(adocaoModel);
                 _context.SaveChanges();
                 return RedirectToAction(nameof(Index));
             }
             catch
             {
                 return View();

             }*/
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
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Email", adocaoModel.UsuarioId);
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
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Email", adocaoModel.UsuarioId);
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
