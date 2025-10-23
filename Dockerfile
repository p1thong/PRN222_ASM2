# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files
COPY ["ASM1.WebMVC/ASM1.WebMVC.csproj", "ASM1.WebMVC/"]
COPY ["ASM1.Service/ASM1.Service.csproj", "ASM1.Service/"]
COPY ["ASM1.Repository/ASM1.Repository.csproj", "ASM1.Repository/"]

# Restore dependencies
RUN dotnet restore "ASM1.WebMVC/ASM1.WebMVC.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/ASM1.WebMVC"
RUN dotnet build "ASM1.WebMVC.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "ASM1.WebMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Install SQL Server tools (optional, for debugging)
# RUN apt-get update && apt-get install -y curl apt-transport-https && \
#     curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
#     curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
#     apt-get update && \
#     ACCEPT_EULA=Y apt-get install -y msodbcsql18 mssql-tools18

# Expose ports
EXPOSE 80
EXPOSE 443

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "ASM1.WebMVC.dll"]

