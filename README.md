Generate assets:  `shift` + `command` + `p` => `.NET: Generate Assets For Build And Debug`

Find version: `dotnet info`

After getting a nuget package (no neccesary with vs code): `dotnet restore`

Build migration (inside API): `dotnet ef migrations add InitialCreate  -o Data/Migrations`

Update database (inside API): `dotnet ef database update` -> this will produce the `websocket.db` file in the root of the project.

Open database: Select `Sqlite Open Database` from command palette

Create a new class library (in root): `dotnet new classlib -o Core`

Add these projects to solution: `dotnet sln add Core`

Add dependency to API project (in API): `dotnet add reference ../Infrastructure`

Add dependency to Core (in Infrastructure): `dotnet add reference ../Core`

Make references available in our solution (in root): `dotnet restore`

Drop old db (in root): `dotnet ef database drop -p Infrastructure -s API`

Remove migrations (in root): `dotnet ef migrations remove -p Infrastructure -s API`

Add new migrations (in root): `dotnet ef migrations add InitialCreate -p Infrastructure -s API -o Data/Migrations`

https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-6.0

https://www.youtube.com/watch?v=3PyUjOmuFic

`dotnet watch run`

https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-server

If Omnisharp stops suggesting: `https://stackoverflow.com/questions/29975152/intellisense-not-automatically-working-vscode` or `https://www.youtube.com/watch?v=KJYrRv9cShY`

Authentication:  `https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api`

https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio

https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity

https://docs.microsoft.com/en-us/previous-versions/visualstudio/dn253016(v=vs.111)?redirectedfrom=MSDN


Microsoft.AspNetCore.HttpExtensions