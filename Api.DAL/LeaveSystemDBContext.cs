using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Api.Contract.Model;

namespace Api.DAL
{
    public class LeaveSystemDBContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        public DbSet<User> Users => Set<User>();

        public LeaveSystemDBContext()
        {

        }
        public LeaveSystemDBContext(DbContextOptions<LeaveSystemDBContext> options) : base(options)
        {

        }

        // New constructor to allow IConfiguration to be injected when DbContext is created by DI
        public LeaveSystemDBContext(DbContextOptions<LeaveSystemDBContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //  optionsBuilder.LogTo(d => System.Diagnostics.Trace.WriteLine(d));
                //base.OnConfiguring(optionsBuilder);

                // Try to get connection string from IConfiguration (appsettings.json). Fallback to AppConstant if not available.
                var conn = _configuration?.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 0)));

                //optionsBuilder.UseLoggerFactory(LoggerFactory);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasData(
                    new User { Id = 1, Username = "Employee1", Role = "Employee" },
                    new User { Id = 2, Username = "Employee2", Role = "Employee" },
                    new User { Id = 3, Username = "Employee3", Role = "Employee" },
                    new User { Id = 4, Username = "Manager1", Role = "Manager" },
                    new User { Id = 5, Username = "Admin1", Role = "Admin" });
            });
        }
    }
}
