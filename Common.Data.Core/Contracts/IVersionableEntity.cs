using System;

namespace Common.Data.Core.Contracts
{
    public interface IVersionableEntity
    {
        DateTime CreatedAt { get; set; }
        bool Active { get; set; }
    }
}
