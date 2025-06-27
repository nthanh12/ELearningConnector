using System;

namespace ElearningConnector.Models.DTOs
{
    public class BoPhanDto
    {
        public int Id { get; set; }
        public string MaBoPhan { get; set; } = string.Empty;
        public string TenBoPhan { get; set; } = string.Empty;
        public string TomTatBoPhan { get; set; }
        public int DMCongTyID { get; set; }
        public string NoiLamViec { get; set; }
        public string DienThoaiBoPhan { get; set; }
        public int SortNum { get; set; }
        public bool Locked { get; set; }
        //public int Id_ISO { get; set; }
    }
}