namespace Friday.ERP.Core.IServices;

public interface ILoggerManager
{
    void LogInfo(string message);

    void LogWarn(string message);

    void LogDebug(string message);

    void LogError(string message);
}