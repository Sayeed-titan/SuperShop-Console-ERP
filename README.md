# SuperShop Console ERP & POS System

![C#](https://img.shields.io/badge/Language-C%23-blue)
![.NET](https://img.shields.io/badge/.NET-6.0-brightgreen)

**SuperShop Console ERP & POS System** is a C# .NET 6 console application simulating a small retail shop.  
It provides a fully functional **CRUD system** for managing **customers, products, orders, suppliers, stock, and employees**, with **role-based access control** and a **POS-style receipt printing system**.

---

## Features

### Core Features
- **Customer Management:** Add, update, delete, and list customers.
- **Product Management:** CRUD operations, stock tracking, low-stock alerts.
- **Order Management:** Create and cancel orders; stock updates automatically.
- **POS Receipt:** Prints a formatted POS-style voucher for each order.
- **Supplier & Restock:** CRUD for suppliers and receive stock with receipts.
- **Employee & Role Management:** Admin, Sales, and Inventory roles with access control.
- **Reports:**
  - Daily sales summary
  - Best-selling products
  - Low-stock alerts

### Technical Features
- **Persistent Storage:** `ICrudService<T>` + `JsonStore` saves data to JSON files.
- **Dependency Injection** for modular service management.
- **Role-based Access Control:** Admin (full access), Sales (create orders), Inventory (update stock).
- **Console-based Menu System** with nested menus for all operations.

---

## Setup Instructions

1. **Clone the repository:**
```bash
git clone https://github.com/YourUsername/SuperShop.git](https://github.com/Sayeed-titan/SuperShop-Console-ERP.git
cd SuperShop
