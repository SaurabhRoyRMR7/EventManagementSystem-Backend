
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
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventManagement.Services;

namespace _3TierEventManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureService.RegisterContext(builder.Services, builder.Configuration);
            ConfigureService.RegisterRepositories(builder.Services);
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<IEventRegistrationFormService, EventRegistrationFormServiceType>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;  // Set to true in production for secure connections
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))  // Use builder.Configuration here
        };
    });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            //Log.Logger = new LoggerConfiguration()
            //   .ReadFrom.Configuration(builder.Configuration)
            //   .WriteTo.Console()
            //   .WriteTo.File("logs/MyAppLog.txt")
            //   .CreateLogger();
            //// Set Serilog as the logging provider
            //// This will also replace default logging provider with Serilog
            //builder.Host.UseSerilog();
            //builder.Services.AddDbContext<EventManagementSystemDataBaseContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();  
            app.UseAuthorization();
          

            app.UseCors("AllowAngularApp");
            app.MapControllers();

            app.Run();
        }
    }
}
