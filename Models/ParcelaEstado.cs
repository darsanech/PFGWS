using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PFGWS.Models
{
    public class ParcelaEstado
    {
        [PrimaryKey, AutoIncrement, Column("parcelaestadoid")]
        public int parcelaestadoid { get; set; }
        [Column("campingid")]
        public int campingid { get; set; }
        [Column("numeroparcela")]
        public string numeroparcela { get; set; }
        [Column("estadoid")]
        public int estadoid { get; set; }
    }
}
