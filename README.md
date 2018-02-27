DasKeyboard.Q
=============

A .NET Core library for accessing the [Das Keyboard Q REST API][1].

**WARNING:** This is a work-in-progress. The code is only theoretical, meaning
it hasn't been tested. Use at your own risk.

## Usage

### Create a client that uses password credentials

```c#
var client = new Client
{
    AuthenticationMode = AuthenticationMode.Password,
    Credentials = new NetworkCredential("user@example.com", "Passw0rd!"),
};
```

### Create a client that uses client credentials

```c#
var client = new Client
{
    AuthenticationMode = AuthenticationMode.ClientCredentials,
    Credentials = new NetworkCredential(
        "FQg6sYYcTyWTk2YK3qlBWy8k",  // client_id
        "5ulXkuK84cILsQMsIJl2usyK"), // client_secret
};
```

### Create a client that accesses the local API

```c#
var client = new Client
{
    AuthenticationMode = AuthenticationMode.None,
    ApiEndPoint = Client.LocalEndPoint,
};
```

### Get a list of registered devices

```c#
foreach(var device in await client.GetDevices())
{
    Console.WriteLine($"PID: {device.Pid}, Description: {device.Description}");
}
```

### Create a signal

```c#
var signal = new Signal
{
    Name = "Apple Stock increase",
    Pid = "DK5QPID",
    ZoneId = "KEY_A",
    Color = "#008000",
};

await client.CreateSignal(signal); // This also updates the signal object.

Console.WriteLine($"ID: {signal.Id}, Name: {signal.Name}");
```

### Update a signal

```c#
signal.IsRead = true;

await client.UpdateSignal(signal);

Console.WriteLine($"ID: {signal.Id}, Read: {signal.IsRead}");
```

### Delete a signal

```c#
await client.DeleteSignal(signal);

// or delete by Id
await client.DeleteSignal(469359);
```

### Get authorized client apps

```c#
foreach(var appName in await client.GetAuthorizedClients())
{
    Console.WriteLine(appName);
}
```

### Revoke an authorized client app

```c#
await client.RevokeAuthorizedClient("IFTTT");
```

[1]: https://github.com/DasKeyboard/q/blob/master/q-api-doc.md
