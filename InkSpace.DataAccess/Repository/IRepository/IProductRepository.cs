using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface IProductRepository:IRepository<Product>
{
    void Update(Product product);
}