FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["StimikChat/StimikChat.csproj", "StimikChat/"]
RUN dotnet restore "StimikChat/StimikChat.csproj"
COPY . .
WORKDIR "/src/StimikChat"
RUN dotnet build "StimikChat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StimikChat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD  ["dotnet", "StimikChat.dll"]