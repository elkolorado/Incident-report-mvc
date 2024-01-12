using ErrorReports.Areas.Identity.Data;
using ErrorReports.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorReports.Areas.Identity.Data;

public class AppDBContext : IdentityDbContext<AppUser>
{
    public AppDBContext(DbContextOptions<AppDBContext> options)
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

    private class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<AppUser> 
    { 
        public void Configure(EntityTypeBuilder<AppUser> builder) 
        { 
            builder.Property(x => x.FirstName).HasMaxLength(255); 
            builder.Property(x => x.LastName).HasMaxLength(255); 
        } 
    }

    public DbSet<ErrorReport> Incidents { get; set; }
}


