using FreeCourse.Web.Models.ViewModels.Catalog;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Inputs.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Kurs açıklaması")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Kurs fiyatı")]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string? Picture { get; set; }
        [Required]
        [Display(Name = "Kurs süersi")]
        public FeatureViewModel Feature { get; set; }
        [Required]
        [Display(Name = "Kurs kategorisi")]
        public string CategoryId { get; set; }
        [Display(Name ="Kurs resim")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
