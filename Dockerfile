# Utiliza una imagen multi-stage para optimizar el tamaño final
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos de solución y restaura dependencias
COPY ProductsMS.sln ./
COPY ProductMS/ProductMS.csproj ProductMS/
COPY ProductMS.Application/ProductMS.Application.csproj ProductMS.Application/
COPY ProductMS.Commons/ProductMS.Commons.csproj ProductMS.Commons/
COPY ProductMS.Core/ProductMS.Core.csproj ProductMS.Core/
COPY ProductMS.Domain/ProductMS.Domain.csproj ProductMS.Domain/
COPY ProductMS.Infrastructure/ProductMS.Infrastructure.csproj ProductMS.Infrastructure/
COPY ProductMS.Tests/ProductMS.Tests.csproj ProductMS.Tests/
RUN dotnet restore "ProductMS/ProductMS.csproj"

# Copia el resto de los archivos y compila
COPY . .
WORKDIR "/src/ProductMS"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductMS.dll"]
