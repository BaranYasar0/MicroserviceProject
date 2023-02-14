using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SignInInput
    {
        [Display(Name ="E-mail adresiniz.")]
        public string Email { get; set; }
        [Display(Name ="Şifreniz.")]
        public string Password { get; set; }
        [Display(Name = "Beni hatırla.")]
        public bool IsRemember { get; set; }
    }
}
