namespace DreamEchoesCore.RingLib;

internal class Log
{
    public static void LogInfo(string key, string message)
    {
#if DEBUG
        DreamEchoesCore.Instance.Log($"{key}: {message}");
#endif
    }
    public static void LogError(string key, string message)
    {
        DreamEchoesCore.Instance.LogError($"{key}: {message}");
    }
}
