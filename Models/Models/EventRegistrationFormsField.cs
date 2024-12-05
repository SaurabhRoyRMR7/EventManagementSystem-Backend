using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class EventRegistrationFormsField
{
    public int FormFieldId { get; set; }

    public int? EventId { get; set; }

    public string FieldName { get; set; } = null!;

    public string FieldType { get; set; } = null!;

    public bool? IsRequired { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime LastModifiedAt { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual Event? Event { get; set; }

    public virtual ICollection<EventRegistrationResponse> EventRegistrationResponses { get; set; } = new List<EventRegistrationResponse>();

    public virtual ICollection<FieldOption> FieldOptions { get; set; } = new List<FieldOption>();
}
