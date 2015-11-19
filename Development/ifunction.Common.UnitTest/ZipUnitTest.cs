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
    public class ZipUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, byte[]> filesToZip = new Dictionary<string, byte[]>();
            filesToZip.Add("a.txt", Encoding.UTF8.GetBytes("Hello zip!"));
            filesToZip.Add("foldera/a.txt", Encoding.UTF8.GetBytes("Hello zip in folder!"));

            filesToZip.ZipToPath("D:\\test.zip");
            Assert.IsNotNull(filesToZip);
        }
    }
}
