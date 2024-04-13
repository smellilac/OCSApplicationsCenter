public class ApplicationHandler : IApplicationHandler
{
    List<string> activityList = new List<string>();
    private readonly MainModelValidator _mainValidator;
    private readonly IApplicationRepository _repository;
    
    public ApplicationHandler(MainModelValidator _mainValidator,
         ApplicationRepository _repository) 
    {
        this._mainValidator = _mainValidator;
        this._repository = _repository;
    }
    

    public async Task<ApplicationsModel> GetApplicationByIdAsync(Guid id) 
    {
        try 
        {
            var app = await _repository.GetSubmittedApplication(id);
            return app;
        }
        catch (InvalidOperationException)
        {
            try
            {
                return await _repository.GetUnSubmittedApplication(id);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("There is no application with this ID in both tables \n" + ex.Message);
            }
        }
    }
    public async Task<List<SubmittedApplications>> GetApplicationsSubmittedAfterDateAsync(DateTime date) =>
        await _repository.GetSubmittedAfter(date);
    

    public async Task<UnSubmittedApplications> GetDraftApplicationForUserAsync(Guid userId) 
    {
        try 
        {
            // ничего и не изменилось 
            var application = await _repository.GetUnSubmittedForUser(userId);
            return application;
        }
        catch (InvalidOperationException InvalidEx)
        {
            throw new InvalidOperationException("Check ID! There is no draft application with this ID" + InvalidEx);
        } 
    }

    public async Task<List<UnSubmittedApplications>> GetDraftApplicationsAfterDateAsync(DateTime date) =>
        await _repository.GetUnSubmittedOlder(date);

    public async Task<SubmittedApplications> SubmitApplicationForReviewAsync(Guid id) 
    {
        try 
        {
            var appFromDb = await _repository.GetUnSubmittedApplication(id);
            _repository.RemoveUnsubmitted(await _repository.GetUnSubmittedApplication(id));
            appFromDb.Submited = DateTime.UtcNow;
            var anotherApp = ToSubmitted.ToSubmittedApp(appFromDb);
            await _repository.AddToSubmitted(anotherApp);
            return anotherApp;
        }
        catch (InvalidOperationException _)
        {
            throw new InvalidOperationException("You cannot submit this application! Check your application`s ID! \n");
        }
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



    public async Task CreateApplicationAsync(UnSubmittedApplications app) 
    { 
        var validationContext = new ValidationContext<ApplicationsModel>(app); 
        validationContext.RootContextData["ApplyAuthorValidation"] = true;
        var validationResult =await _mainValidator.ValidateAsync(validationContext);
        if (!validationResult.IsValid)
        {
            string errorMessage = string.Join("\n", validationResult.Errors.Select(error => error.ErrorMessage));
            throw new FluentValidation.ValidationException("Validation failed. Errors: \n" + errorMessage);
        }  
        var newUser = new UserModel { Id = app.Author };
        await _repository.AddToUsers(newUser);
        app.Submited = DateTime.MinValue.ToUniversalTime();
        if (app.FirstTimeCreated == DateTime.MinValue) 
            app.FirstTimeCreated = DateTime.UtcNow;
        else 
            app.FirstTimeCreated = DateTime.MinValue.ToUniversalTime();
            await _repository.AddToUnSubmitted(app);
    }

    public async Task<T> UpdateApplicationAsync<T>(T app, Guid AppId) where T : ApplicationsModel
    {
        var validationContext = new ValidationContext<ApplicationsModel>(app); 
        var validationResult =await _mainValidator.ValidateAsync(validationContext);
        if (!validationResult.IsValid)
        {
            string errorMessage = string.Join("\n", validationResult.Errors.Select(error => error.ErrorMessage));
            throw new FluentValidation.ValidationException("Validation failed. Errors: \n" + errorMessage);
        } 
        T appFromDb = await _repository.GenericSearch<T>(AppId);
        appFromDb.Activity = app.Activity;
        appFromDb.Description = app.Description;
        appFromDb.Name = app.Name;
        appFromDb.Outline = app.Outline;

        await SaveAsync();
        return appFromDb;
    }

    public async Task DeleteApplicationAsync(Guid appId) 
    {
        try
        {
            var appFromDb = await _repository.GetUnSubmittedApplication(appId);
            await _repository.RemoveUnsubmitted(appFromDb);
            var user = await _repository.GetUser(appFromDb);
            await _repository.RemoveFromUsers(user);
        }
        catch (InvalidOperationException InvalidEx)
        {
            throw new InvalidOperationException("This application does not exist, check the ID! \t" + InvalidEx.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Something gone wrong, check your`s application \n" + ex.Message);
        }
    }

    public async Task SaveAsync() => await _repository.SaveAsync();
}