# Agency Appointment API

## Project Description
This API provides a solution for an agency to manage customer appointments and issue tokens. Agencies can view the daily queue of appointments.  

**Note:** The hosted API on Azure dont connect to database, to test it fully, please use local database

## Hosted API (Azure)
Swagger documentation for the hosted API:  
[https://agency-appointment-api-gjbbgpcgdkh0dcbm.indonesiacentral-01.azurewebsites.net/swagger/index.html]

## Features (can be tested using local DB)
- Book appointments and issue tokens
- View daily queue of customers
- Specify agency off days (public holidays)
- Set maximum appointments per day with overflow logic

## Technologies Used
- .NET 9 WebAPI
- Entity Framework Core (for local DB)
- Swagger for API documentation
- Dependency Injection / IoC ready classes
- LINQ for data manipulation
- Unit Tests (basic)
- Azure cloud hosting
- GitHub source control
