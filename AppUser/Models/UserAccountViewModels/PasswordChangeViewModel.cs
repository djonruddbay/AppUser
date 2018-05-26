using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Current Password is Required!")]
        [Display(Name = "Current Password", Prompt = "Enter Your Password Here!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is Required!")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password", Prompt = "Enter Your New Password Here!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password Confirmation is Required!")]
        [Compare("NewPassword", ErrorMessage = "The Confirmation Password Does Not Match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password", Prompt = "Re-Enter Your Password Here!")]
        public string ConfirmPassword { get; set; }

    }
}
