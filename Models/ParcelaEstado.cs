using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PFGWS.Models
{
    public class ParcelaEstado
    {
        [Key]
        [Column("campingid")]
        public int campingid { get; set; }
        [Key]
        [Column("numeroparcela")]
        public string numeroparcela { get; set; }
        [Column("estadoid")]
        public int estadoid { get; set; }
    }
}
