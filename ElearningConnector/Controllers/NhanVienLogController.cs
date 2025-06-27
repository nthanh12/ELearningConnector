using Microsoft.AspNetCore.Mvc;
using ElearningConnector.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using ElearningConnector.Models.Common;
using Microsoft.Extensions.Configuration;

namespace ElearningConnector.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/nhanvien")]
    public class NhanVienLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public NhanVienLogController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("haschange")]
        [AllowAnonymous]
        public IActionResult HasChange([FromQuery] DateTime since)
        {
            var configKey = _config["NhanVienChangeChecker:BackgroundServiceApiKey"];
            var headerKey = Request.Headers["X-BG-API-KEY"].FirstOrDefault();
            if (!string.IsNullOrEmpty(configKey) && headerKey == configKey)
            {
                // Cho phép background service truy cập không cần JWT
            }
            else if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("[DEBUG] Authorization failed - no valid key and user not authenticated.");
                return Unauthorized();
            }

            try
            {
                // Chuyển đổi thời gian từ UTC sang local (GMT+7)
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var sinceLocal = TimeZoneInfo.ConvertTimeFromUtc(since, timeZone);

                // DEBUG log (nếu cần)
                Console.WriteLine($"[DEBUG] since (UTC): {since:o}, sinceLocal (GMT+7): {sinceLocal:o}");

                var hasChanged = _context.HSNV_LichSuNhanViens.Any(x => x.NgayThayDoi > sinceLocal);
                return Ok(ApiResponse<bool>.Success(hasChanged));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex}");
                return Ok(ApiResponse<bool>.Error(ApiErrorCode.SystemError));
            }
        }

    }
} 