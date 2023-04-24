using SQLite;

namespace PFGWS.Models
{
    public class Camping
    {
        [PrimaryKey, AutoIncrement, Column("campingid")]
        public int campingid { get; set; }
        [Column("campingname")]
        public string campingname { get; set; }
    }
}
