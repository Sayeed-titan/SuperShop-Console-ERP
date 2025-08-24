using SuperShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Domain.Domain
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public bool IsCancelled { get; set; } = false;
    }
}
