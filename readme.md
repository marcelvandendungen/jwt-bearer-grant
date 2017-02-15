# Issue with jwt-bearer authorization grant in Azure AD

## Scenario
There are two ASP.NET Core Web APIs that use bearer token authentication.
The services also serve static files which are used to consent the APIs into the directory of a consuming application. 
So both APIs have implicit flow enabled in Azure AD.

API2 is calling into API1 from the Web API controller using the [**jwt-bearer grant**](https://tools.ietf.org/html/rfc7523#section-2.1 "Using JWTs as Authorization Grants"). 
API2 has permission to access API1.

A user from a third directory navigates to the SPA served by API2. The user is redirected to Azure AD, signs in 
and consents the API. The user is redirected back to the SPA application and 
an AJAX call is made to the API2 web API. From that controller, another call is 
made to API1. This call is authenticated using the jwt-bearer grant.

## Issue
When the AcquireToken call is made with client credentials for API2 and the JWT token used to call into API2, Azure AD responds with an error:

    Microsoft.IdentityModel.Clients.ActiveDirectory.AdalServiceException: 
    AADSTS50027: Invalid JWT token. AADSTS50027: Invalid JWT token. Token format not valid.
    Trace ID: 4031717e-aa0c-4432-bbd1-b97a738d3e6f
    Correlation ID: 61ae6cd6-6df6-49ee-9145-c16570c28f7b
    Timestamp: 2017-02-13 22:44:01Z ---> System.Net.Http.HttpRequestException:  Response status code does not indicate success: 400 (BadRequest). ---> System.Exception: {"error":"invalid_request","error_description":"AADSTS50027: Invalid JWT token. AADSTS50027: Invalid JWT token. Token format not valid.\r\nTrace ID: 4031717e-aa0c-4432-bbd1-b97a738d3e6f\r\nCorrelation ID: 61ae6cd6-6df6-49ee-9145-c16570c28f7b\r\nTimestamp: 2017-02-13 22:44:01Z","error_codes":[50027,50027],"timestamp":"2017-02-13 22:44:01Z","trace_id":"4031717e-aa0c-4432-bbd1-b97a738d3e6f","correlation_id":"61ae6cd6-6df6-49ee-9145-c16570c28f7b"} 

## Repro steps:
- clone repository locally.
- goto API1 folder and run `dotnet restore`
- goto API2 folder and run `dotnet restore` 


- navigate to Azure portal and select or create a directory (directory1)
- create API1 application in directory1 (API1 - Web app/api - http://localhost:5000).
- enable implicit flow in application manifest.
- make app multi-tenanted.
- save Application ID, App ID Uri and Tenant id (from Help | Show diagnostics)
- set environment variables with names `TenantId1` and `ClientId1` with appropriate values from API1.
- enter the value for ClientId in config.js in API1/wwwroot folder
- in API1 folder, run `dotnet run`.
- navigate your browser to `http://localhost:5000` and sign in with a **user from directory2** and consent API1.


- navigate to Azure portal and select or create a directory (directory2)
- create API2 application in directory2 (API2 - Web app/api - http://localhost:5001).
- enable implicit flow in application manifest.
- make app multi-tenanted.
- give API2 permission to call API1
- save Application ID and Tenant id (from Help | Show diagnostics)
- add client secret (key) to application and save value.
- set environment variables with names `TenantId2` and `ClientId2` with appropriate values from API2.
- set the value of the secret key as environment variable `ClientSecret`
- set the value of the API1 Application ID as environment variable `ResourceUri`.
- enter the value for ClientId in config.js in API2/wwwroot folder
- in API2 folder, run `dotnet run`.


- navigate your browser to `http://localhost:5000` and sign in with a user from directory3.
- consent the API1
- navigate your browser to `http://localhost:5001` and sign in with a user from directory3.
- consent the API2
- check console log in API2 windows for exceptions.