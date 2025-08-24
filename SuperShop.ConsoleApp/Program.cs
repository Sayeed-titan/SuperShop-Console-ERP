using Microsoft.Extensions.DependencyInjection;
using SuperShop.Application.Interfaces;
using SuperShop.Domain.Domain;
using SuperShop.Domain.Entities;
using SuperShop.Domain.Interfaces;
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
var suppliers = provider.GetRequiredService<ICrudService<Supplier>>();
var receipts = provider.GetRequiredService<ICrudService<StockReceipt>>();
var employees = provider.GetRequiredService<ICrudService<Employee>>();


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
if (employees.Count() == 0)
{
    employees.Add(new Employee
    {
        Username = "admin",
        Password = "admin123",
        Role = Role.Admin
    });
    Console.WriteLine("✅ Admin user created: username=admin, password=admin123");
}

Employee? currentUser = null;


MainMenu();

void MainMenu()
{
    while (true)
    {
        Console.WriteLine("\n=== SuperShop Console ERP ===");
        Console.WriteLine("0. Login");
        Console.WriteLine("1. Customers");
        Console.WriteLine("2. Products");
        Console.WriteLine("3. Orders");
        Console.WriteLine("4. Suppliers & Restock");
        Console.WriteLine("5. Employees (Admin only)");
        Console.WriteLine("6. Reports");
        Console.WriteLine("X. Exit");
        Console.Write("Choose: ");

        var key = Console.ReadLine()?.ToUpper();

        switch (key)
        {
            case "0": Login(); break;       
            case "1": CustomerMenu(); break;
            case "2": ProductMenu(); break;
            case "3": OrderMenu(); break;
            case "4": SupplierMenu(); break;
            case "5": EmployeeMenu(); break;
            case "6": ReportsMenu(); break;
            case "X": return;
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

void SupplierMenu()
{
    while (true)
    {
        Console.WriteLine("\n--- Suppliers ---");
        Console.WriteLine("1. List Suppliers");
        Console.WriteLine("2. Add Supplier");
        Console.WriteLine("3. Update Supplier");
        Console.WriteLine("4. Delete Supplier");
        Console.WriteLine("5. Receive Stock");
        Console.WriteLine("6. List Stock Receipts");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");
        var key = Console.ReadLine();

        switch (key)
        {
            case "1":
                foreach (var s in suppliers.GetAll())
                    Console.WriteLine($"{s.Id}. {s.Name} | {s.Contact}");
                break;
            case "2":
                Console.Write("Name: "); var n = Console.ReadLine() ?? "";
                Console.Write("Contact: "); var c = Console.ReadLine() ?? "";
                var snew = suppliers.Add(new Supplier { Name = n, Contact = c });
                Console.WriteLine($"Added Supplier #{snew.Id}");
                break;
            case "3":
                Console.Write("Id to update: ");
                if (int.TryParse(Console.ReadLine(), out var uid))
                {
                    var s = suppliers.GetById(uid);
                    if (s is null) { Console.WriteLine("Not found."); break; }
                    Console.Write($"Name ({s.Name}): "); var n2 = Console.ReadLine();
                    Console.Write($"Contact ({s.Contact}): "); var c2 = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(n2)) s.Name = n2;
                    if (!string.IsNullOrWhiteSpace(c2)) s.Contact = c2;
                    Console.WriteLine(suppliers.Update(s) ? "Updated." : "Failed.");
                }
                break;
            case "4":
                Console.Write("Id to delete: ");
                if (int.TryParse(Console.ReadLine(), out var did))
                    Console.WriteLine(suppliers.Delete(did) ? "Deleted." : "Failed.");
                break;
            case "5": ReceiveStock(); break;
            case "6":
                foreach (var r in receipts.GetAll())
                {
                    Console.WriteLine($"\nReceipt #{r.Id} | Supplier {r.SupplierId} | Date {r.Date:u}");
                    foreach (var item in r.Items)
                        Console.WriteLine($" - {item.ProductName} x{item.Quantity}");
                }
                break;
            case "0": return;
        }
    }
}

void EmployeeMenu()
{
    if (currentUser?.Role != Role.Admin)
    {
        Console.WriteLine("❌ Only Admin can manage employees.");
        return;
    }

    while (true)
    {
        Console.WriteLine("\n--- Employees ---");
        Console.WriteLine("1. List Employees");
        Console.WriteLine("2. Add Employee");
        Console.WriteLine("3. Update Employee");
        Console.WriteLine("4. Delete Employee");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");
        var key = Console.ReadLine();

        switch (key)
        {
            case "1":
                foreach (var e in employees.GetAll())
                    Console.WriteLine($"{e.Id}. {e.Username} | Role: {e.Role}");
                break;
            case "2":
                Console.Write("Username: "); var un = Console.ReadLine() ?? "";
                Console.Write("Password: "); var pw = Console.ReadLine() ?? "";
                Console.WriteLine("Roles: 0=Admin, 1=Sales, 2=Inventory");
                Console.Write("Role: ");
                if (!int.TryParse(Console.ReadLine(), out var r) || r < 0 || r > 2) { Console.WriteLine("Invalid role."); break; }

                var enew = employees.Add(new Employee { Username = un, Password = pw, Role = (Role)r });
                Console.WriteLine($"Added Employee #{enew.Id}");
                break;
            case "3":
                Console.Write("Id to update: ");
                if (!int.TryParse(Console.ReadLine(), out var uid)) break;

                var eUpd = employees.GetById(uid);
                if (eUpd == null) { Console.WriteLine("Not found."); break; }

                Console.Write($"Username ({eUpd.Username}): "); var nu = Console.ReadLine();
                Console.Write($"Password ({eUpd.Password}): "); var npw = Console.ReadLine();
                Console.WriteLine("Roles: 0=Admin, 1=Sales, 2=Inventory");
                Console.Write($"Role ({(int)eUpd.Role}): "); var nr = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(nu)) eUpd.Username = nu;
                if (!string.IsNullOrWhiteSpace(npw)) eUpd.Password = npw;
                if (int.TryParse(nr, out var role) && role >= 0 && role <= 2) eUpd.Role = (Role)role;

                Console.WriteLine(employees.Update(eUpd) ? "Updated." : "Failed.");
                break;
            case "4":
                Console.Write("Id to delete: ");
                if (int.TryParse(Console.ReadLine(), out var did))
                    Console.WriteLine(employees.Delete(did) ? "Deleted." : "Failed.");
                break;
            case "0": return;
            default: Console.WriteLine("Invalid choice."); break;
        }
    }
}

void ReceiveStock()
{
    Console.Write("Supplier Id: ");
    if (!int.TryParse(Console.ReadLine(), out var sid))
    {
        Console.WriteLine("Invalid Supplier Id.");
        return;
    }

    var supplier = suppliers.GetById(sid);
    if (supplier is null) { Console.WriteLine("Supplier not found."); return; }

    var receipt = new StockReceipt { SupplierId = sid };
    var prodList = products.GetAll().ToDictionary(p => p.Id);

    while (true)
    {
        Console.Write("Product Id (blank to finish): ");
        var ps = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ps)) break;
        if (!int.TryParse(ps, out var pid) || !prodList.TryGetValue(pid, out var prod))
        {
            Console.WriteLine("Invalid Product.");
            continue;
        }

        Console.Write("Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out var qty) || qty <= 0)
        {
            Console.WriteLine("Invalid quantity.");
            continue;
        }

        prod.StockQuantity += qty;
        products.Update(prod);

        receipt.Items.Add(new StockReceiptItem
        {
            ProductId = prod.Id,
            ProductName = prod.Name,
            Quantity = qty
        });

        Console.WriteLine($"Restocked {qty} of {prod.Name}. New Stock: {prod.StockQuantity}");
    }

    if (receipt.Items.Count > 0)
    {
        receipts.Add(receipt);
        Console.WriteLine($"Saved Receipt #{receipt.Id}");
    }
}

