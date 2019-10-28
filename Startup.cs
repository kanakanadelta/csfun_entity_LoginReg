using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using LoginReg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoginReg
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        
        public IConfiguration Configuration {get;}

            // Here we will "inject" the default IConfiguration service, which will bind to appsettings.json by default
            // and then assign it to the Configuration property.  This happens at the startup of our application.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {

            string mySqlConnection = "server=localhost;userid=root;password=root;port=3306;database=entitylogin;SslMode=None";

            //inject the context we made
            services.AddDbContext<RegContext>(options => options.UseMySql(mySqlConnection));

            // Now we may use the connection string itself, bound to Configuration, to pass the required connection
            // // // credentials to MySQL
            // services.AddDbContext<RegContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"]));

            services.AddSession();
	        services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
	        app.UseStaticFiles();
            app.UseMvc();    // make sure UseMvc comes last!!
        }
    }
}
