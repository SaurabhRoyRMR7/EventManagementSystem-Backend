using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class Organizer
{
    public int OrganizerId { get; set; }

    public int UserId { get; set; }

    public string OrganizationName { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? LastModifiedAt { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual User User { get; set; } = null!;
}
