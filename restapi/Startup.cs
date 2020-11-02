using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using restapi.Filters;
using restapi.Models;

namespace restapi
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
         services.Configure<HotelInfo>(
            Configuration.GetSection("Info"));

         // use in-memory db for dev+test
         // TODO: swap out for real db
         services.AddDbContext<HotelApiDbContext>(
            options => options.UseInMemoryDatabase("bfostdb"));

         services.AddControllers();

         services
            .AddMvc(options =>
            {
               options.Filters.Add<JsonExceptionFilter>();
               options.Filters.Add<RequireHttpsOrCloseAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
         services.AddRouting(options => options.LowercaseUrls = true);

         services.AddSwaggerDocument();

         services.AddApiVersioning(options=>
         {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new MediaTypeApiVersionReader();
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionSelector =
               new CurrentImplementationApiVersionSelector(options);
         });

         services.AddCors(options =>
         {
            options.AddPolicy("AllowMyApp",
               policy => policy.WithOrigins("https://myreactapp.example.com")); // policy.AllowAnyOrigin() - for testing. REMOVE IN PROD!
         });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3();
         }
         else
         {
            app.UseHsts();
         }

         app.UseCors("AllowMyApp"); // policyName from ConfigureServices()

         // clients connecting to http port get a redirect response pointed to https port (307)
         // removed due to RequireHttpsOrCloseAttribute Filter
         //app.UseHttpsRedirection();

         app.UseRouting();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });
      }
   }
}
