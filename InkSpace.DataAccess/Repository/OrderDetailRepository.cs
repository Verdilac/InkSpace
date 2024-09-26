using System.Linq.Expressions;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository;

public class OrderDetailRepository(ApplicationDbContext db) : Repository<OrderDetail>(db), IOrderDetailRepository
{
    private readonly ApplicationDbContext _db = db;


    public void Update(OrderDetail orderDetail) {
        _db.OrderDetails.Update(orderDetail);
    }

 
}