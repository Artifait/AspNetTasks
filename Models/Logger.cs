
namespace AspNetTasks.Models
{
    public delegate void LogString(string message);

    public class LoggerAuthRequest
    {
        public readonly ILogger<LoggerAuthRequest> Logger;
        public LoggerAuthRequest(ILogger<LoggerAuthRequest> logger)
        {
            Logger = logger;
        }

        public void LogString(string message) { 
            Logger.LogInformation($"[{DateTime.Now.ToLongTimeString()}]: {message}");
        }
    }
}
