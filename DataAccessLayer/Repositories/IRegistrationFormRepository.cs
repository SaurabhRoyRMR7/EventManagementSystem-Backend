using EventManagementAPI.Models;
using EventManagementAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.EventRegistrationFormRepository
{
 

    public interface IEventRegistrationFormRepository
    {
        Task<EventRegistrationFormsField> AddFieldAsync(EventRegistrationFormsFieldDTO fieldDTO);
        Task<IEnumerable<EventRegistrationFormsFieldDTO>> GetFieldsByEventIdAsync(int eventId);
        Task<EventRegistrationFormsField> GetFieldByIdAsync(int fieldId);
    }
}
