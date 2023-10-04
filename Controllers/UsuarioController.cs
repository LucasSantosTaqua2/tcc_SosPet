using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOSPets.Data;
using SOSPets.Models;
using SOSPets.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SOSPets.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Contexto _context;

        public UsuarioController(Contexto context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, int id)
        {
            try
            {
                password = password.GerarHash();
                UsuarioModel usuarioLogado = _context.UsuarioModels.Where(a => a.Email == email && a.Password == password).FirstOrDefault();
                if (usuarioLogado == null)
                {
                    TempData["erro"] = "Login e senha invalidos";
                    return View();
                }
                else
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, usuarioLogado.Nome));
                    claims.Add(new Claim(ClaimTypes.Sid, usuarioLogado.Id.ToString()));


                    var userIdentity = new ClaimsIdentity(claims, "Acesso");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync("CookieAuthentication", principal, new AuthenticationProperties());

                    return RedirectToAction("Index", "Home");
                }
            }catch
            {
                return View();
            }

           
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
              return _context.UsuarioModels != null ? 
                          View(await _context.UsuarioModels.ToListAsync()) :
                          Problem("Entity set 'Contexto.UsuarioModels'  is null.");
        }

        // GET: Usuario/Details/5
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsuarioModels == null)
            {
                return NotFound();
            }

            var usuarioModel = await _context.UsuarioModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioModel == null)
            {
                return NotFound();
            }

            return View(usuarioModel);
        }

        // GET: Usuario/Create
        public IActionResult Cadastro()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro([Bind("Id,Nome,Email,Tel,Password")] UsuarioModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                usuarioModel.SetSenhaHash();
                _context.Add(usuarioModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(usuarioModel);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsuarioModels == null)
            {
                return NotFound();
            }

            var usuarioModel = await _context.UsuarioModels.FindAsync(id);
            if (usuarioModel == null)
            {
                return NotFound();
            }
            return View(usuarioModel);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Tel,Password")] UsuarioModel usuarioModel)
        {
            if (id != usuarioModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioModelExists(usuarioModel.Id))
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
            return View(usuarioModel);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _context.UsuarioModels.Remove(_context.UsuarioModels.Where(a => a.Id == id).FirstOrDefault());
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsuarioModels == null)
            {
                return Problem("Entity set 'Contexto.UsuarioModels'  is null.");
            }
            var usuarioModel = await _context.UsuarioModels.FindAsync(id);
            if (usuarioModel != null)
            {
                _context.UsuarioModels.Remove(usuarioModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioModelExists(int id)
        {
          return (_context.UsuarioModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
