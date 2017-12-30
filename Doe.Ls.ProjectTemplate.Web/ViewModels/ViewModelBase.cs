using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;

namespace Doe.Ls.ProjectTemplate.Web.ViewModels
{
    public abstract class ViewModelBase
    {
        public string ViewTitle { get; set; }

        public UserInfoExtension CurrentUser { get; set; }

        public IUserTask UserTask { get; set; }

        public ViewModelExtension ViewModelExtension { get; set; }

        public IEnumerable<DbValidationError> Errors { get; set; }
    }
}