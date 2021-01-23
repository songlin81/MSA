# CQRS (Command Query Responsibility Segregation) 

## MongoDB
- Service Name: MongoDB
- Data Directory:	C:\Program Files\MongoDB\Server\4.4\data\
- MongoDB Compass to create Database
	- Database Name: CustomerDB
	- Collection Name: Customers

## RabbitMQ
- http://localhost:15672/ (guest/guest)
- cd "C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.9\sbin" && rabbitmq-plugins.bat enable rabbitmq_management

## Scaffold
- File > New > Project > ASP.NET Core Web Application > Name: CustomerApi > API
- Package Manager Console
	- install-package Microsoft.EntityFrameworkCore
	- install-package Microsoft.EntityFrameworkCore.Sqlite
	- install-package Microsoft.EntityFrameworkCore.Tools.DotNet
	- install-package Microsoft.Extensions.Configuration
	- install-package Microsoft.Extensions.Configuration.Json
	- install-package Microsoft.AspNetCore.All
	- install-package mongocsharpdriver
	- install-package RabbitMQ.Client
- CustomerApi.csproj
	- Add: <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.2" />
- cmd > cd .\CustomerApi
	- dotnet ef migrations add CreateDatabase
	- dotnet ef database update
- Demo >dotnet run
	- Now listening on: http://localhost:58752
	- Postman:
		- Get:	http://localhost:58752/api/Customers
		- Post:	http://localhost:58752/api/Customers
			- Body (raw JSON): 
				{
				  "phones": [
					{
					  "type": 0,
					  "areacode": 321,
					  "number": 0003010
					}
				  ],
				  "email": "georgi@michales.com",
				  "name": "Georgia Michales",
				  "age": 12
				}
		- Get: http://localhost:58752/api/Customers/1
