using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.Areas.Identity.Data;

public class TutorBridgeContext : IdentityDbContext<User>
{
    public TutorBridgeContext(DbContextOptions<TutorBridgeContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion<string>();

        builder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Booking>()
            .HasOne(b => b.Subject)
            .WithMany()
            .HasForeignKey(b => b.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Booking>()
            .HasOne(b => b.Timeslot)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TimeslotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Timeslot>()
            .HasOne(t => t.Tutor)
            .WithMany()
            .HasForeignKey(t => t.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TutorSubject>()
            .HasOne(ts => ts.Tutor)
            .WithMany()
            .HasForeignKey(ts => ts.TutorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TutorSubject>()
            .HasOne(ts => ts.Subject)
            .WithMany()
            .HasForeignKey(ts => ts.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }

public DbSet<TutorBridge.Models.Booking> Booking { get; set; } = default!;

public DbSet<TutorBridge.Models.Subject> Subject { get; set; } = default!;

public DbSet<TutorBridge.Models.Timeslot> Timeslot { get; set; } = default!;

public DbSet<TutorBridge.Models.TutorSubject> TutorSubject { get; set; } = default!;
}
