using EventManagementAPI.DTO;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.EventService
{
    public interface IEventService
    {
        Task<EventDTO> CreateEventAsync(CreateEventDTO createEventDTO);
        Task<bool> DeleteEventAsync(int eventId);
        Task<EventDTO> GetEventByIdAsync(int id);
        Task<List<ParticipantDTO>> GetEventParticipantsAsync(int eventId);
        Task<List<EventDTO>> GetEventsAsync();
        Task<List<EventDTO>> GetRegisteredEventsByUserIdAsync(int userId);

        Task<List<EventDTO>> GetEventsByOrganizerAsync(int userId);
        Task<List<EventDTO>> GetUpcomingEventsAsync();
        Task<bool> TogglePublishEventAsync(int eventId);
        Task<string> RegisterForEventAsync(int eventId, EventRegistrationDTO registrationDTO);
        Task<string> UnregisterFromEventAsync(int eventId, EventUnRegistrationDTO unregistrationDTO);
        Task<bool> IsUserRegisteredForEventAsync(int eventId, int userId);
    }
}
