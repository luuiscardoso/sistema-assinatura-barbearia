#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["APIAssinaturaBarbearia/APIAssinaturaBarbearia.csproj", "APIAssinaturaBarbearia/"]
COPY ["APIAssinaturaBarbearia.Application/APIAssinaturaBarbearia.Application.csproj", "APIAssinaturaBarbearia.Application/"]
COPY ["APIAssinaturaBarbearia.Domain/APIAssinaturaBarbearia.Domain.csproj", "APIAssinaturaBarbearia.Domain/"]
COPY ["APIAssinaturaBarbearia.CrossCutting/APIAssinaturaBarbearia.CrossCutting.csproj", "APIAssinaturaBarbearia.CrossCutting/"]
COPY ["APIAssinaturaBarbearia.Infrastructure/APIAssinaturaBarbearia.Infrastructure.csproj", "APIAssinaturaBarbearia.Infrastructure/"]
RUN dotnet restore "./APIAssinaturaBarbearia/APIAssinaturaBarbearia.csproj"
COPY . .
WORKDIR "/src/APIAssinaturaBarbearia"
RUN dotnet build "./APIAssinaturaBarbearia.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./APIAssinaturaBarbearia.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIAssinaturaBarbearia.dll"]