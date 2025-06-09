Morgana's Challenge - .NET

This solution includes:

A CMS microservice (UmbracoCMS) powered by Umbraco 15 using SQLite.
An API Gateway-style microservice (UmbracoBridge) exposing anonymous endpoints to interact with the CMS.
An orchestrator (MorganaAppHost) using .NET Aspire to manage and run the services together.
It follows clean architecture principles with separation of concerns across presentation, application, domain, and infrastructure layers.


MorganasChallenge/
├── UmbracoCMS/           # CMS prioject
├── UmbracoBridge/        # Web API that consumes the CMS
└── MorganaAppHost/       # Local orchestrator with Aspire

Technologies and Patterns

.NET 9
Umbraco 15+ (Delivery API, Management API)
ASP.NET Core Web API
OAuth2 Client Credentials
Swagger / Swashbuckle
Scalar UI
.NET Aspire AppHost
Layered architecture (Controller - Service - Validator)



Setup and Run Locally

Prerequisites

.NET 9 SDK

Visual Studio 2022+ or Visual Studio Code


Clone the repo:

git clone https://github.com/Bernie-ramone/MorganasChallenge.git
cd MorganasChallenge

Restore and build:

dotnet restore
dotnet build

Run all services via Aspire AppHost:

dotnet run --project MorganaAppHost.AppHost


1. External Web API interacting with Umbraco

Project: UmbracoBridge
Endpoints: POST /document-type, DELETE /document-type/{id}
Service UmbracoManagementService handles authentication and API calls

2. Custom Validation

Class: DocumentTypeValidator
Rules:
Non-empty alias
Icon must start with icon-
Required fields enforced

3. Unit Tests

Project: UmbracoBridge.Tests
Tool: xUnit

4. Swagger Enabled

Configured using AddSwaggerGen in both UmbracoBridge and UmbracoCMS

Side Quest 1: Delivery API + API Key

Enabled in UmbracoCMS
Configured via appsettings.json
Endpoint: GET /umbraco/delivery/api/v1/content
Header: Api-Key: my-api-key

Side Quest 2: Aspire AppHost

Project: MorganaAppHost.AppHost
Orchestrates UmbracoCMS and UmbracoBridge
Uses builder.AddProject<>()


Side Quest 3: Scalar UI

Modern UI to consume Swagger
File: /UmbracoBridge/wwwroot/scalar/index.html
URL: /scalar/index.html

Side Quest 4: isOk Endpoint

Project: UmbracoCMS
Route: GET /api/health?isOk=true
Returns 200 or 400 based on the query param

Side Quest 5: Swagger in CMS

Swagger enabled in UmbracoCMS
Routes exposed using [ApiController] + MapControllers()


Running the Project

Run via AppHost:

dotnet run --project MorganaAppHost.AppHost

Access:
https://localhost:{port}/swagger → Swagger UI
https://localhost:{port}/scalar/index.html → Scalar UI
https://localhost:{port}/umbraco → Umbraco Backoffice
GET /api/health?isOk=true → Health check endpoint


Example Requests & Responses

POST /document-type
Content-Type: application/json

{
  "alias": "exampleDoc",
  "name": "Example",
  "description": "Sample doc type",
  "icon": "icon-document",
  "allowedAsRoot": true,
  "variesByCulture": false,
  "variesBySegment": false,
  "collection": null,
  "isElement": true
}


GET /healthcheck


DELETE /document-type/{id}

DELETE /document-type/d2b1281e-a1d0-4ef2-9cb2-4f3cfe80ac29



Verification Instructions

1. Open Swagger for UmbracoBridge:

https://localhost:{port}/swagger

2. Test the 3 endpoints manually or using Postman:

/healthcheck

/document-type (POST and DELETE)

3. Check Umbraco CMS health:

Visit https://localhost:{port}/umbraco

Login with admin credentials (configured in setup)

Confirm the document types reflect the created/removed ones.

4. (If implemented) Access Scalar UI at:

https://localhost:{port}/scalar/index.html

5. Confirm Delivery API protected with API Key:

Test /umbraco/delivery/api/v1/content with header Api-Key















umbraco
https://localhost:44364/umbraco/install

Name :admin
Email : admin@admin.com
Password : admin12345


Create API user
Create an Api User to allow external services to authenticate with the Umbraco Management API.

Name * ApiUser
Email * ApiUser@umbraco.com
User group *

Client Credentials

Create client credential
Id *umbraco-back-office-ApiUserUmbraco   ApiUserUmbraco
Secret * admin12345


