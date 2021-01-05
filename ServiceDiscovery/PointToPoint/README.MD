# Point to Point

Here is conventional Point to Point services: a simple API built with ASP .NET Core and a HTTP API client. 
The dependent services is based on the configuration file which is inspected at deployment time.

```
dotnet run
```

- http://localhost:5000/swagger/
- POST http://localhost:5000/api/parts (RAW: Json)
  -- {"id":68489,"title":"FM01","partNumber":"DNM-3393","startTime":"2021-08-01T09:40:17.716299+08:00"}
  -- Response's location will give: http://localhost:5000/api/parts/68489

Next See [Local Registry](../LocalRegistry/README.MD)
