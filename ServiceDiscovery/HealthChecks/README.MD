# Health Checks

Health checks provide a mechanism for the client to know whether or not it's dependant services are still up and running.
This would prevent unnecessary requests to dead servers. There are various ways that these checks can be implemented. For this
sample, the API service provides a health endpoint accessible at `/api/health/status`.

Make sure you have Consul running:
```
consul agent -dev
```

Next start the API service. Open 2 command terminals and navigate to the location of the API project:
```
dotnet run
```

The service will register itself with Consul to use health checks.You should now see requests being made every 10 seconds or so to `/api/health/status`; also available at the service tab in the Consul UI.

Also, do a `dotnet run` in a command terminal in the directory of the client project. The client will check the health of the service
before make a request for data.

Next [Load Balancing](../LoadBalancing/README.MD)