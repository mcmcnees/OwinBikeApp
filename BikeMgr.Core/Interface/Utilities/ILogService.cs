using System;

namespace BikeMgr.Core.Interface.Utilities
{
    public interface ILogService
    {
        void LogInformation(string format, params object[] parameters);
        void LogError(string format, params object[] parameters);
        void LogError(Exception ex, string format, params object[] parameters);
        void LogWarning(string format, params object[] parameters);
    }
}
