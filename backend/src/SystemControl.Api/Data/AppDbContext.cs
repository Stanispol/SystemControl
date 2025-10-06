using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SystemControl.Api.Models;
using SystemControl.Api.Data;


namespace SystemControl.Api.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<DefectComment> DefectComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasMany(p => p.Defects).WithOne(d => d.Project).HasForeignKey(d => d.ProjectId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Defect>(b =>
            {
                b.HasKey(d => d.Id);
                b.Property(d => d.Priority).HasConversion<string>();
            });

            builder.Entity<DefectComment>(b =>
            {
                b.HasKey(c => c.Id);
                b.HasOne(c => c.Defect).WithMany(d => d.Comments).HasForeignKey(c => c.DefectId);
            });
        }
    }
}
