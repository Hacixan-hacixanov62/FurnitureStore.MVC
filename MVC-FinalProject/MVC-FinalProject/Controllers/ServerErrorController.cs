using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class ServerErrorController : Controller
    {
        public IActionResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
