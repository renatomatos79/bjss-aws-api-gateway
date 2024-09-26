# Use the official .NET Core SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY . .
RUN dotnet restore

# Build the project
RUN dotnet publish -c Release -o out

# Use the smaller runtime image for deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out ./

# Expose port 80 for the application
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "eventsapi.dll"]