using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow
{
    public partial class WorkflowAction
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string ActionStatus { get; set; }
        public string ActionDescription { get; set; }
        public string GetFullDescription(Position position)
            {
            return string.Format(this.ActionDescription, position.PositionNumber, position.PositionTitle);
            }
        public string GetFullDescription(RolePositionDescription rolePositionDescription)
            {
            return string.Format(this.ActionDescription, rolePositionDescription.DocNumber, rolePositionDescription.Title);
            }
        public bool Populated { get; set; }
        public override bool Equals(object obj)
            {
            if(obj == null || GetType() != obj.GetType()) return false;
            var a = (WorkflowAction)obj;

            return this.ActionId == a.ActionId;
            }
        
        protected bool Equals(WorkflowAction other)
        {
            return ActionId == other.ActionId;
        }


        public static bool operator ==(WorkflowAction x, WorkflowAction y)
        {
            return x.ActionId == y.ActionId;
        }

        public static bool operator !=(WorkflowAction x, WorkflowAction y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ActionId;
                hashCode = (hashCode*397) ^ (ActionName != null ? ActionName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ActionStatus != null ? ActionStatus.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ActionDescription != null ? ActionDescription.GetHashCode() : 0);
                return hashCode;
            }
        }
        
        public static void Populate(IRepositoryFactory factory, WorkflowAction action)
        {
            if (action.Populated) return;
            var actionDb = new ServiceRepository(factory).WfActionRepository().GetWorkflowActionById(action.ActionId);
            action.ActionDescription = actionDb.WfActionDescription;
            action.ActionName = actionDb.WfActionName;
            action.ActionStatus = actionDb.WfActionStatus;
            action.Populated = true;
            }
        public static void Populate(IRepositoryFactory factory, List<WorkflowAction> actions)
            {
            foreach(var action in actions)
                {
                if(action.Populated) continue;
                var actionDb = new ServiceRepository(factory).WfActionRepository().GetWorkflowActionById(action.ActionId);
                action.ActionDescription = actionDb.WfActionDescription;
                action.ActionName = actionDb.WfActionName;
                action.ActionStatus = actionDb.WfActionStatus;
                }

            }
        
        public Enums.StatusValue GetStatus()
        {
            if(this==BringToDraft)return Enums.StatusValue.Draft;
            if(this == Clone) return Enums.StatusValue.Draft;
            if(this == CreateNewVersion) return Enums.StatusValue.Draft;

            if(this==Approve)return Enums.StatusValue.Approved;
            if(this == MarkAsImported) return Enums.StatusValue.Imported;

            if(this==Submit)return Enums.StatusValue.Submitted;

            if(this==Endorse)return Enums.StatusValue.Endorsed;
            if(this==Delete)return Enums.StatusValue.Deleted;
            if(this==Archive)return Enums.StatusValue.Inactive;
            
            
            throw new InvalidOperationException();

        }

        public override string ToString()
        {
            return $"{ActionId}-{ActionName}";
        }
    }
}