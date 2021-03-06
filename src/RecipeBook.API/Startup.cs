﻿using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.API.Extensions;
using RecipeBook.API.Middlewares;
using RecipeBook.API.Models;
using RecipeBook.Data.CosmosDb;
using RecipeBook.Data.Manager;
using RecipeBook.Manager;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace RecipeBook.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddCors();

            services.Configure<DocumentDbOptions>(Configuration.GetSection("DocumentDbOptions"));
            services.Configure<SignCredentials>(Configuration.GetSection("SignCredentials"));

            services.AddTransient<IRecipeBookDataManager, RecipeBookDataManager>();
            services.AddSingleton<ICurrentUserContext, CurrentUserProvider>();
            services.AddSingleton<ICurrentUser, CurrentUserProvider>();

            services.AddAutoMapper(typeof(RecipeBookRequestHandler).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(RecipeBookRequestHandler).GetTypeInfo().Assembly);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["SignCredentials:TokenAuthority"],
                            ValidAudience = Configuration["SignCredentials:TokenAuthority"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration["SignCredentials:TokenSecret"]))
                        };
                    });

            services.AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;

                    fv.RegisterValidatorsFromAssembly(typeof(RecipeBookRequestHandler).GetTypeInfo().Assembly);
                    fv.RegisterValidatorsFromAssembly(typeof(Startup).GetTypeInfo().Assembly);
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });
            app.UseGlobalExceptionHandler();
            app.UseAuthentication();
            app.UseMiddleware<CurrentUserMiddleware>();
            app.UseHttpsRedirection();            
            app.UseMvc();
        }
    }
}
