using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class CartController : Controller
    {
        ICartService cartService;
        IOrderService orderService;

        public CartController(ICartService CartService, IOrderService OrderService)
        {
            this.cartService = CartService;
            this.orderService = OrderService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var model = cartService.GetCartItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToCart(string id)
        {
            cartService.AddToCart(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(string id)
        {
            cartService.RemoveFromCart(this.HttpContext, id);

            return RedirectToAction("Index");
        }

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);
            return PartialView("_CartSummaryPartial", cartSummary);
        }

        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order Created";

            //process payment
            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, cartItems);
            cartService.ClearCart(this.HttpContext);

            return RedirectToAction("ThankYou", new {OrderId = order.Id});
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }

    }
}