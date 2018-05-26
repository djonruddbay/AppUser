using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppUser.Models
{
    public class ProfilesModel
    {
        public int Id { get; set; }

        [StringLength(15)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Create Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreate { get; set; }

        [Display(Name = "Last Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateLastUpdate { get; set; }

        [Required(ErrorMessage = "First Name Required!")]
        [StringLength(50)]
        [Display(Name = "First Name", Prompt = "First Name!")]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9-()~,.'\s]*$", ErrorMessage = "Please enter a valid First Name!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required!")]
        [StringLength(50)]
        [Display(Name = "Last Name", Prompt = "Last Name!")]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9-()~,.'\s]*$", ErrorMessage = "Please enter a valid Last Name!")]
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

        [StringLength(2)]
        [Display(Name = "State")]
        public string State { get; set; }

        [StringLength(10)]
        [RegularExpression("\\d{5}(-\\d{4})?", ErrorMessage = "Please Enter a Valid Zip Code!")]
        [Display(Name = "Zip Code", Prompt = "99999-9999")]
        public string ZipCode { get; set; }

        [StringLength(10)]
        public string ProfileImage { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [Display(Name = "User Id")]
        public ApplicationUser ApplicationUser { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<WebSitesMenuModel> WebSitesMenu { get; set; }
    }
}
