﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities.Identity
{
    public class AppUser:IdentityUser<string>
    {
        public string FullName { get; set; }
    }
}
