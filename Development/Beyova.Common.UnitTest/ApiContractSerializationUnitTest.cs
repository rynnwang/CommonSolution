using System;
using Beyova.ProgrammingIntelligence;
using Beyova.ApiTracking;
using Beyova.ApiTracking.Model;
using Beyova.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class ApiContractSerializationUnitTest
    {
        [TestMethod]
        public void Serialization()
        {
            var apiDataContractDefinition = typeof(string);
        }
    }
}
