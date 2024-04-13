public class ApplicationRepository : IApplicationRepository
{
    private readonly PostgreContext _context;

    public ApplicationRepository(PostgreContext context) 
    {
        _context = context;
    }
    
    public async Task<SubmittedApplications> GetSubmittedApplication(Guid id) => 
        await _context.Applications.SingleAsync(app => app.Id == id);

    public async Task<UnSubmittedApplications> GetUnSubmittedApplication(Guid id) => 
        await _context.UnSubmittedApps.SingleAsync(app => app.Id == id);

    public async Task<List<SubmittedApplications>> GetSubmittedAfter(DateTime date) => 
        await _context.Applications
            .Where(app => app.Submited > date)
            .ToListAsync();
    
    public async Task<List<UnSubmittedApplications>> GetUnSubmittedOlder(DateTime date) =>
        await _context.UnSubmittedApps
            .Where(app => app.FirstTimeCreated > date)
            .ToListAsync();

    public async Task<UnSubmittedApplications> GetUnSubmittedForUser(Guid userId) =>
        await _context.UnSubmittedApps.SingleAsync(app => app.Author == userId);  

    public async Task RemoveUnsubmitted(UnSubmittedApplications UnsubApp) 
    {
        _context.UnSubmittedApps.Remove(UnsubApp);
        await SaveAsync();
    }

    public async Task AddToSubmitted(SubmittedApplications SubApp) =>
        await _context.Applications.AddAsync(SubApp);

    public async Task AddToUsers(UserModel newUser) =>
        await _context.Users.AddAsync(newUser);
    
    public async Task RemoveFromUsers(UserModel newUser) 
    {
        _context.Users.Remove(newUser);
        await SaveAsync();
    }

    public async Task<UserModel> GetUser(UnSubmittedApplications UnsApp) 
    {
        return await _context.Users
            .SingleAsync(user => user.Id == UnsApp.Author);
    }    
    public async Task AddToUnSubmitted(UnSubmittedApplications UnSubApp) =>
        await _context.UnSubmittedApps.AddAsync(UnSubApp);

    public async Task<T> GenericSearch<T>(Guid AppId) where T : ApplicationsModel =>
         await _context.Set<T>().SingleAsync(a => a.Id == AppId);
    
    
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}