using Callender.Data;
using Callender.Data.TokenGenerator;
using Callender.Date.PasswordHasher;
using Callender.Date.Repo;
using Callender.Middleware;
using Callender.Model.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callender
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
            //add cors 
            //----------------------
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_Cors",
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            //----------------------
            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<UserContext>(opt =>
               opt.UseSqlServer(Configuration.GetConnectionString("CallenderConection"))
           );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Callender", Version = "v1" });
            });
            //AUTHENTICATION CONFIG
            AuthenticationConfiguration authenticationConfiguration = new();
            Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                    ValidIssuer = authenticationConfiguration.Issuer
                   ,
                    ValidAudience = authenticationConfiguration.Audience
                   ,
                    ValidateIssuer = true
                   ,
                    ValidateAudience = true
                   ,
                    ValidateIssuerSigningKey = true
                   ,
                    ClockSkew = TimeSpan.Zero
                };
            }
                );
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            services.AddSingleton(authenticationConfiguration);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICallenderRepo, CallenderRopo>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<AccessToken>();
            services.AddSingleton<TokenGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Callender v1"));
            }

            app.UseHttpsRedirection();
            //user cors
            app.UseCors("_Cors");

            app.UseAuthentication();
            app.UseRouting();
            
            app.UseAuthorization();
            app.AuthMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
