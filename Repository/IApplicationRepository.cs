public interface IApplicationRepository : IDisposable
{
    Task<ApplicationsModel> SubmitApplicationForReviewAsync(Guid AppId);

    Task<ApplicationsModel> GetDraftApplicationForUserAsync(Guid userId);

    Task<List<ApplicationsModel>> GetApplicationsSubmittedAfterDateAsync(DateTime date);

    Task<List<object>> GetActivityTypesAsync();

    Task<List<ApplicationsModel>> GetDraftApplicationsAfterDateAsync(DateTime date);
    
    Task<ApplicationsModel> GetApplicationByIdAsync(Guid id);

    Task CreateApplicationAsync(ApplicationsModel app);

    Task UpdateApplicationAsync(ApplicationsModel app, Guid AppId);

    Task DeleteApplicationAsync(Guid appId);

    Task SaveAsync();
    Task SaveAsyncUnsubmitted();

    Task SaveAsyncUsers();

}