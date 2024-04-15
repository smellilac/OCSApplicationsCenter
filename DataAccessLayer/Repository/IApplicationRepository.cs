public interface IApplicationRepository 
{
    Task<SubmittedApplications> GetSubmittedApplication(Guid id);

    Task<UnSubmittedApplications> GetUnSubmittedApplication(Guid id);

    Task<List<SubmittedApplications>> GetSubmittedAfter(DateTime date);

    Task<List<UnSubmittedApplications>> GetUnSubmittedOlder(DateTime date);

    Task<UnSubmittedApplications> GetUnSubmittedForUser(Guid userId);

    Task RemoveUnsubmitted(UnSubmittedApplications UnsubApp);

    Task AddToSubmitted(SubmittedApplications SubApp);

    Task AddToUsers(UserModel newUser);

    Task RemoveFromUsers(UserModel newUser);

    Task<UserModel> GetUser(UnSubmittedApplications UnsApp);

    Task AddToUnSubmitted(UnSubmittedApplications UnSubApp);

    Task<T> GenericSearch<T>(Guid AppId) where T : ApplicationsModel;

    Task SaveAsync();
}