using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaphyreProject.Main.Data;
using SaphyreProject.Main.Hubs;
using SaphyreProject.Main.Settings;
using SaphyreProject.Shared.Services;
using System.Linq;

namespace SaphyreProject.Main
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
      services.AddSignalR();
      services.AddControllersWithViews();
      services.AddDbContext<ProfileContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProfileContext")));
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/build";
      });
      services.AddRazorPages();
      services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
      });
      // Custom services
      services.AddScoped<IHubClientFactory, HubClientFactory>();
      services.AddScoped<ProfileHubProxy, ProfileHubProxy>();
      services.Configure<SignalRSettings>(options => Configuration.GetSection("SignalRSettings").Bind(options));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.MapWhen(ctx => ctx.Request.Host.Port == 5002, react =>
      {
        react.UseCors(builder =>
        {
          builder.WithOrigins("https://localhost:5003")
              .AllowAnyHeader()
              .WithMethods("GET", "POST")
              .AllowCredentials();
        });
        if (env.IsDevelopment())
        {
          react.UseDeveloperExceptionPage();
        }
        else
        {
          react.UseExceptionHandler("/Error");
          // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
          react.UseHsts();
        }
        react.UseHttpsRedirection();
        react.UseStaticFiles();
        react.UseSpaStaticFiles();
        react.UseRouting();
        react.UseEndpoints(endpoints =>
        {
          endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
          endpoints.MapHub<ProfileHub>("/profilehub");
        });
        react.UseSpa(spa =>
        {
          spa.Options.SourcePath = "ClientApp";
          if (env.IsDevelopment())
          {
            spa.UseReactDevelopmentServer(npmScript: "start");
          }
        });
      });

      app.MapWhen(ctx => ctx.Request.Host.Port == 5003, blazor =>
      {
        blazor.UseResponseCompression();
        if (env.IsDevelopment())
        {
          blazor.UseDeveloperExceptionPage();
          blazor.UseWebAssemblyDebugging();
        }
        else
        {
          blazor.UseExceptionHandler("/Error");
          // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
          blazor.UseHsts();
        }
        blazor.UseHttpsRedirection();
        blazor.UseBlazorFrameworkFiles();
        blazor.UseStaticFiles();
        blazor.UseRouting();
        blazor.UseEndpoints(endpoints =>
        {
          endpoints.MapRazorPages();
          endpoints.MapControllers();
          endpoints.MapFallbackToFile("index.html");
        });
      });
    }
  }
}
