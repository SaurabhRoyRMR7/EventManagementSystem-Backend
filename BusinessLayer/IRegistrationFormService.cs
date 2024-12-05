using EventManagementAPI.Models;
using EventManagementAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.EventRegistrationFormService
{
    public interface IEventRegistrationFormService
    {
        Task<EventRegistrationFormsField> AddFieldAsync(EventRegistrationFormsFieldDTO fieldDTO);
        Task<IEnumerable<EventRegistrationFormsFieldDTO>> GetFieldsByEventIdAsync(int eventId);
        Task<EventRegistrationFormsField> GetFieldByIdAsync(int fieldId);
    }
}
