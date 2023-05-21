using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PFGWS.Models
{
    public class Parcela
    {
        [PrimaryKey, AutoIncrement, Column("parcelaid")]
        public int parcelaid { get; set; }
        [Column("campingid")]
        public int campingid { get; set; }
        [Column("numeroparcela")]
        public string numeroparcela { get; set; }
        [Column("geometryy")]
        public string geometryy { get; set; }
        [Column("estadoid")]
        public int estadoid { get; set; }
    }
}