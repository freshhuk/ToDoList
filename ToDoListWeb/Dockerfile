# Этап для сборки ASP.NET Core приложения
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Копируем файл проекта
COPY ["ToDoListWeb.csproj", "ToDoListWeb/"]
RUN dotnet restore "ToDoListWeb/ToDoListWeb.csproj"

# Копируем остальные файлы
COPY . .

# Переходим в папку проекта
WORKDIR "/src/ToDoListWeb"

# Собираем проект без использования выходной директории
RUN dotnet build "ToDoListWeb.csproj" -c Release

# Этап для настройки SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest-windows AS sqlserver

ENV ACCEPT_EULA=Y
#ENV SA_PASSWORD=YourStrongPassword
ENV MSSQL_PID=Express
EXPOSE 1433

# Этап для создания финального образа с ASP.NET Core и SQL Server
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final

# Создаем рабочую директорию
WORKDIR /app

# Копируем скомпилированные файлы из другой папки
COPY D:/PublishProject/ToDoListProd .

EXPOSE 80
EXPOSE 443

# Запускаем веб-приложение
ENTRYPOINT ["dotnet", "ToDoListWeb.dll"]

