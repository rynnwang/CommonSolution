using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ifunction.ApiTracking;
using ifunction.ApiTracking.Model;
using ifunction.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.Common.UnitTest
{
    [TestClass]
    public class JsonXmlSerializerUnitTest
    {
        [TestMethod]
        public void Test()
        {
            Dictionary<string, object> item = new Dictionary<string, object>();
            item.Add("key1", 1);
            item.Add("key2", 1.1);
            item.Add("key3", true);
            item.Add("key4", "string1");
            item.Add("key5", new[] { "string1", "string2" });

            item.Add("key6", Guid.NewGuid());
            item.Add("key7", DateTime.Now);
            item.Add("key8", new TimeSpan(10030));

            item.Add("key9", Encoding.UTF8.GetBytes("Hello World"));

            JToken token = JToken.Parse(JsonConvert.SerializeObject(item));

            var y = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(item));

            Assert.IsNotNull(y);
            var xml = JsonXmlSerializer.ToXml(token);

            Assert.IsNotNull(xml);

            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(xml.ToJToken().ToString());
            Assert.IsNotNull(dic);
        }
    }
}
