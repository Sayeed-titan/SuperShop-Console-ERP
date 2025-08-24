using Microsoft.Extensions.DependencyInjection;
using SuperShop.Application.Interfaces;
using SuperShop.Domain.Domain;
using SuperShop.Domain.Entities;
using SuperShop.Infrastructure.Repositories;
using SuperShop.Infrastructure.Services;

// DI container
var services = new ServiceCollection();

// Open generic registration for CRUD services using JSON files
services.AddSingleton(typeof(ICrudService<>), typeof(FileRepository<>));

// Order business service
services.AddSingleton<IOrderService, OrderService>();

var provider = services.BuildServiceProvider();

// Resolve services
var customers = provider.GetRequiredService<ICrudService<Customer>>();
var products = provider.GetRequiredService<ICrudService<Product>>();
var orders = provider.GetRequiredService<IOrderService>();

// Seed items 
if (products.Count() == 0)
{
    products.Add(new Product { Name = "Notebook", Price = 120.00m, StockQuantity = 50 });
    products.Add(new Product { Name = "Pen", Price = 15.00m, StockQuantity = 200 });
}
if (customers.Count() == 0)
{
    customers.Add(new Customer { Name = "Rahim", Phone = "017...", Email = "rahim@example.com" });
    customers.Add(new Customer { Name = "Karim", Phone = "018...", Email = "karim@example.com" });
}

MainMenu();

void MainMenu()
{
    while (true)
    {
        Console.WriteLine("\n=== SuperShop Console ERP ===");
        Console.WriteLine("1. Customers");
        Console.WriteLine("2. Products");
        Console.WriteLine("3. Orders");
        Console.WriteLine("0. Exit");
        Console.Write("Choose: ");
        var key = Console.ReadLine();

        switch (key)
        {
            case "1": CustomerMenu(); break;
            case "2": ProductMenu(); break;
            case "3": OrderMenu(); break;
            case "0": return;
            default: Console.WriteLine("Invalid choice."); break;
        }
    }
}

void CustomerMenu()
{
    while (true)
    {
        Console.WriteLine("\n--- Customers ---");
        Console.WriteLine("1. List");
        Console.WriteLine("2. Add");
        Console.WriteLine("3. Update");
        Console.WriteLine("4. Delete");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");
        var key = Console.ReadLine();

        switch (key)
        {
            case "1":
                foreach (var c in customers.GetAll())
                    Console.WriteLine($"{c.Id}. {c.Name} | {c.Phone} | {c.Email}");
                break;
            case "2":
                Console.Write("Name: "); var name = Console.ReadLine() ?? "";
                Console.Write("Phone: "); var phone = Console.ReadLine() ?? "";
                Console.Write("Email: "); var email = Console.ReadLine() ?? "";
                var cnew = customers.Add(new Customer { Name = name, Phone = phone, Email = email });
                Console.WriteLine($"Added Customer #{cnew.Id}");
                break;
            case "3":
                Console.Write("Id to update: ");
                if (int.TryParse(Console.ReadLine(), out var uid))
                {
                    var c = customers.GetById(uid);
                    if (c is null) { Console.WriteLine("Not found."); break; }
                    Console.Write($"Name ({c.Name}): "); var n = Console.ReadLine();
                    Console.Write($"Phone ({c.Phone}): "); var p = Console.ReadLine();
                    Console.Write($"Email ({c.Email}): "); var e = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(n)) c.Name = n;
                    if (!string.IsNullOrWhiteSpace(p)) c.Phone = p;
                    if (!string.IsNullOrWhiteSpace(e)) c.Email = e;
                    Console.WriteLine(customers.Update(c) ? "Updated." : "Failed.");
                }
                break;
            case "4":
                Console.Write("Id to delete: ");
                if (int.TryParse(Console.ReadLine(), out var did))
                    Console.WriteLine(customers.Delete(did) ? "Deleted." : "Failed.");
                break;
            case "0": return;
        }
    }
}

