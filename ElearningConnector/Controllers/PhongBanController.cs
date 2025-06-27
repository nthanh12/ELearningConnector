using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElearningConnector.Data;
using ElearningConnector.Models.DTOs;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using ElearningConnector.Models.Common;
using ElearningConnector.Models.Requests;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/phongban")]
public class PhongBanController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PhongBanController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách tất cả phòng ban
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PagedRequest request)
    {
        if (request == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var query = _context.DMBoPhans
                .Where(d => !d.Locked)
                .Select(d => new BoPhanDto
                {
                    Id = d.Id,
                    MaBoPhan = d.MaBoPhan,
                    TenBoPhan = d.TenBoPhan,
                    TomTatBoPhan = d.TomTatBoPhan,
                    DMCongTyID = d.DMCongTyID,
                    NoiLamViec = d.NoiLamViec,
                    DienThoaiBoPhan = d.DienThoaiBoPhan,
                    SortNum = d.SortNum,
                    Locked = d.Locked,
                    //Id_ISO = d.Id_ISO
                });
            if (!string.IsNullOrWhiteSpace(request.Keywords))
            {
                var keyword = request.Keywords.Trim().ToLower();
                query = query.Where(d =>
                    (d.TenBoPhan != null && d.TenBoPhan.ToLower().Contains(keyword)) ||
                    (d.MaBoPhan != null && d.MaBoPhan.ToLower().Contains(keyword))
                );
            }
            int skip = (request.Page - 1) * request.PageSize;
            var list = await query.Skip(skip).Take(request.PageSize).ToListAsync();
            return Ok(ApiResponse<List<BoPhanDto>>.Success(list));
        }
        catch
        {
            return Ok(ApiResponse<List<BoPhanDto>>.Error(ApiErrorCode.SystemError));
        }
    }
}
