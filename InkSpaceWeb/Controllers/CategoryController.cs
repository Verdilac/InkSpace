using InkSpaceWeb.Data;
using InkSpaceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace InkSpaceWeb.Controllers;

public class CategoryController(ApplicationDbContext dbContext) : Controller
{
    private readonly ApplicationDbContext _db = dbContext;

    // GET
    public IActionResult Index() {
        
        List<Category> categories = _db.Categories.ToList();
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
            _db.Categories.Add(category);
            _db.SaveChanges(); 
            TempData["Success"] = "Category created successfully";
            return RedirectToAction("Index","Category");
        }

        return View();


    }
    public IActionResult Edit(int? id) {
        if (id is null or 0) {
            return NotFound();
        }
        Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);

        if (categoryFromDb is null) {
            return NotFound();
        }
        
        return View(categoryFromDb);
    }
    
    [HttpPost]
    public IActionResult Edit(Category category) {
        
        if (ModelState.IsValid) {
            _db.Categories.Update(category);
            _db.SaveChanges(); 
            TempData["Success"] = "Category updated successfully";
            return RedirectToAction("Index","Category");
        }

        return View();


    }
    
    public IActionResult Delete(int? id) {
        if (id is null or 0) {
            return NotFound();
        }
        Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);

        if (categoryFromDb is null) {
            return NotFound();
        }
        
        return View(categoryFromDb);
    }
    
    [HttpPost,ActionName("Delete")]
    public IActionResult DeletePost(int? id) {
        
        Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
        if (categoryFromDb is null) {
            return NotFound();
        }
        _db.Categories.Remove(categoryFromDb);
        _db.SaveChanges();
        TempData["Success"] = "Category deleted successfully";
        return RedirectToAction("Index","Category");


    }
}