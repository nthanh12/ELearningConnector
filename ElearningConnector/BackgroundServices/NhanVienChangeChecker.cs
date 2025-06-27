namespace ElearningConnector.BackgroundServices
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class NhanVienChangeChecker : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NhanVienChangeChecker> _logger;
        private readonly NhanVienChangeCheckerOptions _options;
        private DateTime _lastChecked;

        public NhanVienChangeChecker(
            IHttpClientFactory httpClientFactory,
            ILogger<NhanVienChangeChecker> logger,
            IOptions<NhanVienChangeCheckerOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;
            _lastChecked = DateTime.UtcNow.AddMinutes(-_options.CheckIntervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var since = _lastChecked.ToString("o");
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.CheckApiUrl}?since={since}");
                    request.Headers.Add("X-BG-API-KEY", _options.BackgroundServiceApiKey);
                    var response = await httpClient.SendAsync(request, stoppingToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<bool>>(content);
                        if (apiResponse != null && apiResponse.Result)
                        {
                            _logger.LogInformation($"[NhanVienChangeChecker] Đã gửi thông báo thành công lúc {DateTime.UtcNow}");
                        }
                        else
                        {
                            _logger.LogInformation($"[NhanVienChangeChecker] Không có thay đổi nhân viên kể từ {since}.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"[NhanVienChangeChecker] Gửi thông báo thất bại: {response.StatusCode}");
                    }
                    _lastChecked = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[NhanVienChangeChecker] Gửi thông báo thất bại do exception");
                }

                // Nếu là 2h sáng, break để service tự restart
                var now = DateTime.Now;
                if (now.Hour == 2 && now.Minute < _options.CheckIntervalMinutes)
                {
                    _logger.LogInformation("[NhanVienChangeChecker] Đã đến 2h sáng, kết thúc vòng lặp để service tự động khởi động lại.");
                    break;
                }

                await Task.Delay(TimeSpan.FromMinutes(_options.CheckIntervalMinutes), stoppingToken);
            }
        }

        public class ApiResponse<T>
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public T Result { get; set; }
        }
    }
} 