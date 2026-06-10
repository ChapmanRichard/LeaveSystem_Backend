using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.DAL
{
    public class LeaveSystemDBContext : DbContext
    {
        private readonly IConfiguration? _configuration;

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
        }
    }
}
