using FreeCourse.Web.Models.ViewModels.Basket;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
        Task<BasketViewModel> GetBasket();
        Task<bool> DeleteBasket();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> DeleteBasketItem(string courseId);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();

    }
}
