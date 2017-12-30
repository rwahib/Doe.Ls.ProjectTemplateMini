using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
    {
    public class DefaultOrganisationalModel
        {
        /// <summary>
        /// Null means all divisions
        /// </summary>
        public List<string> Divisions { get; set; }
        /// <summary>
        /// Null means all directorates
        /// </summary>
        public List<int> Directorates { get; set; }
        /// <summary>
        /// Null means all BusinessUnite
        /// </summary>
        public List<int> BusinessUnites { get; set; }

        public static DefaultOrganisationalModel BuildDefaultOrganisationalModel(UserInfoExtension user,
            IRepositoryFactory factory)
        {
            var defaultModel = new DefaultOrganisationalModel();
            if (user.HasAnyAdminRoleExceptHr())
            {
                var service = new ServiceRepository(factory);

                foreach (var userRoleModel in user.ActiveRoleOrgLevelList)
                {
                    var role = (Enums.UserRole) userRoleModel.RoleId;
                    switch (role)
                    {
                        
                        case Enums.UserRole.BusinessUnitDataEntry:
                        case Enums.UserRole.BusinessUnitAuthor:
                            var bu = service.BusinessUnitRepository().GetBUnitById(userRoleModel.StructureId.ToInteger());
                            if (bu == null)
                                bu = new BusinessUnit
                                {
                                    BUnitId = -1,
                                    BUnitName = "NA",
                                    DirectorateId = -1,
                                    Directorate = new Directorate { DirectorateId = -1,DirectorateName = "NA",ExecutiveCod = "-1"},
                                    
                                };
                                if (defaultModel.BusinessUnites == null)
                            {
                                
                                defaultModel.BusinessUnites = new List<int>{bu.BUnitId};
                            }
                            else
                            {
                                defaultModel.BusinessUnites.Add(bu.BUnitId);
                            }

                            if(defaultModel.Directorates == null)
                                {
                                defaultModel.Directorates = new List<int> { bu.DirectorateId };
                                }
                            else
                                {
                                defaultModel.Directorates.Add(bu.DirectorateId);
                                }

                            if(defaultModel.Divisions == null)
                                {
                                defaultModel.Divisions = new List<string> { bu.Directorate.ExecutiveCod };
                                }
                            else
                                {
                                defaultModel.Divisions.Add(bu.Directorate.ExecutiveCod);
                                    defaultModel.Divisions = defaultModel.Divisions.Distinct().ToList();
                                }

                            break;
                        case Enums.UserRole.DirectorateDataEntry:
                            
                        case Enums.UserRole.DirectorateEndorser:
                            var dir =
                           service.DirectorateRepository().GetDirectorateById(userRoleModel.StructureId.ToInteger());
                            if (dir == null)
                            {
                                dir = new Directorate {DirectorateId = -1, DirectorateName = "NA", ExecutiveCod = "-1",BusinessUnits = new List<BusinessUnit>()};
                            }

                            if(defaultModel.BusinessUnites == null)
                            {
                                defaultModel.BusinessUnites = dir.BusinessUnits.Select(b => b.BUnitId).ToList();
                            }
                            else
                                {
                                defaultModel.BusinessUnites.AddRange(dir.BusinessUnits.Select(b => b.BUnitId).ToList());
                                    defaultModel.BusinessUnites = defaultModel.BusinessUnites.Distinct().ToList();
                                }

                            if(defaultModel.Directorates == null)
                                {
                                defaultModel.Directorates = new List<int> { dir.DirectorateId };
                                }
                            else
                                {
                                defaultModel.Directorates.Add(dir.DirectorateId);
                                }

                            if(defaultModel.Divisions == null)
                                {
                                defaultModel.Divisions = new List<string> { dir.ExecutiveCod };
                                }
                            else
                                {
                                defaultModel.Divisions.Add(dir.ExecutiveCod);
                                }
                            break;
                        case Enums.UserRole.DivisionEditor:
                            
                        case Enums.UserRole.DivisionApprover:
                            var div = service.ExecutiveRepository().GetExecutiveByCode(userRoleModel.StructureId);
                            if (div == null)
                            {

                               div = new Executive{ExecutiveCod = "-1", ExecutiveTitle = "NA",Directorates = new List<Directorate>()};
                               

                                }
                            if(defaultModel.BusinessUnites == null)
                            {
                                defaultModel.BusinessUnites =
                                    div.Directorates.SelectMany(d => d.BusinessUnits.Select(b => b.BUnitId)).ToList();
                            }
                            else
                                {
                                defaultModel.BusinessUnites.AddRange(div.Directorates.SelectMany(d => d.BusinessUnits.Select(b => b.BUnitId)).ToList());
                                defaultModel.BusinessUnites = defaultModel.BusinessUnites.Distinct().ToList();
                                }


                            if(defaultModel.Directorates == null)
                            {
                                defaultModel.Directorates = div.Directorates.Select(d => d.DirectorateId).ToList();
                            }
                            else
                                {
                                defaultModel.Directorates.AddRange(div.Directorates.Select(d => d.DirectorateId).ToList());
                                    defaultModel.Directorates = defaultModel.Directorates.Distinct().ToList();
                                }

                            if(defaultModel.Divisions == null)
                                {
                                defaultModel.Divisions = new List<string> { div.ExecutiveCod };
                                }
                            else
                                {
                                defaultModel.Divisions.Add(div.ExecutiveCod);
                                }

                            break;
                        
                    }
                }
            }


            return defaultModel;
        }
        }
    }
