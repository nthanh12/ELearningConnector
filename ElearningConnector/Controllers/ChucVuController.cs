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

[ApiController]
[Authorize]
[Controller]
[Route("api/chucvu")]
public class ChucVuController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChucVuController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách tất cả chức vụ
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
            var query = _context.DMChucDanhs
                //.Where(p => p.TrangThaiHoatDong)
                .Select(c => new ChucDanhDto
                {
                    Id = c.Id,
                    MaChucDanh = c.MaChucDanh,
                    TenChucDanh = c.TenChucDanh,
                    MoTaChucDanh = c.MoTaChucDanh,
                    DMBoPhanID = c.DMBoPhanID,
                    LoaiDoiTuong = c.LoaiDoiTuong,
                    HeSoDieuChinh = (float)c.HeSoDieuChinh,
                    NhomLuong = c.NhomLuong,
                    SoNgayNghiPhepMacDinh = c.SoNgayNghiPhepMacDinh
                });
            if (!string.IsNullOrWhiteSpace(request.Keywords))
            {
                var keyword = request.Keywords.Trim().ToLower();
                query = query.Where(c =>
                    (c.TenChucDanh != null && c.TenChucDanh.ToLower().Contains(keyword)) ||
                    (c.MaChucDanh != null && c.MaChucDanh.ToLower().Contains(keyword))
                );
            }
            int skip = (request.Page - 1) * request.PageSize;
            var list = await query.Skip(skip).Take(request.PageSize).ToListAsync();
            return Ok(ApiResponse<List<ChucDanhDto>>.Success(list));
        }
        catch
        {
            return Ok(ApiResponse<List<ChucDanhDto>>.Error(ApiErrorCode.SystemError));
        }
    }
}