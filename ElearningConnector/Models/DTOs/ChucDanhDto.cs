using System;

namespace ElearningConnector.Models.DTOs
{
    public class ChucDanhDto
    {
        public int Id { get; set; }
        public string MaChucDanh { get; set; } = string.Empty;
        public string TenChucDanh { get; set; } = string.Empty;
        public string MoTaChucDanh { get; set; }
        public int DMBoPhanID { get; set; }
        public bool LoaiDoiTuong { get; set; } = false;
        public float HeSoDieuChinh { get; set; }
        public int NhomLuong { get; set; }
        public int SoNgayNghiPhepMacDinh { get; set; }
    }
}