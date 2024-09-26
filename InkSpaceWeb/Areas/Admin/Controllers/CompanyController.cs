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
public class CompanyController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index() {

        List<Company> companies = unitOfWork.Company.GetAll().ToList();

        return View(companies);
    }

    public IActionResult Upsert(int? id) {
        
        IEnumerable<SelectListItem> categoryList = unitOfWork.Category.GetAll().Select(obj=>new SelectListItem
        {
            Text = obj.Name,
            Value = obj.Id.ToString()
        });

        if (id is null or 0) {
            //Create
             return View(new Company());
        }

        //update 
        Company company = unitOfWork.Company.Get(item=>item.Id==id);
        return View(company);

    }
    
    [HttpPost]
    public IActionResult Upsert(Company company) {
 
        if (ModelState.IsValid) {
            
            if (company.Id is 0) {
                unitOfWork.Company.Add(company);
            }
            else {
                unitOfWork.Company.Update(company);
            }
            
            unitOfWork.Save();
            TempData["Success"] = "Company created successfully";
            return RedirectToAction("Index","Company");
        }
        else {
            //here we are Populating the category list in case the model is not valid , so the form will populate valid categories in the dropdown
  
            return View(company);
        }
        

    }
    
    
   
    



    #region API Calls

    [HttpGet]
    public IActionResult GetAll() {
        List<Company> companys = unitOfWork.Company.GetAll().ToList();
        return Json(new {data = companys});
    }
    

    [HttpDelete]
    public IActionResult Delete(int? id) {
        var companyToBeDeleted = unitOfWork.Company.Get(item=>item.Id == id);
        if (companyToBeDeleted==null) {
            return Json(new {success = false, message = "Error Deleting:Company not found"});
        }
       
        unitOfWork.Company.Remove(companyToBeDeleted);
        unitOfWork.Save();
        return Json(new {success = true, message = "Company deleted successfully"});
    }
    #endregion
}