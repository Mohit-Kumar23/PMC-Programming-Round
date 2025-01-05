# PMC-Programming-Round
Source code for the programming test round).

The code is developed as per the requirement of 2nd Round interview for the position of Senior Software Engineer (.Net).

The Code is developed by keeping the scope limited and API model is created for easy interaction.

To Run the code do the following:
1) Clone the repository
2) Load the "InventorySystem.csproj"
3) Build the Project to ensure no errors are shown in your system.
4) After successful build, Run the code.
5) Swagger UI will open in the default browser
6) 3 Endpoints will be visible as per the requirement.
7) Give the necessary input to run any Http Request.
8) Output will be shown just below in the Response section.

NOTE: For Adding new items I have assumed, that ItemID will be 0 or NULL when passed as request. Also for Expensive Item functionality
if no price or 0 price is send then minimum price for expensive item is assumed to be 100.

For the Database backend I have used https://www.freesqldatabase.com/
To see the database, do the following:
1) Login to https://www.phpmyadmin.co/
2) Following are the credentials:

Host: sql12.freesqldatabase.com
Database name: sql12755325
Database user: sql12755325
Database password: 9VeTSMthWp
Port number: 3306

The connection string is also mentioned in Web.config file.

About the project dependencies
1) The project is built with .Net Framework 4.8.1
2) For connecting with remote SQL Database, I have installed MySql.Data in the project
3) For dependency inject I have installed Microsoft.Extensions.DependencyInjection
4) For swagger I hae installed Swashbuckle.