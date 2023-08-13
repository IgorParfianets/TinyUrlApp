# TinyUrl

## 1. About
This project implements a service for shortening URL using ASP.NET Core WebAPI for back-end and React application for front-end.

### 1.1. Main application features
- create shorten link using input alias
- get a list of created links
- remove created links

## 2. How to configure
### 2.1. Configure WebAPI application
#### 2.1.1. Change database connection string
Firstly, you should change connection string in `appsettings.json`

```json
 "ConnectionStrings": {
    "Default": "Server=myServer;Database=myDataBase;Trusted_Connection=True;TrustServerCertificate=True;"
  },
```

#### 2.1.2. Initiate database
Open Package Manager Console in Visual Studio (View -> Other Windows -> Package Manager Console). Choose TinyUrl.DataBase in default project.

Use the following command to create or update the database schema. 

```console
  PM> update-database
```

#### 2.1.3. About CORS policy
By default, WebAPI accepts requests from "http://localhost:3000" client. You need change origin or allow to accept requests from all origins to do this, replace this line of code ".WithOrigins("// host") with this one ".AllowAnyOrigin()"
This code is located in TinyUrl.API -> Program.cs 

```csharp
builder.Services.AddCors(options =>
 {
  options.AddPolicy(myCorsPolicyName, policyBuilder =>
  {
      policyBuilder
            .WithOrigins("http://localhost:3000") 
            .AllowAnyHeader()
            .AllowAnyMethod();
   });
  });
```

### 2.2. Configure React application
#### 2.2.1. Install NPM
You need to install Node.js to run your React application. For more information on installing Node.js (https://nodejs.org/en/download/).

#### 2.2.2. Change envrironment variables
You need to specify the WebAPI application address in the environment file. The environment file is located via the path (../tiny-url/src/enviroment/enviroment.js)

Check and, if necessary, make changes to the following block of code:

```javascript
  apiUrl: 'https://localhost:7229/api/',
```


## 3. How to run
### 3.1. How to run WebAPI application
Run the project using the standard Visual Studio tools or the dotnet CLI.

### 3.2. How to run React application
Open the folder with the application in the terminal. The relative path should look like this: `~\ReactApp\tiny-url`.

To start the application, enter the following command:

```powershell
  npm run
```


## 4. Description of the project architecture.
### 4.1. Summary
The application consists of two independent projects - the Web API project and the React project.

### 4.2. Web API project
The application is based on ASP.NET Core Web API and Microsoft SQL Server. Entity Framework Core is used to work with the database. The interaction between the application and the database is done using Command Query Separation (CQS)

The application writes logs to a new file for each run. Logging is based on the Serilog library.

Key functions of the server part:
- CQS
- REST API 
- Swagger
- API documentation 

### 4.3. React application

The solution is divided into the following logical parts:
- **components:** files that contain the UI logic
- **environment:** file that contains environment variables
- **guard:** file that checks the user's rights to visit certain pages
- **models:** files containing data models to be displayed on the UI
- **pages:** files that represent separate pages consisting of other components
- **services:** files that contain business logic (such as token service, url service, user service)
- **storage:** files that responsible for data storage
- **utils:** file that make it easier to work with tokens

## 5. Key features:
ASP.Net Core WebAPI, Entity Framework Core, Microsoft SQL Server, C#, JavaScript, Serilog, Automapper, MediatR, Dependepcy Injection, Swagger, Tailwind, React Hook Form, Axios, ReactJS.

