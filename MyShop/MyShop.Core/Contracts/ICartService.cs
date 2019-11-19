using System.Collections.Generic;
using System.Web;
using MyShop.Core.ViewModels;

namespace MyShop.Core.Contracts
{
    public interface ICartService
    {
        void AddToCart(HttpContextBase httpContext, string productId);
        void RemoveFromCart(HttpContextBase httpContext, string itemId);
        List<CartItemViewModel> GetCartItems(HttpContextBase httpContext);
        CartSummaryViewModel GetCartSummary(HttpContextBase httpContext);
        void ClearCart(HttpContextBase httpContext);
    }
}