using System.ComponentModel.DataAnnotations;

namespace AppUser.Models
{
    public class WebSitesModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is Required!")]
        [Display(Name = "Description", Prompt = "Enter Website Description Here!")]
        [StringLength(30)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Website is Required!")]
        [Display(Name = "Web Site", Prompt = "Enter Website Here!")]
        [Url(ErrorMessage = "Valid WebSite is Required!")]
        public string WebSiteURL { get; set; }

        [Required]
        public int WebSitesMenuId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public WebSitesMenuModel WebSitesMenu { get; set; }
    }
}
