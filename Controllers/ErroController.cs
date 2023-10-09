using Microsoft.AspNetCore.Mvc;

namespace SOSPets.Controllers
{
    public class ErroController : Controller
    {
        public IActionResult ErroView()
        {
            return View();
        }
    }
}
