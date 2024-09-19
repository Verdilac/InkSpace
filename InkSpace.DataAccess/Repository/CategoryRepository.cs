using System.Linq.Expressions;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository;

public class CategoryRepository(ApplicationDbContext db) : Repository<Category>(db), ICategoryRepository
{
    private readonly ApplicationDbContext _db = db;


    public void Update(Category category) {
        _db.Categories.Update(category);
    }

 
}