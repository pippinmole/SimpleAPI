FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SimpleAPI/SimpleAPI.csproj", "./"]
RUN dotnet restore "SimpleAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SimpleAPI/SimpleAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleAPI/SimpleAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleAPI.dll"]
