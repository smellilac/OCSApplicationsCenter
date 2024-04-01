public class ApplicationRepository : IApplicationRepository
{
    List<string> activityList = new List<string>();
     private readonly SubmittedAppsDb _context;
    private readonly UsersContext _contextUsers;
     private readonly UnSubmittedAppsDb _contextUnSubm;
     private readonly ApplicationsModelValidator _validator;
     public ApplicationRepository(SubmittedAppsDb _context,  
        UnSubmittedAppsDb _contextUnSubm, 
        UsersContext _contextUsers,
        ApplicationsModelValidator _validator) 
     {
        this._context = _context;
        this._contextUnSubm = _contextUnSubm;
        this._contextUsers = _contextUsers;
        this._validator = _validator;
     }


    public async Task<ApplicationsModel> GetApplicationByIdAsync(Guid id) =>
        await _context.Applications.FindAsync(new object[] {id});



    public async Task<List<ApplicationsModel>> GetApplicationsSubmittedAfterDateAsync(DateTime date)
    {
        var applications = await _context.Applications
            .Where(app => app.Submited > date)
            .ToListAsync();

        return applications;
    }

    public async Task<ApplicationsModel> GetDraftApplicationForUserAsync(Guid userId) 
    {
        var applications = await _contextUnSubm.UnSubmittedApps
            .SingleAsync(app => app.Author == userId);
            
        return applications;
    }

    public async Task<List<ApplicationsModel>> GetDraftApplicationsAfterDateAsync(DateTime date)
    {
        var applications = await _contextUnSubm.UnSubmittedApps
            .Where(app => app.FirstTimeCreated > date)
            .ToListAsync();

        return applications;
    }

    public async Task<ApplicationsModel> SubmitApplicationForReviewAsync(Guid id) 
    {
        var appFromDb = await _contextUnSubm.UnSubmittedApps
            .SingleAsync(app => app.Id == id);

        _contextUnSubm.UnSubmittedApps.Remove(appFromDb);
        appFromDb.Submited = DateTime.Now;
        await _context.AddAsync(appFromDb);
        return appFromDb;

    }

    public async Task<List<object>> GetActivityTypesAsync() 
{
    var activityList = new List<string>(3);
    activityList.FillList(); 

    List<object> activityTypesWithDescription = new List<object>();

    foreach (string activity in activityList)
    {
        string description = ForOutputDescription.GetActivityDescription(activity);
        activityTypesWithDescription.Add(new { activity, description });
    }

    return await Task.FromResult(activityTypesWithDescription);
}



    public async Task CreateApplicationAsync(ApplicationsModel app) 
    { 
        var validationResult = await _validator.ValidateAsync(app);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }      
        var existingAppForUser = await _contextUnSubm.UnSubmittedApps
            .FirstOrDefaultAsync(a => a.Author == app.Author);

        if (existingAppForUser != null)
            throw new InvalidOperationException("User already has an existing application. Only one application per user is allowed.");
        else 
        {
            var newUser = new UserModel { Id = app.Author };
            await _contextUsers.Users.AddAsync(newUser);
        }
        if (app.FirstTimeCreated == DateTime.MinValue) 
            app.FirstTimeCreated = DateTime.Now;
        await _contextUnSubm.UnSubmittedApps.AddAsync(app);
    }

    public async Task UpdateApplicationAsync(ApplicationsModel app, Guid AppId)
    {   
        ApplicationsModel appFromDb = await _contextUnSubm.UnSubmittedApps
            .FirstOrDefaultAsync(a => a.Id == AppId);
        appFromDb.Activity = app.Activity;
        appFromDb.Description = app.Description;
        appFromDb.Name = app.Name;
        appFromDb.Outline = app.Outline;
    }

    public async Task DeleteApplicationAsync(Guid appId)
    {
        var appFromDb = await _context.Applications.FindAsync(new object[]{appId});
        if (appFromDb == null) return;
        _context.Applications.Remove(appFromDb);
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
    public async Task SaveAsyncUnsubmitted() => await _contextUnSubm.SaveChangesAsync();
    public async Task SaveAsyncUsers() => await _contextUsers.SaveChangesAsync();

    private bool _disposed = false;

    private bool _disposedUnsubmitted = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;

        if (!_disposedUnsubmitted)
        {
            if (disposing)
            {
                _contextUnSubm.Dispose();
            }
        }
        _disposedUnsubmitted = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}