using System;

namespace Common.Data.Core.Contracts
{
    public interface IIdentifiableEntity
    {
        Guid Id { get; set; }
    }
}