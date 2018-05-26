using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppUser.Models
{
    public class WebSitesMenuModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Menu Item Sequence # is Required!")]
        [Range(1, 1000, ErrorMessage = "Sequence Number Can Not Be Zero!")]
        [Display(Name = "Sequence Number", Prompt = "Enter Menu Item Sequence # Here!")]
        public int SequenceNumber { get; set; }

        [Required(ErrorMessage = "Menu Item Description is Required!")]
        [Display(Name = "Description", Prompt = "Enter Menu Item Description Here!")]
        [StringLength(30)]
        public string Description { get; set; }

        [Required]
        public int ProfileId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ProfilesModel Profile { get; set; }
        public ICollection<WebSitesModel> WebSites { get; set; }
    }
}
