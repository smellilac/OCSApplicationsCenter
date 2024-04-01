public  class ForOutputDescription
{
    public static string GetActivityDescription(string activity)
    {
        switch (activity)
        {
            case "Report":
                return "Доклад, 35-45 минут";
            case "Masterclass":
                return "Мастеркласс, 1-2 часа";
            case "Discussion":
                return "Дискуссия / круглый стол, 40-50 минут";
            default:
                return "Описание отсутствует";
        }
    }
}