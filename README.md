# Northwind API Project

This repository contains a RESTful API project built with ASP.NET Core Web API, providing endpoints to interact with the Northwind sample database.

## Overview

The Northwind database is a commonly used database for learning and practicing SQL and database-related concepts. This project exposes various endpoints to perform CRUD operations on entities within the NorthWind database such as Customers, Products, Orders, etc.

## Features

- **RESTful API**: Built using ASP.NET Core Web API, adhering to REST principles for easy consumption and integration.
- **CRUD Operations**: Supports Create, Read, Update, and Delete operations on various entities.
- **Validation**: Input validation to ensure data integrity and consistency.
- **Error Handling**: Comprehensive error handling to provide meaningful responses to clients.
## Usage
The API supports various endpoints such as:

- `GET /api/customers` - Retrieve all customers.
- `POST /api/customers` - Create a new customer.
- `GET /api/orders` - Retrieve all orders.
- `PUT /api/orders/{id}` - Update an order by ID.
- `Patch /api/Customer/UpdatePatch` - Update Customer by Id choose the field to update only not all the fields should be updated. in this way:- 
"path": "string", # the column you want to update
"op": "string", # op must be replace
"value": "string"  # new value

For detailed API documentation, visit [Swagger UI](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0) after running the application.
