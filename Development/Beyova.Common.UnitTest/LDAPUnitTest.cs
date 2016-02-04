using System;
using Beyova.ProgrammingIntelligence;
using Beyova.ApiTracking;
using Beyova.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;

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

        [TestMethod]
        public void TestCompile()
        {
            CSharpCodeProvider _provider = new CSharpCodeProvider();
            Sandbox sandbox = new Sandbox("sandbox");
            var assembly = sandbox.AddDynamicAssembly(_provider, new List<string>(new string[] { "System.dll" }), @"
namespace Beyova.Compile
{
    public class Test
    {
        public int Run(int x)
        {
            return x + 1;
        }
    }
}
");
            var type = assembly.GetType("Beyova.Compile.Test");
            var obj = assembly.CreateInstance("Beyova.Compile.Test", false);
            var method = type.GetMethod("Run");
            var result = method.Invoke(obj, (2 as object).AsArray());


        }
    }
}


namespace Beyova.Compile
{
    public class Test
    {
        public int Run(int x)
        {
            return x + 1;
        }
    }
}