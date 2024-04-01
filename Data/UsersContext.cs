public class UsersContext : DbContext
{
    public DbSet<UserModel> Users => Set<UserModel>();

       public UsersContext(DbContextOptions<UsersContext> options) : base(options) {}
}