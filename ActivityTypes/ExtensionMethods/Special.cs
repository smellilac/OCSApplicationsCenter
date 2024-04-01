public static class ExtensionsForActivity
{
    public static void FillList(this List<string> activityList)
    {
        foreach (string activity in Enum.GetNames(typeof(Activity)))
        {
            activityList.Add(activity);
        }
    }
}