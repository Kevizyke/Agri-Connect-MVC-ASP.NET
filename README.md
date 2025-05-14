# Agri-Energy Connect MVC Application

[![.NET 9 Build Status](https://img.shields.io/badge/.NET-9-blue.svg?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![License: No License](https://img.shields.io/badge/License-No%20License-red.svg?style=for-the-badge)](https://unlicense.org/)

## 🎬 Youtube

[![Watch the video](https://img.shields.io/badge/Watch-YouTube-red?style=for-the-badge&logo=youtube&logoColor=white)]([Link to your YouTube video about the app])

## ✨ Features

**General Features:**

* ✅ **User Authentication:** Secure login and logout functionality for both employee and farmer roles, likely handled by an authentication microservice.
* 🎨 **Responsive Design:** Utilizes Bootstrap for a consistent and responsive user interface across different devices.
* **Theming:** Custom styling applied, including an agricultural-themed navbar and a visually engaging home page with a background image and prominent text.

**Employee Role:**

* 🧑‍💼 **Farmer Registration:** Employees can register new farmers via an API call to a farmer management microservice.
* 📜 **Farmer List:** Employees can view a list of all registered farmers fetched from the farmer management microservice.
* 📦 **Product Management (Filtering):** Employees can filter products based on category, production date range, and the farmer who produced them, utilizing an API call to a product catalog microservice.

**Farmer Role:**

* 🚜 **My Products:** Farmers can add products and view a list of the products they have registered, retrieved via an API call to the product catalog microservice.

**Home Page:**

* 🏠 Displays a welcoming message with the application name "Agri-Energy Connect".
* 🏷️ Includes a subheading: "Connecting Farmers with Clean Energy Solutions."
* 🌄 Features a background image related to agriculture and energy.

## 🛠️ Microservices Architecture

This application follows a microservices architecture, with the MVC application acting as a client to the following backend services:

* 👤 **UserService:** Manages user authentication and user roles. Stores user data in its own SQLite database.
* 📦 **ProductService:** Manages product information, including filtering. Stores product data in its own SQLite database.

The MVC application communicates with these services via their respective RESTful HTTP APIs.

## ⚙️ Technologies Used

![.NET 9](https://img.shields.io/badge/.NET-9-blue?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/.NET%20ASP.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)
![jQuery](https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)

## 🚀 Setup and Installation

Follow these steps to get the application running:

1.  **Clone the Repository:**
    ```bash
    git clone [repository URL]
    cd AgriEnergyConnect
    ```

    The repository is structured with an `AgriEnergyConnectMVC` folder containing the MVC application and a `Microservices` folder containing the source code for the backend microservices.

2.  **Prerequisites:**
    * ✅ [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) installed on your machine.

3.  **🏃 Running the Microservices:**
    You need to run each microservice separately. Navigate to the directory of each microservice within the `Microservices` folder and run the following commands in separate terminal windows:

    ```bash
    cd Microservices/UserService
    dotnet build
    dotnet run
    # Note the port this service runs on (e.g., https://localhost:5001).
    ```

    ```bash
    cd Microservices/ProductService
    dotnet build
    dotnet run
    # Note the port this service runs on (e.g., https://localhost:5002).
    ```

    Ensure that the microservices are running and accessible on their respective ports before starting the MVC application.

4.  **⚙️ Configuring the MVC Application:**
    The MVC application needs to know the base URLs of your running microservices. They are defined in the `Program.cs` file within the `AgriEnergyConnectMVC` folder. You will need to update the `Program.cs` with the correct URLs for each microservice:

    ```csharp
    // AgriEnergyConnectMVC/Program.cs

    builder.Services.AddHttpClient("UserService", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5001"); // Replace with your UserService port
    });

    builder.Services.AddHttpClient("ProductService", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5002"); // Replace with your ProductService port
    });
    ```

    Replace `https://localhost:5001` and `https://localhost:5002` with the actual URLs (including the port) of your running UserService and ProductService microservices.

5.  **🚀 Running the MVC Application:**
    Navigate to the `AgriEnergyConnectMVC` folder and run the following commands:

    ```bash
    cd AgriEnergyConnectMVC
    dotnet build
    dotnet run
    ```

    This will typically launch the MVC application on `http://localhost:5xxx` (the port number will be indicated in the console output).

## 📂 Key Code Locations

* `AgriEnergyConnectMVC/Controllers/`: ⚙️ MVC Controllers responsible for handling user requests and communicating with the backend microservices.
* `AgriEnergyConnectMVC/Models/`: 📊 MVC Models defining the ViewModels used in the MVC application.
* `AgriEnergyConnectMVC/Views/`: 🖼️ Razor Views for rendering the user interface.
* `AgriEnergyConnectMVC/wwwroot/css/site.css`: 🎨 Custom CSS Styles for the MVC application's theming and layout.
* `AgriEnergyConnectMVC/wwwroot/js/site.js`: ✨ JavaScript Functionality for client-side interactions.
* `AgriEnergyConnectMVC/_Layout.cshtml`: 🧱 The main layout file defining the overall structure of the application's pages.
* `Microservices/UserService/`: 👤 Source code for the user management microservice.
* `Microservices/ProductService/`: 📦 Source code for the product management microservice.

Each microservice project will have its own controllers, models, and data access logic for its SQLite database.

## 📄 License

[![License: No License](https://img.shields.io/badge/License-No%20License-red.svg?style=for-the-badge)](https://unlicense.org/)
