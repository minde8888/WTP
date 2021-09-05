#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WTP.Api/WTP.Api.csproj", "WTP.Api/"]
COPY ["WTP.Services/WTP.Services.csproj", "WTP.Services/"]
COPY ["WTP.Data/WTP.Data.csproj", "WTP.Data/"]
COPY ["WTP.Domain/WTP.Domain.csproj", "WTP.Domain/"]
RUN dotnet restore "WTP.Api/WTP.Api.csproj"
COPY . .
WORKDIR "/src/WTP.Api"
RUN dotnet build "WTP.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WTP.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WTP.Api.dll"]