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
        [Column("needupdate")]
        public bool Needupdate { get; set; }
        [Column("rol")]
        public int Rol { get; set; }
        [Column("ubicacion")]
        public string Ubicacion { get; set; }
    }
}
