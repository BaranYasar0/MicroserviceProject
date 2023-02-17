using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeCourse.Web.Models.ViewModels.Catalog
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs Süresi")]
        [Required]
        public int Duration { get; set; }
    }
}
