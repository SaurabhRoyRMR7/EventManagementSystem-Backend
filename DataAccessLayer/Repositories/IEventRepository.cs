// IEventRepository.cs
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.EventRepository
{
    public interface IEventRepository
    {
        Task<EventDTO> CreateEventAsync(CreateEventDTO createEventDTO);
        Task<bool> DeleteEventAsync(int eventId);
        Task<Event> GetEventByIdAsync(int eventId);
        Task<List<Event>> GetEventsAsync();
        //Task<List<Event>> GetEventsByLocationAsync(string location);
        Task<List<Event>> GetUpcomingEventsAsync();
        Task<List<Event>> GetEventsByOrganizerAsync(int userId);
        Task<List<Event>> GetRegisteredEventsByUserIdAsync(int userId);
        //Task<bool> EventExistsAsync(int eventId);
        //Task<bool> OrganizerExistsAsync(int organizerId);

        Task<bool> TogglePublishEventAsync(int eventId);
        Task<string> RegisterForEventAsync(int eventId, EventRegistrationDTO registrationDTO);
        Task<string> UnregisterFromEventAsync(int eventId, EventUnRegistrationDTO unregistrationDTO);

        Task<List<ParticipantDTO>> GetEventParticipantsAsync(int eventId);
        Task<bool> IsUserRegisteredForEventAsync(int eventId, int userId);
        Task<bool> IsOrganizerExistsAsync(int organizerId);
    }

}