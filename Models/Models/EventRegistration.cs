using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class EventRegistration
{
    public int RegistrationId { get; set; }

    public int? EventId { get; set; }

    public int? UserId { get; set; }

    public DateTime? RegistrationDate { get; set; }
    public string? RegistrationCode { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime LastModifiedAt { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual Event? Event { get; set; }

    public virtual ICollection<EventRegistrationResponse> EventRegistrationResponses { get; set; } = new List<EventRegistrationResponse>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
