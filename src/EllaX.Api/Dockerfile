FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 58924
EXPOSE 44338

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/EllaX.Api/EllaX.Api.csproj", "src/EllaX.Api/"]
RUN dotnet restore "src/EllaX.Api/EllaX.Api.csproj"
COPY . .
WORKDIR "/src/src/EllaX.Api"
RUN dotnet build "EllaX.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EllaX.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EllaX.Api.dll"]