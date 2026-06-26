using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
