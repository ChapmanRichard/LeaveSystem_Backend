using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Contract.Model;
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(x => x.Username).HasMaxLength(50).IsRequired();
                entity.Property(x => x.Role).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<LeaveQuota>(entity =>
            {
                entity.Property(x => x.LeaveType).HasMaxLength(20).IsRequired();
                entity.Property(x => x.TotalDays).HasPrecision(5, 1);
                entity.Property(x => x.RemainingDays).HasPrecision(5, 1);
                //entity.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.User)
                    .WithMany(x => x.LeaveQuotas)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.Property(x => x.LeaveType).HasMaxLength(20).IsRequired();
                entity.Property(x => x.Reason).HasMaxLength(500).IsRequired();
                entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
                entity.Property(x => x.ApprovalComment).HasMaxLength(500);
                entity.Property(x => x.Duration).HasPrecision(5, 1);
                //entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                //entity.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(x => x.Applicant)
                    .WithMany(x => x.SubmittedLeaveRequests)
                    .HasForeignKey(x => x.ApplicantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Approver)
                    .WithMany(x => x.ApprovedLeaveRequests)
                    .HasForeignKey(x => x.ApproverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<StockBase> StockBase { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LeaveQuota> LeaveQuotas { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}
