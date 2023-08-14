# Этап для сборки ASP.NET Core приложения
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY ["D:/PublishProject/ToDoListProd", "."]

# Этап для настройки SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest-windows AS sqlserver

ENV ACCEPT_EULA=Y
#ENV SA_PASSWORD=YourStrongPassword
ENV MSSQL_PID=Express
EXPOSE 1433

# Этап для создания финального образа с ASP.NET Core и SQL Server
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
EXPOSE 443
