using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class Token
    {
        [Key]
        public string ID { get; set; }
        public string UserToken { get; set; }
        public byte TokenValidator { get; set; } = 1;
        public string UserID { get; set; }
    }
}
