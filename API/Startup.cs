using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Infrastructure.Data;
using API.Helpers;
using API.Middleware;
using API.Extensions;
using API.Hubs;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();

            services.AddApplicationServices();
            services.AddSwaggerDocumentation(); 
            services.AddDbContext<UserContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy
                        .WithOrigins("http://localhost:3000", "https://localhost:3000", "https://localhost:5001", "null") // ATT!! remove null before production
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); 
                });
            });
            services.AddSignalR();
        }        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwaggerDocumentation();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessagingHub>("/messagingHub");

            });
        }
    }
}
