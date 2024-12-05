using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class FieldOption
{
    public int OptionId { get; set; }

    public int FormFieldId { get; set; }

    public string OptionText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? LastModifiedAt { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual EventRegistrationFormsField FormField { get; set; } = null!;
}
