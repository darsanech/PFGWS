using SQLite;

namespace PFGWS.Models
{
    public class Cliente
    {
        [PrimaryKey, Column("id")]
        public int id { get; set; }

        public string NomClient { get; set; }
    }
}
