using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
using restapi.Infrastructure;
using restapi.Models;
using restapi.Services;

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

         // AddScoped = new instance of DefaultRoomService created for every request
         // vs. Singleton, which is only created once
         // EF objects like DbContext uses Scoped lifetime
         // so, every service that interacts with the DbContext needs to be Scoped.
         services.AddScoped<IRoomService, DefaultRoomService>();

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
               options.Filters.Add<LinkRewritingFilter>();
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

         services.AddAutoMapper(
            options => options.AddProfile<MappingProfile>());
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
