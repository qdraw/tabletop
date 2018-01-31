# Tabletop

Tabletop is a cloud-based, mobile-ready, event tracking system. We use it to see if the table footbal is free.

  - .NET Core 2
  - ASP.NET MVC
  - Runs on Windows, Linux and Mac OS X
  - Send events from a Arduino with a PIR-Sensor
  - D3 Datavisualisation
  - Magic

And of course Tabletop itself is open source with a [public repository ](https://github.com/qdraw/tabletop) on GitHub.

### Installation

Tabletop requires [.NET Core](https://www.microsoft.com/net/core) v2 to run.

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

Add some test data
```sh
curl -X POST -F 'status=1' -F 'name=test' -H 'Authorization: Bearer secret'
'http://localhost:5000/api/update'
```
**And finaly run the app**

```sh
$ dotnet run
```

#### For production environments...

Is use a pm2 wrapper on a Raspberry Pi
```sh
$ ./publish-linux-arm.sh
$ ./new-pm2.sh
Insert here the production SQL string
```
