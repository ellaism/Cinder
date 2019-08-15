FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Cinder.UI/Cinder.UI.csproj", "src/Cinder.UI/"]
RUN dotnet restore "src/Cinder.UI/Cinder.UI.csproj"
COPY . .
WORKDIR "/src/src/Cinder.UI"
RUN dotnet build "Cinder.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cinder.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinder.UI.dll"]