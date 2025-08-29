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
    public class VehicleServiceTest
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
        public void TestVehicleInsert()
        {
            // Arrange
            var context = CreateContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

            var vehicle = new Vehicle();
            vehicle.Model = "Car Model";
            vehicle.Make = "Car Brand";
            vehicle.Year = 2022;

            var vehicleService = new VehicleService(context);

            // Act
            vehicleService.Insert(vehicle);

            // Assert
            Assert.AreEqual(1, vehicleService.All(1).Count());
        }
        [TestMethod]
        public void TestSearchById()
        {
            // Arrange
            var context = CreateContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

            var vehicle = new Vehicle();
            vehicle.Model = "Car Model";
            vehicle.Make = "Car Brand";
            vehicle.Year = 2022;

            var vehicleService = new VehicleService(context);

            // Act
            vehicleService.Insert(vehicle);
            var dbVehicle = vehicleService.SearchById(vehicle.Id);

            // Assert
            Assert.AreEqual(1, dbVehicle?.Id);
        }
    }
}