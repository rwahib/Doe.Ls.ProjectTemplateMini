using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
    {
    public static class GeneralOrgExtension
        {
        public static IEnumerable<Executive> FilterDefaults(this IEnumerable<Executive> list,UserInfoExtension user)
        {
            if(user==null)return new List<Executive>();
            var defaultModel = user.DefaultOrganisationalModel;
            if (defaultModel.Divisions == null) return list; // divisions

            return list.Where(executive => defaultModel.Divisions.Contains(executive.ExecutiveCod)).ToList();
        }
        public static IEnumerable<Directorate> FilterDefaults(this IEnumerable<Directorate> list, UserInfoExtension user)
            {
            if(user == null) return new List<Directorate>();
            var defaultModel = user.DefaultOrganisationalModel;
            if(defaultModel.Directorates == null) return list;

            return list.Where(directorate => defaultModel.Directorates.Contains(directorate.DirectorateId));
            }

        public static IEnumerable<BusinessUnit> FilterDefaults(this IEnumerable<BusinessUnit> list, UserInfoExtension user)
            {
            if(user == null) return new List<BusinessUnit>();

            var defaultModel = user.DefaultOrganisationalModel;
            if(defaultModel.BusinessUnites == null) return list;

            return list.Where(bu => defaultModel.BusinessUnites.Contains(bu.BUnitId));
            }

        }
    }