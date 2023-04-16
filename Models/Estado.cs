using SQLite;

namespace PFGWS.Models
{
    public class Estado
    {
        [PrimaryKey, Column("estado")]
        public string TipoEstado { get; set; }
    }
}
