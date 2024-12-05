// Controllers/EventRegistrationFormController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Models;

using EventManagementAPI.DTO;
using EventManagementAPI.EventRegistrationFormService;

[Route("api/[controller]")]
[ApiController]
public class EventRegistrationFormController : ControllerBase
{
    private readonly IEventRegistrationFormService _service;

    public EventRegistrationFormController(IEventRegistrationFormService service)
    {
        _service = service;
    }
    // POST: api/event-registration-form (Add a new field for registration form)
    [HttpPost]
    public async Task<ActionResult<EventRegistrationFormsFieldDTO>> AddRegistrationFormField(EventRegistrationFormsFieldDTO fieldDTO)
    {
        var result = await _service.AddFieldAsync(fieldDTO);
        if (result == null)
        {
            return BadRequest("Event not found.");
        }
        return Ok(result);
    }

    // GET: api/event-registration-form/{eventId} (Get all fields for a specific event)
    [HttpGet("{eventId}")]
    public async Task<ActionResult<IEnumerable<EventRegistrationFormsFieldDTO>>> GetEventRegistrationFormFields(int eventId)
    {
        // Get all fields for the event
        var fields = await _service.GetFieldsByEventIdAsync(eventId);
        if (fields == null )
        {
            return NotFound("No fields found for this event.");
        }
        return Ok(fields);
    }

}
