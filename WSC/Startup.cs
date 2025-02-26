using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WSC.Data;
using WSC.Services;
using WSC.Models;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace WSC
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllers();

            // Configure SQLite database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            // Configure authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/accessdenied";
                    options.SlidingExpiration = true;
                });

            services.AddAuthorization();

            // Register app services
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICollectionService, CollectionService>();
            services.AddSingleton<SeriesMapper>();
            services.AddScoped<ProtectedSessionStorage>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Ensure database is created
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Startup>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}