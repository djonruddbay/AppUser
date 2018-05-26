using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "A User-Id is Required!")]
        [StringLength(20, ErrorMessage = "User Id Must be {2} to {1} Characters!", MinimumLength = 5)]
        [Display(Name = "User-Id", Prompt = "Enter A User-Id!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A Password is Required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Enter Your Password!")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The Confirmation Password Does Not Match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password", Prompt = "Re-Enter Password!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "An E-Mail Address is Required!")]
        [EmailAddress(ErrorMessage = "E-Mail Address is not Valid!")]
        [Display(Name = "E-Mail", Prompt = "Enter Your E-Mail Address!")]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "Confirmation E-Mail Must Match E-Mail!")]
        [EmailAddress(ErrorMessage = "Confirmation E-Mail Must Match E-Mail!")]
        [Display(Name = "Confirm E-Mail", Prompt = "Re-Enter E-Mail Address!")]
        public string ConfirmEmail { get; set; }

        [StringLength(14, ErrorMessage = "Please Enter a Valid Phone Number!", MinimumLength = 14)]
        [Display(Name = "Cell Phone", Prompt = "(999) 999-9999")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Security Color is Required!")]
        [StringLength(10)]
        public string SecurityColorAnswer { get; set; }

        [Required(ErrorMessage = "Security Symbol is Required!")]
        [StringLength(10)]
        public string SecuritySymbolAnswer { get; set; }

    }
}
