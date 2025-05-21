dotnet ef migrations add InitialCreate --project ProductMS.Infrastructure --startup-project ProductMS

dotnet ef database update --project ProductMS.Infrastructure --startup-project ProductMS