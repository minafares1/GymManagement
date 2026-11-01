using GymManagement.DAL.DATA.Configurations;
using GymManagement.DAL.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DAL.DATA.DbContext
{

    public class GymDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        //Dependency Injection
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymManagementDb;Trusted_Connection=true;TrustServerCertificate=true;");

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PlanConfigurations());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlanConfigurations).Assembly);
        }

        public DbSet<Plan> Plans { get; set; }

       // public DbSet<GymUser> GymUsers { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }

    }
}
