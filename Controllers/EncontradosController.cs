using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOSPets.Data;
using SOSPets.Models;

namespace SOSPets.Controllers
{
    public class EncontradosController : Controller
    {
        private readonly Contexto _context;
        private string caminhoImagem;

        public EncontradosController(Contexto context, IWebHostEnvironment sistema)
        {
            caminhoImagem = sistema.WebRootPath;
            _context = context;
        }

        // GET: Encontrados
        public async Task<IActionResult> CentralEncontrados(string buscaPet)
        {
            var contexto = _context.EncontradosModels.Include(a => a.Usuario).OrderByDescending(a => a.Data);

            if (!String.IsNullOrWhiteSpace(buscaPet))
            {
                contexto = (IOrderedQueryable<EncontradosModel>)contexto.Where(b => b.Cidade.Contains(buscaPet) || b.Descricao.Contains(buscaPet));
            }

            return View(await contexto.ToListAsync());
        }

        // GET: Encontrados/Details/5
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null || _context.EncontradosModels == null)
            {
                return NotFound();
            }

            var encontradosModel = await _context.EncontradosModels
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encontradosModel == null)
            {
                return NotFound();
            }

            return View(encontradosModel);
        }

        // GET: Encontrados/Create
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public IActionResult CadastrarEncontrado()
        {
            return View();
        }

        // POST: Encontrados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarEncontrado(EncontradosModel encontradosModel, IFormFile imagem)
        {

            string caminhoSalvarImg = caminhoImagem + "\\img\\encontrados\\";
            string nomeImg = Guid.NewGuid() + "_" + imagem.FileName;
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));

            if (!Directory.Exists(caminhoSalvarImg))
            {
                Directory.CreateDirectory(caminhoSalvarImg);
            }

            using (var stream = System.IO.File.Create(caminhoSalvarImg + nomeImg))
            {
                await imagem.CopyToAsync(stream);
            }

            encontradosModel.Imagem = nomeImg;
            encontradosModel.UsuarioId = userId;

            _context.Add(encontradosModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CentralEncontrados));
        }

        // GET: Encontrados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EncontradosModels == null)
            {
                return NotFound();
            }

            var encontradosModel = await _context.EncontradosModels.FindAsync(id);
            if (encontradosModel == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Email", encontradosModel.UsuarioId);
            return View(encontradosModel);
        }

        // POST: Encontrados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Cidade,Data,Imagem,UsuarioId")] EncontradosModel encontradosModel)
        {
            if (id != encontradosModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encontradosModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncontradosModelExists(encontradosModel.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Email", encontradosModel.UsuarioId);
            return View(encontradosModel);
        }

        // GET: Encontrados/Delete/5
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.EncontradosModels == null)
            {
                return NotFound();
            }

            var encontradosModel = await _context.EncontradosModels
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encontradosModel == null)
            {
                return NotFound();
            }

            return View(encontradosModel);
        }

        // POST: Encontrados/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EncontradosModels == null)
            {
                return Problem("Entity set 'Contexto.EncontradosModels'  is null.");
            }
            var encontradosModel = await _context.EncontradosModels.FindAsync(id);
            if (encontradosModel != null)
            {
                _context.EncontradosModels.Remove(encontradosModel);
            }
            
            await _context.SaveChangesAsync();

            if (User.IsInRole("ADM"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Posts", "Usuario");
            }

        }

        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> Devolvido(int? id)
        {
            if (id == null || _context.EncontradosModels == null)
            {
                return NotFound();
            }

            var encontradosModel = await _context.EncontradosModels
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (encontradosModel == null)
            {
                return NotFound();
            }

            return View(encontradosModel);
        }

        // POST: Encontrados/Delete/5
        [HttpPost, ActionName("Devolvido")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DevolvidoConfirmed(int id)
        {
            if (_context.EncontradosModels == null)
            {
                return Problem("Entity set 'Contexto.EncontradosModels'  is null.");
            }
            var encontradosModel = await _context.EncontradosModels.FindAsync(id);
            if (encontradosModel != null)
            {
                _context.EncontradosModels.Remove(encontradosModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Posts", "Usuario");
        }

        private bool EncontradosModelExists(int id)
        {
          return (_context.EncontradosModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
