using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Index()
        {
            Response.StatusCode = 403;
            Response.StatusCode = 401;
            return View();
        }
    }
}
