using System.Linq.Expressions;
using InkSpace.DataAccess.Repository.IRepository;
using InkSpaceWeb.Data;
using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository;

public class OrderHeaderRepository(ApplicationDbContext db) : Repository<OrderHeader>(db), IOrderHeaderRepository
{
    private readonly ApplicationDbContext _db = db;


    public void Update(OrderHeader orderHeader) {
        _db.OrderHeaders.Update(orderHeader);
    }

 
}