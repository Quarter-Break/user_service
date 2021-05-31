using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Database.Configurations;
using UserService.Database.Contexts;
using UserService.Database.Converters;
using UserService.Database.Models.Dto;
using UserService.Messaging;
using UserService.Messaging.Options;
using UserService.Models;
using UserService.Security;
using UserService.Services;

namespace UserService
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Inject controllers.
            services.AddControllers();

            // Inject database context.
            var connection = Configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<UserContext>(
                options => options.UseSqlServer(connection));

            var origin = Configuration.GetValue<string>("AppSettings:CorsPolicy");
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins(origin)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                      });
            });

            // Json settings.
            services.AddMvc().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddFluentValidation();

            // Configure strongly typed settings object.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Configure DI for application services.
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Add swagger.
            services.AddSwaggerGen();

            // Inject converters.
            services.AddScoped<IDtoConverter<User, UserRequest, UserResponse>, UserDtoConverter>();

            // Inject services.
            services.AddTransient<IUserService, UserModelService>();

            // Inject validators.
            services.AddTransient<IValidator<UserRequest>, UserValidator>();

            // Inject RabbitMQ.
            if (Configuration.GetValue<bool>("RabbitMq:Enabled"))
            {
                services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
                services.AddTransient<IUserUpdateSender, UserUpdateSender>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserContext context)
        {
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserService");
                // Serve the swagger UI at the app's root
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
