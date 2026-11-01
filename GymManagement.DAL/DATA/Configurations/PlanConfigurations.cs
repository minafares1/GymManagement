using GymManagement.DAL.DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.DATA.Configurations
{
    public class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(P => P.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(P => P.Description)
                   .HasMaxLength(200);

            builder.Property(P => P.Price)
                   .HasPrecision(10, 2);

            builder.Property(P => P.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.ToTable(TP =>
            {
                TP.HasCheckConstraint("PlanDurationDaysCheck", "DurationDays Between 1 And 365");
            });
        }
    }
}
