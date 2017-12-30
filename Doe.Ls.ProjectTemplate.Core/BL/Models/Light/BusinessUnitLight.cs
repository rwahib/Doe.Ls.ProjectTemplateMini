


using System.ComponentModel.DataAnnotations;
using Doe.Ls.EntityBase.MVCExtensions;
namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light
    {
    public class BusinessUnitLight
        {

        public System.Int32 BUnitId { get; set; }

        public System.Int32 DirectorateId { get; set; }
        public string ExecutiveCode { get; set; }

        public System.Int32 HierarchyId { get; set; }

        public System.String BUnitName { get; set; }

        public System.String DirectorateName { get; set; }
        public System.String BUnitDescription { get; set; }
        public System.String StatusName { get; set; }

        public System.String ExecutiveTitle { get; set; }
        
        public System.Int32 StatusId { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public System.String CreatedBy { get; set; }

        public System.DateTime LastModifiedDate { get; set; }

        public System.String LastModifiedBy { get; set; }

        public Enums.Privilege Privilege { get; set; }


        public override string ToString()
        {
            return $"{BUnitId}-{BUnitName}-{DirectorateId}-{DirectorateName}";
        }
        }
    }
