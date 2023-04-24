using SQLite;

namespace PFGWS.Models
{
    public class Parcela
    {
        [PrimaryKey, Column("campingid")]
        public int campingid { get; set; }
        [PrimaryKey, Column("numeroparcela")]
        public string numeroparcela { get; set; }
    }
}
