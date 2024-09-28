using System.Security.Claims;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpace.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InkSpaceWeb.ViewComponents;

public class ShoppingCartViewComponent(IUnitOfWork unitOfWork) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() 
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null) {
            if (HttpContext.Session.GetInt32(SD.SessionCart) == null) {
                HttpContext.Session.SetInt32(SD.SessionCart,
                    unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }
                
            return View(HttpContext.Session.GetInt32(SD.SessionCart));
        }
        else {
            HttpContext.Session.Clear();
            return View(0);
        }
    }
    
}