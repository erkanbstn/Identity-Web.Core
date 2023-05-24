using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUI.Core.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Policy = "Permission.Order.Read")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
