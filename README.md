# ToDoList

## 📌 Introduction
This project is written on a multi-tier architecture. Serves to track your tasks, there is also registration using the jwt token. Local MSSQL database is used.

## 🛠 Technologies Used
+ C#
+ .Net
+ SQL
+ Entity Framework
## 🗂 Repository Structure
+ ```ToDoListWebAPI```: This is an API where task management is implemented (Creation, deletion, updating).
+ ```ToDoListWebDomain```: All base models of the project are stored here.
+ ```ToDoListWebInfrastructure```: Contains a database context implementation and a database context interface.
+ ```ToDoListWebServices```: Contains authorization and authentication using the jwt token. As well as changing your credentials.
+ ```ToDoListWebUI```: This is where the entire client side is located. All necessary cshtml pages. Implemented sending and processing requests to the server side via httpclient.
+ ```UnitTests```: Contains unit tests for the project.
