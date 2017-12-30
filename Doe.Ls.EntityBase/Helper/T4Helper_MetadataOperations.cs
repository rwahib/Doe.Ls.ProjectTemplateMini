using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using Doe.Ls.EntityBase.MVCExtensions;

namespace Doe.Ls.EntityBase.Helper
{
    public static partial class T4Helper
    {
        public static class MetadataOperations
        {
            public static string GetDispayPropertyName(EntityType entityType)
            {
                return PropertyOperations.GetDispayPropertyName(entityType);
            }

            public static Type GetMetadaClassType(EntityType entityType)
            {
                var className = GetFullClassName(entityType);
                Type classType = Type.GetType(className);
                if (classType != null)
                {
                    var atts = classType.GetCustomAttributes(true);
                    var metadataAttribute = atts.FirstOrDefault(a => a is MetadataTypeAttribute) as MetadataTypeAttribute;
                    if (metadataAttribute != null)
                    {
                        var metadataClassType = metadataAttribute.MetadataClassType;

                        return metadataClassType;
                    }
                }

                return null;
            }

            public static object[] GetMetadataClassCustomAttributes(EntityType entityType)
            {
                Type classType = GetMetadaClassType(entityType);
                if (classType != null)
                {
                    var atts = classType.GetCustomAttributes(true);
                    var metadataAttribute = atts.FirstOrDefault(a => a is MetadataTypeAttribute) as MetadataTypeAttribute;
                    if (metadataAttribute != null)
                    {
                        var metadataClassType = metadataAttribute.MetadataClassType;
                        var metaAttribuites = metadataClassType.GetCustomAttributes(true);

                        return metaAttribuites;
                    }
                }

                return null;
            }

            public static T GetMetadataClassCustomAttribute<T>(EntityType entityType) where T : Attribute
            {
                Type classType = GetMetadaClassType(entityType);
                if (classType != null)
                {
                    var atts = classType.GetCustomAttributes(true);
                    return atts.FirstOrDefault(at => (at is T)) as T;
                }

                return null;
            }

            public static object[] GetMetadataPropertyCustomAttributes(Type attributeType, EdmProperty edmProp, EntityType entityType)
            {
                Type classType = GetMetadaClassType(entityType);
                if (classType != null)
                {

                    var property = classType.GetProperties(BindingFlags.Default).FirstOrDefault(p => p.Name.Equals(edmProp.Name, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        return property.GetCustomAttributes(true);
                    }
                }

                return null;
            }

            public static T GetMetadataPropertyCustomAttribute<T>(EdmProperty edmProp, EntityType entityType) where T : Attribute
            {


                Type classType = GetMetadaClassType(entityType);
                if (classType != null)
                {
                    if (
                        !classType.GetProperties()
                            .Any(p => p.Name.Equals(edmProp.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        return null;

                    }

                    var property = classType.GetProperties().FirstOrDefault(p => p.Name.Equals(edmProp.Name, StringComparison.OrdinalIgnoreCase));



                    if (property != null)
                    {
                        return property.GetCustomAttributes(true).FirstOrDefault(at => (at is T)) as T;
                    }
                }

                return null;
            }

            public static bool HasAnyRichText(EntityType entityType)
            {
                return entityType.Properties.Any(property => PropertyOperations.IsRichText(property, entityType));
            }

            public static bool IsSecure(EntityType entityType)
            {
                var att = GetMetadataClassCustomAttribute<SecureAttribute>(entityType);
                if (att != null && att.Secure) return true;
                return false;

            }

            public static string AccessRoles(EntityType entityType)
            {
                var att = GetMetadataClassCustomAttribute<SecureAttribute>(entityType);
                if (att != null && att.Secure) return att.Roles;
                return string.Empty;

            }
        }



    }
}