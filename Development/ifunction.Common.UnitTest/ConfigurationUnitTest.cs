using System;
using ifunction.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ifunction.Common.UnitTest
{
    [TestClass]
    public class ConfigurationUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var jsonConfigurationReader = new JsonConfigurationReader();

            Assert.IsNotNull(jsonConfigurationReader);
            Assert.IsTrue(jsonConfigurationReader.SettingsCount > 0);
        }
    }
}
