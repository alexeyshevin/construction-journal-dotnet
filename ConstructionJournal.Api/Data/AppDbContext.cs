using ConstructionJournal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionJournal.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WorkLog> WorkLogs => Set<WorkLog>();
    public DbSet<WorkType> WorkTypes => Set<WorkType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkType>(entity =>
        {
            entity.ToTable("work_types");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasColumnName("name").IsRequired();
            entity.Property(x => x.Unit).HasColumnName("unit").IsRequired();

            entity.HasData(
                new WorkType { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Бетонные работы", Unit = "м³" },
                new WorkType { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Кладочные работы", Unit = "м²" },
                new WorkType { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Земляные работы", Unit = "м³" }
            );
        });

        modelBuilder.Entity<WorkLog>(entity =>
        {
            entity.ToTable("work_logs");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.WorkDate).HasColumnName("work_date").IsRequired();
            entity.Property(x => x.WorkTypeId).HasColumnName("work_type_id").IsRequired();
            entity.Property(x => x.Amount).HasColumnName("amount").HasPrecision(12, 2).IsRequired();
            entity.Property(x => x.Unit).HasColumnName("unit").IsRequired();
            entity.Property(x => x.PerformerName).HasColumnName("performer_name").IsRequired();
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

            entity.HasOne(x => x.WorkType)
                .WithMany(x => x.WorkLogs)
                .HasForeignKey(x => x.WorkTypeId);
        });
    }
}