dotnet ef migrations add addWeight

export ASPNETCORE_ENVIRONMENT="Production"
dotnet ef database update
