#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Web/RestaurantsPlatform.Web/RestaurantsPlatform.Web.csproj", "Web/RestaurantsPlatform.Web/"]
COPY ["Services/RestaurantsPlatform.Services.Messaging/RestaurantsPlatform.Services.Messaging.csproj", "Services/RestaurantsPlatform.Services.Messaging/"]
COPY ["Data/RestaurantsPlatform.Data/RestaurantsPlatform.Data.csproj", "Data/RestaurantsPlatform.Data/"]
COPY ["Data/RestaurantsPlatform.Data.Common/RestaurantsPlatform.Data.Common.csproj", "Data/RestaurantsPlatform.Data.Common/"]
COPY ["RestaurantsPlatform.Common/RestaurantsPlatform.Common.csproj", "RestaurantsPlatform.Common/"]
COPY ["Data/RestaurantsPlatform.Data.Models/RestaurantsPlatform.Data.Models.csproj", "Data/RestaurantsPlatform.Data.Models/"]
COPY ["Web/RestaurantsPlatform.Web.Infrastructure/RestaurantsPlatform.Web.Infrastructure.csproj", "Web/RestaurantsPlatform.Web.Infrastructure/"]
COPY ["Services/RestaurantsPlatform.Services.Mapping/RestaurantsPlatform.Services.Mapping.csproj", "Services/RestaurantsPlatform.Services.Mapping/"]
COPY ["Services/RestaurantsPlatform.Services.Data/RestaurantsPlatform.Services.Data.csproj", "Services/RestaurantsPlatform.Services.Data/"]
COPY ["Web/RestaurantsPlatform.Web.ViewModels/RestaurantsPlatform.Web.ViewModels.csproj", "Web/RestaurantsPlatform.Web.ViewModels/"]
COPY ["RestaurantsPlatform.Web.Common/RestaurantsPlatform.Web.Common.csproj", "RestaurantsPlatform.Web.Common/"]
COPY ["Services/RestaurantsPlatform.Services/RestaurantsPlatform.Services.csproj", "Services/RestaurantsPlatform.Services/"]
COPY ["Data/RestaurantPlatform.Seed/RestaurantsPlatform.Seed.csproj", "Data/RestaurantPlatform.Seed/"]
RUN dotnet restore "Web/RestaurantsPlatform.Web/RestaurantsPlatform.Web.csproj"
COPY . .
WORKDIR "/src/Web/RestaurantsPlatform.Web"
RUN dotnet build "RestaurantsPlatform.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantsPlatform.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantsPlatform.Web.dll"]

RUN /root/.dotnet/tools/dotnet-ef migrations add InitialMigrations