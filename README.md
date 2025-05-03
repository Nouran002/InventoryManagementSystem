
# Inventory Management System

## Overview

This Inventory Management System is a web-based application designed to help businesses efficiently manage and track their products, stock levels, and warehouse operations. It allows users to add, update, delete, and view products and stock movements across multiple warehouses. The system is built using ASP.NET Core and Entity Framework Core for the backend, and provides secure APIs that can be consumed by any frontend.

## Features

- **Product Management:**
  - Create Product.
  - Delete Product.
  - Update Product.
  - Show Product Details.
  - Get All Products.
  

- **Warehouse Management:**
  - Add Warehouse.
  - Delete warehouse.
  - Update Warehouse.
  - Get All Warehouses With Its Products
  - Assign products to warehouses.
  - Track product quantity per warehouse.
  

- **Inventory Transactions:**
  - Add Stock: Increase the quantity of a specific product.
  - Remove Stock: Decrease the quantity of a specific product.
  - Transfer Stock: Transfer stock between warehouses


- **Making a reporting**
  - List products below their LowStockThreshold.
  - Dawnload List products below their LowStockThreshold As Pdf.
  - Transaction History: Retrieve transaction history for a specific product or time period

- **Other Features**
  - Schedule a background job that runs daily to check for low-stock products and logs a notification(Using Hangfire).
  - Implemented rate-limiting or throttling on report endpoints to prevent abusef.
- **Architectural patterns:**
  - Using Clean Architecture 3-Tier Architecture & Repository Pattern
  





