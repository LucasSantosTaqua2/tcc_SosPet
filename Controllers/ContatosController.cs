using Microsoft.AspNetCore.Mvc;

namespace SOSPets.Controllers
{
    public class ContatosController : Controller
    {
        public IActionResult Contatos()
        {
            return View();
        }
    }
}
