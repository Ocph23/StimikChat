using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StimikChatServer.Models;
using StimikChatServer.Models.DataContext;

namespace StimikChatServer
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
            
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(
                  "CorsPolicy",
                  builder => builder
                  .AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
            });
            services.AddSignalR();

            services.Configure<StimikChatDatabaseSettings>(Configuration.GetSection(nameof(StimikChatDatabaseSettings)));
            services.AddSingleton<IStimikChatDatabaseSettings>(sp =>sp.GetRequiredService<IOptions<StimikChatDatabaseSettings>>().Value);
            services.AddScoped(typeof(IChatContext),typeof(ChatContext));
            services.AddScoped(typeof(IConnectionContext), typeof(ConnectionContext));
            services.AddScoped(typeof(IUserContext), typeof(UserContext));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
