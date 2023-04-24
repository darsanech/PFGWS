using SQLite;

namespace PFGWS.Models
{
    public class EstadoProducto
    {
        [PrimaryKey, AutoIncrement, Column("estadoproductoid")]
        public int estadoproductoid { get; set; }
        [Column("estadoproductoname")]
        public string estadoproductoname { get; set; }
    }
}
