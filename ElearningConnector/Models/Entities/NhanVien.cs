using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElearningConnector.Models.Entities
{
    public class NhanVien
    {
        [Key]
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
        public bool NghiViec { get; set; }
    }
}