# Agri-Energy Connect MVC Application

## Overview

Agri-Energy Connect is a web application built with ASP.NET Core MVC designed to connect farmers with clean energy solutions and facilitate the management of agricultural products. The application caters to two primary user roles: **Employees** and **Farmers**, each with specific functionalities.

This application communicates with several backend microservices (also built with ASP.NET Core Web API) to handle data persistence and specific business logic. Each microservice manages its own SQLite database.

## Youtube


## Features

**General Features:**

* **User Authentication:** Secure login and logout functionality for both employee and farmer roles, likely handled by an authentication microservice.
* **Responsive Design:** Utilizes Bootstrap for a consistent and responsive user interface across different devices.
* **Theming:** Custom styling applied, including an agricultural-themed navbar and a visually engaging home page with a background image and prominent text.

**Employee Role:**

* **Farmer Registration:** Employees can register new farmers via an API call to a farmer management microservice.
* **Farmer List:** Employees can view a list of all registered farmers fetched from the farmer management microservice.
* **Product Management (Filtering):** Employees can filter products based on category, production date range, and the farmer who produced them, utilizing an API call to a product catalog microservice.

**Farmer Role:**

* **My Products:** Farmers can add products and view a list of the products they have registered, retrieved via an API call to the product catalog microservice.

**Home Page:**

* Displays a welcoming message with the application name "Agri-Energy Connect".
* Includes a subheading: "Connecting Farmers with Clean Energy Solutions."
* Features a background image related to agriculture and energy.

## Microservices Architecture

This application follows a microservices architecture, with the MVC application acting as a client to the following (example) backend services:

* **UserService:** Manages user authentication and user roles. Stores user data in its own SQLite database.
* **ProductService:** Manages product information, including filtering. Stores product data in its own SQLite database.

The MVC application communicates with these services via their respective RESTful HTTP APIs.

## Technologies Used

* **ASP.NET Core MVC (.NET 9):** The framework for building the web application.
* **C#:** The primary programming language for both the MVC application and the microservices.
* **HTML, CSS, JavaScript:** For the front-end structure, styling, and basic interactivity.
* **Bootstrap:** A CSS framework for responsive layout and styling.
* **jQuery:** A JavaScript library for DOM manipulation and AJAX.
* **System.Net.Http.IHttpClientFactory:** Used in the MVC application to make HTTP requests to the microservices.
* **Newtonsoft.Json:** For serializing and deserializing JSON data exchanged with the microservices.
* **Microsoft.AspNetCore.Authentication:** For handling user authentication within the MVC application (potentially relying on tokens issued by a UserService).
* **Microsoft.AspNetCore.Authorization:** For managing user roles and access control within the MVC application.
* **SQLite:** A lightweight, file-based database used by each microservice.
* **Google Fonts (Playfair Display, Oswald):** For custom typography.

## Setup and Installation

This application requires running both the MVC frontend and the backend microservices.

**1. Clone the Repository:**

```bash
git clone [repository URL]
cd AgriEnergyConnect
```

The repository is structured with an `AgriEnergyConnectMVC` folder containing the MVC application and a `Microservices` folder containing the source code for the backend microservices.

**2. Prerequisites:**

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) installed on your machine.

**3. Running the Microservices:**

You need to run each microservice separately. Navigate to the directory of each microservice within the `Microservices` folder and run the following commands:

```bash
cd Microservices/UserService
dotnet build
dotnet run
# Note the port this service runs on (usually indicated in the console).

cd ../ProductService
dotnet build
dotnet run
# Note the port this service runs on.
```

Ensure that the microservices are running and accessible on their respective ports before starting the MVC application.

**4. Configuring the MVC Application:**

The MVC application needs to know the base URLs of your running microservices. They are defined in the `Program.cs` file within the `MVC` folder. You will need to update the `Program.cs` with the correct URLs for each microservice:

```csharp
builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001"); // UserService port
});

builder.Services.AddHttpClient("ProductService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5002"); // ProductService port
});
```

Replace `https://localhost:5xxx` with the actual ports your microservices are running on.

**5. Running the MVC Application:**

Navigate to the `MVC` folder and run the following commands:

```bash
cd MVC
dotnet build
dotnet run
```

This will typically launch the MVC application on `http://localhost:5xxx` (the port number will be indicated in the console output).

## Key Code Locations

* **`AgriEnergyConnectMVC/Controllers/`:** Contains the MVC controllers responsible for handling user requests and communicating with the backend microservices.
* **`AgriEnergyConnectMVC/Models/`:** Defines the ViewModels used in the MVC application, often representing data transferred to and from the microservices.
* **`AgriEnergyConnectMVC/Views/`:** Contains the Razor views for rendering the user interface.
* **`AgriEnergyConnectMVC/wwwroot/css/site.css`:** Holds the custom CSS styles for the MVC application.
* **`AgriEnergyConnectMVC/wwwroot/js/site.js`:** Contains custom JavaScript functionality for the MVC application.
* **`AgriEnergyConnectMVC/_Layout.cshtml`:** The main layout file for the MVC application.
* **`Microservices/UserService/`:** Contains the source code for the user management microservice.
* **`Microservices/ProductService/`:** Contains the source code for the product management microservice.

Each microservice project will have its own controllers, models, and potentially data access logic for its SQLite database.

## License

No License.
