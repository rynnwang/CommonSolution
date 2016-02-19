using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.RestApi;

namespace Beyova.RestClientGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "CommonServiceRestClient";
            var outputFolder = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, "../../../Beyova.CommonAdminService/");
            RestApiClientGenerator generator = new RestApiClientGenerator("Beyova.CommonAdminService", name);
            var filePath = Path.Combine(outputFolder, name + ".cs");
            //generator.GenerateCode<OnlineSchoolServiceCore>(filePath);
        }
    }
}
