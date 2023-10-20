# TKW Challenge - User Team Management App

## Description

**Challenge - Web App**
- Create a web application that performs CRUD operations for managing users and teams.
- Users can belong to multiple teams.
- The application provides a user-friendly interface for managing users and teams efficiently.

**Key Considerations**

- The web application uses a MySQL database to store user and team information.
- The database structure, including fields and relationships, has been thoughtfully designed for optimal functionality.
- The application has been developed in ASP.NET Core Web App MVC, adhering to best development practices.
- The system is designed to handle multiple users and teams efficiently, making it suitable for production use.


## Connecting to the Database

To connect to the MySQL database, follow these steps:

1. Ensure you have Docker installed on your system.

2. Run a MySQL Docker container with the following command:

   ```
   docker pull mysql
   docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=root -p 3306:3306 -d mysql
   ```

   This command will create a MySQL container with the root password set to "root" and expose the database on port 3306.

3. To create the database, use Entity Framework Core migrations. Run the following commands in your project directory:

   ```
   dotnet-ef migrations add InitialCreate
   dotnet-ef database update
   ```

   This will create the database using the connection string defined in your `appsettings.json`.

4. If you prefer to use a different database configuration, update the connection string in your `appsettings.json` as follows:

   ```json
   "DefaultConnection": "Server=YOUR_MYSQL_SERVER;Port=YOUR_PORT;Database=YOUR_DATABASE_NAME;User=YOUR_USER;Password=YOUR_PASSWORD;",
   ```

   Replace `YOUR_MYSQL_SERVER`, `YOUR_PORT`, `YOUR_DATABASE_NAME`, `YOUR_USER`, and `YOUR_PASSWORD` with your specific MySQL server information.

## Usage

  To run the app, navigate to the project root folder and run:
  ```
   dotnet run
   ```
   By default the app will be running at: https://localhost:7252/


---

[Author - João Batista](https://www.linkedin.com/in/jotapebatista/)

