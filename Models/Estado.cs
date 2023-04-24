using SQLite;

namespace PFGWS.Models
{
    public class Estado
    {
        [PrimaryKey, AutoIncrement, Column("estadoid")]
        public int estadoid { get; set; }
        [Column("estadoname")]
        public string estadoname { get; set; }
    }
}
