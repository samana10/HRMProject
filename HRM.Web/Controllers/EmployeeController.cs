using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
