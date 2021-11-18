using Alura.ListaLeitura.HttpClients;
using Alura.ListaLeitura.Seguranca;
using Alura.WebAPI.WebApp.Formatters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Alura.WebAPI.WebApp
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
            services.AddControllersWithViews();

            //Frans adição
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AuthDB"));
            });


            ////Autenticação esquema Identity
            //services.AddIdentity<Usuario, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 3;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireLowercase = false;
            //}).AddEntityFrameworkStores<AuthDbContext>();








            //IHttpContextAccessor faz a adção do cabeçalho de autorização
            services.AddHttpContextAccessor();


            //Autenticação esquema Cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/Usuario/Login";
                });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Usuario/Login";
            //});










            //HttpClient
            services.AddHttpClient<LivroApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44391/api/");
                //client.BaseAddress = new Uri("https://localhost:6000/api/");
            });

            services.AddHttpClient<AuthApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44389/api/");
                //client.BaseAddress = new Uri("https://localhost:5000/api/");
            });
            

            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new LivroCsvFormatter());
            }).AddXmlSerializerFormatters();
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
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}