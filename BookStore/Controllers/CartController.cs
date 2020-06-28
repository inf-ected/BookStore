using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IOrderProcessor _orderProcessor;

        public CartController(IProductRepository productRepository, IOrderProcessor orderProcessor, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderProcessor = orderProcessor;
            _orderRepository = orderRepository;
        }

        // GET: Cart
        public ActionResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = HttpContext.Session["Cart"] as Cart,
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public RedirectToRouteResult AddToCart(Cart cart, Guid Id, string returnUrl)
        {
            Product product = _productRepository.Products.FirstOrDefault(x => x.Id == Id);
            cart = HttpContext.Session["Cart"] as Cart;
            if (cart == null)
                cart = new Cart();

            //cart = HttpContext.Session["Cart"] as Cart ?? new Cart();



            if (product != null)
            {
                cart.AddItem(product, 1);
                HttpContext.Session["Cart"] = cart;
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        [HttpPost]
        public RedirectToRouteResult RemoveFromCart(Cart cart, Guid Id, string returnUrl)
        {
            if (Id == null || Id == Guid.Empty)
            {
                ModelState.AddModelError("", "Product not found!");
                return RedirectToAction("Index", new { returnUrl });
            }

            cart = HttpContext.Session["Cart"] as Cart ?? new Cart();

            Product product = _productRepository.Products.FirstOrDefault(x => x.Id == Id);
            if (product != null)
            {
                cart.RemoveItem(product);
                HttpContext.Session["Cart"] = cart;

            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult CheckOut()
        {
            return View(new ShippingDetail() { OrderId = _orderRepository.GetLastOrder() });
        }
        [HttpPost]
        public ViewResult CheckOut(Cart cart, ShippingDetail shippingDetail)
        {
            cart = HttpContext.Session["Cart"] as Cart;
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            if (ModelState.IsValid)
            {
                //save cart зачем ??? унас даже дбСета нет для него 
                var order = new Order {
                    Id = _orderRepository.GetLastOrder(),
                    Cart=cart,
                    CartId = cart.Id,
                    Date = DateTime.UtcNow
                };
                if (User.Identity.IsAuthenticated)
                { 
                    // order.ClientId = Convert.ToInt32(User.Identity.GetUserId()); // но так это ИД юзера а не клиента ?
                    // order.Client = System.Web.HttpContext.Current.User.Identity.Name;  // как получить инстанс залогиненого юзера ? 
                }
                _orderRepository.SaveOrder(order);
                _orderProcessor.ProcessOrder(cart, shippingDetail);
                cart.Clear();
                HttpContext.Session["Cart"] = null;
                return View("Complete");
            }
            else
            {
                return View(shippingDetail);
            }
        }
    }
}