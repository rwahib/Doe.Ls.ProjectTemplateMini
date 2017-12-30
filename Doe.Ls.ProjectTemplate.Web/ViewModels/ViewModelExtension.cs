using System.Data.Entity.Core.Metadata.Edm;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Helper;

namespace Doe.Ls.ProjectTemplate.Web.ViewModels
{
    public class ViewModelExtension
    {
        public FormMethod FormMethod { get; set; }
        public FormType FormType { get; set; }
        public RequestType RequestType { get; set; }
        public EntityType EntityType { get; set; }

    }
}