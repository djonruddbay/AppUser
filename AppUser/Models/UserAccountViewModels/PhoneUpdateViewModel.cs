using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class PhoneUpdateViewModel
    {
        [StringLength(14, ErrorMessage = "Please Enter a Valid Phone Number!", MinimumLength = 14)]
        [Display(Name = "Cell Phone", Prompt = "(999) 999-9999")]
        public string PhoneNumber { get; set; }
    }
}
