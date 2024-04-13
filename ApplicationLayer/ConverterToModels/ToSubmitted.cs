public class ToSubmitted
{
    public static SubmittedApplications ToSubmittedApp(UnSubmittedApplications appModel)
    {
        SubmittedApplications result = new SubmittedApplications();

        result.Author = appModel.Author;
        result.Id = appModel.Id;
        result.Name = appModel.Name;
        result.Description = appModel.Description;
        result.Activity = appModel.Activity;
        result.Outline = appModel.Outline;
        result.FirstTimeCreated = appModel.FirstTimeCreated;
        result.Submited = appModel.Submited;

        return result;
    }
}