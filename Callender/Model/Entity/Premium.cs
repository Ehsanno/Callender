using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class Premium
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime Expire { get; set; }
    }
}
