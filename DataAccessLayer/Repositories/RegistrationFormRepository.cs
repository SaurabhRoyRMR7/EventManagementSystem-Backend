// Repositories/EventRegistrationFormRepository.cs

using EventManagementAPI.Models;
using EventManagementAPI.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using EventManagement.EventRegistrationFormRepository;


namespace EventManagement.EventRegistrationFormRepository
{
    public class EventRegistrationFormRepositoryType : IEventRegistrationFormRepository
    {
        private readonly EventManagementSystemDataBaseContext _context;

        public EventRegistrationFormRepositoryType(EventManagementSystemDataBaseContext context)
        {
            _context = context;
        }

        public async Task<EventRegistrationFormsField> AddFieldAsync(EventRegistrationFormsFieldDTO fieldDTO)
        {
            var eventItem = await _context.Events.FindAsync(fieldDTO.EventId);
            if (eventItem == null)
            {
                return null; // Event not found
            }

            var field = new EventRegistrationFormsField
            {
                EventId = fieldDTO.EventId,
                FieldName = fieldDTO.FieldName,
                FieldType = fieldDTO.FieldType,
                IsRequired = fieldDTO.IsRequired,
                CreatedAt = DateTime.Now,
                CreatedBy = "System",
                LastModifiedAt = DateTime.Now,
                LastModifiedBy = "System"
            };

            _context.EventRegistrationFormsFields.Add(field);
            await _context.SaveChangesAsync();

            return field;
        }

        public async Task<IEnumerable<EventRegistrationFormsFieldDTO>> GetFieldsByEventIdAsync(int eventId)
        {
            var fields = await _context.EventRegistrationFormsFields
                .Where(f => f.EventId == eventId)
                .ToListAsync();

            if (fields == null || fields.Count == 0)
            {
                return null; 
            }

            var fieldDTOs = new List<EventRegistrationFormsFieldDTO>();

            foreach (var field in fields)
            {
                var fieldDTO = new EventRegistrationFormsFieldDTO
                {
                    FormFieldId = field.FormFieldId,
                    EventId = field.EventId ?? 0,
                    FieldName = field.FieldName,
                    FieldType = field.FieldType,
                    IsRequired = field.IsRequired ?? true
                };

                if (field.FieldType == "Dropdown" || field.FieldType == "Radio")
                {
                    var options = await _context.FieldOptions
                        .Where(o => o.FormFieldId == field.FormFieldId)
                        .Select(o => o.OptionText)
                        .ToListAsync();

                    if (field.FieldType == "Dropdown")
                    {
                        fieldDTO.DropdownOptions = options;
                    }
                    else if (field.FieldType == "Radio")
                    {
                        fieldDTO.RadioOptions = options;
                    }
                }

                fieldDTOs.Add(fieldDTO);
            }

            return fieldDTOs;
        }

        public async Task<EventRegistrationFormsField> GetFieldByIdAsync(int fieldId)
        {
            return await _context.EventRegistrationFormsFields
                .FirstOrDefaultAsync(f => f.FormFieldId == fieldId);
        }
    }

}