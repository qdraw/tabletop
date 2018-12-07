# Tabletop

Tabletop is a cloud-based, mobile-ready, event tracking system. We use it to see if the table footbal is free.

## Builds
[![Travis](https://img.shields.io/travis/qdraw/tabletop.svg)](https://travis-ci.org/qdraw/tabletop/) [![Build status](https://ci.appveyor.com/api/projects/status/fw7gojff1220kj0r/branch/master?svg=true)](https://ci.appveyor.com/project/qdraw/tabletop/branch/master) [![codecov](https://codecov.io/gh/qdraw/tabletop/branch/master/graph/badge.svg)](https://codecov.io/gh/qdraw/tabletop) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/qdrawmedia)

### Features
  - Display if room __is free__ or _in use_
  - History views per day
  - Display Latest Activity
  - Realtime Magic

### Technical summary
  - .NET Core 2.1
  - ASP.NET MVC
  - Runs on Windows, Linux and Mac OS X
  - Send events from a Arduino with a PIR-Sensor
  - [D3.v4](https://d3js.org/) Javascript Datavisualisation

And of course Tabletop itself is open source with a [public repository ](https://github.com/qdraw/tabletop) on GitHub.

### Installation

Tabletop requires [.NET Core](https://www.microsoft.com/net/core) v2.1 to run.

Install the dependencies
```sh
$ cd tabletop/tabletop
$ dotnet restore
```
  - **Linux and Mac OS X**, Update SQL-Server connection string in `appsettings.Production.json`
  - **Windows:** You can use the localdb for testing

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=tcp:database.database.windows.net,1433;Database=databasename;Persist Security Info=False;User ID=adminusername;Password=adminpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    }
}
```
Run Database migration
```sh
$ dotnet ef database update
```

Add a test user using the SQL command
```sql
INSERT INTO [dbo].[ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible,Bearer)
VALUES ('cc9299c5-03a6-409a-be35-30981acfa7ac','Test Channel','test','true','true','secret')
```
**And finaly run the app**

```sh
$ dotnet run
```

Add some test data
```sh
curl -X POST -F 'status=1' -F 'name=test' -H 'Authorization: Bearer secret'
'http://localhost:5000/api/update'
```
#### Arduino client

I use a [Arduino Ethernet shield](https://www.arduino.cc/en/Reference/Ethernet) with the [Ethernet2](https://github.com/adafruit/Ethernet2) library and a PIR-sensor. To Setup additional libraries check: https://www.arduino.cc/en/Guide/Libraries.

The Arduino code is open source in the [tabletop_client](tabletop_client) folder

Adjust the bearer and server in `tabletop/tabletop_client/tabletop_client.ino`
```cpp
char clientName[] = "test";
char Bearer[] = "secret";
char server[] = "qdraw.nl"; // without http://
// the url will be: http://qdraw.nl/tabletop/api/update
```

To setup the PIR-sensor please check this scheme:
![Tabletop Scheme](tabletop_client/tabletop_scheme.gif "Tabletop Scheme")


#### For production environments...

You can use the environment variable `TABLETOP_SQL`. If there is no variable the app will check `appsettings.json` or `appsettings.Production.json`.

Personaly I run this application on a linux-arm server and use PM2 to manage the process. [PM2](http://pm2.keymetrics.io/) is a production process manager for Node.js but you can be used to manage binary executables.

##### Building for linux-arm
Create a new build for linux-arm
```sh
$ cd tabletop/tabletop
$ ./publish-linux-arm.sh
```
##### Creating a new PM2 instance
This bash script ask for a SQL Server connectionstring
```sh
$ ./new-pm2.sh
Insert here the production SQL string
```
