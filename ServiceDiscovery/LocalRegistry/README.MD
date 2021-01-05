# Local Registry

Instead of relying on a single service entry in a configuration file, the application is provided with a pool of service using retry policy from [Polly](https://github.com/App-vNext/Polly) library.

In the server project run
```
dotnet run"
```

Then from a command prompt in the client project folder run the following
```
dotnet run
```

You should see the client print "Trying next server", before successful making a request to your running service.

Next see [Self Registration](../SelfRegistration/README.MD)
