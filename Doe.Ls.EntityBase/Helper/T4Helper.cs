using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.MVCExtensions;
using Microsoft.VisualStudio.TextTemplating;

namespace Doe.Ls.EntityBase.Helper
{
     public static partial class T4Helper
    {

        private static string _classNameFormat = null;

        public static void SetAssemblyClassNameFormatFromContext(DbContext ctx)
        {
            if (_classNameFormat == null)
            {
                _classNameFormat = GetClassNameFormat(ctx);
            }

        }

        public static string GetFullClassName(EntityType entityType)
        {
            ValidateT4HelperInitialised();
            return string.Format(_classNameFormat, entityType.Name);
        }

        private static void ValidateT4HelperInitialised()
        {
            if (_classNameFormat == null)
            {

                throw new InvalidOperationException("classNameFormat is not initialised");
            }
        }

        public static class PropertyOperations
        {

            public static bool IsRequired(EdmProperty property, EntityType entityType, DbContext ctx)
            {
                var edmMetadataHelper = new EdmMetadataHelper(ctx);
                var ssProperty = edmMetadataHelper.GetStorageSpaceProperty(entityType, property, ctx);

                var identity = ssProperty.TypeUsage.Facets.FirstOrDefault(f => f.Name == "StoreGeneratedPattern");
                var isIdentity = identity != null && identity.Value.ToString() == "Identity";
                return !ssProperty.Nullable && (!isIdentity);
            }

            public static bool IsInteger(EdmProperty property)
            {
                var type = property.PrimitiveType.ClrEquivalentType;
                return type == typeof(int) || type == typeof(Int32) || type == typeof(Int64) || type == typeof(UInt16) ||
                       type == typeof(UInt32) || type == typeof(UInt64);
            }

            public static bool IsBinary(EdmProperty property)
            {
                var type = property.PrimitiveType.ClrEquivalentType;
                return type == typeof(byte[]);
            }

            public static bool IsTimeSpan(EdmProperty property)
            {
                var type = property.PrimitiveType.ClrEquivalentType;
                return type == typeof(TimeSpan);
            }
            public static bool IsNullableTimeSpan(EdmProperty property)
            {
                var type = property.PrimitiveType.ClrEquivalentType;
                return type == typeof(TimeSpan?);
            }
            public static bool IsNumber(EdmProperty property)
            {
                var type = property.PrimitiveType.ClrEquivalentType;
                return type == typeof(double) || type == typeof(float) || type == typeof(decimal);
            }

            public static bool IsTextArea(EdmProperty property, EntityType entityType, DbContext ctx)
            {

                var hasRichTextAttribute = MetadataOperations.GetMetadataPropertyCustomAttribute<RichTextAttribute>(property, entityType);
                if (hasRichTextAttribute != null) return true;

                var edmMetadataHelper = new EdmMetadataHelper(ctx);
                var ssProperty = edmMetadataHelper.GetStorageSpaceProperty(entityType, property, ctx);

                if (ssProperty.TypeName.ToLower().Contains("char"))
                {
                    return ssProperty.IsMaxLength || ssProperty.MaxLength >= 512;
                }

                return false;
            }

            public static bool IsRichText(EdmProperty property, EntityType entityType)
            {
                var hasRichTextAttribute = MetadataOperations.GetMetadataPropertyCustomAttribute<RichTextAttribute>(property, entityType);

                return hasRichTextAttribute != null;

            }

            public static bool IsText(EdmProperty property)
            {
                return property.PrimitiveType.ClrEquivalentType == typeof(string);
            }

            public static int GetMaxLength(EdmProperty property, EntityType entityType, DbContext ctx)
            {
                var edmMetadataHelper = new EdmMetadataHelper(ctx);
                var ssProperty = edmMetadataHelper.GetStorageSpaceProperty(entityType, property, ctx);

                return ssProperty.MaxLength.HasValue ? ssProperty.MaxLength.Value : -1;
            }

            public static bool IsPhone(EdmProperty property)
            {
                return property.Name.ToLower().Contains("phone") || property.Name.ToLower().Contains("mobile") ||
                       property.Name.ToLower().Contains("cell");
            }

