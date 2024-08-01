using System.Security.Claims;
using System.Text;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Interfaces.DataAccess.Schedules;
using ApplicationCore.Interfaces.DataAccess.Staffs;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                return services.AddDbContext<HomeNursingContext>(options => options.UseSqlServer(connectionString)); // Specify the migrations assembly
            }
            else
            {
                return services.AddDbContext<HomeNursingContext>(options => options.UseInMemoryDatabase("HomeNursing"));
            }
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IScheduleRepository, ScheduleRepository>();

            services.AddScoped<IStaffRepository, StaffRepository>();

            return services;
        }

        public static IServiceCollection AddHandlingToken(this IServiceCollection services, IConfiguration configuration)
        {

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return services;
        }

        public static async Task SeedDataAdmin(this IServiceProvider serviceProvider, IServiceProvider services)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // Ensure the admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Check if the admin user exists
            var adminUser = await userManager.FindByEmailAsync("nguyennhat070596@gmail596.com");
            if (adminUser == null)
            {
                // Create the admin user
                adminUser = new IdentityUser
                {
                    UserName = "nhat_nm",
                    Email = "nguyennhat070596@gmail.com",
                };
                var result = await userManager.CreateAsync(adminUser, "Visaothe13_");

                if (result.Succeeded)
                {
                    // Assign the admin role to the user
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    // Add admin claims
                    await userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.Role, "Admin"));
                }
            }
        }
    }
}

