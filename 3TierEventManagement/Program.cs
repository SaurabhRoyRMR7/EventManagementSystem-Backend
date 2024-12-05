
//using EventManagement.EventRepository;
using EventManagement.ConfigureService;
using EventManagement.EventService;
using EventManagementAPI.EventRegistrationFormService;
//using EventManagement.UserRepository;
using EventManagement.UserService;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using EventManagement.EventRegistrationFormService;

namespace _3TierEventManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddScoped<IEventRepository, EventRepository>();
            //builder.Services.AddScoped<IUserRepository, UserRepository>();
            ConfigureService.RegisterContext(builder.Services, builder.Configuration);
            ConfigureService.RegisterRepositories(builder.Services);
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<IEventRegistrationFormService, EventRegistrationFormServiceType>();

            builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
       });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")  // Allow Angular frontend
                          .AllowAnyHeader()                      // Allow any headers
                          .AllowAnyMethod();                     // Allow any HTTP methods (GET, POST, etc.)
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddDbContext<EventManagementSystemDataBaseContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowAngularApp");
            app.MapControllers();

            app.Run();
        }
    }
}
