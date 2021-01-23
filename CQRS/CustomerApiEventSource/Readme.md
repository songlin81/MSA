# Event Sourcing

## Project Architecture
- The aggregations take care of summarizing the data to the repository interface, the one which stores the events in an Event Store (in-memory dictionary).
- An Event Bus layer supplies the features needed to send the events asynchronously from one side to the others.
- The Event Handlers save the customers to the Mongo database.
- Mongo is going to be the database to supply data for the read model, because the NoSQL adapts better to the query side.

## Project structure
- Commons: it’ll store common interfaces for the bus components, and repositories, as well as constants, exceptions, etc.
- CQRSLite:frame for CQRS
- ReadModels: it’ll contain the services to provide reading features for the queries
- WriteModels: the domain, commands, events, event store for the command side.
- Services: contains the CustomerService.

## The Write Models
- Aggregate the command’s data and store it to the event store as events.
- Each operation over the session must be committed. At the end of this operation, the CQRSLite framework publishes the event as a message to the IEventPublisher object it has injected.

## The Read Models
- It constitutes the Mongo entities and the Mongo repository.

## Demo
- http://localhost:58752/api/Customers?email=songlin81@gmail.com
