FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/EllaX.Api.Explorer/EllaX.Api.Explorer.csproj", "src/EllaX.Api.Explorer/"]
COPY ["src/EllaX.Mvc/EllaX.Mvc.csproj", "src/EllaX.Mvc/"]
COPY ["src/EllaX.Logic/EllaX.Logic.csproj", "src/EllaX.Logic/"]
COPY ["src/EllaX.Data/EllaX.Data.csproj", "src/EllaX.Data/"]
COPY ["src/EllaX.Core/EllaX.Core.csproj", "src/EllaX.Core/"]
COPY ["src/EllaX.Clients/EllaX.Clients.csproj", "src/EllaX.Clients/"]
RUN dotnet restore "src/EllaX.Api.Explorer/EllaX.Api.Explorer.csproj"
COPY . .
WORKDIR "/src/src/EllaX.Api.Explorer"
RUN dotnet build "EllaX.Api.Explorer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EllaX.Api.Explorer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EllaX.Api.Explorer.dll"]