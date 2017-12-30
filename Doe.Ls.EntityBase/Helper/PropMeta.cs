using System.Data.Entity.Core.Metadata.Edm;

namespace Doe.Ls.EntityBase.Helper
{
    public class PropMeta {
        public EdmProperty Property { get; set; }
        public bool FK { get; set; }
        public EntityType PareEntityType { get; set; }
        public EdmProperty FkProperty { get; set; }
        public override string ToString()
        {
            return FK ? $"{Property.Name}-{FkProperty.Name}-{PareEntityType.Name}" : $"{Property.Name}";
        }
    }
}