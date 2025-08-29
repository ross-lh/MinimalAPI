using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MinimalAPI.Entities;

namespace Test.Entities
{
    [TestClass]
    public class VehicleTest
    {
        [TestMethod]
        public void TestVehicleEntity()
        {
            var vehicle = new Vehicle();

            // Act
            vehicle.Id = 1;
            vehicle.Model = "Model X";
            vehicle.Make = "Brand Y";
            vehicle.Year = 2020;

            // Assert
            Assert.AreEqual(1, vehicle.Id);
            Assert.AreEqual("Model X", vehicle.Model);
            Assert.AreEqual("Brand Y", vehicle.Make);
            Assert.AreEqual(2020, vehicle.Year);
        }
    }
}