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
    public class DesaparecidosController : Controller
    {
        private readonly Contexto _context;
        private string caminhoImagem;

        public DesaparecidosController(Contexto context, IWebHostEnvironment sistema)
        {
            _context = context;
            caminhoImagem = sistema.WebRootPath;
        }

        // GET: Desaparecidos
        public async Task<IActionResult> CentralDesaparecidos(string buscaPet)
        {
            var contexto = _context.DesaparecidosModel.Include(a => a.Usuario).OrderByDescending(a => a.Data);

            if (!String.IsNullOrWhiteSpace(buscaPet))
            {
                contexto = (IOrderedQueryable<DesaparecidosModel>)contexto.Where(b => b.Cidade.Contains(buscaPet) || b.Descricao.Contains(buscaPet));
            }

            return View(await contexto.ToListAsync());
        }

        // GET: Desaparecidos/Details/5
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null || _context.DesaparecidosModel == null)
            {
                return NotFound();
            }

            var desaparecidosModel = await _context.DesaparecidosModel
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desaparecidosModel == null)
            {
                return NotFound();
            }

            return View(desaparecidosModel);
        }

        // GET: Desaparecidos/Create
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public IActionResult CadastrarDesaparecido()
        {
            return View();
        }

        // POST: Desaparecidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarDesaparecido(DesaparecidosModel desaparecidosModel, IFormFile imagem)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));


            string caminhoSalvarImg = caminhoImagem + "\\img\\desaparecidos\\";
            string nomeImg = Guid.NewGuid() + "_" + imagem.FileName;

            if (!Directory.Exists(caminhoSalvarImg))
            {
                Directory.CreateDirectory(caminhoSalvarImg);
            }

            using (var stream = System.IO.File.Create(caminhoSalvarImg + nomeImg))
            {
                await imagem.CopyToAsync(stream);
            }

            desaparecidosModel.Imagem = nomeImg;
            desaparecidosModel.UsuarioId = userId;

            _context.Add(desaparecidosModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CentralDesaparecidos));
        }

        // GET: Desaparecidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DesaparecidosModel == null)
            {
                return NotFound();
            }

            var desaparecidosModel = await _context.DesaparecidosModel.FindAsync(id);
            if (desaparecidosModel == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Email", desaparecidosModel.UsuarioId);
            return View(desaparecidosModel);
        }

        // POST: Desaparecidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Cidade,Data,Imagem,UsuarioId")] DesaparecidosModel desaparecidosModel)
        {
            if (id != desaparecidosModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(desaparecidosModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesaparecidosModelExists(desaparecidosModel.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.UsuarioModels, "Id", "Email", desaparecidosModel.UsuarioId);
            return View(desaparecidosModel);
        }

        // GET: Desaparecidos/Delete/5
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.DesaparecidosModel == null)
            {
                return NotFound();
            }

            var desaparecidosModel = await _context.DesaparecidosModel
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desaparecidosModel == null)
            {
                return NotFound();
            }

            return View(desaparecidosModel);
        }

        // POST: Desaparecidos/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DesaparecidosModel == null)
            {
                return Problem("Entity set 'Contexto.DesaparecidosModel'  is null.");
            }
            var desaparecidosModel = await _context.DesaparecidosModel.FindAsync(id);
            if (desaparecidosModel != null)
            {
                _context.DesaparecidosModel.Remove(desaparecidosModel);
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


        // GET: Desaparecidos/Delete/5
        public async Task<IActionResult> Encontrado(int? id)
        {
            if (id == null || _context.DesaparecidosModel == null)
            {
                return NotFound();
            }

            var desaparecidosModel = await _context.DesaparecidosModel
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (desaparecidosModel == null)
            {
                return NotFound();
            }

            return View(desaparecidosModel);
        }

        // POST: Desaparecidos/Delete/5
        [HttpPost, ActionName("Encontrado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EncontradoConfirmed(int id)
        {
            if (_context.DesaparecidosModel == null)
            {
                return Problem("Entity set 'Contexto.DesaparecidosModel'  is null.");
            }
            var desaparecidosModel = await _context.DesaparecidosModel.FindAsync(id);
            if (desaparecidosModel != null)
            {
                _context.DesaparecidosModel.Remove(desaparecidosModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Posts", "Usuario");
        }

        private bool DesaparecidosModelExists(int id)
        {
          return (_context.DesaparecidosModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
