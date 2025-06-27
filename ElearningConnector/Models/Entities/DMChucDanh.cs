using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElearningConnector.Models.Entities
{
    public class DMChucDanh
    {
        [Key]
        public int Id { get; set; }
        public string MaChucDanh { get; set; }
        public string TenChucDanh { get; set; }
        public string MoTaChucDanh { get; set; }
        public int DMBoPhanID { get; set; }
        public bool LoaiDoiTuong { get; set; } = false;
        public double HeSoDieuChinh { get; set; }
        public byte NhomLuong { get; set; }
        public byte SoNgayNghiPhepMacDinh { get; set; }

    }
}
