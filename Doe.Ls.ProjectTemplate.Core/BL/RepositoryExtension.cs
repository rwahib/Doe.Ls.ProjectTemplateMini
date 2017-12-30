using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL
{
    public static class RepositoryExtension
    {
        public static int GetDbNewId<T>(this IRepository<T> rep, int increment = 1)
        {
            //var name = $"'{typeof(T).Name}'";
            var name = typeof(T).Name;
            return GetDbNewId(rep,name, increment);
            
        }

        public static int GetDbNewId<T>(this IRepository<T> rep,string entityName, int increment = 1)
        {
            var ctx = (rep.UnitOfWork.DbContext as SampleProjectTemplateEntities);

            var result = ctx.udpGetNewKey(entityName, increment).ToArray();

            if (!result.Any() || result.FirstOrDefault() == null || !result.FirstOrDefault().HasValue) return -1;

            return result.FirstOrDefault().Value;

        }


        public static void ResetDbKey<T>(this IRepository<T> rep,IRepositoryFactory factory, int val)
        {
            var name = typeof(T).Name;
            
            var gRepository=new ServiceRepository(factory);
            var objRep = gRepository.AppObjectInfoRepository();
            var obj = objRep.GetEntityByKey(name);
            if (obj != null)
            {
                obj.CounterValue = val;
                objRep.Update(obj);
            }
            
        }
    }
}
