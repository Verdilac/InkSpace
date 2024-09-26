using System.Linq.Expressions;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository;

public class ShoppingCartRepository(ApplicationDbContext db) : Repository<ShoppingCart>(db), IShoppingCartRepository
{
    private readonly ApplicationDbContext _db = db;


    public void Update(ShoppingCart shoppingCart) {
        _db.ShoppingCarts.Update(shoppingCart);
    }

 
}