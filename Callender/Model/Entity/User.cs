using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class User
    {
        [Key]
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
    public class AccountCreateDto
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
