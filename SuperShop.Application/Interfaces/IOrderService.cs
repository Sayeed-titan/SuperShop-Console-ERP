using SuperShop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(int customerId, List<(int productId, int quantity)> items);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetByCustomer(int customerId);
        bool CancelOrder(int orderId);
    }
}
