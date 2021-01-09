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
- CustomersServices: 	http://localhost:9001/api/values
						http://localhost:9001/api/values/1
- APIGateway: 			http://localhost:9000/customers
						http://localhost:9000/customers/123

## Rate Limit
- API Services:			http://localhost:9001/api/values
- API Gateway:			http://localhost:9000/customers (trigger request within 5 seconds to observe the 429 status code)

## Qos
- API Services:			http://localhost:9001/api/values
- API Gateway:			http://localhost:9000/customers (Reload 3 times the circuit breaker will suspect the next request for predefined 20 seconds)

## Load Balancing
- API Services: 		http://localhost:9001/api/values (Service A)
						http://localhost:9002/api/values (Service B)
- API Gateway			http://localhost:9000/ (Swap between Service A & B)

## Service Discovery Consul
- Consul:				http://localhost:8500/ui/($ consul agent -dev)
- API Service A:		http://localhost:9999/api/values
- API Service B:		http://localhost:9998/api/values
- API Gateway:			http://localhost:9000/api/values (results toggled between ports 9999 and 9998)					

## Swagger
- API Gateway:			http://localhost:9000/swagger/

## Request Aggregation
- API Service:			http://localhost:4873/api/ocelot/9
						http://localhost:4873/api/ocelot/aggrJay
						http://localhost:4873/api/ocelot/consultJay?id=11
						http://localhost:4873/api/ocelot/identityJay?id=12
- API Gateway:			http://localhost:5000/ocelot/9
						http://localhost:5000/ocelot/aggrJay
						http://localhost:5000/ocelot/aggrKim
						http://localhost:5000/ocelot/consultJay?id=111
						http://localhost:5000/ocelot/identityJay?id=222
						http://localhost:5000/ocelot/8001/identityJay
						http://localhost:5000/ocelot/8002/identityJay
						http://localhost:5000/ocelot/ratelimit/999
						http://localhost:5000/ocelot/qos/777
						http://localhost:5000/aggrLeader
						http://localhost:5000/aggrLeaderAdvanced
