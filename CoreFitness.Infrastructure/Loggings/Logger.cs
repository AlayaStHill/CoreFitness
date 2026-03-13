using CoreFitness.Domain.Abstractions.Loggings;
using System.Diagnostics;

namespace CoreFitness.Infrastructure.Loggings;

public sealed class Logger : IDomainLogger
{
    private readonly string _logFilePath;

    public Logger()
    {
        // kan hamna i bin? Directory.GetCurrentDirectory() --> loggarna hamnar i projektets rootmapp
        string logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        Directory.CreateDirectory(logDirectory);

        _logFilePath = Path.Combine(logDirectory, $"log-{DateTime.Now:yyyy-MM-dd}.txt");
    }

    public void Log(string message)
    {
        string formatted = $"{DateTime.Now} | {message}";

        Console.WriteLine(formatted);
        // syns i Output-fönstret   
        Debug.WriteLine(formatted);

        File.AppendAllText(_logFilePath, formatted + Environment.NewLine);
    }

    public void Log(Exception exception)
    {
        if (exception is null)
            return;

        Log(exception.ToString());
    }
}
