using Doe.Ls.ProjectTemplate.Core.BL.Models;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
{
    public class RolePositionDescTask
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Url => !string.IsNullOrWhiteSpace(RelativeUrl) ? RelativeUrl : FixedUrl;

        public string RelativeUrl{ get; }
      
        public string FixedUrl { get; }
        
        public Enums.ValidationTasksStatus Status { get; set; }

        
        public RolePositionDescTask(string name, string description, string relativeUrl,  string fixedUrl)
        {

            Name = name;
            Description = description;
            FixedUrl = fixedUrl;
            RelativeUrl = relativeUrl;
            Status =Enums.ValidationTasksStatus.Incomplete;

        }
        public RolePositionDescTask(string name, string description, string relativeUrl, string fixedUrl, Enums.ValidationTasksStatus status)
            {

            Name = name;
            Description = description;
            FixedUrl = fixedUrl;
            RelativeUrl = relativeUrl;
            Status = status;
            }

        public override string ToString()
        {
            return $"{Name}- {Description}- {Url}";
        }
    }
}
