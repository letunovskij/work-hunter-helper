# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WorkHunter/WorkHunter.Api/WorkHunter.Api.csproj", "WorkHunter/WorkHunter.Api/"]
COPY ["WorkHunter.BackgroundTasks/WorkHunter.BackgroundTasks.csproj", "WorkHunter.BackgroundTasks/"]
COPY ["WorkHunter/WorkHunter.Services/WorkHunter.Services.csproj", "WorkHunter/WorkHunter.Services/"]
COPY ["EmailSender/EmailSender.csproj", "EmailSender/"]
COPY ["WorkHunter/Common/Common.csproj", "WorkHunter/Common/"]
COPY ["WorkHunter/Abstractions/WorkHunter.Abstractions.csproj", "WorkHunter/Abstractions/"]
COPY ["WorkHunter/WorkHunter.Models/WorkHunter.Models.csproj", "WorkHunter/WorkHunter.Models/"]
COPY ["WorkHunter/WorkHunter.Data/WorkHunter.Data.csproj", "WorkHunter/WorkHunter.Data/"]
RUN dotnet restore "./WorkHunter/WorkHunter.Api/WorkHunter.Api.csproj"
COPY . .
WORKDIR "/src/WorkHunter/WorkHunter.Api"
RUN dotnet build "./WorkHunter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WorkHunter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkHunter.Api.dll"]