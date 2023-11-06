using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOSPets.Data;
using SOSPets.Models;
using SOSPets.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;

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
        public async Task<IActionResult> Login(string email, string password, int id, UsuarioModel usuarioModel)
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
                    if( usuarioLogado.Is_Admin == true)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "ADM"));
                    }
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); 

            return RedirectToAction("Index", "Home");
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("ADM"))
            {
              return _context.UsuarioModels != null ? 
                          View(await _context.UsuarioModels.ToListAsync()) :
                          Problem("Entity set 'Contexto.UsuarioModels'  is null.");

            } else
            {
                return RedirectToAction("ErroView", "Erro");
            }
        }

        // GET: Usuario/Details/5
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null || _context.UsuarioModels == null)
            {
                return RedirectToAction("ErroView", "Erro");
            }

            var usuarioModel = await _context.UsuarioModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioModel == null)
            {
                return RedirectToAction("ErroView", "Erro");
            }

            if (id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid)))
            {
                HomeViewModel homeViewModel = new HomeViewModel();
                int usuario = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));
                int usuarioIdAdocao = Convert.ToInt32(homeViewModel.adocao.UsuarioId);
                int usuarioIdDesa = Convert.ToInt32(homeViewModel.desaparecidos.UsuarioId);
                int usuarioIdEncon = Convert.ToInt32(homeViewModel.encontrados.UsuarioId);

                if (usuarioIdAdocao == usuario)
                {
                    List<AdocaoModel> adocaoLista = _context.AdocaoModel.Include(a => a.Usuario).OrderByDescending(a => a.Data).ToList();
                    homeViewModel.listAdocao = adocaoLista;
                }
                if(usuarioIdDesa == usuario)
                {
                    List<DesaparecidosModel> desaLista = _context.DesaparecidosModel.Include(a => a.Usuario).OrderByDescending(a => a.Data).Take(4).ToList();
                    homeViewModel.listDesaparecidos = desaLista;
                }
                if(usuarioIdEncon == usuario)
                {
                    List<EncontradosModel> encoLista = _context.EncontradosModels.Include(a => a.Usuario).OrderByDescending(a => a.Data).Take(4).ToList();
                    homeViewModel.listEncontrados = encoLista;
                }


                return View(usuarioModel);
            } else
            {
                return RedirectToAction("ErroView", "Erro");  
            }
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
            
            if (_context.UsuarioModels.Where(w => w.Email == usuarioModel.Email).FirstOrDefault() == null)
            {
                usuarioModel.SetSenhaHash();
                _context.Add(usuarioModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            } else
            {
                ModelState.AddModelError("", "E-mail já cadastrado");
                return View();
            }
            /*return View(usuarioModel); */
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
        public IActionResult Edit(int id, UsuarioModel usuarioModel)
        {
            if (id != usuarioModel.Id)
            {
                return RedirectToAction("ErroView", "Erro");
            }

            _context.Update(usuarioModel);
            _context.SaveChanges();
            return View("Detalhes");

            /*if (ModelState.IsValid)
            {
                try
                {
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
            }*/
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
