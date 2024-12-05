using EventManagement.EventRegistrationFormRepository;
using EventManagementAPI.DTO;
using EventManagementAPI.EventRegistrationFormService;
using EventManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.EventRegistrationFormService
{
    public class EventRegistrationFormServiceType : IEventRegistrationFormService
    {
        private readonly IEventRegistrationFormRepository _repository;

        public EventRegistrationFormServiceType(IEventRegistrationFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<EventRegistrationFormsField> AddFieldAsync(EventRegistrationFormsFieldDTO fieldDTO)
        {
            return await _repository.AddFieldAsync(fieldDTO);
        }

        public async Task<IEnumerable<EventRegistrationFormsFieldDTO>> GetFieldsByEventIdAsync(int eventId)
        {
            return await _repository.GetFieldsByEventIdAsync(eventId);
        }

        public async Task<EventRegistrationFormsField> GetFieldByIdAsync(int fieldId)
        {
            return await _repository.GetFieldByIdAsync(fieldId);
        }
    }
}