            public static bool IsEmail(EdmProperty property)
            {
                return property.Name.ToLower().Contains("email");
            }

            public static bool IsPassword(EdmProperty property)
            {
                return property.Name.ToLower().Contains("password");
            }

            public static bool IsDate(EdmProperty property)
            {
                if (IsDateTime(property)) return false;
                return property.UnderlyingPrimitiveType.ClrEquivalentType == typeof(DateTime);
            }

            public static bool IsDateTime(EdmProperty property)
            {
                if (property.UnderlyingPrimitiveType.ClrEquivalentType == typeof(DateTime))
                {
                    if (property.Name.ToLower().Contains("time") || property.Name.ToLower().Contains("lastmodified"))
                        return true;
                }
                return false;
            }

            public static bool IsKeyString(EntityType entityType)
            {
                return entityType.KeyProperties.First().PrimitiveType.ClrEquivalentType == typeof(string);
            }

            public static string GetKeyLocalVariableName(EntityType entityType)
            {
                if (IsKeyString(entityType))
                {
                    var firstOrDefault = entityType.KeyProperties.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        var sb = new StringBuilder();

                        return firstOrDefault.Name.CamelCase();
                    }
                    else
                    {
                        return "entityKey";
                    }
                }
                else
                {
                    return "id";
                }
            }

            public static bool IsCheckbox(EdmProperty edmProp)
            {
                return edmProp.IsPrimitiveType && edmProp.PrimitiveType.ClrEquivalentType == typeof(bool);
            }

            public static bool IsKey(EdmProperty edmProp, EntityType entityType)
            {
                return entityType.KeyMembers.Any(k => k.Name == edmProp.Name);
            }

            /// <summary>
            /// Determines whether [is key automatic increment] for [the specified edm property].
            /// </summary>            
            /// <param name="entityType">Type of the entity.</param>
            /// <param name="ctx">The CTX.</param>
            /// <returns></returns>
            public static bool IsKeyAutoIncrement(EntityType entityType, DbContext ctx)
            {
                var edmHelper = new EdmMetadataHelper(ctx);

                var storageProperty = edmHelper.GetStorageSpaceProperty(entityType, entityType.KeyProperties.FirstOrDefault(), ctx);

                if (!IsInteger(storageProperty))
                {
                    return false;
                }

                var identity = storageProperty.TypeUsage.Facets.FirstOrDefault(f => f.Name == "StoreGeneratedPattern");
                var isIdentity = identity != null && identity.Value.ToString() == "Identity";
                return !storageProperty.Nullable && (isIdentity);

            }

            public static string GetDispayPropertyName(EntityType entityType)
            {

                var displayAttr = MetadataOperations.GetMetadataClassCustomAttribute<DisplayPropertyAttribute>(entityType);
                if (displayAttr != null)
                {

                    return displayAttr.PropertyName;
                }




                var stringProperties = from prop in entityType.Properties
                                       where prop.IsPrimitiveType && prop.PrimitiveType.Name.ToLower().Contains("string")
                                       select prop;

                if (stringProperties.Any())
                {
                    if (stringProperties.Any(p => p.Name.ToLower().EndsWith("name")))
                    {
                        return stringProperties.Where(p => p.Name.ToLower().EndsWith("name")).First().Name;

                    }

                    if (stringProperties.Any(p => p.Name.ToLower().EndsWith("title")))
                    {
                        return stringProperties.Where(p => p.Name.ToLower().EndsWith("title")).First().Name;

                    }
                    return stringProperties.First().Name;

                }

                return entityType.Properties.First().Name;
            }

            public static DisplayPropertyAttribute GetDispayPropertyAttribute(EntityType entityType)
            {

                var displayAttr = MetadataOperations.GetMetadataClassCustomAttribute<DisplayPropertyAttribute>(entityType);
                if (displayAttr != null)
                {

                    return displayAttr;
                }

                return null;
            }

            public static bool IsBoolean(EdmProperty property, EntityType entityType)
            {
                if (property.Name.Equals("active", StringComparison.InvariantCultureIgnoreCase))
                {

                }
                var type = property.PrimitiveType.ClrEquivalentType;
                return type == typeof(bool) || type == typeof(Boolean);
            }

