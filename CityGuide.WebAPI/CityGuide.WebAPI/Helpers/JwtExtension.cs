using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CityGuide.WebAPI.Helpers
{
    public static class JwtExtension
    {
        public static void AddApplicationError(this HttpResponse httpResponse, string message)
        {
            httpResponse.Headers.Add("Application-Error",message);
            httpResponse.Headers.Add("Access-Control-Allow-Origin","*");
            httpResponse.Headers.Add("Access-Control-Expose-Header","Application-Error");
        }
    }
}
