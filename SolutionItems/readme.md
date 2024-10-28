# API Documentation

## Project Setup
- Please run the database script file provided in this folder XERO_Db_.sql
- This project requires a SQL Database. Depending on your local configuration, you may need to update appsettings.json in XERO.API
- Set XERO.API as your startup project and run using IIS Express.

---

## Useful Information
- Data Access Project was generated using the following scaffold command:
#### Scaffold-DbContext "Server=.;Database=XERO;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Context XeroDbContext -Force


---
## Postman Tests
- A JSON file has been included in this folder that can be imported into postman to test the API and the API validation.  The file is titled 'XERO.postman_collection.json'
---