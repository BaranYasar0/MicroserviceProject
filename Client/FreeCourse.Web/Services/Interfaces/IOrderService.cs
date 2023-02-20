using FreeCourse.Web.Models.Inputs.Order;
using FreeCourse.Web.Models.ViewModels.Order;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        //senkron iletişim-direk olarak microservisine istek yapılacak
        Task<OrderStatusViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput);

        //asenkron iletişim,sipariş bilgileri rabbitmq ya gönderilecek
        Task<OrderSuspendViewModel> SuspendOrder(CheckOutInfoInput checkOutInfoInput);

        Task<List<OrderViewModel>> GetOrder();

    }
}
