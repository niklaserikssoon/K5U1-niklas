FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["CloudNativeInventory.Api/CloudNativeInventory.Api.csproj", "CloudNativeInventory.Api/"]
RUN dotnet restore "CloudNativeInventory.Api/CloudNativeInventory.Api.csproj"

COPY . .
RUN dotnet publish "CloudNativeInventory.Api/CloudNativeInventory.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

USER app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CloudNativeInventory.Api.dll"]
