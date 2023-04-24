using SQLite;

namespace PFGWS.Models
{
    public class ProductoCode
    {
        [PrimaryKey, AutoIncrement, Column("productecodeid")]
        public int productecodeid { get; set; }
        [Column("productocode")]
        public string productocode { get; set; }
        [Column("productoid")]
        public int productoid { get; set; }
        [Column("estadoproductoid")]
        public int estadoproductoid { get; set; }
    }
}
