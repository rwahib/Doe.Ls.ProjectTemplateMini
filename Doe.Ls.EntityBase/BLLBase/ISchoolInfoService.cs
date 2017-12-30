using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.EntityBase.BLLBase
{
    public interface ISchoolInfoService : IDomainService
    {
        SchoolInfo GetSchoolBySchoolId(int schoolId);
    }
}
