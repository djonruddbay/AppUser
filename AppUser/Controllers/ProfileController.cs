using AppUser.Controllers;
using AppUser.Data;
using AppUser.Models;
using AppUser.Models.ProfileViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppUser.Views.Profile
{
    [Authorize(Roles = "AppUser")]

    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProfileController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ManageController> logger,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            hostingEnvironment = environment;
        }
        [BindProperty]
        public ProfilesModel Profile { get; set; }
        [BindProperty]
        public IFormFile UserImage { set; get; }

        // GET: ProfilesModels/ProfileView
        [HttpGet]
        public async Task<IActionResult> ProfileEditView(string statusMessage, string smColor)
        {
            @TempData["StatusMessage"] = statusMessage;
            @TempData["StatusMessageHidden"] = String.IsNullOrEmpty(statusMessage) ? "hidden" : "";
            @TempData["SMBackGroundColor"] = smColor == "orange" ? smColor : "green";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var Profile = await _context.Profiles
                 .SingleOrDefaultAsync(m => m.UserId == user.Id);

            return View(Profile);
        }

        //POST: ProfilesModels/ProfileEditView
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEditView([Bind("FirstName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,RowVersion")] ProfilesModel profileViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var ProfileToUpdate = await _context.Profiles
                 .FirstOrDefaultAsync(m => m.UserId == user.Id);

            if (ProfileToUpdate == null)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile");
            }
            _context.Entry(ProfileToUpdate)
               .Property("RowVersion").OriginalValue = Profile.RowVersion;

            ProfileToUpdate.DateLastUpdate = DateTime.Now;
            if (await TryUpdateModelAsync(
                ProfileToUpdate, "",
                p => p.AccountNumber, p => p.FirstName, p => p.LastName,
                p => p.AddressLine1, p => p.AddressLine2, p => p.City, p => p.State, p => p.ZipCode,
                p => p.DateLastUpdate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Profile Successfully Updated!" });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (ProfilesModel)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile");
                    }

                    ModelState.Remove("Profile.RowVersion");
                }
            }

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "NOT Updated ~ Changed By Another", smColor = "orange" });
        }


        [HttpGet]
        public async Task<IActionResult> ProfileCreate()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View();
        }

        // POST: ProfilesModels/ProfileCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileCreate(ProfileCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Profile = new ProfilesModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                UserId = user.Id,
                ProfileImage = "000",
            };

            _context.Profiles.Add(Profile);
            await _context.SaveChangesAsync();

            Profile.AccountNumber = DateTime.Now.Year.ToString() + "-" + DateTime.Now.ToString("MM") + "-" +
                Profile.Id.ToString().PadLeft(7, '0').
                Substring(Profile.Id.ToString().PadLeft(7, '0').Length - 7);

            var userFolder =
                hostingEnvironment.WebRootPath + "\\AppUserData\\" + Profile.AccountNumber + "\\";

            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            if (this.UserImage != null)
            {
                var fileExt = Path.GetExtension(this.UserImage.FileName);
                var fileName = "ProfileImage_001" + fileExt;
                this.UserImage.CopyTo(new FileStream(userFolder + fileName, FileMode.Create));
                Profile.ProfileImage = "001" + fileExt;
            }

            Profile.DateCreate = DateTime.Now;
            Profile.DateLastUpdate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Profile Successfully Created!" });
        }

        // GET: ProfilesModels/ProfileImageEdit
        [HttpGet]
        public async Task<IActionResult> ProfileImageEdit(ProfileImageEditViewModel profileImageEditView)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var Profile = await _context.Profiles
                 .SingleOrDefaultAsync(m => m.UserId == user.Id);
            if (Profile == null)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile");
            }

            return View(profileImageEditView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileImageEdit(IFormFile NewUserImage)
        {
            if (NewUserImage == null)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Image Not Updated" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var Profile = await _context.Profiles
                 .SingleOrDefaultAsync(m => m.UserId == user.Id);
            if (Profile == null)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile");
            }

            int.TryParse(Profile.ProfileImage.Substring(0, 3), out int imageNumber);
            imageNumber = imageNumber + 1;

            if (imageNumber == 0 || imageNumber == 100)
            {
                imageNumber = 1;
            }
            var fileExt = Path.GetExtension(NewUserImage.FileName);
            Profile.ProfileImage = imageNumber.ToString("D3") + fileExt;
            var fileName = "ProfileImage_" + Profile.ProfileImage;
            var userFolder = hostingEnvironment.WebRootPath + "\\AppUserData\\" + Profile.AccountNumber + "\\";

            /*just a little housekeeping*/
            string[] profileImageList = Directory.GetFiles(userFolder, "ProfileImage*");
            foreach (string i in profileImageList)
            {
                try
                {
                    System.IO.File.Delete(i);
                }
                catch
                {
                    // do nothing...will delete later
                }
            }

            NewUserImage.CopyTo(new FileStream(userFolder + fileName, FileMode.Create));

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Profile Image Successfully Updated!" });
        }

        protected override void Dispose(bool disposing)
        { if (disposing) { _context.Dispose(); } }
    }
}
