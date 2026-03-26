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
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<TutorBridge.Models.Booking> Booking { get; set; } = default!;

public DbSet<TutorBridge.Models.Subject> Subject { get; set; } = default!;

public DbSet<TutorBridge.Models.Timeslot> Timeslot { get; set; } = default!;

public DbSet<TutorBridge.Models.TutorSubject> TutorSubject { get; set; } = default!;
}
