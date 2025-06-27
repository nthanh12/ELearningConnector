using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElearningConnector.Models.Entities
{
    public class DMBoPhan
    {
        [Key]
        public int Id { get; set; }
        public string MaBoPhan { get; set; }
        public string TenBoPhan { get; set; }
        public string TomTatBoPhan { get; set; }
        public int DMCongTyID { get; set; }
        public string NoiLamViec { get; set; }
        public string DienThoaiBoPhan { get; set; }
        public byte SortNum { get; set; }
        public bool Locked { get; set; }
        //public int Id_ISO { get; set; }

    }
}