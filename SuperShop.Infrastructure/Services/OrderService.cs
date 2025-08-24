using SuperShop.Application.Interfaces;
using SuperShop.Domain.Domain;
using SuperShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICrudService<Order> _orders;
        private readonly ICrudService<Customer> _customers;
        private readonly ICrudService<Product> _products;

        public OrderService(ICrudService<Order> orders,
                            ICrudService<Customer> customers,
                            ICrudService<Product> products)
        {
            _orders = orders;
            _customers = customers;
            _products = products;
        }

        public Order CreateOrder(int customerId, List<(int productId, int quantity)> items)
        {
            var customer = _customers.GetById(customerId)
                ?? throw new InvalidOperationException("Customer not found");

            if (items.Count == 0) throw new InvalidOperationException("Order must have items");

            var order = new Order { CustomerId = customerId, OrderDate = DateTime.UtcNow };
            var productList = _products.GetAll().ToDictionary(p => p.Id);

            foreach (var (productId, qty) in items)
            {
                if (!productList.TryGetValue(productId, out var product))
                    throw new InvalidOperationException($"Product {productId} not found");

                if (qty <= 0) throw new InvalidOperationException("Quantity must be > 0");
                if (product.StockQuantity < qty)
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}");

                // Snapshot pricing & update stock
                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = qty,
                    UnitPrice = product.Price
                });

                product.StockQuantity -= qty;
                _products.Update(product);
            }

            return _orders.Add(order);
        }

        public IEnumerable<Order> GetAll() => _orders.GetAll();

        public IEnumerable<Order> GetByCustomer(int customerId) =>
            _orders.GetAll().Where(o => o.CustomerId == customerId);

        public bool CancelOrder(int orderId)
        {
            var order = _orders.GetById(orderId);
            if (order is null || order.IsCancelled) return false;

            // Restock products
            var products = _products.GetAll().ToDictionary(p => p.Id);
            foreach (var item in order.Items)
            {
                if (products.TryGetValue(item.ProductId, out var p))
                {
                    p.StockQuantity += item.Quantity;
                    _products.Update(p);
                }
            }

            order.IsCancelled = true;
            return _orders.Update(order);
        }
    }
}
