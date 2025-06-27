using System;

namespace ElearningConnector.Models.DTOs
{

    public class NhanVienDto
    {
        public int Id { get; set; }
        public int DMBoPhanID { get; set; }
        public int DMChucDanhID { get; set; }
        public string TenDayDu { get; set; } = string.Empty;
        public string? SoCMND { get; set; }
        public DateTime? NgayCapCMND { get; set; }
        public string? NoiCapCMND { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? Email { get; set; }
        public string? HoKhauThuongTru { get; set; }
        public string? NoiOHienTai { get; set; }
        public string? DienThoaiDiDong { get; set; }
        public string? DienThoaiNhaRieng { get; set; }
        public string? DienThoaiLamViec { get; set; }
        public bool TrangThaiCongTac { get; set; }
        //public int Status { get; set; } 
    }
}
