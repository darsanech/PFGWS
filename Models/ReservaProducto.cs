using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PFGWS.Models
{
    public class ReservaProducto
    {
        [Key]
        [Column("idreserva")]
        public int idreserva { get; set; }
        [Key]
        [Column("producteid")]
        public int producteid { get; set; }
        [Column("quantitat")]
        public int quantitat { get; set; }
    }
}
