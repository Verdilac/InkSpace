using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface IOrderDetailRepository :IRepository<OrderDetail>
{
    void Update(OrderDetail orderDetail);
    
}