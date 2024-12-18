using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Models;

public partial class EventManagementSystemDataBaseContext : DbContext
{
    public EventManagementSystemDataBaseContext()
    {
    }

    public EventManagementSystemDataBaseContext(DbContextOptions<EventManagementSystemDataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventRegistration> EventRegistrations { get; set; }

    public virtual DbSet<EventRegistrationFormsField> EventRegistrationFormsFields { get; set; }

    public virtual DbSet<EventRegistrationResponse> EventRegistrationResponses { get; set; }

    public virtual DbSet<FieldOption> FieldOptions { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }
    //public virtual DbSet<Message> Messages { get; set; }
    //public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PTU1DELL0029;Database=EventManagementSystemDataBase;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C8106CDC1BD6");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsPublished).HasDefaultValue(false);
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__Organize__4CA06362");
        });

        modelBuilder.Entity<EventRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__EventReg__6EF58810AB94CF0F");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RegistrationCode).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Event).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EventRegi__Event__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.EventRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__EventRegi__UserI__5629CD9C");
        });

        modelBuilder.Entity<EventRegistrationFormsField>(entity =>
        {
            entity.HasKey(e => e.FormFieldId).HasName("PK__EventReg__531C0C333C6459ED");

            entity.ToTable("EventRegistrationFormsField");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.FieldName).HasMaxLength(255);
            entity.Property(e => e.FieldType).HasMaxLength(50);
            entity.Property(e => e.IsRequired).HasDefaultValue(true);
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

            entity.HasOne(d => d.Event).WithMany(p => p.EventRegistrationFormsFields)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EventRegi__Event__4F7CD00D");
        });

        modelBuilder.Entity<EventRegistrationResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__EventReg__1AAA646CE80B2B1A");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

            entity.HasOne(d => d.FormField).WithMany(p => p.EventRegistrationResponses)
                .HasForeignKey(d => d.FormFieldId)
                .HasConstraintName("FK__EventRegi__FormF__5EBF139D");

            entity.HasOne(d => d.Registration).WithMany(p => p.EventRegistrationResponses)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EventRegi__Regis__5DCAEF64");
        });

        modelBuilder.Entity<FieldOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__FieldOpt__92C7A1FFCD992BDB");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.OptionText).HasMaxLength(100);

            entity.HasOne(d => d.FormField).WithMany(p => p.FieldOptions)
                .HasForeignKey(d => d.FormFieldId)
                .HasConstraintName("FK__FieldOpti__FormF__693CA210");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.HasKey(e => e.OrganizerId).HasName("PK__Organize__AD7DAD0293CB01AA");

            entity.Property(e => e.ContactInfo).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.OrganizationName).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Organizers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Organizer__UserI__45F365D3");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38DFB25B6E");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);

            entity.HasOne(d => d.Registration).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RegistrationId)
                .HasConstraintName("FK__Payments__Regist__6383C8BA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C61531BD7");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053462EACF08").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.GoogleId).HasMaxLength(255);
            entity.Property(e => e.LastModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__UserRoleI__403A8C7D");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A553EC1826C");

            entity.ToTable("UserRole");

            entity.HasIndex(e => e.RoleName, "UQ__UserRole__8A2B6160A99485C1").IsUnique();

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });
        //modelBuilder.Entity<Message>(entity =>
        //{
        //    entity.HasKey(m => m.MessageId);

        //    // Configure SenderId (user who sent the message)
        //    entity.HasOne(m => m.Sender)
        //          .WithMany() // No need to have a collection in User for messages
        //          .HasForeignKey(m => m.SenderId)
        //          .OnDelete(DeleteBehavior.ClientSetNull)
        //          .HasConstraintName("FK_Message_Sender");

        //    // Configure ChatRoomId (chat room where the message belongs)
        //    entity.HasOne(m => m.ChatRoom)
        //          .WithMany(c => c.Messages)
        //          .HasForeignKey(m => m.ChatRoomId)
        //          .OnDelete(DeleteBehavior.Cascade)
        //          .HasConstraintName("FK_Message_ChatRoom");

        //    entity.Property(m => m.Content).HasMaxLength(1000); // Limit for message content
        //    entity.Property(m => m.Timestamp).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        //});

        //modelBuilder.Entity<ChatRoom>(entity =>
        //{
        //    entity.HasKey(c => c.ChatRoomId);

        //    // Configure relationship to Event
        //    entity.HasOne(c => c.Event)
        //          .WithMany() // Assuming Event can have many chat rooms (one-to-many relationship)
        //          .HasForeignKey(c => c.EventId)
        //          .OnDelete(DeleteBehavior.Cascade)
        //          .HasConstraintName("FK_ChatRoom_Event");

        //    entity.Property(c => c.Name).HasMaxLength(255);
        //});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
