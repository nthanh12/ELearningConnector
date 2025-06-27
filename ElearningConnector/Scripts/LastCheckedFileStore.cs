using System;
using System.IO;

public class LastCheckedFileStore
{
    private readonly string _filePath;
    private readonly TimeSpan _fallbackDuration;

    public LastCheckedFileStore(string filePath, TimeSpan? fallbackDuration = null)
    {
        _filePath = filePath;
        _fallbackDuration = fallbackDuration ?? TimeSpan.FromMinutes(30);
    }

    public DateTime GetLastCheckedAt()
    {
        try
        {
            if (!File.Exists(_filePath))
                return DateTime.UtcNow - _fallbackDuration;

            var content = File.ReadAllText(_filePath).Trim();

            if (DateTime.TryParse(content, out var parsed))
                return parsed.ToUniversalTime();

            return DateTime.UtcNow - _fallbackDuration;
        }
        catch
        {
            return DateTime.UtcNow - _fallbackDuration;
        }
    }

    public void SaveLastCheckedAt(DateTime utcTime)
    {
        try
        {
            File.WriteAllText(_filePath, utcTime.ToString("o"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Ghi lastCheckedAt thất bại: {ex.Message}");
        }
    }
} 