﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UpdatePasswordRequest
    {
        public int UserID { get; set; }
        public string NewPassword { get; set; }
    }
}

