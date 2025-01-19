using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.Db;
using System.Security.Claims;
using System.Security.Cryptography;

namespace OnlineShop.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly OnlineShopContext _context;
        public HomeController(OnlineShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user= _context.Users.FirstOrDefault(x => x.Id == userId);
            return View(user);
        }
    }
}
