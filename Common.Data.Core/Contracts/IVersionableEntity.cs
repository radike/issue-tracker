using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Core.Contracts
{
    public interface IVersionableEntity
    {
        DateTime CreatedAt { get; set; }
        bool Active { get; set; }
    }
}
