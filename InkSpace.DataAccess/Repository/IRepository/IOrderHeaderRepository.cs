using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository :IRepository<OrderHeader>
{
    void Update(OrderHeader orderHeader);
    
}