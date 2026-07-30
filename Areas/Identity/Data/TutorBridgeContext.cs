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

        builder.Entity<Booking>().HasQueryFilter(b =>
            b.DeletedAt == null &&
            b.User.DeletedAt == null &&
            b.Timeslot.DeletedAt == null &&
            b.Timeslot.Tutor.DeletedAt == null);

        builder.Entity<Timeslot>()
            .HasOne(t => t.Tutor)
            .WithMany()
            .HasForeignKey(t => t.TutorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Timeslot>().HasQueryFilter(t => t.DeletedAt == null && t.Tutor.DeletedAt == null);

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

        builder.Entity<TutorSubject>().HasQueryFilter(ts => ts.Tutor.DeletedAt == null);

        builder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
    }

    public override int SaveChanges()
    {
        ApplySoftDelete();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplySoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplySoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ISoftDeletable softDeletable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        softDeletable.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        softDeletable.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }

    public DbSet<TutorBridge.Models.Booking> Booking { get; set; } = default!;

public DbSet<TutorBridge.Models.Subject> Subject { get; set; } = default!;

public DbSet<TutorBridge.Models.Timeslot> Timeslot { get; set; } = default!;

public DbSet<TutorBridge.Models.TutorSubject> TutorSubject { get; set; } = default!;
}
