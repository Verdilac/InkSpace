using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext db):Repository<Product>(db),IProductRepository
{
    private readonly ApplicationDbContext _db = db;
    
    
    public void Update(Product product) {
        _db.Products.Update(product);
    }
}