

namespace EventManagementAPI.DTO
{
    public class EventDTO
    {
        public int EventId { get; set; }

        public int OrganizerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
    }
    public class CreateEventDTO
    {
        //public int UserId { get; set; }
        public int OrganizerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public List<EventRegistrationFormsFieldDTO> RegistrationFields { get; set; } = new List<EventRegistrationFormsFieldDTO>();
    }
    public class EventRegistrationFormsFieldDTO
    {
        public int FormFieldId { get; set; }
        public int? EventId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }
        public List<string> DropdownOptions { get; set; } = new List<string>();  // For Dropdown options
        public List<string> RadioOptions { get; set; } = new List<string>();
    }
    public class EventRegistrationDTO
    {
        public int RegistrationId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }

        public string RegistrationCode { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? CreatedBy { get; set; } = null!;

        public string? LastModifiedBy { get; set; } = null!;
        public List<EventRegistrationResponseDTO> RegistrationResponses { get; set; }
    }
    public class EventUnRegistrationDTO
    {
       
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
    public class EventRegistrationResponseDTO
    {
        public int FieldId { get; set; }
        public string ResponseValue { get; set; }



    }
    public class ParticipantDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
