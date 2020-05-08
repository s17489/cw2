using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using cw2.Services;

using cw2.Middlewares;
using cw2.Models;

namespace cw2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEnrollmentDbServices, EnrollmentDbServices>();
            services.AddControllers();

           /* services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<LoggingMiddleware>();
           /* app.UseSwagger();
            app.UseSwaggerUI(
                config => {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
                });*/
            app.Use(async (context, next) => {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("ERROR [Middleware]: Nie podano numeru indexu");
                    return;
                }
               
                var index = context.Request.Headers["Index"].ToString();
                
                StudentDbService trueStudent = new StudentDbService();
                
                if (!trueStudent.trueStudent(index))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Brak indexu w bazie");
                    return;
                }
                await next();
            });

            

             app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
