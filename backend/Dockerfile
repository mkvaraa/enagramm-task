# ---------- Build stage ----------
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
    WORKDIR /src
    
    # Copy csproj files first for better Docker layer caching:
    # if only source code changes, restore won't re-run
    COPY MoneyTransfer.sln .
    COPY src/MoneyTransfer.Api/MoneyTransfer.Api.csproj                       src/MoneyTransfer.Api/
    COPY src/MoneyTransfer.Application/MoneyTransfer.Application.csproj       src/MoneyTransfer.Application/
    COPY src/MoneyTransfer.Domain/MoneyTransfer.Domain.csproj                 src/MoneyTransfer.Domain/
    COPY src/MoneyTransfer.Infrastructure/MoneyTransfer.Infrastructure.csproj src/MoneyTransfer.Infrastructure/
    COPY tests/MoneyTransfer.UnitTests/MoneyTransfer.UnitTests.csproj         tests/MoneyTransfer.UnitTests/
    
    RUN dotnet restore src/MoneyTransfer.Api/MoneyTransfer.Api.csproj
    
    # Copy all sources and publish
    COPY . .
    RUN dotnet publish src/MoneyTransfer.Api/MoneyTransfer.Api.csproj \
        -c Release -o /app/publish /p:UseAppHost=false
    
    # ---------- Runtime stage ----------
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
    WORKDIR /app
    COPY --from=build /app/publish .
    
    ENV ASPNETCORE_URLS=http://+:8080
    EXPOSE 8080
    
    ENTRYPOINT ["dotnet", "MoneyTransfer.Api.dll"]