using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int RegistrationId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual EventRegistration Registration { get; set; } = null!;
}
