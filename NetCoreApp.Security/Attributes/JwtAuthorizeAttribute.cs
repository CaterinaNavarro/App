using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreApp.Crosscutting.Exceptions;
using System;

namespace NetCoreApp.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];

            if (user == null) throw new ClientErrorException("Unauthorized", StatusCodes.Status401Unauthorized);
            
        }
    }
}
