# Docker related parameters

Create a .env file based on the .env.example file in the root of the repo.

# Connection stirng for the API

Run the following commands with the correct parameters:
```
dotnet user-secrets init --project src/Api
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=appdb;Username=app;Password=devpassword" --project src/Api
```