using InkSpace.DataAccess.Repository.IRepository;
using InkSpace.Models.ViewModels;
using InkSpaceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InkSpaceWeb.Controllers;


[Area("Admin")]
public class ProductController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index() {

        List<Product> products = unitOfWork.Product.GetAll().ToList();

        return View(products);
    }

    public IActionResult Create() {
        
        IEnumerable<SelectListItem> categoryList = unitOfWork.Category.GetAll().Select(obj=>new SelectListItem
        {
            Text = obj.Name,
            Value = obj.Id.ToString()
        });
        ProductVM productVm = new()
        {
            CategoryList = categoryList,
            Product = new Product()
        };
        return View(productVm);
    }
    
    [HttpPost]
    public IActionResult Create(ProductVM productVm) {
 
        if (ModelState.IsValid) {
            unitOfWork.Product.Add(productVm.Product);
            unitOfWork.Save();
            TempData["Success"] = "Product created successfully";
            return RedirectToAction("Index","Product");
        }
        else {
            //here we are Populating the category list in case the model is not valid , so the form will reload with valid categories in the dropdown
            productVm.CategoryList = unitOfWork.Category.GetAll().Select(obj=>new SelectListItem
            {
                Text = obj.Name,
                Value = obj.Id.ToString()
            });
            return View(productVm);
        }
        

    }
    
    
    public IActionResult Edit(int? id) {
        if (id is null or 0) {
            return NotFound();
        }
        Product? productFromDb = unitOfWork.Product.Get(item=>item.Id == id);
        

        if (productFromDb is null) {
            return NotFound();
        }
        
        return View(productFromDb);
    }
    
    [HttpPost]
    public IActionResult Edit(Product product) {
        
        if (ModelState.IsValid) {
            unitOfWork.Product.Update(product);
            unitOfWork.Save();
            TempData["Success"] = "Product updated successfully";
            return RedirectToAction("Index","Product");
        }

        return View();


    }
    
    public IActionResult Delete(int? id) {
        if (id is null or 0) {
            return NotFound();
        }
        Product? productFromDb = unitOfWork.Product.Get(item=>item.Id == id);

        if (productFromDb is null) {
            return NotFound();
        }
        
        return View(productFromDb);
    }
    
    [HttpPost,ActionName("Delete")]
    public IActionResult DeletePost(int? id) {
        
        Product? productFromDb = unitOfWork.Product.Get(item=>item.Id == id);
        if (productFromDb is null) {
            return NotFound();
        }
        unitOfWork.Product.Remove(productFromDb);
        unitOfWork.Save();
        TempData["Success"] = "Product deleted successfully";
        return RedirectToAction("Index","Product");


    }
}