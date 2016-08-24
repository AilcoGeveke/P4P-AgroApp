using Microsoft.EntityFrameworkCore;

namespace AWA.Models
{
    public class AgroContext : DbContext
    {
        public AgroContext(DbContextOptions options) : base(options) { }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EmployeeAssignment> EmployeeAssignments { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<RoadPlate> RoadPlates { get; set; }
        public DbSet<TimesheetRecord> TimesheetRecords { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Username).IsUnique();
        }
    }
}