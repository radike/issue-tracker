using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Core.Contracts
{
    public interface IIdentifiableEntity
    {
        Guid EntityId { get; set; }
    }
}
