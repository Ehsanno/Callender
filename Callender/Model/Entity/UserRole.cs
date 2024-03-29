﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.Model.Entity
{
    public class UserRole
    {
        [Key]
        public string Row { get; set; } = Guid.NewGuid().ToString();
        public string UserID { get; set; }
        public string RoleID { get; set; } = "4";
    }
}
