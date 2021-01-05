# Self Registration

When our server starts up it will contact the registration service to register itself via [Consul](https://www.consul.io/) by HashiCorp.

The following command is enough in most cases to run a single node consul environment(navigate to directory consul_1.9.0_windows_amd64)
```
consul agent -dev
```

Once the API service starts up successfuly, you should be able to see a registration entry in the Consul UI:
http://localhost:8500/ui

Next start the API project. 
```
dotnet run
```

Now for the client project:
```
dotnet run
```

If everything is configured currectly, your client will retrieve the list of registered servers from our registration serivce
AKA Consul. Feel free to start up another instance of the API in another command prompt window.

Next [Health Checks](../HealthChecks/README.MD)