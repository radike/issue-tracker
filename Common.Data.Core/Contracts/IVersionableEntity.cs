﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Core.Contracts
{
    public interface IVersionableEntity
    {
        bool Active { get; set; }
    }
}
