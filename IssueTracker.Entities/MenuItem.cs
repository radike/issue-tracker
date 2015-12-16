using Common.Data.Core.Contracts;
using System;

namespace IssueTracker.Entities
{
    public class MenuItem : IIdentifiableEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string IconKey { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int MyProperty { get; set; }
    }
}
