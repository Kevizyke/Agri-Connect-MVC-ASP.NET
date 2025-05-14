# Agri-Energy Connect Application

[![.NET 9 Build Status](https://img.shields.io/badge/.NET-9-blue.svg?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![License: No License](https://img.shields.io/badge/License-No%20License-red.svg?style=for-the-badge)](https://unlicense.org/)

## 🎬 YouTube Video

[![Watch the video](https://img.shields.io/badge/Watch-YouTube-red?style=for-the-badge&logo=youtube&logoColor=white)]([Link to YouTube video about the app])

## ✨ Key Features

Agri-Energy Connect offers these main functionalities:

**For All Users:**

* ✅ **Secure Login:** Keeps your account safe.
* 📱 **Easy to Use Design:** Works well on computers, tablets, and phones.
* 🎨 **Agricultural Theme:** Features a design that reflects farming and nature.

**For Employees:**

* 🧑‍💼 **Register New Farmers:** Allows adding new farmers to the system.
* 📜 **Farmer Directory:** A simple list of all registered farmers.
* 🔍 **Find Products:** Easily search for products by type, date, and farmer.

**For Farmers:**

* 🚜 **Manage My Products:** Add new products and see a list of your existing ones.

**Home Page:**

* 🏠 Welcomes you to "Agri-Energy Connect".
* 🏷️ Briefly explains: "Connecting Farmers with Clean Energy Solutions."
* 🌄 Features a relevant background image.

## ⚙️ How the System Works (Simplified)

Agri-Energy Connect uses a main website and separate "tools" working behind the scenes. These tools help manage different parts of the application and store information.

* **👤 User Service:** Handles who can log in and their roles (employee or farmer).
* **📦 Product Service:** Manages all the details about the products.

The main website talks to these tools to show you information and let you do things within the application.

## 🛠️ Technologies Used

Here are the main technologies that power Agri-Energy Connect:

* **.NET 9:** A strong framework for building web applications.
* **C#:** The primary programming language.
* **HTML, CSS, JavaScript:** Standard languages for creating websites.
* **Bootstrap:** Helps make the website look good on different devices.
* **jQuery:** A tool that makes the website more interactive.
* **SQLite:** A simple way for each "tool" to store its own information.
* **Google Fonts:** Special fonts for better readability and style.

## 🚀 Getting Started (For Developers)

If you want to run this application on your computer:

1.  **Get the Code:** Download the project files.
    ```bash
    git clone [repository URL]
    cd AgriEnergyConnect
    ```

2.  **Install Software:** Make sure you have the .NET 9 software installed ([https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)).

3.  **Start the Backend Tools:** Open separate command prompts and run each of the following:

    ```bash
    cd Microservices/UserService
    dotnet build
    dotnet run
    # Note the web address (URL) shown here.
    ```

    ```bash
    cd Microservices/ProductService
    dotnet build
    dotnet run
    # Note the web address (URL) shown here.
    ```

4.  **Tell the Website the Tool Addresses:** Go to the `AgriEnergyConnectMVC` folder and open the `Program.cs` file. Find the sections that look like this and replace the example addresses with the ones you noted in the previous step:

    ```csharp
    // AgriEnergyConnectMVC/Program.cs

    builder.Services.AddHttpClient("UserService", client =>
    {
        client.BaseAddress = new Uri("YOUR_USER_SERVICE_URL");
    });

    builder.Services.AddHttpClient("ProductService", client =>
    {
        client.BaseAddress = new Uri("YOUR_PRODUCT_SERVICE_URL");
    });
    ```

5.  **Run the Website:** Finally, in the `AgriEnergyConnectMVC` folder, run:

    ```bash
    dotnet build
    dotnet run
    ```

    The website should open in your browser (usually at `http://localhost:5xxx`).

## 🛠️ Existing login credentials

**Employee**
username: employee1@example.com
password: Employee123!

**Farmer**
username: farmer1@example.com
password: Farmer123!

## 📂 Where to Find Things in the Code

* `AgriEnergyConnectMVC/Controllers/`: Handles user actions and gets data.
* `AgriEnergyConnectMVC/Models/`: How the application's data is structured.
* `AgriEnergyConnectMVC/Views/`: Files that determine what the user sees.
* `AgriEnergyConnectMVC/wwwroot/css/site.css`: Instructions for the website's appearance.
* `AgriEnergyConnectMVC/wwwroot/js/site.js`: Code that makes the website interactive.
* `AgriEnergyConnectMVC/_Layout.cshtml`: The basic structure of all web pages.
* `Microservices/UserService/`: Code for managing user information.
* `Microservices/ProductService/`: Code for managing product information.

## 📄 License

[![License: No License](https://img.shields.io/badge/License-No%20License-red.svg?style=for-the-badge)](https://unlicense.org/)
