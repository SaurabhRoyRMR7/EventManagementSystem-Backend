// EventRepository.cs
using Microsoft.EntityFrameworkCore;
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace EventManagement.EventRepository
{
    public class EventRepositoryType : IEventRepository
    {
        private readonly EventManagementSystemDataBaseContext _context;
        private readonly IMapper _mapper;
        public EventRepositoryType(EventManagementSystemDataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<EventDTO> CreateEventAsync(CreateEventDTO createEventDTO)
        {
            Console.WriteLine("I am in create event");
            var organizerExists = await _context.Organizers
                .AnyAsync(o => o.OrganizerId == createEventDTO.OrganizerId);

            if (!organizerExists)
            {
                throw new ArgumentException("Organizer not found");
            }
            //var eventEntity = _mapper.Map<Event>(createEventDTO);

            var eventEntity = new Event
            {
                OrganizerId = createEventDTO.OrganizerId,
                Title = createEventDTO.Title,
                Description = createEventDTO.Description,
                StartDate = createEventDTO.StartDate,
                EndDate = createEventDTO.EndDate,
                Location = createEventDTO.Location,
                Price = createEventDTO.Price,
                IsPublished = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "System"
            };

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync(); // Save event entity

            // Add registration fields if they exist
            if (createEventDTO.RegistrationFields != null && createEventDTO.RegistrationFields.Any())
            {
                foreach (var field in createEventDTO.RegistrationFields)
                {
                    var formFieldEntity = new EventRegistrationFormsField
                    {
                        EventId = eventEntity.EventId,
                        FieldName = field.FieldName,
                        FieldType = field.FieldType,
                        IsRequired = field.IsRequired,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System",
                        LastModifiedAt = DateTime.Now,
                        LastModifiedBy = "System"
                    };
                    //var formFieldEntity = _mapper.Map<EventRegistrationFormsField>(field);
                    formFieldEntity.EventId = eventEntity.EventId;

                    _context.EventRegistrationFormsFields.Add(formFieldEntity);
                    await _context.SaveChangesAsync(); // Save the field

                    // Add field options (if any)
                    if (field.FieldType == "Dropdown" && field.DropdownOptions.Any())
                    {
                        foreach (var option in field.DropdownOptions)
                        {
                            var fieldOption = new FieldOption
                            {
                                FormFieldId = formFieldEntity.FormFieldId,
                                OptionText = option,
                                CreatedAt = DateTime.Now,
                                CreatedBy = "System"
                            };

                            _context.FieldOptions.Add(fieldOption);
                        }
                    }
                    else if (field.FieldType == "Radio" && field.RadioOptions.Any())
                    {
                        foreach (var option in field.RadioOptions)
                        {
                            var fieldOption = new FieldOption
                            {
                                FormFieldId = formFieldEntity.FormFieldId,
                                OptionText = option,
                                CreatedAt = DateTime.Now,
                                CreatedBy = "System"
                            };

                            _context.FieldOptions.Add(fieldOption);
                        }
                    }
                }


                await _context.SaveChangesAsync();
            }

            //Return the created event as EventDTO
           var eventDTO = new EventDTO
            {
                EventId = eventEntity.EventId,
                OrganizerId = eventEntity.OrganizerId,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                Location = eventEntity.Location,
                Price = eventEntity.Price,
                IsPublished = (bool) eventEntity.IsPublished
            };
            

            return eventDTO;
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            // Find the event by Id
            var eventEntity = await _context.Events.FindAsync(eventId);

            if (eventEntity == null)
            {
                return false;  // Return false if event not found
            }

            // Remove the event
            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();

            return true;  // Return true if deletion is successful
        }

        //Task<bool> IEventRepository.EventExistsAsync(int eventId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<EventDTO> GetEventByIdAsync(int id)
        {
            // Find the event by id
            var eventItem = await _context.Events.FindAsync(id);

           
            if (eventItem == null)
            {
                return null;
            }

            //Map the Event entity to EventDTO
            var eventDTO = new EventDTO
            {
                EventId = eventItem.EventId,
                OrganizerId = eventItem.OrganizerId,
                Title = eventItem.Title,
                Description = eventItem.Description,
                StartDate = eventItem.StartDate,
                EndDate = eventItem.EndDate,
                Location = eventItem.Location,
                Price = eventItem.Price,
                IsPublished = (bool)eventItem.IsPublished
            };
            //var eventDTO = _mapper.Map<EventDTO>(eventItem);

            return eventDTO;
        }

        public async Task<List<ParticipantDTO>> GetEventParticipantsAsync(int eventId)
        {
            // Fetch participants for a specific event, joining EventRegistrations with User
            var participants = await _context.EventRegistrations
                                             .Where(er => er.EventId == eventId)
                                             .Include(er => er.User)  // Including the associated User
                                             .Select(er => new ParticipantDTO
                                             {
                                                 UserId = er.User.UserId,
                                                 Name = er.User.Name,
                                                 Email = er.User.Email,
                                                 CreatedAt = er.User.CreatedAt
                                             })
                                             .ToListAsync();

            return participants;
        }
      

        public async Task<List<Event>> GetEventsByOrganizerAsync(int userId)
        {
            // Find the organizer associated with the userId
            var organizer = await _context.Organizers
                                          .FirstOrDefaultAsync(o => o.UserId == userId);

            if (organizer == null)
            {
                return null;  // Return null if the organizer is not found
            }

            // Get the events created by the found organizer
            var events = 
                await _context.Events
                                       .Where(e => e.OrganizerId == organizer.OrganizerId)
                                       .Select(e => new Event(){
                                           EventId = e.EventId,
                                           Title = e.Title,
                                           Description = e.Description,
                                           StartDate = e.StartDate,
                                           EndDate = e.EndDate ,
                                           Location= e.Location,
                                           Price = e.Price,
                                           IsPublished= (bool)e.IsPublished
                                       })
                                       .ToListAsync();

            return events;  
        }
        public async Task<List<Event>> GetEventsAsync()
        {
            // Fetch all events, including related data (e.g., EventRegistrations)
            var events = await _context.Events
                                        .Include(e => e.EventRegistrations) 
                                        .ToListAsync();

            return events;  
        }
        public async Task<List<Event>> GetRegisteredEventsByUserIdAsync(int userId)
    {
        // Fetch events that the user is registered for
        var registrations = await _context.EventRegistrations
            .Where(er => er.UserId == userId)
            .Include(er => er.Event) // Include the Event entity
            .Select(er => er.Event) // Select the Event entity itself
            .ToListAsync();

            var eventDTOs = registrations.Select(e => new Event
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


            return eventDTOs;
    }

      

        public async Task<List<EventDTO>> GetUpcomingEventsAsync()
        {
            
            var upcomingEvents = await _context.Events
                .Where(e => (bool)(e.StartDate >= DateTime.Now & e.IsPublished))
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            var eventDTOs = upcomingEvents.Select(e => new EventDTO
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

            return eventDTOs;
        }

       
        public async Task<bool> TogglePublishEventAsync(int eventId)
        {
            
            var eventToUpdate = await _context.Events.FindAsync(eventId);

            if (eventToUpdate == null)
            {
                throw new ArgumentException("Event not found");
            }

          
            eventToUpdate.IsPublished = !eventToUpdate.IsPublished;

            _context.Entry(eventToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return (bool)eventToUpdate.IsPublished; 
        }


        public async Task<string> RegisterForEventAsync(int eventId, EventRegistrationDTO registrationDTO)
        {
            var eventEntity = await _context.Events
                .Include(e => e.EventRegistrationFormsFields)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found");
            }

           
            var registration = new EventRegistration
            {
                EventId = eventId,
                UserId = registrationDTO.UserId,
                RegistrationDate = DateTime.Now,
                RegistrationCode = registrationDTO.RegistrationCode,
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "System",
            };

            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            // Save registration responses
            foreach (var response in registrationDTO.RegistrationResponses)
            {
                var eventField = eventEntity.EventRegistrationFormsFields
                    .FirstOrDefault(f => f.FormFieldId == response.FieldId);
                if (eventField == null)
                {
                    continue;
                }

                var registrationResponse = new EventRegistrationResponse
                {
                    RegistrationId = registration.RegistrationId,
                    FormFieldId = response.FieldId,
                    ResponseValue = response.ResponseValue,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System",
                    LastModifiedAt = DateTime.Now,
                    LastModifiedBy = "System",
                };

                _context.EventRegistrationResponses.Add(registrationResponse);
            }

            await _context.SaveChangesAsync();

            return "Registration successful!";
        }

        public async Task<string> UnregisterFromEventAsync(int eventId, EventUnRegistrationDTO unregistrationDTO)
        {
            // Find the registration for the event and user
            var registration = await _context.EventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == unregistrationDTO.UserId);

            if (registration == null)
            {
                return "You are not registered for this event.";
            }

            // Remove registration responses
            var responses = await _context.EventRegistrationResponses
                .Where(r => r.RegistrationId == registration.RegistrationId)
                .ToListAsync();

            _context.EventRegistrationResponses.RemoveRange(responses);

            // Remove the registration
            _context.EventRegistrations.Remove(registration);

            await _context.SaveChangesAsync();

            return "Successfully unregistered from the event.";
        }
        public async Task<bool> IsUserRegisteredForEventAsync(int eventId, int userId)
        {
            // Check if the registration exists for the given event and user
            var registration = await _context.EventRegistrations
                                              .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            return registration != null;
        }

        public async Task<bool> IsOrganizerExistsAsync(int organizerId)
        {
            var organizerExists = await _context.Organizers
               .AnyAsync(o => o.OrganizerId == organizerId);

            if (!organizerExists)
            {
               return false;
            }

            return true;
        }
    }
}