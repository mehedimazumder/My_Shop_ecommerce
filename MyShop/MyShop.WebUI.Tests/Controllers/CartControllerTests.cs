using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CanAddCartItem()
        {
            //SetUp
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product>products = new MockContext<Product>();
            IRepository<Order>Orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(Orders);
            var controller = new CartController(cartService, orderService, customers);
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            //Act

            //cartService.AddToCart(httpContext, "1");
            controller.AddToCart("1");
            Cart cart = carts.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual("1", cart.CartItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetCartSummaryViewModel()
        {
            IRepository<Cart> carts = new MockContext<Cart>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> Orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();


            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            Cart cart = new Cart();
            cart.CartItems.Add(new CartItem() { ProductId = "1", Quantity = 2 });
            cart.CartItems.Add(new CartItem() { ProductId = "2", Quantity = 1 });
            carts.Insert(cart);

            ICartService cartService = new CartService(products, carts);
            IOrderService orderService = new OrderService(Orders);
            var controller = new CartController(cartService, orderService, customers);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommercecart") { Value = cart.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);


            var result = controller.CartSummary() as PartialViewResult;
            var cartSummary = (CartSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(3, cartSummary.CartCount);
            Assert.AreEqual(25.00m, cartSummary.CartTotal);
        }

        [TestMethod]
        public void CanCheckOutAndCreateOrder()
        {
            IRepository<Customer> customers = new MockContext<Customer>();
            IRepository<Product> products = new MockContext<Product>();
            products.Insert(new Product(){ Id = "1", Price = 10.00m});
            products.Insert(new Product(){ Id = "2", Price = 2.00m});

            IRepository<Cart> carts = new MockContext<Cart>();
            Cart cart =new Cart();
            cart.CartItems.Add(new CartItem(){ ProductId = "1", Quantity = 2, CartId = cart.Id});
            cart.CartItems.Add(new CartItem(){ ProductId = "2", Quantity = 1, CartId = cart.Id});

            carts.Insert(cart);

            ICartService cartService = new CartService(products, carts);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            customers.Insert(new Customer(){ Id = "1", Email = "m@m.com", ZipCode = "123"});

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("m@m.com", "forms"), null);

            var controller = new CartController(cartService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.User = FakeUser;
            httpContext.Request.Cookies.Add(new HttpCookie("eCommercecart")
            {
                Value = cart.Id
            });

            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(),controller );

            //Act
            Order order = new Order();
            controller.Checkout(order);

            //Assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, cart.CartItems.Count);

            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);
        }
    }
}
    