using EventManagement.EventRepository;
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<EventDTO> CreateEventAsync(CreateEventDTO createEventDTO)
        {
            // Validate input data
            if (createEventDTO == null)
            {
                throw new ArgumentNullException(nameof(createEventDTO), "Event data cannot be null.");
            }

            // Validate if Organizer exists
            var organizerExists = await _eventRepository.IsOrganizerExistsAsync(createEventDTO.OrganizerId);
            if (!organizerExists)
            {
                throw new ArgumentException("Organizer not found.");
            }

           
            var eventDTO = await _eventRepository.CreateEventAsync(createEventDTO);
            return eventDTO;
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            // Check if the event exists
            var eventExists = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventExists == null)
            {
                throw new ArgumentException("Event not found.");
            }

            // Delete the event using the repository
            return await _eventRepository.DeleteEventAsync(eventId);
        }

        public async Task<EventDTO> GetEventByIdAsync(int id)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(id);
            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found.");
            }

            return new EventDTO
            {
                EventId = eventEntity.EventId,
                OrganizerId = eventEntity.OrganizerId,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                Location = eventEntity.Location,
                Price = eventEntity.Price,
                IsPublished = (bool)eventEntity.IsPublished
            };
        }

        public async Task<List<ParticipantDTO>> GetEventParticipantsAsync(int eventId)
        {
            // Ensure the event exists
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found.");
            }

            // Get the participants for the event
            return await _eventRepository.GetEventParticipantsAsync(eventId);
        }
        public async Task<List<EventDTO>> GetEventsAsync()
        {
            // Fetch all events using the repository
            var events = await _eventRepository.GetEventsAsync();

            // If no events are found, you can throw an exception or return an empty list
            if (events == null || !events.Any())
            {
                throw new ArgumentException("No events found.");
            }

            // Map the Event entities to EventDTOs
            var eventDTOs = events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                OrganizerId = e.OrganizerId,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                Price = e.Price,
                IsPublished = (bool)e.IsPublished
            }).ToList();

            return eventDTOs;
        }

        public async Task<List<EventDTO>> GetRegisteredEventsByUserIdAsync(int userId)
        {
            // Fetch events that the user is registered for using the repository
            var events = await _eventRepository.GetRegisteredEventsByUserIdAsync(userId);

            // If no events are found, return an empty list or throw an exception depending on your requirements
            if (events == null || !events.Any())
            {
                return new List<EventDTO>(); // Return an empty list if no events are found
            }

            // Map the Event entities to EventDTOs
            var eventDTOs = events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                OrganizerId = e.OrganizerId,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                Price = e.Price,
                IsPublished = (bool)e.IsPublished
            }).ToList();

            return eventDTOs;
        }



        public async Task<List<EventDTO>> GetEventsByOrganizerAsync(int userId)
        {
            // Fetch events created by the organizer
            var events = await _eventRepository.GetEventsByOrganizerAsync(userId);

            if (events == null || !events.Any())
            {
                throw new ArgumentException("No events found for this organizer.");
            }

            return events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                OrganizerId = e.OrganizerId,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                Price = e.Price,
                IsPublished = (bool)e.IsPublished
            }).ToList();
        }

        public async Task<List<EventDTO>> GetUpcomingEventsAsync()
        {
            var upcomingEvents = await _eventRepository.GetUpcomingEventsAsync();

            if (upcomingEvents == null || !upcomingEvents.Any())
            {
                throw new ArgumentException("No upcoming events found.");
            }

            return upcomingEvents.Select(e => new EventDTO
            {
                EventId = e.EventId,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Location = e.Location,
                Price = e.Price,
                IsPublished = (bool)e.IsPublished
            }).ToList();
        }

        public async Task<bool> TogglePublishEventAsync(int eventId)
        {
            // Check if event exists before toggling
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found.");
            }

            // Toggle publish status
            return await _eventRepository.TogglePublishEventAsync(eventId);
        }

        public async Task<string> RegisterForEventAsync(int eventId, EventRegistrationDTO registrationDTO)
        {
            // Validate registration input
            if (registrationDTO == null)
            {
                throw new ArgumentNullException(nameof(registrationDTO), "Registration data cannot be null.");
            }

            // Ensure the event exists
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found.");
            }

            // Ensure the user is not already registered
            var isUserRegistered = await _eventRepository.IsUserRegisteredForEventAsync(eventId, registrationDTO.UserId);
            if (isUserRegistered)
            {
                return "User is already registered for this event.";
            }

            // Register for the event and save responses
            return await _eventRepository.RegisterForEventAsync(eventId, registrationDTO);
        }

        public async Task<string> UnregisterFromEventAsync(int eventId, EventUnRegistrationDTO unregistrationDTO)
        {
            // Ensure the user is registered for the event
            var isUserRegistered = await _eventRepository.IsUserRegisteredForEventAsync(eventId, unregistrationDTO.UserId);
            if (!isUserRegistered)
            {
                return "User is not registered for this event.";
            }

            // Unregister from the event
            return await _eventRepository.UnregisterFromEventAsync(eventId, unregistrationDTO);
        }

        public async Task<bool> IsUserRegisteredForEventAsync(int eventId, int userId)
        {
            return await _eventRepository.IsUserRegisteredForEventAsync(eventId, userId);
        }
    }
}
