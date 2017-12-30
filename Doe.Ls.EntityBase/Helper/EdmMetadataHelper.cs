using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Doe.Ls.EntityBase.Helper
{
    public class EdmMetadataHelper
    {
        public DbContext Context { get; private set; }

        public EdmMetadataHelper(DbContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            Context = ctx;
        }

        public List<EntityType> GetEntityList(DataSpace dataSpace = DataSpace.CSpace)
        {
            var query = from meta in (Context as IObjectContextAdapter).ObjectContext.MetadataWorkspace.GetItems(DataSpace.CSpace)
                        where meta.BuiltInTypeKind == BuiltInTypeKind.EntityType
                        select meta as EntityType;

                        
            return query.ToList();
        }

        public EdmProperty GetStorageSpaceProperty(EntityType entityType, EdmProperty property, DbContext ctx)
        {

            var enities =
                from meta in
                    (Context as IObjectContextAdapter).ObjectContext.MetadataWorkspace.GetItems(DataSpace.SSpace)
                where meta.BuiltInTypeKind == BuiltInTypeKind.EntityType
                select meta as EntityType;


            var ett = enities.FirstOrDefault(ent => ent.Name == entityType.Name);


            return ett == null ? property : ett.Properties.FirstOrDefault(p => p.Name == property.Name);
        }

        public IEnumerable<EdmProperty> GetPropertyList(EntityType entityType, DbContext ctx, DataSpace dataSpace = DataSpace.SSpace)
        {
            var enities =
                from meta in
                    (Context as IObjectContextAdapter).ObjectContext.MetadataWorkspace.GetItems(dataSpace)
                where meta.BuiltInTypeKind == BuiltInTypeKind.EntityType
                select meta as EntityType;


            var ett = enities.FirstOrDefault(ent => ent.Name == entityType.Name);

            return ett != null ? ett.Properties : Enumerable.Empty<EdmProperty>();
        }

    }
}
