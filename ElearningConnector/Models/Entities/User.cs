using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElearningConnector.Models.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int PK_UserID { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public int UserType { get; set; }
        public int isOnline { get; set; }
    }
}