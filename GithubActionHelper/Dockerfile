FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GithubActionHelper/GithubActionHelper.csproj", "GithubActionHelper/"]
RUN dotnet restore "GithubActionHelper/GithubActionHelper.csproj"
COPY . .
WORKDIR "/src/GithubActionHelper"
RUN dotnet build "GithubActionHelper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GithubActionHelper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GithubActionHelper.dll"]
