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
        [PrimaryKey, AutoIncrement, Column("idreserva")]
        public int idreserva { get; set; }

        [Column("clientename")]
        public string clientename { get; set; }
        [Column("numeroparcela")]
        public string numeroparcela { get; set; }
        [Column("campingid")]
        public int campingid { get; set; }
        [Column("datainici")]
        public string datainici { get; set; }
        [Column("datafinal")]
        public string datafinal { get; set; }
        [Column("preu")]
        public int Preu { get; set; }
        [Column("estadoid")]
        public int estadoid { get; set; }
        [Column("extra")]
        public string Extra { get; set; }
        [Column("userid")]
        public int userid { get; set; }
    }
}
