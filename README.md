# Forum API
RESTful Web API for internet forum project built with ASP.NET, allowing users to create boards, topics, reply to posts and manage accounts.

## Features
    • User registration and authentication
    • Creating and managing boards and topics
    • Posts and replies within topics
    • Photo storage for user and post related media
    • Combination of role and resource-based authorization    

## Technologies
    • ASP.NET (.NET 10)
    • Entity Framework Core
    • SQL Server 
    • Cloudinary     

## Requirements
    • .NET 10 SDK
    • SQL Server 2025 instance

## Configuration  
Project requires some essential configuration provided in appsettings.json and secrets.json files.  
If configuration cannot be retrieved from configuration files an **InvalidOperationException** will be thrown.

**1. appsettings.json**  
  The appsettings.json file contains the base application configuration.
  The most important part of this file that needs to be provided is **database connection string**.  
  **File template:**
  ```
  {
    "ConnectionStrings": {
      "DefaultConnection": "YOUR_DATABASE_CONNECTION_STRING"
    },
    "Serilog": {
      "MinimumLevel": {
        "Override": {
          "Microsoft": "Warning",
          "Microsoft.EntityFrameworkCore": "Information",
          "System": "Information"
        }
      },
      "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Formatting.Compact" ],
      "WriteTo": [
        {
          "Name": "Console",
          "Args": {
            "outputTemplate": "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine} {Message:lj}{NewLine}{Exception}"
          }
        },
        {
          "Name": "File",
          "Args": {
            "path": "Logs/Forum-Api-.log",
            "rollingInterval": "Day",
            "rollOnFileSizeLimit": true,
            "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
          }
        }
      ]
    },
    "AllowedHosts": "*"
  }
  ```

**3. User Secrets (secrets.json)**  
  The secrets.json file contains information that is sensitive and cannot be stored in repository.  
  This includes:  
  ```
  • Automapper license key  
  • Token key, used for generating security tokens
  • Cloudinary login settings
  • Admin account configuration for data seeding
  ```
   
  **File template:**  
  ```
  {
    "Forum": {
      "AutomapperLicenseKey": "YOUR_AUTOMAPPER_LICENSE_KEY",
      "TokenKey": "AT_LEAST_64_CHARACTERS",
      "CloudinarySettings": {
        "CloudName": "YOUR_CLOUDINARY_CLOUD_NAME",
        "ApiKey": "YOUR_CLOUDINARY_API_KEY",
        "ApiSecret": "YOUR_CLOUDINARY_API_SECRET"
      },
      "AdminAccount": {
        "DisplayName": "ADMIN_NAME",
        "Email": "VALID_EMAIL_ADDRESS",
        "Password": "PASSWORD_AT_LEAST_8_LETTERS_AND_1_NUMBER"
      }
    }
  }
  ```

## Database migrations
Migrations will apply automatically on application startup.

## Project structure
Project is layered using package by feature file organization.

## Running the project
Project can be started using Visual Studio or .NET CLI using:
```
dotnet build  
dotnet run
```  
The application should be available at:  
https://localhost:7164  
