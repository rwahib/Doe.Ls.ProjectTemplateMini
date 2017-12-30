using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{
    public class RoleCapabilityModel
    {
        public List<RoleCapability> RoleCapabilities { get; private set; }
        
        public bool IsManager { get; private set; }
        public string GradeCode { get; private set; }
        public int RoleDescriptionId { get; private set; }

        

        public static Result ValidCapabilityLevels(ServiceRepository srv, string gradeCode, List<RoleCapability> rolecapabilityList, bool isManager)
        {
            var matrix = srv.RoleDescCapabilityMatrixRepository().List().FirstOrDefault(m => m.GradeCode == gradeCode);
            var result = new Result();

            var totalCapabilitiesCnt = 16; //for non-manager
            if (isManager)
            {
                //20 capability items (names)
                //to include People Management
                totalCapabilitiesCnt = 20;
            }

            var foundational_cnt = rolecapabilityList.Count(f => f.CapabilityLevelId == (int)Enums.CapablityLevel.Foundational);
            var adept_cnt = rolecapabilityList.Count(f => f.CapabilityLevelId == (int)Enums.CapablityLevel.Adept);
            var intermediate_cnt = rolecapabilityList.Count(f => f.CapabilityLevelId == (int)Enums.CapablityLevel.Intermediate);
            var advanced_cnt = rolecapabilityList.Count(f => f.CapabilityLevelId == (int)Enums.CapablityLevel.Advanced);
            var highlyAdv_cnt = rolecapabilityList.Count(f => f.CapabilityLevelId == (int)Enums.CapablityLevel.HighlyAdvanced);

            var focus_cnt = rolecapabilityList.Count(f => f.Highlighted);

            var hasError = false;
            StringBuilder sb = new StringBuilder();
            if (foundational_cnt + adept_cnt + intermediate_cnt + advanced_cnt + highlyAdv_cnt < totalCapabilitiesCnt)
            {
                //error
                sb.Append("The total number of " + totalCapabilitiesCnt + " capabilities must to be completed (please note, that count excludes the items from Occupation Specific). <br />");
                hasError = true;
            }

            if (foundational_cnt < matrix.Foundational_Min || foundational_cnt > matrix.Foundational_Max)
            {
                if (matrix.Foundational_Min > 0 && matrix.Foundational_Min == matrix.Foundational_Max)
                {
                    sb.Append("The number of Foundational are: '" + matrix.Foundational_Min + "'. <br />");
                }
                else
                {
                    sb.Append("The number of Foundational are between: '" + matrix.Foundational_Min + " - " +
                              matrix.Foundational_Max + "'. <br />");
                }
                hasError = true;
            } 
            
            if (intermediate_cnt < matrix.Intermediate_Min || intermediate_cnt > matrix.Intermediate_Max)
            {
                if (matrix.Intermediate_Min > 0 && matrix.Intermediate_Min == matrix.Intermediate_Max)
                {
                    sb.Append("The number of Intermediate are: '" + matrix.Intermediate_Min + "'. <br />");
                }
                else
                {
                    sb.Append("The number of Intermediate are between: '" + matrix.Intermediate_Min + " - " +
                              matrix.Intermediate_Max + "'. <br />");
                }
                hasError = true;
            }

            if (adept_cnt < matrix.Adept_Min || adept_cnt > matrix.Adept_Max)
            {
                if (matrix.Adept_Min > 0 && matrix.Adept_Min == matrix.Adept_Max)
                {
                    sb.Append("The number of Adept are: '" + matrix.Adept_Min + "'. <br />");
                }
                else
                {
                    sb.Append("The number of Adept are between: '" + matrix.Adept_Min +
                              " - " + matrix.Adept_Max + "'.<br />");
                }
                hasError = true;

            }


            if (advanced_cnt < matrix.Advanced_Min || advanced_cnt > matrix.Advanced_Max)
            {
                if (matrix.Advanced_Min > 0 && matrix.Advanced_Min == matrix.Advanced_Max)
                {
                    sb.Append("The number of Advanced are: '" + matrix.Advanced_Min + "'. <br />");
                }
                else
                {
                    sb.Append("The number of Advanced are between: '" + matrix.Advanced_Min + " - " +
                              matrix.Advanced_Max + "'. <br />");
                }
                hasError = true;
            }

            if (highlyAdv_cnt < matrix.HighlyAdvanced_Min || highlyAdv_cnt > matrix.HighlyAdvanced_Max)
            {
                if (matrix.HighlyAdvanced_Min > 0 && matrix.HighlyAdvanced_Min == matrix.HighlyAdvanced_Max)
                {
                    sb.Append("The number of Highly Advanced are: '" + matrix.HighlyAdvanced_Min + "'. <br />");
                }
                else
                {
                    sb.Append("The number of Highly Advanced are between: '" + matrix.HighlyAdvanced_Min + " - " +
                              matrix.HighlyAdvanced_Max + "'. <br />");
                }
                hasError = true;
            }

            if (focus_cnt < matrix.FocusCapabilities_Min || focus_cnt > matrix.FocusCapabilities_Max)
            {
                if (matrix.FocusCapabilities_Min > 0 && matrix.FocusCapabilities_Min == matrix.FocusCapabilities_Max)
                {
                    sb.Append("The number of Focuses are: '" + matrix.FocusCapabilities_Min + "'. <br />");
                }
                else
                {
                    sb.Append("The number of Focuses are between: '" + matrix.FocusCapabilities_Min + " - " +
                              matrix.FocusCapabilities_Max + "'. <br />");
                }
                hasError = true;
            }

            if (hasError)
            {
                result.Status = Status.Error;
                result.Message = "Please check the Capability Comparison Guide below, ensuring that: <br />" + sb.ToString();
            }
            else
            {
                result.Status = Status.Success;
                result.Message = "All good";
            }
            return result;

        }


        /// <summary>
        /// Testable version
        /// </summary>
       public RoleCapabilityModel ParseBuildRoleCapabilityList(FormCollection collection, ServiceRepository srv)
        {
            var model = new RoleCapabilityModel();
            var roleDescriptionId = collection["RoleDescriptionId"];

            model.RoleDescriptionId = Convert.ToInt32(roleDescriptionId);
            model.IsManager = collection["ManagerRole"] == "on" || collection["ManagerRole"] == "value";

            var rolePositionDesc =
                srv.RolePositionDescriptionRepository().List().First(r => r.RolePositionDescId == model.RoleDescriptionId);

            model.GradeCode = rolePositionDesc.GradeCode;

            RoleCapabilities = new List<RoleCapability>();

            var cnames = srv.CapabilityNameRepository().List().ToList();
            
            try
            {
                foreach (var cname in cnames)
                {
                    var rc = new RoleCapability();
                    var selLvlname = "CapabilityLevelId_" + cname.CapabilityNameId;
                    var selhigh = "Highlighted_" + cname.CapabilityNameId;
                    var highlighted = collection[selhigh].IsOn();
                    if (!string.IsNullOrEmpty(collection[selLvlname]) && collection[selLvlname] != "0")
                    {
                        rc.CapabilityLevelId = Convert.ToInt32(collection[selLvlname]);
                        rc.CapabilityNameId = cname.CapabilityNameId;
                        rc.RoleDescriptionId = model.RoleDescriptionId;
                        rc.Highlighted = highlighted;
                        rc.CreatedBy = srv.SessionService().GetCurrentUser().UserName;
                        rc.DateCreated = DateTime.Now;
                        rc.LastUpdated = DateTime.Now;
                        rc.LastModifiedBy = srv.SessionService().GetCurrentUser().UserName;

                        RoleCapabilities.Add(rc);

                    }
                }
                model.RoleCapabilities = RoleCapabilities;


            }
            catch (Exception exception)
            {
                var errors = srv.RoleDescriptionRepository().GetBackendValidationErrors().ToList();
                errors.Add(new DbValidationError("DB ", "Oops! something went wrong while saving Role Capabilities. " + exception.Message));
            }

            return model;
        }




    }
}
