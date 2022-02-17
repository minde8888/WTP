using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WTP.Api.Configuration;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Data.Repositorys;
using WTP.Domain.Entities.Auth;
using WTP.Domain.Entities.Roles;
using WTP.Domain.Entities.Settings;
using WTP.Services.Services;
using WTP.Services.Services.AppMapper;

namespace WTP.Api
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
            services.AddAutoMapper(typeof(ApplicationMapper));
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(Configuration["ConnectionStrings:LocalConnectionString"]));

            services.AddIdentity<ApplicationUser, ApplicationRole>(o => o.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = Configuration["JwtConfig:Issuer"],
                ValidAudience = Configuration["JwtConfig:Audience"],

                // Allow to use seconds for expiration of token
                // Required only when token lifetime less than 5 minutes
                // THIS ONE
                ClockSkew = TimeSpan.Zero,
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailReposidory, MailReposidory>();

            services.AddScoped(typeof(DbContext), typeof(AppDbContext));
            services.AddScoped(typeof(IManagerRepository), typeof(ManagerRepository));
            services.AddScoped(typeof(IEmployeesRepository), typeof(EmployeesRepository));
            services.AddScoped(typeof(IPostRepository), typeof(PostRepository));
            services.AddScoped(typeof(IProjectRepository), typeof(ProjectRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IEmailPassword), typeof(EmailPassword));
            services.AddScoped(typeof(IProgressPlan), typeof(ProgressPlanRepository));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));    

            services.AddScoped<AuthService>();
            services.AddScoped<ImagesService>();
            services.AddScoped<ManagerService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<ProgressPlanService>();

            services.AddTransient<IAdminRepository, AdminRepository>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WTP.Api", Version = "v1" });
            });

            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {                    
                    ReferenceHandler = ReferenceHandler.Preserve           
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Codepedia-Custom-Header-Response", "Satinder singh");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                await next();
            });

            app.UseCors(options => options.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WTP.Api v1"));
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });

            if (env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}