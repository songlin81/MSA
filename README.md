# Microservice Service Discovery Patterns with ASP.NET Core

This repo contains Service Discovery patterns with ASP.NET Core using tools like [Consul](https://www.consul.io) and [RabbitMQ](http://www.rabbitmq.com/).

### Patterns Covered
- [Point to Point](ServiceDiscovery/PointToPoint)
- [Local Registry](ServiceDiscovery/LocalRegistry)
- [Self Registration](ServiceDiscovery/SelfRegistration)
- [Health Checks](ServiceDiscovery/HealthChecks)
- [Load Balancing](ServiceDiscovery/LoadBalancing)

### Preview
<p float="left";>
	<img src="https://raw.githubusercontent.com/songlin81/MSA/master/ServiceDiscovery/Tool/consul.jpg" alt="Consul" width="600"/>
</p>
<p float="left";>
	<img src="https://raw.githubusercontent.com/songlin81/MSA/master/ServiceDiscovery/Tool/console.jpg" alt="Console" width="600"/>
</p>


# Polly 
### Resilience policies
```
 dotnet build && dotnet run
```
- [Retry](ResiliencePolicies/RetryPolicy)
- [Circuit Breaker](ResiliencePolicies/CircuitBreakerPolicy)


# Ocelot
- [API Gateway](Ocelot/APIGatewayDemo)
- [JWT Authentication](Ocelot/APIGatewayJWTAuthenticationDemo)
- [API Logging](Ocelot/APIGatewayLoggingDemo)
- [Rate Limit](Ocelot/APIGatewayRateLimitDemo)
- [Qos](Ocelot/APIGatewayQoSDemo)
- [Load Balancing](Ocelot/APIGatewayLBDemo)
- [](Ocelot/)

