using InkSpace.DataAccess.Repository.IRepository;
using InkSpace.Models.ViewModels;
using InkSpace.Utility;
using InkSpaceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InkSpaceWeb.Controllers;


[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment) : Controller
{
    public IActionResult Index() {

        List<Product> products = unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

        return View(products);
    }

    public IActionResult Upsert(int? id) {
        
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
        if (id is null or 0) {
            //Create
             return View(productVm);
        }

        //update 
        productVm.Product = unitOfWork.Product.Get(item=>item.Id==id);
        return View(productVm);

    }
    
    [HttpPost]
    public IActionResult Upsert(ProductVM productVm,IFormFile? file) {
 
        if (ModelState.IsValid) {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (file is not null) {
                string fileName = Guid.NewGuid().ToString() +Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVm.Product.ImageUrl)) {
                    var oldImagePath = Path.Combine(wwwRootPath + productVm.Product.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath)) {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                    
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
                    file.CopyTo(fileStream);
                }
                productVm.Product.ImageUrl  = @"\images\product\" + fileName;
            }

            if (productVm.Product.Id is 0) {
                unitOfWork.Product.Add(productVm.Product);
            }
            else {
                unitOfWork.Product.Update(productVm.Product);
            }
            
            unitOfWork.Save();
            TempData["Success"] = "Product created successfully";
            return RedirectToAction("Index","Product");
        }
        else {
            //here we are Populating the category list in case the model is not valid , so the form will populate valid categories in the dropdown
            productVm.CategoryList = unitOfWork.Category.GetAll().Select(obj=>new SelectListItem
            {
                Text = obj.Name,
                Value = obj.Id.ToString()
            });
            return View(productVm);
        }
        

    }
    
    
   
    



    #region API Calls

    [HttpGet]
    public IActionResult GetAll() {
        List<Product> products = unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
        return Json(new {data = products});
    }
    

    [HttpDelete]
    public IActionResult Delete(int? id) {
        var productToBeDeleted = unitOfWork.Product.Get(item=>item.Id == id);
        if (productToBeDeleted==null) {
            return Json(new {success = false, message = "Error Deleting:Product not found"});
        }
        var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath + productToBeDeleted.ImageUrl);
        if (System.IO.File.Exists(oldImagePath)) {
            System.IO.File.Delete(oldImagePath);
        }
        unitOfWork.Product.Remove(productToBeDeleted);
        unitOfWork.Save();
        return Json(new {success = true, message = "Product deleted successfully"});
    }
    #endregion
}