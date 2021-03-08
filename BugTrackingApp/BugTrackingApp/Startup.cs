using BugTrackingApp.Domain.Core;
using BugTrackingApp.Domain.Interfaces;
using BugTrackingApp.Infrastructure.Business;
using BugTrackingApp.Infrastructure.Data;
using BugTrackingApp.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BugTrackingApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var connectionString = configuration.GetSection("connectionString")?.Value;//"Server=(localdb)\\mssqllocaldb;Database=productsdb;Trusted_Connection=True;";
            //  services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IRepository<Project>, ProjectRepository>(provider => new ProjectRepository(connectionString));
            services.AddTransient<IRepository<Issue>, IssueRepository>(provider => new IssueRepository(connectionString));
            services.AddTransient<IProjectService, ProjectService>();
            services.AddControllers();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }

}
