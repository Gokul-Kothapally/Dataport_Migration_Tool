using Microsoft.EntityFrameworkCore;

namespace ArchivingTool.Service.Arms.Data
{
    public partial class ArchivingContext : DbContext
    {
        public ArchivingContext() { }

        public ArchivingContext(DbContextOptions<ArchivingContext> options)
            : base(options) { }

        public ArchivingContext(string connectionString)
            : base(GetOptions(connectionString)) { }

        private static DbContextOptions<ArchivingContext> GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions
                   .UseSqlServer(new DbContextOptionsBuilder<ArchivingContext>(), connectionString)
                   .Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
