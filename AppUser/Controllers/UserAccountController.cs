using AppUser.Data;
using AppUser.Models;
using AppUser.Models.UserAccountViewModels;
using AppUser.Services;
using AppUser.Views.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace AppUser.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public UserAccountController(
             ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            @TempData["SecurityColorSelection"] = "";
            @TempData["SecuritySymbolSelection"] = "";
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            @TempData["SecurityColorSelection"] = model.SecurityColorAnswer;
            @TempData["SecuritySymbolSelection"] = model.SecuritySymbolAnswer;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    SecurityColorAnswer = model.SecurityColorAnswer,
                    SecuritySymbolAnswer = model.SecuritySymbolAnswer
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.AppUser);

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //  User.IsInRole(UserRoles.??????) doesn't work here
                    //  apparently "User" isn't loaded at this time

                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (_userManager.GetRolesAsync(user).Result.ToList().Contains("AppUser"))
                    {
                        _logger.LogInformation("User logged in.");
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction(nameof(LogInNotAllowed));
                    }
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordRecovery()
        {
            @TempData["VerifyUser"] = "No";
            @TempData["FirstPass"] = "Yes";
            @TempData["SecurityColorSelection"] = "";
            @TempData["SecuritySymbolSelection"] = "";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordRecovery(string firstPass, PasswordRecoveryViewModel model)
        {
            @TempData["VerifyUser"] = "No";
            @TempData["FirstPass"] = firstPass;

            if (String.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("model.UserName", $"User_Id Must Be Entered...Retry!");
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User-Id Not On File...Retry!");
                return View(model);
            }

            if (user.LockoutEnd > DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "User-Id Locked...Retry Later!");
                return View(model);
            }

            if (!_userManager.GetRolesAsync(user).Result.ToList().Contains("AppUser"))
            {
                ModelState.AddModelError(string.Empty, "User-Id Not Allowed...Retry!");
                return View(model);
            }

            TempData["VerifyUser"] = "Yes";

            if (firstPass == "Yes")
            {
                TempData["FirstPass"] = "No";
                return View(model);
            }

            var errcnt = 0;

            if (String.IsNullOrEmpty(model.SecurityColorAnswer))
            {
                ModelState.AddModelError("model.SecurityColorAnswer", $"Security Color Must Be Selected!");
                errcnt += 1;
            }
            else if(model.SecurityColorAnswer != user.SecurityColorAnswer)
                {
                ModelState.AddModelError("model.SecurityColorAnswer", $"Color Does Not Match Color On File!");
                errcnt += 1;
            }

            if (String.IsNullOrEmpty(model.SecuritySymbolAnswer))
            {
                ModelState.AddModelError("model.SecuritySymbolAnswer", $"Security Symbol Must Be Selected!");
                errcnt += 1;
            }
            else if (model.SecuritySymbolAnswer != user.SecuritySymbolAnswer)
            {
                ModelState.AddModelError("model.SecuritySymbolAnswer", $"Symbol Does Not Match Symbol On File!");
                errcnt += 1;
            }

            @TempData["SecurityColorSelection"] = model.SecurityColorAnswer;
            @TempData["SecuritySymbolSelection"] = model.SecuritySymbolAnswer;

            if (errcnt > 0)
            {
                return View(model);
            }

            var pwCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            return RedirectToAction(nameof(PasswordReset), new {id = user.Id, pwCode, });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordReset(string id, string pwCode = null)
        {
            if (id == null)
            { 
                return NotFound();
            }
            if (pwCode == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            TempData["UserId"] = id;
            TempData["PasswordResetStatus"] = "Incomplete";
            var model = new PasswordResetViewModel { Code = pwCode };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(string id, PasswordResetViewModel model)
        {
            TempData["PasswordResetStatus"] = "Incomplete";
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["PasswordResetStatus"] = "Complete";
            }
            else
            {
               AddErrors(result);
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogInNotAllowed()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> PasswordChange()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {

                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AppUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel model)
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

            if (model.OldPassword == model.NewPassword)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Passwords Equal ~ No Update!" });
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            @ViewData["StatusMessage"] = "Password Successfully Updated";

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Password Successfully Updated!" });
        }

        [HttpGet]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> SecuritySelectionsUpdate()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            @TempData["SecurityColorSelection"] = "";
            @TempData["SecuritySymbolSelection"] = "";

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AppUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SecuritySelectionsUpdate(SecuritySelectionsUpdateViewModel model)
        {
            @TempData["SecurityColorSelection"] = model.SecurityColorAnswer;
            @TempData["SecuritySymbolSelection"] = model.SecuritySymbolAnswer;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (model.SecurityColorAnswer == user.SecurityColorAnswer &&
                model.SecuritySymbolAnswer == user.SecuritySymbolAnswer)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Security Selections Unchanged!" });
            }

            ApplicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == user.Id);

            ApplicationUser.SecurityColorAnswer = model.SecurityColorAnswer;
            ApplicationUser.SecuritySymbolAnswer = model.SecuritySymbolAnswer;

            _context.Attach(ApplicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(ApplicationUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("User changed their security selections successfully.");
            @ViewData["StatusMessage"] = "Security Selections Successfully Updated";

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Security Selections Successfully Updated!" });
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }


        [HttpGet]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> EMailUpdate()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AppUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EMailUpdate(EMailUpdateViewModel model)
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

            if (model.Email == user.Email)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "E-Mails Equal ~ No Change!" });
            }

            var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
            if (!setEmailResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }

            _logger.LogInformation("User changed their email successfully.");

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "E-Mail Successfully Updated!" });
        }

        [HttpGet]
        [Authorize(Roles = "AppUser")]
        public async Task<IActionResult> PhoneUpdate()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AppUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PhoneUpdate(PhoneUpdateViewModel model)
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

            if (model.PhoneNumber == user.PhoneNumber)
            {
                return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Phone Numbers Equal ~ No Change!" });
            }

            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred setting phone for user with ID '{user.Id}'.");
            }

            _logger.LogInformation("User changed their phone successfully.");

            return RedirectToAction(nameof(ProfileController.ProfileEditView), "Profile", new { statusMessage = "Phone Successfully Updated!" });
        }

        protected override void Dispose(bool disposing)
        { if (disposing) { _context.Dispose(); } }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
