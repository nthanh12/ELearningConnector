using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Newtonsoft.Json;

namespace ElearningConnector.Tests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // --- 1. AuthController ---
        [Fact]
        public async Task AuthController_ReturnsInvalidCredentials_WhenUserOrPassWrong()
        {
            var request = new { UserName = "saiuser", Password = "saipass" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authen", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0002\"");
            responseString.Should().Contain("Tài khoản hoặc mật khẩu không chính xác");
        }

        [Fact]
        public async Task AuthController_ReturnsJwt_WhenLoginSuccess()
        {
            var request = new { UserName = "phuongloan", Password = "thanh123" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authen", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        // --- 2. ChucVuController ---
        [Fact]
        public async Task ChucVuController_ReturnsInvalidApiKey_WhenApiKeyIsWrong()
        {
            var request = new { ApiKey = "sai-api-key" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/chucvu", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0003\"");
            responseString.Should().Contain("API-KEY không tồn tại hoặc không chính xác");
        }

        [Fact]
        public async Task ChucVuController_ReturnsSuccess_WhenApiKeyIsValid()
        {
            var request = new { ApiKey = "f3b9a2d4-5c1e-47d8-8a11-2e4f8a7b0c2d" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/chucvu", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        // --- 3. CanBoController ---
        [Fact]
        public async Task CanBoController_ReturnsInvalidApiKey_WhenApiKeyIsWrong()
        {
            var request = new { ApiKey = "sai-api-key" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/danhsachcanbo", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0003\"");
            responseString.Should().Contain("API-KEY không tồn tại hoặc không chính xác");
        }

        [Fact]
        public async Task CanBoController_ReturnsSuccess_WhenApiKeyIsValid()
        {
            var request = new { ApiKey = "f3b9a2d4-5c1e-47d8-8a11-2e4f8a7b0c2d" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/danhsachcanbo", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        // --- 4. PhongBanController ---
        [Fact]
        public async Task PhongBanController_ReturnsInvalidApiKey_WhenApiKeyIsWrong()
        {
            var request = new { ApiKey = "sai-api-key" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/phongban", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0003\"");
            responseString.Should().Contain("API-KEY không tồn tại hoặc không chính xác");
        }

        [Fact]
        public async Task PhongBanController_ReturnsSuccess_WhenApiKeyIsValid()
        {
            var request = new { ApiKey = "f3b9a2d4-5c1e-47d8-8a11-2e4f8a7b0c2d" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/phongban", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        // Helper: Đăng nhập lấy JWT
        private async Task<string> GetJwtTokenAsync(string username, string password)
        {
            var loginRequest = new { UserName = username, PassWord = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authen", content);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(responseString);
            return (string)obj.result;
        }

        // --- 5. Test API danh sách với JWT (phân trang, lọc) ---
        [Fact]
        public async Task CanBoController_ReturnsPagedResult_WithJwt()
        {
            var token = await GetJwtTokenAsync("youruser", "yourpass");
            var request = new { Page = 1, PageSize = 5, Keywords = "" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/danhsachcanbo") { Content = content };
            httpRequest.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _client.SendAsync(httpRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        [Fact]
        public async Task ChucVuController_ReturnsPagedResult_WithJwt()
        {
            var token = await GetJwtTokenAsync("youruser", "yourpass");
            var request = new { Page = 1, PageSize = 5, Keywords = "" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/chucvu") { Content = content };
            httpRequest.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _client.SendAsync(httpRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        [Fact]
        public async Task PhongBanController_ReturnsPagedResult_WithJwt()
        {
            var token = await GetJwtTokenAsync("youruser", "yourpass");
            var request = new { Page = 1, PageSize = 5, Keywords = "" };
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/phongban") { Content = content };
            httpRequest.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _client.SendAsync(httpRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }

        // --- 6. Test API log thay đổi nhân viên ---
        [Fact]
        public async Task NhanVienLogController_HasChange_ReturnsResult()
        {
            var response = await _client.GetAsync("/api/nhanvien/haschange");
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseString.Should().Contain("\"code\":\"0000\"");
            responseString.Should().Contain("result");
        }
    }
} 