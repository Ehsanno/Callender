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
        public string Pass { get; set; }
        public string Phone{ get; set; }
    }
    public class AccountCreateDto
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Pass { get; set; }
        [MinLength(11)]
        public string Phone{ get; set; }
    }
    public class LoginInformationDto
    {
        public string UserName { get; set; }
        [Required]
        [MaxLength(15)]
        public string Pass { get; set; }
    }
}
