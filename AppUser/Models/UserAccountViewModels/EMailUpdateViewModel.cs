using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class EMailUpdateViewModel
    {
        [Required(ErrorMessage = "An E-Mail Address is Required!")]
        [EmailAddress(ErrorMessage = "E-Mail Address is not a Valid Address!")]
        [Display(Name = "E-Mail Address", Prompt = "Enter Your E-Mail Address Here!")]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "Confirmation E-Mail Must Match E-Mail!")]
        [EmailAddress(ErrorMessage = "Confirmation E-Mail Must Match E-Mail!")]
        [Display(Name = "Confirm E-Mail", Prompt = "Re-Enter Your E-Mail Address Here!")]
        public string ConfirmEmail { get; set; }
    }
}
