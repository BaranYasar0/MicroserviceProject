using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Inputs.Order
{
    public class CheckOutInfoInput
    {
        [Display(Name = "İl")]
        public string? Province { get; set; }
        [Display(Name = "İlçe")]

        public string? District { get; set; }
        [Display(Name = "Cadde")]

        public string? Street { get; set; }
        [Display(Name = "Posta kodu")]

        public string? ZipCode { get; set; }
        [Display(Name = "Adres")]

        public string? Line { get; set; }

        [Display(Name = "Kart ismi")]
        public string CardName { get; set; }
        [Display(Name = "Kart numarası")]
        public string CardNumber { get; set; }
        [Display(Name = "Süre")]
        public string Expiration { get; set; }
        [Display(Name = "CVV")]
        public string CVV { get; set; }

    }
}
