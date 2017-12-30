using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.EntityBase {
    public class BaseSettings {
        public string StmpHost{ get; set; }

        public static BaseSettings GetFromConfig()
        {
            return new BaseSettings();
        }
    }
}
