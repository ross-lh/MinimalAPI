using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MinimalAPI.Entities;

namespace Test.Entities
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void TestAdminEntity()
        {
            var adm = new Admin();

            // Act
            adm.Id = 1;
            adm.UserName = "test user";
            adm.Password = "pass123";
            adm.Profile = "Adm";

            // Assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("test user", adm.UserName);
            Assert.AreEqual("pass123", adm.Password);
            Assert.AreEqual("Adm", adm.Profile);
        }
    }
}