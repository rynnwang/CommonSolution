using System;
using System.Collections.Generic;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    [ApiContract("v1")]
    public interface IConfigurationProvider
    {
        [ApiOperation("Configuration", HttpConstants.HttpMethod.Get)]
        string GetConfiguration(Guid? userKey);
    }
}
