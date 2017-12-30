using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
    {
   public  class RpdPositionsModel
        {
        public int RolePositionDescId { get; set; }
        public List<int> PositionIds { get; set; }

       public static RpdPositionsModel ParsModel(NameValueCollection formCollection)
       {
           int dummy;
           var model = new RpdPositionsModel
           {
               PositionIds = new List<int>()
           };
           if (int.TryParse(formCollection["RolePositionDescId"], out dummy))
           {
               model.RolePositionDescId = dummy;
           }
           else
           {
               throw new InvalidOperationException("Invalid role / position description number");
           }

           foreach (var key in formCollection.AllKeys.Where(k => k.StartsWith("checkbox_")).ToList())
           {
               if (formCollection[key].ToLower() == "on" || formCollection[key].ToLower() == "true")
               {
                   if (key.Split('_').Length > 1)
                   {
                       var ids = key.Split('_')[1];
                        if(int.TryParse(ids, out dummy))
                            {
                            model.PositionIds.Add(dummy);
                            }
                        }
               
               }
           }
           


            return model;
       }
        }
    }
