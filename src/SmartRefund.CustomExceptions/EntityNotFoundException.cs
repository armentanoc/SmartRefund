
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public string SpecificEntity { get; private set; }
        public uint Id { get; private set; }

        public EntityNotFoundException(string specificEntity, uint id)
            : base($"Entity {specificEntity} with id {id} doesn't exist.")
        {
            SpecificEntity = specificEntity;
            Id = id;
        }
        public EntityNotFoundException(string specificEntity)
            : base($"Entity with property {specificEntity} doesn't exist.")
        {
            SpecificEntity = specificEntity;
        }
    }
}
