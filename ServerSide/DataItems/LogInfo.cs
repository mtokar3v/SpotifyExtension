namespace SpotifyExtension.DataItems
{
    public static class LogInfo
    {
        public static string NewLog(string message, string? stackTrace = "") => $"{DateTime.Now.ToString()}\n{message} {stackTrace}";
    }
}
