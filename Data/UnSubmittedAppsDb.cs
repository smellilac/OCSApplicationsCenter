public class UnSubmittedAppsDb : DbContext
{
    public DbSet<ApplicationsModel> UnSubmittedApps => Set<ApplicationsModel>();

       public UnSubmittedAppsDb(DbContextOptions<UnSubmittedAppsDb> options) : base(options) {}
}