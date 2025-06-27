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
[Route("api/danhsachcanbo")]
public class CanBoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CanBoController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách tất cả cán bộ
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
            var query = from nv in _context.NhanViens
                        join bp in _context.DMBoPhans on nv.DMBoPhanID equals bp.Id
                        join cv in _context.DMChucDanhs on nv.DMChucDanhID equals cv.Id
                        join us in _context.Users on nv.Id equals us.PK_UserID
                        where !nv.NghiViec
                        select new NhanVienDto
                        {
                            Id = nv.Id,
                            DMBoPhanID = nv.DMBoPhanID,
                            DMChucDanhID = nv.DMChucDanhID,
                            TenDayDu = nv.TenDayDu,
                            SoCMND = nv.SoCMND,
                            NgayCapCMND = nv.NgayCapCMND,
                            NoiCapCMND = nv.NoiCapCMND,
                            NgaySinh = nv.NgaySinh,
                            Email = nv.Email,
                            HoKhauThuongTru = nv.HoKhauThuongTru,
                            NoiOHienTai = nv.NoiOHienTai,
                            DienThoaiDiDong = nv.DienThoaiDiDong,
                            DienThoaiNhaRieng = nv.DienThoaiNhaRieng,
                            DienThoaiLamViec = nv.DienThoaiLamViec,
                            TrangThaiCongTac = !nv.NghiViec
                        };

            if (!string.IsNullOrWhiteSpace(request.Keywords))
            {
                var keyword = request.Keywords.Trim().ToLower();
                query = query.Where(nv =>
                    (nv.TenDayDu != null && nv.TenDayDu.ToLower().Contains(keyword)) ||
                    (nv.SoCMND != null && nv.SoCMND.ToLower().Contains(keyword)) ||
                    (nv.Email != null && nv.Email.ToLower().Contains(keyword))
                );
            }

            int skip = (request.Page - 1) * request.PageSize;
            var list = await query.Skip(skip).Take(request.PageSize).ToListAsync();

            return Ok(ApiResponse<List<NhanVienDto>>.Success(list));
        }
        catch
        {
            return Ok(ApiResponse<List<NhanVienDto>>.Error(ApiErrorCode.SystemError));
        }
    }
}
