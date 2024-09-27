using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository :IRepository<OrderHeader>
{
    void Update(OrderHeader orderHeader);
    
    void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    
}