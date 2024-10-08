using System.Security.Claims;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpace.Models.ViewModels;
using InkSpace.Utility;
using InkSpaceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;


namespace InkSpaceWeb.Controllers;

[Area("customer")]
[Authorize]
public class CartController(IUnitOfWork unitOfWork) : Controller
{
    [BindProperty]
    public ShoppingCartVM ShoppingCartVM { get; set; }
    
    
    // GET
    public IActionResult Index() {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        ShoppingCartVM = new() {
            ShoppingCartList = unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
            OrderHeader = new()
        };
        
        foreach(var cart in ShoppingCartVM.ShoppingCartList) {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        
        return View(ShoppingCartVM);
    }
    
    public IActionResult Summary() {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM = new() {
            ShoppingCartList = unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
            OrderHeader = new()
        };
        ShoppingCartVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.Get(u => u.Id == userId);
        ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
        ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
        ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
        ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
        ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
        ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
        foreach (var cart in ShoppingCartVM.ShoppingCartList) {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        return View(ShoppingCartVM);
    }
    
    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPOST() {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM.ShoppingCartList = unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
            includeProperties: "Product");
        
        ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
        ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
        ApplicationUser applicationUser = unitOfWork.ApplicationUser.Get(u => u.Id == userId);
       
        foreach (var cart in ShoppingCartVM.ShoppingCartList) {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        if (applicationUser.CompanyId.GetValueOrDefault() == 0) {
            // regular customer account 
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
        }
        else {
            // company user - Approved For Delayed Payment
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
        }
        unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        unitOfWork.Save();
        foreach(var cart in ShoppingCartVM.ShoppingCartList) {
            OrderDetail orderDetail = new() {
                ProductId = cart.ProductId,
                OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };
            unitOfWork.OrderDetail.Add(orderDetail);
            unitOfWork.Save();
        }
        
        if (applicationUser.CompanyId.GetValueOrDefault() == 0) {
            // regular customer account and  need to capture payment
            //Stripe Logic
            var domain = "https://localhost:7299/";
            var options = new SessionCreateOptions {
                SuccessUrl = domain+ $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain+"customer/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach(var item in ShoppingCartVM.ShoppingCartList) {
                var sessionLineItem = new SessionLineItemOptions {
                    PriceData = new SessionLineItemPriceDataOptions {
                        UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            
            
        }
        return RedirectToAction(nameof(OrderConfirmation),new { id=ShoppingCartVM.OrderHeader.Id });
    }
    
    public IActionResult OrderConfirmation(int id) {
        OrderHeader orderHeader = unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
        if(orderHeader.PaymentStatus!= SD.PaymentStatusDelayedPayment) {
            //this is an order by customer
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid") {
                unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                unitOfWork.Save();
            }
            HttpContext.Session.Clear();
        }
        List<ShoppingCart> shoppingCarts = unitOfWork.ShoppingCart
            .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
        unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
        unitOfWork.Save();
        return View(id);
    }
    
    
    public IActionResult Plus(int cartId) {
        var cartFromDb = unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
        cartFromDb.Count += 1;
        unitOfWork.ShoppingCart.Update(cartFromDb);
        unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Minus(int cartId) {
        var cartFromDb = unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);
        if (cartFromDb.Count <= 1) {
            //remove that from cart
            HttpContext.Session.SetInt32(SD.SessionCart, unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            unitOfWork.ShoppingCart.Remove(cartFromDb);
        }
        else {
            cartFromDb.Count -= 1;
            unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Remove(int cartId) {
        var cartFromDb = unitOfWork.ShoppingCart.Get(u => u.Id == cartId,tracked:true);
        HttpContext.Session.SetInt32(SD.SessionCart, unitOfWork.ShoppingCart
            .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
        unitOfWork.ShoppingCart.Remove(cartFromDb);
        unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }
    
    
    
    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart) {
        if (shoppingCart.Count <= 50) {
            return shoppingCart.Product.Price;
        }
        
        if (shoppingCart.Count <= 100) {
            return shoppingCart.Product.Price50;
        }
        else {
            return shoppingCart.Product.Price100;
        }
        
    }
}