using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "New Password is Required!")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password", Prompt = "Enter Your New Password Here!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password Confirmation is Required!")]
        [Compare("NewPassword", ErrorMessage = "The Confirmation Password Does Not Match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password", Prompt = "Re-Enter Your Password Here!")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
