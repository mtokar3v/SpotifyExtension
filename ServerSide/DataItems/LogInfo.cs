namespace SpotifyExtension.DataItems
{
    public static class LogInfo
    {
        public static string NewLog(string message, string context = "") => $"{DateTime.Now.ToString()} {message} {context}";
    }
}
