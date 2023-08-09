#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ToDoListWeb/ToDoListWeb.csproj", "ToDoListWeb/"]
RUN dotnet restore "ToDoListWeb/ToDoListWeb.csproj"
COPY . .
WORKDIR "/src/ToDoListWeb"
RUN dotnet build "ToDoListWeb.csproj" -c Release -o /app/build

# ¬ этом блоке устанавливаем базовый образ SQL Server и создаем нашу базу данных
FROM mcr.microsoft.com/mssql/server:2022-latest

# ”станавливаем переменные окружени€ дл€ базы данных
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=YourStrong!Passw0rd

#  опируем скрипты дл€ создани€ таблиц и данных
COPY init.sql /usr/src/app/

# ¬ыполн€ем скрипты дл€ создани€ таблиц и данных при запуске контейнера
CMD /opt/mssql/bin/sqlservr & /usr/src/app/init.sql
