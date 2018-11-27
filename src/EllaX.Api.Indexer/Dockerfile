FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/EllaX.Api.Indexer/EllaX.Api.Indexer.csproj", "src/EllaX.Api.Indexer/"]
COPY ["src/EllaX.Mvc/EllaX.Mvc.csproj", "src/EllaX.Mvc/"]
COPY ["src/EllaX.Logic/EllaX.Logic.csproj", "src/EllaX.Logic/"]
COPY ["src/EllaX.Data/EllaX.Data.csproj", "src/EllaX.Data/"]
COPY ["src/EllaX.Core/EllaX.Core.csproj", "src/EllaX.Core/"]
COPY ["src/EllaX.Clients/EllaX.Clients.csproj", "src/EllaX.Clients/"]
COPY ["src/EllaX.Logic.Indexing/EllaX.Logic.Indexing.csproj", "src/EllaX.Logic.Indexing/"]
RUN dotnet restore "src/EllaX.Api.Indexer/EllaX.Api.Indexer.csproj"
COPY . .
WORKDIR "/src/src/EllaX.Api.Indexer"
RUN dotnet build "EllaX.Api.Indexer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EllaX.Api.Indexer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EllaX.Api.Indexer.dll"]