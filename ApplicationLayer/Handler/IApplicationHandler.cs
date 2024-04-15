public interface IApplicationHandler 
{
    Task<SubmittedApplications> SubmitApplicationForReviewAsync(Guid AppId);

    Task<UnSubmittedApplications> GetDraftApplicationForUserAsync(Guid userId);

    Task<List<SubmittedApplications>> GetApplicationsSubmittedAfterDateAsync(DateTime date);

    Task<List<object>> GetActivityTypesAsync();

    Task<List<UnSubmittedApplications>> GetDraftApplicationsAfterDateAsync(DateTime date);
    
    Task<ApplicationsModel> GetApplicationByIdAsync(Guid id);

    Task CreateApplicationAsync(UnSubmittedApplications app);

    // Task<UnSubmittedApplications> UpdateApplicationAsync(UnSubmittedApplications app, Guid AppId);
    Task<T> UpdateApplicationAsync<T>(T app, Guid AppId) where T : ApplicationsModel;
    Task DeleteApplicationAsync(Guid appId);

    Task SaveAsync();
}