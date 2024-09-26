using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository :IRepository<ShoppingCart>
{
    void Update(ShoppingCart shoppingCart);
    
}