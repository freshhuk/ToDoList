using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebDomain.Domain.Models;
using ToDoListWebDomain.Domain.Entity;
using ToListWebUI.HttpServisec;
using RichardSzalay.MockHttp;
using System.Security.Cryptography;


namespace UnitTests
{

    public class UIAPIHttpResultServicesTest
    {




        [Fact]
        public async Task AddTaskDbAsyncTest()
        {
            //Arrange

            var mockHttp = new MockHttpMessageHandler();

            // фейковый HttpClient для успешного запроса
            mockHttp.When("http://localhost:5133/api/Task/AddTaskDb")
                .Respond(HttpStatusCode.OK, new StringContent("successful"));

            // Создайте экземпляр вашего класса, передав фейковый HttpClient
            var httpClient = new HttpClient(mockHttp);


            var mockLogger = new Mock<ILogger<APIToDoListHttpServices>>();

            var controller = new APIToDoListHttpServices(httpClient, mockLogger.Object);


            //Act
            var model = new ToDoTask()
            {
                Id = 1,
                NameTask = "Test",
                DescriptionTask = "Test",
                Status = "In progress",
                TaskTime = DateTime.Now,
            };
            var result = await controller.AddTaskDbAsync(model);

            //Assert
            Assert.Equal("successful", result);

        }
        [Fact]
        public async Task DeleteTaskDbAsyncTest()
        {
            //Arrange

            var mockHttp = new MockHttpMessageHandler();
            // фейковый HttpClient для успешного запроса
            mockHttp.When("http://localhost:5133/api/Task/DeleteTaskDb")
                .Respond(HttpStatusCode.OK, new StringContent("successful"));

            // Создайте экземпляр вашего класса, передав фейковый HttpClient
            var httpClient = new HttpClient(mockHttp);

            var mockLogger = new Mock<ILogger<APIToDoListHttpServices>>();

            var controller = new APIToDoListHttpServices(httpClient, mockLogger.Object);

            //Act
            var result = await controller.DeleteTaskDbAsync(1);
            //Assert
            Assert.Equal("successful", result);
        }
        [Fact]
        public async Task ChangeTaskDbAsyncTest()
        {
            //Arrange

            var mockHttp = new MockHttpMessageHandler();
            // фейковый HttpClient для успешного запроса
            mockHttp.When("http://localhost:5133/api/Task/ChangeTaskDb")
                .Respond(HttpStatusCode.OK, new StringContent("successful"));

            // Создайте экземпляр вашего класса, передав фейковый HttpClient
            var httpClient = new HttpClient(mockHttp);

            var mockLogger = new Mock<ILogger<APIToDoListHttpServices>>();

            var controller = new APIToDoListHttpServices(httpClient, mockLogger.Object);

            //Act
            var chamgemodel = new ChangeTaskModel()
            {
                Id = 2,
                TaskName = "test",
                TaskDescription = "test",
                TaskStatus = "test",
                TaskData = DateTime.Now,
            };
            var result = await controller.ChangeTaskDbAsync(chamgemodel);
            //Assert
            Assert.Equal("successful", result);
        }
    }
}
