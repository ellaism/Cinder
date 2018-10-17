FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 53082
EXPOSE 44347

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/EllaX.UI.Web/EllaX.UI.Web.csproj", "src/EllaX.UI.Web/"]
RUN dotnet restore "src/EllaX.UI.Web/EllaX.UI.Web.csproj"
COPY . .
WORKDIR "/src/src/EllaX.UI.Web"
RUN dotnet build "EllaX.UI.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "EllaX.UI.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EllaX.UI.Web.dll"]