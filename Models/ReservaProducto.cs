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


        public bool Equals(ReservaProducto other)
        {
            if (other is null)
                return false;

            return this.idreserva == other.idreserva && this.producteid == other.producteid && this.quantitat == other.quantitat;
        }

        public override bool Equals(object obj) => Equals(obj as ReservaProducto);
        public override int GetHashCode() => (idreserva, producteid, quantitat).GetHashCode();
    }
}
