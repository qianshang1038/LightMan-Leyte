using UnityEngine;
using System.IO;

public static class Logger
{
    public enum LogLevel
    {
        None,
        Error,
        Warning,
        Info,
        Debug
    }

    // ����Logger
    public static bool isLoggingEnabled = true;

    // ��ǰ��־����
    public static LogLevel CurrentLogLevel = LogLevel.Debug;

    // �����Ƿ������ļ���־
    public static bool isFileLoggingEnabled = false;

    private static string logFilePath;

    static Logger()
    {
        // ������־�ļ�·��
        logFilePath = Path.Combine(Application.persistentDataPath, "log.txt");
    }

    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
        if (!isLoggingEnabled) return;

        if (level <= CurrentLogLevel)
        {
            Debug.Log(message);
            if (isFileLoggingEnabled)
            {
                WriteToFile($"INFO: {message}");
            }
        }
    }

    public static void Log(int value, LogLevel level = LogLevel.Info)
    {
        Log(value.ToString(), level);
    }

    public static void LogWarning(string message)
    {
        if (!isLoggingEnabled) return;

        if (CurrentLogLevel >= LogLevel.Warning)
        {
            Debug.LogWarning(message);
            if (isFileLoggingEnabled)
            {
                WriteToFile($"WARNING: {message}");
            }
        }
    }

    public static void LogError(string message)
    {
        if (!isLoggingEnabled) return;

        if (CurrentLogLevel >= LogLevel.Error)
        {
            Debug.LogError(message);
            if (isFileLoggingEnabled)
            {
                WriteToFile($"ERROR: {message}");
            }
        }
    }

    private static void WriteToFile(string message)
    {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"{System.DateTime.Now}: {message}");
        }
    }

    public static void EnableLogging(bool enable)
    {
        isLoggingEnabled = enable;
    }
}
