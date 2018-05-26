using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "User-Id is Required!")]
        [Display(Name = "User Id", Prompt = "Enter Your User Id!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        [Display(Name = "Password", Prompt = "Enter Your Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
