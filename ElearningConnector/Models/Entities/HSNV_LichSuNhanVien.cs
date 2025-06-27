using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElearningConnector.Models.Entities
{
    [Table("HSNV_LichSuNhanVien")]
    public class HSNV_LichSuNhanVien
    {
        [Key]
        public int id { get; set; }
        public int UserID { get; set; }
        public int? DMChucDanhID { get; set; }
        public int? DMBoPhanID { get; set; }
        public int? DMToNhomID { get; set; }
        public int? NhomLuongID { get; set; }
        public int? BacLuongID { get; set; }
        public DateTime NgayThayDoi { get; set; }
        public string? GhiChu { get; set; }
        public int? LoaiNhanVienID { get; set; }
    }
} 