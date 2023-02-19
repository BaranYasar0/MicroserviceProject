namespace FreeCourse.Web.Models.ViewModels.Order
{
    public class OrderStatusViewModel
    {
        public int OrderId { get; set; }
        public string Error { get; set; }
        public bool IsSuccesful { get; set; }
    }
}