            public static string GetDefaultStatusValueName(EntityType entityType)
            {
                var property = GetProperty("StatusId", entityType);
                var customPropertyAttribute = MetadataOperations.GetMetadataPropertyCustomAttribute<ComputedPropertyAttribute>(property, entityType);
                if (customPropertyAttribute != null && customPropertyAttribute.ComputedPropertyType == ComputedPropertyType.Status)
                {
                    return customPropertyAttribute.DefaultStatusValue.ToString();
                }
                return string.Empty;
            }
        }

        public static string GetFormAction(EntityType entityType, FormType formType)
        {
            return $"\"~/{entityType.Name}/{formType.ToString()}\"";
        }

        public static string GetFormId(EntityType entityType, FormType formType)
        {
            return $"\"form-{GetLocalVariableName(entityType)}-{formType.ToString().ToLower()}\"";
        }

        public static string CleanClassName(string name)
        {

            var badValues = new[] { "_", ".", "-", "&", " ", "!" };
            foreach (var bad in badValues)
            {
                name = name.Replace(bad, "");
            }
            return name;
        }

        public static Type GetMetadataClassFor(EntityType entityType)
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

        public static string GetLocalVariableName(EntityType entityType)
        {
            var cleanName = CleanClassName(entityType.Name);
            return cleanName[0].ToString().ToLower() + cleanName.Substring(1);
        }

        public static string Wordify(string camelCaseWord)
        {
            return camelCaseWord.Wordify();
        }

        public static string Capitalize(string word)
        {
            return word[0].ToString().ToUpper() + word.Substring(1);
        }

        public static void Validate(EdmProperty property, EntityType entityType, DbContext ctx, object value)
        {
            foreach (var mdp in entityType.MetadataProperties)
            {
            }
        }

        public static PropMeta[] ListPropertiesWithFkInfo(EntityType entityType)
        {

            // initialise
            var list = entityType.Properties.Select(prop => new PropMeta { Property = prop, FK = false }).ToList();


            foreach (var entityMember in entityType.NavigationProperties)
            {
                if (entityMember.RelationshipType.RelationshipEndMembers.Skip(1).Take(1).First().GetEntityType().Name ==
                    entityType.Name)
                {
                    foreach (var foreignKey in entityMember.GetDependentProperties())
                    {
                        string fkName = foreignKey.Name;
                        var result = list.Single(mp => mp.Property.Name == fkName);
                        result.FK = true;

                        var lookupEntity = entityMember.RelationshipType.RelationshipEndMembers.First().GetEntityType();
                        result.PareEntityType = lookupEntity;
                        result.FkProperty = GetSingleKeyProperty(lookupEntity);
                    }
                }
            }

            return list.ToArray();
        }

        public static EdmProperty GetSingleKeyProperty(EntityType entityType)
        {
            return entityType.KeyProperties.First();
        }

        public static EdmProperty GetProperty(string propertyName, EntityType entityType)
        {
            return entityType.Properties.SingleOrDefault(p => p.Name == propertyName);
        }

        public static string[] DispayPropertyListForListView(EntityType entityType)
        {
            var listOfProperties = new List<string>();
            foreach (var mp in ListPropertiesWithFkInfo(entityType))
            {
                if (!mp.FK) listOfProperties.Add(mp.Property.Name);
                else
                {
                    listOfProperties.Add(mp.PareEntityType.Name + "." + PropertyOperations.GetDispayPropertyName(mp.PareEntityType));
                }
            }

            return listOfProperties.ToArray();
        }

        public static List<EntityType> GetLookupEntities(EntityType entityType)
        {
            var lookupProperties = entityType.DeclaredNavigationProperties.Where(np => np.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One || np.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne)
                .Select(np => np.ToEndMember.GetEntityType()).ToList()
                ;
            return lookupProperties;

        }

        public static List<EntityType> GetChildEntities(EntityType entityType)
        {
            var manyProperties = entityType.DeclaredNavigationProperties.Where(np => np.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many).Select(np => np.ToEndMember.GetEntityType()).ToList();
            return manyProperties;

        }

