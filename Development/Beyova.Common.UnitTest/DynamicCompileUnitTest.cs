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
    public class DynamicCompileUnitTest
    {
        [TestMethod]
        public void TestCompile()
        {
            CSharpCodeProvider _provider = new CSharpCodeProvider();
            Sandbox sandbox = new Sandbox("sandbox");
            var assembly = sandbox.CreateDynamicAssembly(_provider, new List<string>(new string[] { "System.dll" }), null, null, @"
    public class Test
    {
        public int Run(int x)
        {
            return x + 1;
        }
    }
");
            var obj = assembly.CreateInstance("Test");
            var method = obj.GetType().GetMethod("Run");
            var result = method.Invoke(obj, (2 as object).AsArray());


        }
    }
}