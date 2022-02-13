using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class UserCarrier
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string CarrierID { get; set; }
    }
    public class SetUserCarrier
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string CarrierID { get; set; }
    }
}
