using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class NhanVienChangeChecker
{
    private readonly HttpClient _httpClient;
    private readonly LastCheckedFileStore _fileStore;
    private readonly string _apiUrl;

    public NhanVienChangeChecker(HttpClient httpClient, string apiUrl, LastCheckedFileStore fileStore)
    {
        _httpClient = httpClient;
        _apiUrl = apiUrl;
        _fileStore = fileStore;
    }

    public async Task<bool> CheckAndUpdateAsync()
    {
        var since = _fileStore.GetLastCheckedAt();
        Console.WriteLine($"[CHECK] Gọi API kiểm tra thay đổi từ {since:o}");

        try
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}?since={since:o}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<bool>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var hasChanged = result?.Result ?? false;
            Console.WriteLine($"[RESULT] Has changed: {hasChanged}");

            // lưu lại thời điểm kiểm tra
            _fileStore.SaveLastCheckedAt(DateTime.UtcNow);

            return hasChanged;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Gọi API thất bại: {ex.Message}");
            return false;
        }
    }

    private class ApiResponse<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
} 