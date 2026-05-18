FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["K5U1-niklas.csproj", "./"]
RUN dotnet restore "K5U1-niklas.csproj"

COPY . .
RUN dotnet publish "K5U1-niklas.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

USER app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "K5U1-niklas.dll"]
