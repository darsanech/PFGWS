using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PFGWS.Models
{
    public class Suscripcion
    {
        [Key]
        [Column("userid")]
        public int userid { get; set; }
        [Key]
        [Column("campingid")]
        public int campingid { get; set; }
        [Column("needupdate")]
        public bool update { get; set; }
    }
}
