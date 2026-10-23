using GymManagement.Configurations;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DbContext
{

    public class GymDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymManagementDb;Trusted_Connection=true;TrustServerCertificate=true;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfigurations());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
