using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ifunction.ApiTracking;
using ifunction.ApiTracking.Model;
using ifunction.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace ifunction.Common.UnitTest
{
    [TestClass]
    public class RawHttpRequestUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string raw = @"
GET https://www.telerik.com/UpdateCheck.aspx?isBeta=False HTTP/1.1
User-Agent: Fiddler/4.6.0.2 (.NET 4.0.30319.42000; WinNT 10.0.10240.0; zh-CN; 4xAMD64)
Pragma: no-cache
Host: www.telerik.com
Accept-Language: zh-CN
Referer: http://fiddler2.com/client/4.6.0.2
Accept-Encoding: gzip, deflate
Connection: close
";

            var request = raw.CreateHttpWebRequestByRaw();
            var result = request.SmartReadResponseAsText();

            Assert.IsNotNull(result);

        }
    }
}
