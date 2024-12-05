using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int OrganizerId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Location { get; set; }

    public decimal Price { get; set; }

    public bool? IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime LastModifiedAt { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual ICollection<EventRegistrationFormsField> EventRegistrationFormsFields { get; set; } = new List<EventRegistrationFormsField>();

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();

    public virtual Organizer Organizer { get; set; } = null!;
}
