using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreApp.Crosscutting.Exceptions;
using NetCoreApp.Infrastructure.Data;
using System;
using System.Net;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace NetCoreApp.API.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddExceptionModule(this IServiceCollection @this, IHostEnvironment environment)
        {
            @this
               .AddProblemDetails(setup =>
               {
                   setup.IncludeExceptionDetails = (_context, _exception) => !environment.IsDevelopment();

                   setup.GetTraceId = (context) => Guid.NewGuid().ToString();

                   setup.OnBeforeWriteDetails = (context, problemDetail) =>
                   {
                       problemDetail.Instance = context.Request.Path;
                   };

                   setup.Map<ClientErrorException>(e => new Microsoft.AspNetCore.Mvc.ProblemDetails
                   {
                       Title = e.Message,
                       Detail = e.Thrower(),
                       Type = e.GetType().FullName.Replace('.', '-'),
                       Status = e.StatusCode
                   });

                   setup.Map<Exception>(e => new Microsoft.AspNetCore.Mvc.ProblemDetails
                   {
                       Title = e.Message,
                       Detail = e.Thrower(),
                       Type = e.GetType().FullName.Replace('.', '-'),
                       Status = (int)HttpStatusCode.InternalServerError
                   });
               });

            return @this;
        }

        public static IServiceCollection AddSwaggerModule(this IServiceCollection @this)
        {
            @this.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreApp API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the BearerScheme\n\n\n" +
                    "Enter 'Bearer' [space] and your token in the text input below",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                             Scheme = "oauth2",
                             Name = "Bearer",
                             In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return @this;
        }

    }
}