void ProductMenu()
{
    while (true)
    {
        Console.WriteLine("\n--- Products ---");
        Console.WriteLine("1. List");
        Console.WriteLine("2. Add");
        Console.WriteLine("3. Update");
        Console.WriteLine("4. Delete");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");
        var key = Console.ReadLine();

        switch (key)
        {
            case "1":
                foreach (var p in products.GetAll())
                    Console.WriteLine($"{p.Id}. {p.Name} | Price: {p.Price} | Stock: {p.StockQuantity} | Active: {p.IsActive}");
                break;
            case "2":
                Console.Write("Name: "); var name = Console.ReadLine() ?? "";
                Console.Write("Price: "); var priceOk = decimal.TryParse(Console.ReadLine(), out var price);
                Console.Write("Stock: "); var stockOk = int.TryParse(Console.ReadLine(), out var stock);
                if (!priceOk || !stockOk) { Console.WriteLine("Invalid input."); break; }
                var pnew = products.Add(new Product { Name = name, Price = price, StockQuantity = stock, IsActive = true });
                Console.WriteLine($"Added Product #{pnew.Id}");
                break;
            case "3":
                Console.Write("Id to update: ");
                if (int.TryParse(Console.ReadLine(), out var uid))
                {
                    var p = products.GetById(uid);
                    if (p is null) { Console.WriteLine("Not found."); break; }
                    Console.Write($"Name ({p.Name}): "); var n = Console.ReadLine();
                    Console.Write($"Price ({p.Price}): "); var rp = Console.ReadLine();
                    Console.Write($"Stock ({p.StockQuantity}): "); var rs = Console.ReadLine();
                    Console.Write($"Active ({p.IsActive}): "); var ra = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(n)) p.Name = n;
                    if (decimal.TryParse(rp, out var np)) p.Price = np;
                    if (int.TryParse(rs, out var ns)) p.StockQuantity = ns;
                    if (bool.TryParse(ra, out var na)) p.IsActive = na;
                    Console.WriteLine(products.Update(p) ? "Updated." : "Failed.");
                }
                break;
            case "4":
                Console.Write("Id to delete: ");
                if (int.TryParse(Console.ReadLine(), out var did))
                    Console.WriteLine(products.Delete(did) ? "Deleted." : "Failed.");
                break;
            case "0": return;
        }
    }
}

void OrderMenu()
{
    while (true)
    {
        Console.WriteLine("\n--- Orders ---");
        Console.WriteLine("1. List All");
        Console.WriteLine("2. List By Customer");
        Console.WriteLine("3. Create Order");
        Console.WriteLine("4. Cancel Order");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");
        var key = Console.ReadLine();

        switch (key)
        {
            case "1":
                foreach (var o in orders.GetAll())
                    PrintOrder(o);
                break;
            case "2":
                Console.Write("Customer Id: ");
                if (int.TryParse(Console.ReadLine(), out var cid))
                    foreach (var o in orders.GetByCustomer(cid))
                        PrintOrder(o);
                break;
            case "3":
                Console.Write("Customer Id: ");
                if (!int.TryParse(Console.ReadLine(), out var custId)) { Console.WriteLine("Invalid."); break; }

                var orderItems = new List<(int productId, int quantity)>();
                while (true)
                {
                    Console.Write("Product Id (blank to finish): ");
                    var ps = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ps)) break;
                    if (!int.TryParse(ps, out var pid)) { Console.WriteLine("Invalid."); continue; }

                    Console.Write("Quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out var q) || q <= 0) { Console.WriteLine("Invalid."); continue; }

                    orderItems.Add((pid, q));
                }

                try
                {
                    var newOrder = orders.CreateOrder(custId, orderItems);
                    Console.WriteLine($"Created Order #{newOrder.Id}  Total: {newOrder.TotalAmount}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                break;
            case "4":
                Console.Write("Order Id: ");
                if (int.TryParse(Console.ReadLine(), out var oid))
                    Console.WriteLine(orders.CancelOrder(oid) ? "Cancelled." : "Failed.");
                break;
            case "0": return;
        }
    }
}

void PrintOrder(Order o)
{
    Console.WriteLine($"\nOrder #{o.Id} | CustomerId: {o.CustomerId} | Date: {o.OrderDate:u} | Cancelled: {o.IsCancelled}");
    foreach (var it in o.Items)
        Console.WriteLine($"  - {it.ProductName} x{it.Quantity} @ {it.UnitPrice} = {it.Subtotal}");
    Console.WriteLine($"  Total: {o.TotalAmount}");
}
