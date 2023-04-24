using SQLite;

namespace PFGWS.Models
{
    public class Producto
    {
        [PrimaryKey, AutoIncrement, Column("producteid")]
        public int producteid { get; set; }
        [Column("productoname")]
        public string productoname { get; set; }
    }
}
