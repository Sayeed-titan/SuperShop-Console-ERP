Project Description

SuperShop Console ERP & POS System is a C# .NET 6 console application designed to simulate a small retail shop management system. It provides a fully functional CRUD system for managing customers, products, orders, suppliers, stock, and employees with role-based access control. The application includes a POS-style receipt printing system, mimicking the output of a real cash register.

The system uses JSON files for persistent storage, ensuring that all data (orders, customers, stock levels, and employees) are saved and loaded automatically between application runs.

It is intended as a learning project for mastering C# object-oriented programming, generic services, dependency injection, and console-based user interfaces, while also demonstrating a simple ERP and POS workflow.

Features
Core Features

Customer Management: Add, update, delete, and list customers.

Product Management: CRUD operations, stock tracking, and low-stock alerts.

Order Management: Create and cancel orders; automatically updates stock.

POS Receipt: Prints a formatted POS-style voucher for each order.

Supplier & Restock: CRUD for suppliers and receive stock with receipts.

Employee & Role Management: Admin, Sales, and Inventory roles with access control.

Reports:

Daily sales summary

Best-selling products

Low-stock alerts

Technical Features

Persistent Storage via ICrudService<T> and JsonStore.

Dependency Injection for service management.

Role-based Access Control:

Admin: Full access

Sales: Create orders

Inventory: Update stock

Console-based Menu System with nested menus for all operations.
