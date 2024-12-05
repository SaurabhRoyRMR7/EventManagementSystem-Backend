using System;
using System.Collections.Generic;

namespace EventManagementAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public int UserRoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? GoogleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime LastModifiedAt { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public virtual ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();

    public virtual ICollection<Organizer> Organizers { get; set; } = new List<Organizer>();

    public virtual UserRole UserRole { get; set; } = null!;
}
