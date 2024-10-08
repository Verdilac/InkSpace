using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface ICategoryRepository :IRepository<Category>
{
    void Update(Category category);
    
}