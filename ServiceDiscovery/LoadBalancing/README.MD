# Load Balancing

Leverage RabbitMQ as the load balancing appliance.

### setup RabbitMQ

- set HOMEDRIVE=C:/conf/rabbitmq
- C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.9\sbin>rabbitmq-plugins.bat enable rabbitmq_management
- C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.9\sbin>rabbitmq-server.bat start
  - Open at http://localhost:15672 with acouunt guest/guest

### Demo

Open a command prompt and navigate to the location of the Sales Service project. Now run:
```
dotnet run
```
Open another command prompt and start another instance of the Sales Service project.
Open a command prompt and navigate to the location of the client project. Now run:
```
dotnet run
```
The client sends 2 requests to RabbitMQ. Each request being send to a different instance to been processed.