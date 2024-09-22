using System.Diagnostics;
using InkSpace.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using InkSpaceWeb.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace InkSpaceWeb.Controllers;

[Area("Customer")]
public class HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
{



    public IActionResult Index() {
        IEnumerable<Product> products = unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(products);
    }
    public IActionResult Details(int productId) {
        Product product = unitOfWork.Product.Get(item=>item.Id==productId,includeProperties: "Category");
        return View(product);
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}