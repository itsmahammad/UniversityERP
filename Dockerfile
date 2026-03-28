FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/UniversityERP.API/UniversityERP.API.csproj", "src/UniversityERP.API/"]
COPY ["src/UniversityERP.Application/UniversityERP.Application.csproj", "src/UniversityERP.Application/"]
COPY ["src/UniversityERP.Domain/UniversityERP.Domain.csproj", "src/UniversityERP.Domain/"]
COPY ["src/UniversityERP.Infrastructure/UniversityERP.Infrastructure.csproj", "src/UniversityERP.Infrastructure/"]

RUN dotnet restore "src/UniversityERP.API/UniversityERP.API.csproj"

COPY . .

RUN dotnet publish "src/UniversityERP.API/UniversityERP.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UniversityERP.API.dll"]
