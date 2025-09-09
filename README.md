JearCarrier — Carrier Management System

📌 Overview
This project is a Carrier Management System built for JEAR.
It provides the ability to add, update, delete, search, and view carriers using a clean three-layer architecture:
ASP.NET Web Forms — UI with a form, GridView, and CSV export.
ASP.NET Core Web API — REST endpoints documented with Swagger.
SQL Server — relational database with indexes and constraints for performance and integrity.

🧰 Tech Stack

ASP.NET Core Web API (CRUD endpoints, Swagger / OpenAPI)
Entity Framework Core (SQL Server provider, Migrations, LINQ → SQL)
ASP.NET Web Forms (UI with GridView + HttpClient)
SQL Server (Carriers table with indexes + RowVersion for concurrency)
C# / .NET 7 + .NET Framework 4.7.2


🚀 Features

Add, view, update, and delete carriers.
Search and paging on CarrierName, City, or State.
Client-side validation for required fields and email format.
CSV export of carriers from the UI.
Swagger UI for testing API endpoints.
EF Core migrations for schema management.

🗄 Database Schema

Carriers Table
CarrierName (required)
Address, Address2
City (required), State (required), Zip (required)
Contact (required), Phone (required), Fax
Email (required, unique)
RowVersion (for optimistic concurrency)
Indexes on: CarrierName, City, State


▶️ Quick Start

Clone the repo:
git clone https://github.com/YOUR-USERNAME/JearCarrier.git
cd JearCarrier

Configure the database connection in appsettings.json (not committed, use the provided appsettings.Development.example.json).
Apply EF Core migrations:
dotnet ef database update

Run the API:
dotnet run --project JearCarrier
Swagger will be available at: http://localhost:<port>/swagger
Update Web.config in JearCarrier.webforms with the correct API port:
<add key="ApiBaseUrl" value="http://localhost:<port>/api" />

Run the WebForms project in Visual Studio and start testing.
