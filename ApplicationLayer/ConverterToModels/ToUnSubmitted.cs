public static class ToUnSubmitted
{
    public static UnSubmittedApplications ToUnSubmittedApp(this SubmittedApplications app)
    {
        UnSubmittedApplications result = new UnSubmittedApplications();

        result.Author = app.Author;
        result.Id = app.Id;
        result.Name = app.Name;
        result.Description = app.Description;
        result.Activity = app.Activity;
        result.Outline = app.Outline;
        result.FirstTimeCreated = app.FirstTimeCreated;
        result.Submited = app.Submited;

        return result;
    }
}