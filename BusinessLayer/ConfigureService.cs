using EventManagement.EventRepository;
using EventManagement.UserRepository;
using EventManagement.EventRegistrationFormRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Models;




namespace EventManagement.ConfigureService
{
    public class ConfigureService
    {
        public static void RegisterContext(IServiceCollection builderServices, IConfiguration configuration)
        {
            builderServices.AddDbContext<EventManagementSystemDataBaseContext>(option => option.UseSqlServer(configuration.GetConnectionString("Database")));
        }
        public static void RegisterRepositories(IServiceCollection builderServices)
        {
            builderServices.AddScoped<IEventRepository, EventRepositoryType>();
           
            builderServices.AddScoped<IUserRepository,UserRepositoryType>();

            builderServices.AddScoped<IEventRegistrationFormRepository, EventRegistrationFormRepositoryType>();
        }
      
    }
}
