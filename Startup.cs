﻿using Microsoft.OpenApi.Models;
using SortedCodingTest.Services;
using System.Reflection;

namespace SortedCodingTest
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient<RainfallService>();
            // Caching 
            services.AddDistributedMemoryCache();

            services.AddMvcCore(options =>
            {
                options.EnableEndpointRouting = false;

            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            services.AddAuthorization();
            services.AddControllers();
            services.AddHttpClient();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rainfall Api", Version = "v1" });

                // Include the XML comments in Swagger
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });


            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromHours(12);
            });
        }

        public void ConfigureAppConfiguration(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration
                    .AddJsonFile(@"appsettings.Development.json", false, true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Program>()
                    .Build();
            }
            if (builder.Environment.IsProduction())
            {
                builder.Configuration
                    .AddJsonFile(@"appsettings.json", false, true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Program>()
                    .Build();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rainfall Api"));
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            if (env.IsDevelopment())
            {
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }
            else
            {
                app.UseCors("AllowAny");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        }
    }
}
