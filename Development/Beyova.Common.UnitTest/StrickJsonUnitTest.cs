using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Beyova.ApiTracking;
using Beyova.ApiTracking.Model;
using Beyova.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class StrickJsonUnitTest
    {
        class ClassA
        {
            public string String { get; set; }

            public float Float { get; set; }

            public double Double { get; set; }

            public decimal Decimal { get; set; }
        }

        class ClassB : ClassA
        {
            public string AnotherString { get; set; }
        }

        [TestMethod]
        public void Test()
        {

            ClassA obj = new ClassB
            {
                String = "string",
                Float = 1.2f,
                Decimal = 1.2M,
                Double = 1.2d,
                AnotherString = "AString"
            };

            var x = StrickJsonSerializer.ToStickJson<ClassA>(obj);
            Assert.IsNotNull(x);


            StringBuilder builder = new StringBuilder();
            JsonWriter writer = new JsonTextWriter(new StringWriter(builder))
            {
                Formatting = Formatting.Indented
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, obj, typeof(ClassA));
            var y = builder.ToString();
            Assert.IsNotNull(y);

            //JsonSerializerInternalWriter
        }


    }
}
