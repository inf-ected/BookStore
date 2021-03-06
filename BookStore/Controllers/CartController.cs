﻿using BookStore.Domain.Entities;
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
        private ICartRepository _cartRepository;
        public CartController(
            IProductRepository productRepository, 
            IOrderProcessor orderProcessor, 
            IOrderRepository orderRepository,
            ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _orderProcessor = orderProcessor;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
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
            { cart = new Cart();
              // _cartRepository.SaveCart(cart);
            }

            //cart = HttpContext.Session["Cart"] as Cart ?? new Cart();

            

            if (product != null)
            {
                _cartRepository.AddItem(cart, product, 1);
                //cart.AddItem(product, 1);
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
                _cartRepository.RemoveItem(cart, product);
                //cart.RemoveItem(product);
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

            if (cart.LinesCollection.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            if (ModelState.IsValid)
            {
                //save cart зачем ??? унас даже дбСета нет для него 
                var order = new Order
                {
                    Id = _orderRepository.GetLastOrder(),
                    Cart = cart,
                    CartId = cart.Id,
                    Date = DateTime.UtcNow
                };
                if (User.Identity.IsAuthenticated)
                {
                    Client client = new Client
                    {
                        ApplicationUserId = Convert.ToInt32(User.Identity.GetUserId()),
                        Mobile = "",
                        Name = System.Web.HttpContext.Current.User.Identity.Name
                    };
                    //client.Save();
                     order.ClientId = client.Id; // но так это ИД юзера а не клиента ?
                   
                }
                _orderRepository.SaveOrder(order);
                _orderProcessor.ProcessOrder(cart, shippingDetail);
                _cartRepository.Clear(cart);
                //cart.Clear();
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