using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.MVCExtensions
    {
    public static class EntityModelHelper
        {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signatureEntity"></param>
        /// <param name="factory"></param>
        /// <returns>List of found property names</returns>
        public static string[] UpdateSignature(this object signatureEntity, IRepositoryFactory factory)
            {
            var list = new List<string>();
            var entityType = signatureEntity.GetType();

            var createdDatePInfo = GetPropertyByName(entityType, "CreatedDate") ??
                                   GetPropertyByName(entityType, "DateCteated");
            if(createdDatePInfo != null) list.Add(createdDatePInfo.Name);

            var lastModifiedDatePInfo = (GetPropertyByName(entityType, "LastModifiedDate") ??
                                         GetPropertyByName(entityType, "DateModified")) ??
                                        GetPropertyByName(entityType, "DateUpdated");
            if(lastModifiedDatePInfo != null) list.Add(lastModifiedDatePInfo.Name);

            var createdByPInfo = GetPropertyByName(entityType, "CreatedBy");
            if(createdByPInfo != null) list.Add(createdByPInfo.Name);

            var lastModifiedByPInfo = (GetPropertyByName(entityType, "LastModifiedBy") ??
                                       GetPropertyByName(entityType, "ModifiedBy")) ??
                                      GetPropertyByName(entityType, "UpdatedBy");
            if(lastModifiedByPInfo != null) list.Add(lastModifiedByPInfo.Name);

            var timeStampPInfo = GetPropertyByName(entityType, "TimeStamp");
            if(timeStampPInfo != null) list.Add(timeStampPInfo.Name);

            lastModifiedDatePInfo?.SetValue(signatureEntity, DateTime.Now);

            if(createdDatePInfo != null)
                {
                var createdDateValue = createdDatePInfo.GetValue(signatureEntity);

                if(createdDateValue is DateTime && (DateTime)createdDateValue == DateTime.MinValue)
                    {
                    createdDatePInfo.SetValue(signatureEntity, DateTime.Now);
                    }
                else if(createdDateValue is DateTime? && !((DateTime?)createdDateValue).HasValue)
                    {
                    createdDatePInfo.SetValue(signatureEntity, DateTime.Now);
                    }
                }

            if(createdByPInfo != null)
                {
                var createdByValue = createdByPInfo.GetValue(signatureEntity) as string;

                if(string.IsNullOrWhiteSpace(createdByValue))
                    {
                    createdByPInfo.SetValue(signatureEntity, factory.GetService<ISessionService>().GetCurrentUser().UserName);
                    }
                }

            lastModifiedByPInfo?.SetValue(signatureEntity, factory.GetService<ISessionService>().GetCurrentUser().UserName);

            var timeStampValue = timeStampPInfo?.GetValue(signatureEntity);

            if(timeStampValue is DateTime)
            {
                timeStampPInfo.SetValue(signatureEntity, DateTime.Now);
            }

            return list.ToArray();
            }

        public static void RemoveList(this ModelStateDictionary state, string[] propNames)
            {
            foreach(var propName in propNames)
                {
                state.Remove(propName);
                }

            }
        public static void RemoveTrailingList(this ModelStateDictionary state)
        {
            var propNames = new[]
            {
                "CreatedDate", "DateCteated", "LastModifiedDate", "DateModified", "LastModifiedDate", "DateUpdated",
                "CreatedBy", "LastModifiedBy", "ModifiedBy", "UpdatedBy", "TimeStamp"
            };

            foreach(var propName in propNames)
                {
                state.Remove(propName);
                }

            }
        
        public static PropertyInfo GetPropertyByName(Type theType, string propertyName)
            {
            if(string.IsNullOrWhiteSpace(propertyName) || theType == null) return null;

            foreach(var pInfo in theType.GetProperties().Where(p => p.CanRead
            && p.CanWrite
            && p.MemberType == MemberTypes.Property
            && !p.GetSetMethod().IsStatic
            && p.GetSetMethod() != null).ToList())
                {
                if(pInfo.Name.Equals(propertyName))
                    {
                    return pInfo;
                    }

                }
            return null;
            }

        }
    }