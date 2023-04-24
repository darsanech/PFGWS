using SQLite;

namespace PFGWS.Models
{
    [SQLite.Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("userid")]
        public int userid { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("pass")]
        public string Password { get; set; }
    }
}
