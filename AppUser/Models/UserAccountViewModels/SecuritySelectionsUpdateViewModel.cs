using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class SecuritySelectionsUpdateViewModel
    {
        [Required(ErrorMessage = "Security Color is Required!")]
        [StringLength(10)]
        public string SecurityColorAnswer { get; set; }

        [Required(ErrorMessage = "Security Symbol is Required!")]
        [StringLength(10)]
        public string SecuritySymbolAnswer { get; set; }
    }
}
