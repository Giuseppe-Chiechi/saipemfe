using Microsoft.EntityFrameworkCore;
using SaipemE_PTW.Shared.Models.PWT;
using SaipemE_PTW.Shared.Models.Logger; // Added
using System.Text.Json; // For JSON conversion

namespace SaipemE_PTW.Producer.Data
{
 public class AppDbContext : DbContext
 {
 public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

 public DbSet<AttachmentType> AttachmentTypes => Set<AttachmentType>();
 public DbSet<AttachmentTypeLocalization> AttachmentTypeLocalizations => Set<AttachmentTypeLocalization>();
 public DbSet<LogMessage> LogMessages => Set<LogMessage>(); // Added

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
 base.OnModelCreating(modelBuilder);

 modelBuilder.Entity<AttachmentType>(e =>
 {
 e.ToTable("AttachmentTypes");
 e.HasKey(x => x.Id);
 e.Property(x => x.Code).HasMaxLength(50).IsRequired();
 e.HasMany(x => x.Localizations)
 .WithOne(l => l.AttachmentType)
 .HasForeignKey(l => l.AttachmentTypeId)
 .OnDelete(DeleteBehavior.Cascade);
 e.HasIndex(x => x.Code).IsUnique();
 });

 modelBuilder.Entity<AttachmentTypeLocalization>(e =>
 {
 e.ToTable("AttachmentTypeLocalizations");
 e.HasKey(x => x.Id);
 e.Property(x => x.Language).HasMaxLength(5).IsRequired();
 e.Property(x => x.Name).HasMaxLength(200).IsRequired();
 e.Property(x => x.Description).HasMaxLength(1000);
 e.HasIndex(x => new { x.AttachmentTypeId, x.Language }).IsUnique();
 });

 // LogMessage configuration
 modelBuilder.Entity<LogMessage>(e =>
 {
 e.ToTable("LogMessages");
 e.HasKey(l => l.CorrelationId); // Use CorrelationId as PK
 e.Property(l => l.CorrelationId).HasMaxLength(100).IsRequired();
 e.Property(l => l.Level).HasMaxLength(20).IsRequired();
 e.Property(l => l.Message).HasMaxLength(2000).IsRequired();
 e.Property(l => l.ExceptionType).HasMaxLength(200);
 e.Property(l => l.ExceptionMessage).HasMaxLength(2000);
 e.Property(l => l.ExceptionStackTrace).HasMaxLength(8000);
 e.Property(l => l.ClientInfo).HasMaxLength(500);
 e.Property(l => l.AppVersion).HasMaxLength(50).IsRequired();

 // Store Properties dictionary as JSON string (nullable)
 e.Property(l => l.Properties)
 .HasConversion(
 v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
 v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, object?>>(v, (JsonSerializerOptions)null))
 .HasColumnType("nvarchar(max)");

 e.Property(l => l.Timestamp).IsRequired();
 });
 }
 }
}
