using BusinessLogic.Interfaces.IAuthServices;
using BusinessLogic.Services.AuthServices;
using Entities.Data;
using Entities.Models.Entities;
using Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using System;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Restaurant.Common
{
    public static class ConfigurationHelper
    {

        //Configured Logger
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }


        //Configured ConnectionString to Db
        public static void ConfigureConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ResDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("sqlConnection"));
            });
        }


        //Configured Identity
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Stores.MaxLengthForKeys = 128;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ResDbContext>().AddDefaultTokenProviders().AddSignInManager();
        }

        //Configured Jwt
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<String>("JWTSecretKey")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signinKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }

        //Configured Session
        public static void ConfigureSession(this IServiceCollection services)
        {
            // Add Service Sessions
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "Basket";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }


        //Update Swagger to support jwt authorization
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            services.AddSwaggerGen(c =>
            {
                var fileDoc = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer schene.",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Id="Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                }});
            });
        }



    }
}
