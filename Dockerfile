# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY BestPicksHub/BestPicksHub.csproj BestPicksHub/
RUN dotnet restore BestPicksHub/BestPicksHub.csproj
COPY . .
WORKDIR /src/BestPicksHub
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Create directory for SQLite database
RUN mkdir -p /app/data

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/bestpickshub.db"

EXPOSE 8080
ENTRYPOINT ["dotnet", "BestPicksHub.dll"]
