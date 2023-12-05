FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AgileX.Api/AgileX.Api.csproj", "AgileX.Api/"]
COPY ["AgileX.Contracts/AgileX.Contracts.csproj", "AgileX.Contracts/"]
COPY ["AgileX.Infrastructure/AgileX.Infrastructure.csproj", "AgileX.Infrastructure/"]
COPY ["AgileX.Application/AgileX.Application.csproj", "AgileX.Application/"]
COPY ["AgileX.Domain/AgileX.Domain.csproj", "AgileX.Domain/"]
RUN dotnet restore "AgileX.Api/AgileX.Api.csproj"
COPY . .
WORKDIR "/src/AgileX.Api"
RUN dotnet build "AgileX.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgileX.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgileX.Api.dll"]