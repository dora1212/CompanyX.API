Documentation

1. Download SQL Server Management Studio and install SQL Server.
2. Download CompanyX.API from github, unzip it and open it in visual studio. 
3. Delete ALL the files from folder CompanyX.API.DataAccess\Migrations
3. Open Package Manager Console, change default project to CompanyX.API.DataAccess, run the following commands:
	add-migration initialMigration
	update-database
4. Check the newly created CompanyXDB database in SQL Server management studio. Tables will be empty. Data will be seeded when you run the application. 
5. Run application.
