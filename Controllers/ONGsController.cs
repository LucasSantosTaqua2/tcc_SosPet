using Microsoft.AspNetCore.Mvc;

namespace SOSPets.Controllers
{
    public class ONGsController : Controller
    {
        public IActionResult ONGs()
        {
            return View();
        }
    }
}
