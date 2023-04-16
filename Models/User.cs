using SQLite;

namespace PFGWS.Models
{
    [SQLite.Table("Users")]
    public class User
    {
        [PrimaryKey, Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string password { get; set; }
    }
}
