using SQLite;

namespace PFGWS.Models
{
    public class Producto
    {
        [PrimaryKey, Column("producto")]
        public string NomProducte { get; set; }
    }
}
