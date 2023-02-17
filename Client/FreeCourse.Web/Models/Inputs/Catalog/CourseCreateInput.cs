using FreeCourse.Web.Models.ViewModels.Catalog;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Inputs.Catalog
{
    public class CourseCreateInput
    {
        [Display(Name ="Kurs İsmi")]
        [Required]
        public string Name { get; set; }
        
        [Display(Name = "Kurs Açıklaması")]
        [Required]
        public string Description { get; set; }
        
        [Display(Name = "Kurs Fiyatı")]
        [Required]
        public decimal Price { get; set; }
        public string? UserId { get; set; }
        public string? Picture { get; set; }
        public FeatureViewModel Feature { get; set; }
        
        [Display(Name = "Kurs Kategorisi")]
        [Required]
        public string CategoryId { get; set; }
    }
}
