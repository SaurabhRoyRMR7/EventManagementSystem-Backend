using EventManagementAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using EventManagement.EventService;
using Microsoft.AspNetCore.Mvc.Diagnostics;
//using EventManagement.EventRepository;
using EventManagementAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3TierEventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        //private readonly IEventRepository _eventRepository;
        private readonly IEventService _eventService;

        public EventController( IEventService eventService)
        {
            //_eventRepository = eventRepository;
            _eventService = eventService;
        }
        // GET: api/<EventController>
        [HttpPost]
        public async Task<ActionResult<EventDTO>> CreateEvent([FromBody] CreateEventDTO createEventDTO)
        {

            try
            {
                var eventDTO = await _eventService.CreateEventAsync(createEventDTO);
                return CreatedAtAction(nameof(GetEvent), new { id = eventDTO.EventId }, eventDTO);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var eventDTO = await _eventService.GetEventByIdAsync(id);
            if (eventDTO == null)
            {
                return NotFound();
            }

            return Ok(eventDTO);
        }

        // GET api/<EventController>/5
        [HttpDelete("{action}/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            // Call the repository method to delete the event
            var success = await _eventService.DeleteEventAsync(eventId);

            if (!success)
            {
                return NotFound(new { message = "Event not found." });
            }

            return Ok(new { message = "Event deleted successfully." });
        }

        [HttpPut("{id}/publish")]
        public async Task<IActionResult> TogglePublishEvent(int id)
        {
           
            var success = await _eventService.TogglePublishEventAsync(id);


           
            return Ok(new { isPublished = success });
        }

       

        [HttpGet("organizer/{userId}/events")]
        public async Task<IActionResult> GetEventsByOrganizer(int userId)
        {
            
            var events = await _eventService.GetEventsByOrganizerAsync(userId);

            if (events == null)
            {
                return NotFound(new { message = "Organizer not found." });
            }

          
            return Ok(events);
        }
        [HttpGet("events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            // Call the repository method to get all events
            var events = await _eventService.GetEventsAsync();

            // If there are no events, return a NotFound result
            if (events == null || events.Count == 0)
            {
                return NotFound("No events found.");
            }

            // Return a 200 OK response with the list of events
            return Ok(events);
        }
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetUpcomingEvents()
        {
            // Call the repository method to get upcoming events
            var upcomingEvents = await _eventService.GetUpcomingEventsAsync();

            if (upcomingEvents == null )
            {
                return NotFound("No upcoming events found.");
            }

            return Ok(upcomingEvents);
        }
        [HttpGet("GetRegisteredEvents/{userId}")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetRegisteredEvents(int userId)
        {
            // Call the repository method to get the events the user is registered for
            var registeredEvents = await _eventService.GetRegisteredEventsByUserIdAsync(userId);

            if (registeredEvents == null )
            {
                return NotFound("No events found for the user.");
            }

            // Map the event entities to DTOs
            var eventDTOs = registeredEvents.Select(e => new EventDTO
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

            // Return the DTOs in the response
            return Ok(eventDTOs);
        }
        [HttpPost("{eventId}/register")]
        public async Task<IActionResult> RegisterForEvent(int eventId, [FromBody] EventRegistrationDTO registrationDTO)
        {
            Console.WriteLine($"Registering for event {eventId}");

            // Call the repository method to handle registration
            var result = await _eventService.RegisterForEventAsync(eventId, registrationDTO);

            if (result == "Event not found.")
            {
                return NotFound(result); // Event not found
            }

            return Ok(new { Message = result }); // Registration successful
        }
        [HttpPost("{eventId}/unregister")]
        public async Task<IActionResult> UnregisterFromEvent(int eventId, [FromBody] EventUnRegistrationDTO unregistrationDTO)
        {
            Console.WriteLine($"Unregistering from event {eventId}");

            // Call the repository method to handle unregistration
            var result = await _eventService.UnregisterFromEventAsync(eventId, unregistrationDTO);

            if (result == "You are not registered for this event.")
            {
                return NotFound(result); // User not registered
            }

            return Ok(new { Message = result }); // Successfully unregistered
        }

        [HttpGet("{eventId}/is-registered/{userId}")]
        public async Task<IActionResult> CheckUserRegistration(int eventId, int userId)
        {
            // Call the repository method to check user registration
            var isRegistered = await _eventService.IsUserRegisteredForEventAsync(eventId, userId);

            // Return the registration status
            return Ok(new { isRegistered });
        }
        [HttpGet("participants/{eventId}")]
        public async Task<IActionResult> GetEventParticipants(int eventId)
        {
            
            var participants = await _eventService.GetEventParticipantsAsync(eventId);

            if (participants == null )
            {
                return NotFound("No participants found for this event.");
            }

            return Ok(participants);  
        }


    }
}
