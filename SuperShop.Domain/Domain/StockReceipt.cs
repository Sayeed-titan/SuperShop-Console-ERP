using SuperShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Domain.Domain
{
    public class StockReceipt : IEntity
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public List<StockReceiptItem> Items { get; set; } = new();
    }

    public class StockReceiptItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
    }
}
