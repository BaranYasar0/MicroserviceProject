using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Inputs.Order;
using FreeCourse.Web.Models.Inputs.Payment;
using FreeCourse.Web.Models.ViewModels.Order;
using FreeCourse.Web.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderStatusViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput)
        {
            var basket = await _basketService.Get();
            var payment = new PaymentInfoInput
            {
                CardName = checkOutInfoInput.CardName,
                CardNumber = checkOutInfoInput.CardNumber,
                Expiration = checkOutInfoInput.Expiration,
                CVV = checkOutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.ReceivePayment(payment);
            //if (!responsePayment)
            //    return new OrderStatusViewModel() { Error = "Ödeme alınmadı", IsSuccesful = false };

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput { District = checkOutInfoInput.District, Line = checkOutInfoInput.Line, ZipCode = checkOutInfoInput.ZipCode, Province = checkOutInfoInput.Province, Street = checkOutInfoInput.Street }
            };

            basket.BasketItems.ForEach(x =>
            {
                OrderItemCreateInput orderItemCreateInput = new() { ProductId = x.CourseId, PictureUrl = "a", ProductName = x.CourseName ,Price=x.GetCurrentPrice};
                orderCreateInput.OrderItems.Add(orderItemCreateInput);
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

            if (!response.IsSuccessStatusCode)
                return new OrderStatusViewModel() { Error = "Sipariş oluşturulamadı", IsSuccesful = false };
            var orderCreatedViewModel =await response.Content.ReadFromJsonAsync<Response<OrderStatusViewModel>>();
            orderCreatedViewModel.Data.IsSuccesful = true;
            _basketService.Delete();
                return orderCreatedViewModel.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response.Data;
        }

        public Task SuspendOrder(CheckOutInfoInput checkOutInfoInput)
        {
            throw new NotImplementedException();
        }
    }
}
