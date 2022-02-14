using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class SubPremum
    {
        [Key]
        public string ID { get; set; }
        public string SubPremiumID { get; set; }
        public string PremiumID { get; set; }
    }
}
