
FROM mcr.microsoft.com/dotnet/aspnet:7.0-windowsservercore-ltsc2022 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-windowsservercore-ltsc2022 AS build
WORKDIR /src
COPY ["ToDoListWebAPI/ToDoListWebAPI.csproj", "ToDoListWebAPI/"]
RUN dotnet restore "ToDoListWebAPI/ToDoListWebAPI.csproj"
COPY . .
WORKDIR "/src/ToDoListWebAPI"
RUN dotnet build "ToDoListWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoListWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListWebAPI.dll"]