using SQLite;

namespace PFGWS.Models
{
    public class Suscripcion
    {
        [PrimaryKey, Column("userid")]
        public int userid { get; set; }
        [PrimaryKey, Column("campingid")]
        public int campingid { get; set; }
        [Column("update")]
        public bool update { get; set; }
    }
}
