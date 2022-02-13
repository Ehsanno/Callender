using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class Carrier
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string CarrierName { get; set; }
    }
}
