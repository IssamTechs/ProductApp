using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Reporting.API.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductApp.API.Extensions
{
    public static class StartupExtensions
    {
        public static void AddSwaggerPage(this IServiceCollection services, string version = "V1")
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(i => i.FullName);
            });
        }

        public static void UseSwaggerPage(this IApplicationBuilder app, string version = "V1")
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/{version}/swagger.json", $"{Assembly.GetEntryAssembly().GetName().Name} Api {version}");
                //c.RoutePrefix = string.Empty;
            });
        }

        public static void AddMvcWithErrorHandlingFilter(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ErrorHandlingFilter());
                options.Filters.Add(typeof(ApiResultMiddleWare));
                options.RespectBrowserAcceptHeader = true;
            }).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
        }

        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            status = "Failed",
                            errorCode = context.Response.StatusCode,
                            errorMessage = contextFeature.Error?.ToString()
                        }).ToString());
                    }
                });
            });
        }
    }
}