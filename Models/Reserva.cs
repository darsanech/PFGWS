using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFGWS.Models
{
    public class Reserva
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        [Column("cliente")]
        public string Cliente { get; set; }
        [Column("parcela")]
        public string Parcela { get; set; }
        [Column("camping")]
        public string Camping { get; set; }
        [Column("producte")]
        public string Producte { get; set; }
        [Column("idproducte")]
        public string ProducteId { get; set; }
        [Column("dataini")]
        public string DataIni { get; set; }
        [Column("datafi")]
        public string DataFi { get; set; }
        [Column("preu")]
        public int Preu { get; set; }
        [Column("estado")]
        public string Estado { get; set; }
        [Column("extra")]
        public string Extra { get; set; }
        [Column("lastmod")]
        public string LastMod { get; set; }
    }
}
