using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InkSpaceWeb.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [DisplayName("Category Name")]
    public  string Title { get; set; }
    
    public string Description { get; set; }
    
    [Required] 
    public String ISBN { get; set; }
    
    [Required]
    public string Author { get; set; }
 
    [Required] [Display(Name = "List Price")] [Range(0,1000)]
    public double ListPrice { get; set; }
    
    [Required] [Display(Name = "Price For 1-50")] [Range(0,1000)]
    public double Price { get; set; }
    
    [Required] [Display(Name = "Price For 50+")] [Range(0,1000)]
    public double Price50 { get; set; }
    
    [Required] [Display(Name = "Price For 100+")] [Range(0,1000)]
    public double Price100 { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")] [ValidateNever]
    public Category Category { get; set; }

    [ValidateNever]
    public string ImageUrl { get; set; }
}