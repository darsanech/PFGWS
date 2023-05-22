using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFGWS.Models
{
    class BearerToken
    {
        public BearerToken() { }
        public string Token { get; set; }
        public int UserId { get; set; }
        public int ExpirationInMinutes { get; set; }
        public int Rol { get; set; }


    }
}
