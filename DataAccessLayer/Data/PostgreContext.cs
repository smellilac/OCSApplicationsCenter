public class PostgreContext : DbContext
{
    public DbSet<SubmittedApplications> Applications { get; set; }
    public DbSet<UnSubmittedApplications> UnSubmittedApps { get; set; }
    public DbSet<UserModel> Users { get; set; }

    public PostgreContext(DbContextOptions<PostgreContext> options) : base(options) {}
}