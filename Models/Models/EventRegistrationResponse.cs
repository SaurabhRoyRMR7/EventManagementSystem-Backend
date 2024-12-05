using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class EventRegistrationResponse
{
    public int ResponseId { get; set; }

    public int? RegistrationId { get; set; }

    public int? FormFieldId { get; set; }

    public string? ResponseValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime LastModifiedAt { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual EventRegistrationFormsField? FormField { get; set; }

    public virtual EventRegistration? Registration { get; set; }
}
