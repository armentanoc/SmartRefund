
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity
    {
        public uint Id { get; private set; }
        public void SetId(uint id)
        {
            Id = id;
        }
    }
}
