using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MinimalAPI.Entities;
using MinimalAPI.Services;
using MinimalAPI.Data;

namespace Test.Services
{
    [TestClass]
    public class AdminServiceTest
    {
        private DatabaseContext CreateContextTest()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DatabaseContext(configuration);
        }


        [TestMethod]
        public void TestAdminInsert()
        {
            // Arrange
            var context = CreateContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

            var adm = new Admin();
            adm.UserName = "test user";
            adm.Password = "pass123";
            adm.Profile = "Adm";

            var adminService = new AdminService(context);

            // Act
            adminService.Insert(adm);

            // Assert
            Assert.AreEqual(1, adminService.All(1).Count());
        }
        [TestMethod]
        public void TestSearchById()
        {
            // Arrange
            var context = CreateContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

            var adm = new Admin();
            adm.UserName = "test user";
            adm.Password = "pass12345";
            adm.Profile = "Adm";

            var adminService = new AdminService(context);

            // Act
            adminService.Insert(adm);
            var dbAdmin = adminService.SearchById(adm.Id);

            // Assert
            Assert.AreEqual(1, dbAdmin?.Id);
        }
    }
}