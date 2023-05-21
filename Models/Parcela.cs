﻿using SQLite;
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
        [Column("latitud")]
        public string latitud { get; set; }
        [Column("longitud")]
        public string longitud { get; set; }
    }
}