void Login()
{
    Console.Write("Username: ");
    var username = Console.ReadLine() ?? "";
    Console.Write("Password: ");
    var password = Console.ReadLine() ?? "";

    var user = employees.GetAll()
        .FirstOrDefault(e => e.Username == username && e.Password == password);

    if (user == null)
    {
        Console.WriteLine("❌ Invalid credentials");
        return;
    }

    currentUser = user;
    Console.WriteLine($"✅ Logged in as {currentUser.Username} ({currentUser.Role})");
}

void ReportsMenu()
{
    Console.WriteLine("\n--- Reports ---");
    Console.WriteLine("1. Daily Sales Summary");
    Console.WriteLine("2. Best-selling Products");
    Console.WriteLine("3. Low-stock Alerts");
    Console.WriteLine("0. Back");
    Console.Write("Choose: ");

    var key = Console.ReadLine();

    switch (key)
    {
        case "1": DailySalesSummary(); break;
        case "2": BestSellingProducts(); break;
        case "3": LowStockAlerts(); break;
        case "0": return;
        default: Console.WriteLine("Invalid choice."); break;
    }
}

void DailySalesSummary()
{
    Console.Write("Enter date (yyyy-MM-dd) or blank for today: ");
    var input = Console.ReadLine();
    var date = string.IsNullOrWhiteSpace(input) ? DateTime.Today : DateTime.Parse(input);

    var ordersOnDate = orders.GetAll()
        .Where(o => o.OrderDate.Date == date.Date && !o.IsCancelled);

    decimal total = ordersOnDate.Sum(o => o.TotalAmount);
    int count = ordersOnDate.Count();

    Console.WriteLine($"\n📅 Sales Summary for {date:yyyy-MM-dd}");
    Console.WriteLine($"Total Orders: {count}");
    Console.WriteLine($"Total Sales: {total:C}");
}

void BestSellingProducts()
{
    var productSales = new Dictionary<int, int>(); // ProductId => total quantity sold

    foreach (var o in orders.GetAll().Where(x => !x.IsCancelled))
        foreach (var it in o.Items)
        {
            if (!productSales.ContainsKey(it.ProductId)) productSales[it.ProductId] = 0;
            productSales[it.ProductId] += it.Quantity;
        }


    var sorted = productSales.OrderByDescending(p => p.Value).Take(10);

    Console.WriteLine("\n🏆 Best-selling Products:");
    foreach (var kv in sorted)
    {
        var p = products.GetById(kv.Key);
        Console.WriteLine($"{p?.Name ?? "Unknown"} - Sold Qty: {kv.Value}");
    }
}

void LowStockAlerts(int threshold = 10)
{
    var lowStock = products.GetAll().Where(p => p.StockQuantity <= threshold);

    Console.WriteLine($"\n⚠️ Products with stock <= {threshold}:");
    foreach (var p in lowStock)
        Console.WriteLine($"{p.Id}. {p.Name} - Stock: {p.StockQuantity}");
}

