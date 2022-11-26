using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Context;
using ToDoList.Repositories;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using AutoMapper;
using ToDoList.DTO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ToDoList
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
            //Read the Key from Appsettings    
            var appsettingsRead = Configuration.GetSection("AppSettings");

            //add the key to Services    
            services.Configure<AppSettingDTO>(appsettingsRead);
            //JWT Authentication    
            var settings = appsettingsRead.Get<AppSettingDTO>();
            var key = Encoding.ASCII.GetBytes(settings.key);
            var connection = settings.connection;

            //string connection = "data source=localhost\\SQLEXPRESS;initial catalog=todoDB;Trusted_Connection=True;persist security info=True;";
            //string connection = "Server=todolist-demo.database.windows.net;Initial Catalog=todoDB;Persist Security Info=False;User ID=todo-admin;Password=allen123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<TodoDBContext>(options => options.UseSqlServer(connection));

            //JWT Authentication    
            services.AddAuthentication(au =>
            {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // dependancy injection setup
            services.AddScoped<ITodoRepository, TodoRepository>();   // global variable for injection
            services.AddScoped<IUserRepository, UserRepository>();   // global variable for injection

            // Windows Authentication
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Todo List API",
                    Version = "v1",
                    Description = "A simple ASP.NET Core Web API of TodoList",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo List V1");
                x.RoutePrefix = "swagger";
                x.DocumentTitle = "Title Documentation";
                x.DocExpansion(DocExpansion.List);
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
