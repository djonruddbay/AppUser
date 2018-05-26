using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.UserAccountViewModels
{
    public class PasswordRecoveryViewModel
    {
        [Display(Name = "Your User-Id", Prompt = "Enter Your User-Id!")]
        public string UserName { get; set; }

        [StringLength(10)]
        public string SecurityColorAnswer { get; set; }

        [StringLength(10)]
        public string SecuritySymbolAnswer { get; set; }
    }
}