        public static bool IsFormInput(EdmProperty edmProp, EntityType entityType, DbContext ctx)
        {
            if (PropertyOperations.IsTextArea(edmProp, entityType, ctx)) return false;
            return edmProp.IsPrimitiveType;
        }

        public static string GetInputType(EdmProperty edmProp, EntityType entityType, DbContext ctx)
        {


            if (PropertyOperations.IsDate(edmProp) || PropertyOperations.IsDateTime(edmProp)) return "date";
            if (PropertyOperations.IsEmail(edmProp)) return "email";
            if (PropertyOperations.IsPhone(edmProp)) return "tel";
            if (PropertyOperations.IsCheckbox(edmProp)) return "checkbox";
            if (PropertyOperations.IsInteger(edmProp)) return "number";
            return PropertyOperations.IsPassword(edmProp) ? "password" : "text";
        }

        public static EntityType GetEntityType(string name, DbContext ctx)
        {
            var helper = new EdmMetadataHelper(ctx);
            return helper.GetEntityList(DataSpace.SSpace).SingleOrDefault(e => e.Name == name);
        }

        public static List<EdmProperty> GetKeys(EntityType entityType)
        {
            return entityType.KeyProperties.ToList();
        }

        public static bool HasSingleKeyNumber(EntityType entityType)
        {
            if (entityType.KeyProperties.Count != 1) return false;
            return PropertyOperations.IsInteger(entityType.KeyProperties.SingleOrDefault());

        }

        public static string GetComputedCodeForInsert(EntityType entityType, DbContext ctx)
        {
            var sb = new StringBuilder();
            foreach (var prop in entityType.Properties)
            {
                var attribute = MetadataOperations.GetMetadataPropertyCustomAttribute<ComputedPropertyAttribute>(prop, entityType);
                if (attribute == null) continue;

                switch (attribute.ComputedPropertyType)
                {
                    case ComputedPropertyType.CreationDate:
                    case ComputedPropertyType.LastModifiedDate:
                        {
                            sb.Append($"\n\t\t\tentity.{prop.Name} = DateTime.Now;");
                            break;
                        }
                    case ComputedPropertyType.LastModifiedUser:
                    case ComputedPropertyType.CreatedUser:
                        {
                            sb.Append($"\n\t\t\tentity.{prop.Name} = this.GetCurrentUser().UserName;");
                            break;
                        }

                    case ComputedPropertyType.AutoIncrement:
                        {
                            sb.Append($"\n\t\t\tentity.{prop.Name} = this.GetNewKey();");
                            break;
                        }

                    case ComputedPropertyType.Status:
                        {
                            if (PropertyOperations.IsInteger(prop))
                            {
                                sb.Append($"\n\t\t\tentity.{prop.Name} = {attribute.DefaultStatusValue};");
                            }
                            else
                            {
                                sb.Append($"\n\t\t\tentity.{prop.Name} = \"{attribute.DefaultStatusValue}\";");
                            }
                            break;
                        }

                }
            }

            return sb.ToString();
        }

        public static string GetComputedCodeForUpdate(EntityType entityType, DbContext ctx)
        {
            var sb = new StringBuilder();
            foreach (var prop in entityType.Properties)
            {
                var attribute = MetadataOperations.GetMetadataPropertyCustomAttribute<ComputedPropertyAttribute>(prop, entityType);
                if (attribute == null) continue;
                switch (attribute.ComputedPropertyType)
                {
                    case ComputedPropertyType.LastModifiedDate:
                        {
                            sb.Append($"\n\t\t\tentity.{prop.Name} = DateTime.Now;");
                            break;
                        }
                    case ComputedPropertyType.LastModifiedUser:
                        {
                            sb.Append($"\n\t\t\tentity.{prop.Name} = this.GetCurrentUser().UserName;");
                            break;
                        }

                }
            }

            return sb.ToString();
        }
         
        private static string GetClassNameFormat(DbContext ctx)
        {
            var assemblyFullPath = ctx.GetType().Assembly.FullName;
            var slices = assemblyFullPath.Split(',');
            var classFullPath = slices[0] + ".{0}";
            assemblyFullPath = assemblyFullPath.Insert(0, classFullPath + ",");
            return assemblyFullPath.ToString();
        }

    }
}