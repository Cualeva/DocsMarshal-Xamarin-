﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsMarshal.Entities
{
    public class BaseJsonResult
    {
        public bool Error { get; set; }
        public string ErrorDescription { get; set; }
        public bool LoggedOut { get; set; }
    }

    public class BaseJsonResult<T> : BaseJsonResult
    {
        public T Data { get; set; }
    }
}
