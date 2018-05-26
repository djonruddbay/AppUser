using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppUser.Models.ProfileViewModels
{
    public class ProfileCreateViewModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Profile Image Required!")]
        public IFormFile UserImage { set; get; }

        [Required(ErrorMessage = "First Name Required!")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9-()~,.'\s]*$", ErrorMessage = "Please enter a valid First Name!")]
        [Display(Name = "First Name", Prompt = "First Name!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required!")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9-()~,.'\s]*$", ErrorMessage = "Please enter a valid Last Name!")]
        [Display(Name = "Last Name", Prompt = "Last Name!")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Address Line 1", Prompt = "Address Line 1!")]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        [Display(Name = "Address Line 2", Prompt = "Address Line 2!")]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        [Display(Name = "City", Prompt = "City Name!")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [RegularExpression("\\d{5}(-\\d{4})?", ErrorMessage = "Please Enter a Valid Zip Code!")]
        [Display(Name = "Zip Code", Prompt = "Zip Code!")]
        public string ZipCode { get; set; }
    }
}
