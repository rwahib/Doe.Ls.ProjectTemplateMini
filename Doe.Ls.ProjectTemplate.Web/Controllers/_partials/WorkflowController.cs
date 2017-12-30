using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers._partials
    {
    public class WorkflowController : AppControllerBase
        {

        protected int GetNumberOfLinkedPositions(int rolePosDescriptionId, IUserTask task)
            {
            CheckUser();
            var linked = GetLinkedPositions(rolePosDescriptionId, task);
            var count = linked.Count();
            ViewBagWrapper.VariableBag.SetIntVariable("LinkedPositionsCount", count, ViewData);
            return count;
            }
        protected IEnumerable<Position> GetLinkedPositions(int rolePosDescriptionId, IUserTask task)
            {
            CheckUser();
            var allLinks = ServiceRepository.PositionRepository().GetAllLinkedPositionsById(rolePosDescriptionId);
            var linked = task.FilterLinkedPositions(allLinks);
            return linked;

            }

        protected IUserTask GetTask()
        {
            CheckUser();
            return UserTaskFactory.GetTask(CurrentUser, ServiceRepository.RepositoryFactory);
            }
        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }

        private void CheckUser()
            {
            if(CurrentUser == null || CurrentUser.IsGuest)
                {

                FormsAuthentication.SignOut();
                SessionService.Abandon();
                
                }


            }

        }
    }