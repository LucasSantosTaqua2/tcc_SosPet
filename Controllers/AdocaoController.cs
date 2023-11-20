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
            _context = context;
            caminhoImagem = sistema.WebRootPath;
        }

        // GET: Adocao
        public async Task<IActionResult> CentralDeAdocoes(string buscaPet)
        {
            var contexto = _context.AdocaoModel.Include(a => a.Usuario).OrderByDescending(a => a.Data);

            if (!String.IsNullOrWhiteSpace(buscaPet))
            {
                contexto = (IOrderedQueryable<AdocaoModel>)contexto.Where(b => b.Sexo.Contains(buscaPet) || b.Nome.Contains(buscaPet) || b.Porte.Contains(buscaPet) || b.Raca.Contains(buscaPet) || b.Idade.Contains(buscaPet) || b.Cor.Contains(buscaPet) || b.Cidade.Contains(buscaPet));
            }

            return View(await contexto.ToListAsync());
        }

        // GET: Adocao/Details/5
        public async Task<IActionResult> Detalhes(int? id)
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
        public IActionResult CadastrarAdocao()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarAdocao(AdocaoModel adocaoModel, IFormFile imagem)
        {

            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));


            string caminhoSalvarImg = caminhoImagem + "\\img\\adocao\\";
            string nomeImg = Guid.NewGuid() + "_" + imagem.FileName;

            if (!Directory.Exists(caminhoSalvarImg))
            {
                Directory.CreateDirectory(caminhoSalvarImg);
            }

            using (var stream = System.IO.File.Create(caminhoSalvarImg + nomeImg))
            {
                await imagem.CopyToAsync(stream);
            }

            adocaoModel.Imagem = nomeImg;
            adocaoModel.UsuarioId = userId;

            _context.Add(adocaoModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CentralDeAdocoes));
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

            return View(adocaoModel);
        }

        // POST: Adocao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdocaoModel adocaoModel)
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

            return RedirectToAction("Posts", "Usuario");
        }

        // GET: Adocao/Delete/5
        public async Task<IActionResult> Excluir(int? id)
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
        [HttpPost, ActionName("Excluir")]
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
            return RedirectToAction("Posts", "Usuario");
        }

        private bool AdocaoModelExists(int id)
        {
          return (_context.AdocaoModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
