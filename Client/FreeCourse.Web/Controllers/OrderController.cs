using FreeCourse.Web.Models.Inputs.Order;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();

            ViewBag.basket = basket;
            return View(new CheckOutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutInfoInput checkOutInfoInput)
        {
            //1. yol senkron iletişim
              //var orderStatus = await _orderService.CreateOrder(checkOutInfoInput);
           
            // 2.yol asenkron iletişim
            var orderSuspend = await _orderService.SuspendOrder(checkOutInfoInput);
            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();

                ViewBag.basket = basket;

                ViewBag.error = orderSuspend.Error;

                return View();
            }
            //1. yol senkron iletişim
            //return RedirectToAction(nameof(SuccessfulCheckout), new { orderıd = orderStatus.OrderId });

            //2.yol asenkron iletişim
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            //ViewBag.orderId = orderId;
            TempData["orderId"] = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}

