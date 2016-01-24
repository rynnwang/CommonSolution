using System;
using Beyova.ProgrammingIntelligence;
using Beyova.ApiTracking;
using Beyova.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class LDAPUnitTest
    {
        [TestMethod]
        public void Test()
        {
            var isAuthenticated = LdapExtension.TryAuthenticate("", "", "");
            Assert.IsTrue(isAuthenticated);
        }
    }
}
