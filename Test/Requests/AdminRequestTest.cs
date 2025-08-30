using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalAPI.DTO;
using MinimalAPI.ModelViews;
using NuGet.Frameworks;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdminRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TestAdminRequests()
        {
            // Arrange
            var loginDTO = new LoginDTO
            {
                UserName = "admin1",
                Password = "password1"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

            // Act
            var response = await Setup.client.PostAsync("/admins/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var loggedAdmin = JsonSerializer.Deserialize<LoggedAdmin>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.IsNotNull(loggedAdmin?.UserName ?? "");
            Assert.IsNotNull(loggedAdmin?.Profile ?? "");
            Assert.IsNotNull(loggedAdmin?.Token ?? "");
        }
    }
}