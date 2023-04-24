using SQLite;

namespace PFGWS.Models
{
    public class Cliente
    {
        [PrimaryKey, AutoIncrement, Column("clienteid")]
        public int clienteid { get; set; }
        [Column("clientename")]
        public string clientename { get; set; }
    }
}
