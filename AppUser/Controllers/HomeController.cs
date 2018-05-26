using AppUser.Data;
using AppUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AppUser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ProfilesModel Profile { get; set; }

        public async Task<IActionResult> Index()
        {
            var UserId = _userManager.GetUserId(User);

            if (UserId == null)
            {
                return View();
            }

            var Profile = await _context.Profiles
                  .SingleOrDefaultAsync(m => m.UserId == UserId);

            if (Profile == null)
            {
                return View();
            }

            return RedirectToAction(nameof(AppUserHome), new { profileId = Profile.Id });

        }

        public async Task<IActionResult> AppUserHome(int? profileId)
        {
            if (profileId == null)
            {
                return NotFound();
            }

            var Profile = await _context.Profiles
                .Include(m => m.WebSitesMenu)
                    .ThenInclude(s => s.WebSites)
                .SingleOrDefaultAsync(m => m.Id == profileId);

            TempData["ProfileImage"] = "\\AppUserData\\" + Profile.AccountNumber + "\\ProfileImage_" + Profile.ProfileImage;

            return View(Profile);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
