using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PFGWS.Models
{
    public class Parcela
    {
        [Key]
        [Column("campingid")]
        public int campingid { get; set; }
        [Key]
        [Column("numeroparcela")]
        public string numeroparcela { get; set; }
    }
}
