# MongoDB for the query side of the pattern

## Creating the Query Side
	- Models

## Setting up the Event Handler
	- Publisher bean:	AMQPEventPublisher
	- Subscriber bean:	CustomerMessageListener

## Setting up the Commands
	- Commands

## Testing the New Controller
	- Run the application, data will be persisted to the SQLite database;
	- Then, the customer_created event will be published,he RabbitMQ queue will receive the event;
	- And finally, when the event is received at the listener, process the MongoDB data persisting.
	- All in an asynchronous way.
	- http://localhost:58752/api/Customers
	- http://localhost:58752/api/Customers/getone/3
	- http://localhost:58752/api/Customers/songlin81@gmail.com
