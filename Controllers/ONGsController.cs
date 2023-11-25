using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private string caminhoImagem;


        public ONGsController(Contexto context, IWebHostEnvironment sistema)
        {
            _context = context;
            caminhoImagem = sistema.WebRootPath;
        }

        // GET: ONGs
        public async Task<IActionResult> CentralONGs(string busca)
        {
            var contexto = _context.ONGsModels.OrderByDescending(a => a.Data);

            if (!String.IsNullOrWhiteSpace(busca))
            {
                contexto = (IOrderedQueryable<ONGsModel>)contexto.Where(b => b.Nome.Contains(busca) || b.Cidade.Contains(busca));
            }

            return View(await contexto.ToListAsync());
        }

        // GET: ONGs/Details/5
        public async Task<IActionResult> Detalhes(int? id)
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
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public IActionResult CadastrarONG()
        {
            if(User.IsInRole("ADM"))
            {
                return View();
            } else
            {
                return RedirectToAction("ErroView", "Erro");
            }
        }

        // POST: ONGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarONG(ONGsModel oNGsModel, IFormFile imagem)
        {
            if (User.IsInRole("ADM"))
            {
                string caminhoSalvarImg = caminhoImagem + "\\img\\ONGs\\";
                string nomeImg = Guid.NewGuid() + "_" + imagem.FileName;

                if (!Directory.Exists(caminhoSalvarImg))
                {
                    Directory.CreateDirectory(caminhoSalvarImg);
                }

                using (var stream = System.IO.File.Create(caminhoSalvarImg + nomeImg))
                {
                    await imagem.CopyToAsync(stream);
                }

                oNGsModel.Imagem = nomeImg;

                _context.Add(oNGsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CentralONGs));
            }
            else
            {
                return RedirectToAction("ErroView", "Erro");
            }

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
        public async Task<IActionResult> Edit(int id, ONGsModel oNGsModel, string nome, string desc, string end, string cidade, string tel, string email)
        {




            if (id != oNGsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oNGsModel.Id = id;
                    oNGsModel.Nome = nome;
                    oNGsModel.Descricao = desc;
                    oNGsModel.Endereco = end;
                    oNGsModel.Cidade = cidade;
                    oNGsModel.Tel = tel;
                    oNGsModel.Email = email;
                    oNGsModel.Imagem = oNGsModel.Imagem;
                    oNGsModel.Data = oNGsModel.Data;
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
                return View("CentralONGs");
            }
            return View("CentralONGs");
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
            return RedirectToAction("CentralONGs");
        }

        private bool ONGsModelExists(int id)
        {
          return (_context.ONGsModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
