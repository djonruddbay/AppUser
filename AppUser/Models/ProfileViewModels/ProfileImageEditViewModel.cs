using Microsoft.AspNetCore.Http;

namespace AppUser.Models.ProfileViewModels
{
    public class ProfileImageEditViewModel
    {
        public IFormFile UserImage { set; get; }
    }
}
