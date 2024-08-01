using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Persistence.DataAccess;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Interfaces.DataAccess.Schedules;
using ApplicationCore.Interfaces.DataAccess.Staffs;
using MediatR;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
      
            // Mediator 
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            // Identity User Lockout
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<HomeNursingContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // default lockout setting
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                // setting signin
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 7;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedAccount = true;
                // Vô hiệu hóa yêu cầu xác thực hai yếu tố
                options.Tokens.AuthenticatorIssuer = null;
            });

            var serviceProvider = services.BuildServiceProvider();
            try
            {
                var dbContext = serviceProvider.GetRequiredService<HomeNursingContext>();
                dbContext.Database.Migrate();
            }
            catch
            {
                throw new Exception("Have something wrong with create context - infratructure");
            }

            return services;
        }
    }
}

