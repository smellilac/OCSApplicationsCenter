public class SubmittedAppsDb : DbContext
{
    public DbSet<ApplicationsModel> Applications => Set<ApplicationsModel>();

    public SubmittedAppsDb(DbContextOptions<SubmittedAppsDb> options) : base(options) {} 
}

