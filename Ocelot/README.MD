# Ocelot

## API Gateway
```
dotnet run
```
- APIGateway: 			http://localhost:9000/customers
						http://localhost:9000/api/products
- CustomersAPIServices: http://localhost:9001/api/customers/3
- ProductsAPIServices:	http://localhost:9002/api/products/

## JWT Authentication
```
dotnet run
```
- Author Server:		http://localhost:9009
						test account: admin/admin
- CustomersAPIServices: http://localhost:9001/api/customers/ (authorized)
						http://localhost:9001/api/customers/1		
- APIGateway: 			http://localhost:9000/customers (authorized)
						http://localhost:9000/customers/1
- Console:				
						Sending Request to /customers, without token.
						Result : Unauthorized

						Send Request to /customers , with token.
						Result : OK
						["Customer 1","Customer 2"]

						No Auth Service Here
						Send Request to /customers/1
						Result : OK
						Customer - 1
						
## Logging
