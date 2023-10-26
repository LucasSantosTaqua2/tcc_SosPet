using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOSPets.Data;
using SOSPets.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;

namespace SOSPets.Controllers
{
    public class HomeController : Controller
    {
        private readonly Contexto _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Contexto context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            List<AdocaoModel> adocaoLista = _context.AdocaoModel.Include(a => a.Usuario).OrderByDescending(a => a.Data).Take(4).ToList();
            List<DesaparecidosModel> desaLista = _context.DesaparecidosModel.Include(a => a.Usuario).OrderByDescending(a => a.Data).Take(4).ToList();
            List<EncontradosModel> encoLista = _context.EncontradosModels.Include(a => a.Usuario).OrderByDescending(a => a.Data).Take(4).ToList();

            homeViewModel.listAdocao = adocaoLista;
            homeViewModel.listEncontrados = encoLista;
            homeViewModel.listDesaparecidos = desaLista;


            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}