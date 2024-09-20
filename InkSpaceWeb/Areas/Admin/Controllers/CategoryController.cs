using InkSpace.DataAccess.Repository;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using InkSpaceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace InkSpaceWeb.Controllers;
[Area("Admin")]
public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    // GET
    public IActionResult Index() {

        List<Category> categories = unitOfWork.Category.GetAll().ToList();
        return View(categories);
    }

    public IActionResult Create() {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Category category) {
        // if (category.Name == category.DisplayOrder.ToString()) {
        //     ModelState.AddModelError("Name", "Display order and name must be unique.");
        // }
        if (ModelState.IsValid) {
            unitOfWork.Category.Add(category);
            unitOfWork.Save();
            TempData["Success"] = "Category created successfully";
            return RedirectToAction("Index","Category");
        }

        return View();


    }
    public IActionResult Edit(int? id) {
        if (id is null or 0) {
            return NotFound();
        }
        Category? categoryFromDb = unitOfWork.Category.Get(item=>item.Id == id);
        

        if (categoryFromDb is null) {
            return NotFound();
        }
        
        return View(categoryFromDb);
    }
    
    [HttpPost]
    public IActionResult Edit(Category category) {
        
        if (ModelState.IsValid) {
            unitOfWork.Category.Update(category);
            unitOfWork.Save();
            TempData["Success"] = "Category updated successfully";
            return RedirectToAction("Index","Category");
        }

        return View();


    }
    
    public IActionResult Delete(int? id) {
        if (id is null or 0) {
            return NotFound();
        }
        Category? categoryFromDb = unitOfWork.Category.Get(item=>item.Id == id);

        if (categoryFromDb is null) {
            return NotFound();
        }
        
        return View(categoryFromDb);
    }
    
    [HttpPost,ActionName("Delete")]
    public IActionResult DeletePost(int? id) {
        
        Category? categoryFromDb = unitOfWork.Category.Get(item=>item.Id == id);
        if (categoryFromDb is null) {
            return NotFound();
        }
        unitOfWork.Category.Remove(categoryFromDb);
        unitOfWork.Save();
        TempData["Success"] = "Category deleted successfully";
        return RedirectToAction("Index","Category");


    }
}