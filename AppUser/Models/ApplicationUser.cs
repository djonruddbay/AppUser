using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AppUser.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string SecurityColorAnswer { get; set; }
        public string SecuritySymbolAnswer { get; set; }

        public ProfilesModel Profiles { get; set; }
    }
}
