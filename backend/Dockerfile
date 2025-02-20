FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.sln ./APIAssinaturaBarbearia/

COPY ./APIAssinaturaBarbearia/APIAssinaturaBarbearia/APIAssinaturaBarbearia.csproj ./APIAssinaturaBarbearia/APIAssinaturaBarbearia/
COPY ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.Application/APIAssinaturaBarbearia.Application.csproj ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.Application/
COPY ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.Domain/APIAssinaturaBarbearia.Domain.csproj ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.Domain/
COPY ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.Infrastructure/APIAssinaturaBarbearia.Infrastructure.csproj ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.Infrastructure/
COPY ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.CrossCutting/APIAssinaturaBarbearia.CrossCutting.csproj ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.CrossCutting/
COPY ./APIAssinaturaBarbearia/TestesAPI/TestesAPI.csproj ./APIAssinaturaBarbearia/TestesAPI/

RUN dotnet restore ./APIAssinaturaBarbearia/APIAssinaturaBarbearia.sln

COPY . .

RUN dotnet publish ./APIAssinaturaBarbearia/ -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

EXPOSE 8080

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "APIAssinaturaBarbearia.dll" ]

