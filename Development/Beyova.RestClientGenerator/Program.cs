using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.CommonService.DataAccessController;
using Beyova.CommonServiceInterface;
using Beyova.ProgrammingIntelligence;
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
            var x = generator.GenerateCodeByType(typeof(IAuthenticationProfileService<,,,>));
            //var generics = typeof(UserInfoAccessController<,,>).GetGenericArguments();
            //foreach (var one in generics)
            //{
            //    var c = one.GetGenericParameterConstraints();
            //}

        }
    }
}
