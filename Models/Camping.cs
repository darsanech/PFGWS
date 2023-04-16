using SQLite;

namespace PFGWS.Models
{
    public class Camping
    {
        [PrimaryKey, Column("camping")]
        public string NomCamping { get; set; }
    }
}
