#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BicycleCompany.BLL/BicycleCompany.BLL.csproj", "BicycleCompany.BLL/"]
COPY ["BicycleCompany.DAL/BicycleCompany.DAL.csproj", "BicycleCompany.DAL/"]
COPY ["BicycleCompany.Models/BicycleCompany.Models.csproj", "BicycleCompany.Models/"]
RUN dotnet restore "BicycleCompany.BLL/BicycleCompany.BLL.csproj"
COPY . .
WORKDIR "/src/BicycleCompany.BLL"
RUN dotnet build "BicycleCompany.BLL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BicycleCompany.BLL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BicycleCompany.BLL.dll"]