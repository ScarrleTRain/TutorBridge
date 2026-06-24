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
    }

public DbSet<TutorBridge.Models.Booking> Booking { get; set; } = default!;

public DbSet<TutorBridge.Models.Subject> Subject { get; set; } = default!;

public DbSet<TutorBridge.Models.TimeSlot> TimeSlot { get; set; } = default!;

public DbSet<TutorBridge.Models.TutorSubject> TutorSubject { get; set; } = default!;
}
