using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Data;
using ServiceStack;

namespace Doe.Ls.ProjectTemplate.Core.BL.DomainServices {
    public class TrimTaskManager {
        public TrimTaskManager(ServiceRepository serviceRepository)
        {
            ServiceRepository = serviceRepository;
        }

        public ServiceRepository ServiceRepository { get; private set; }

        public event EventHandler<RolePosDescriptionArg> OnError;
        public event EventHandler<RolePosDescriptionArg> OnSuccess;

        public void SyncAll(string comment, bool force = false, bool async = true)
        {
            if (!async)
            {
                SyncAllCore(comment, force);
                return;
            }
            else
            {
                var task=new Task(()=> { SyncAllCore(comment, force); });
                task.Start();
            }

        }

        private void SyncAllCore(string comment, bool force = false)
        {
            
            var rdPdLiveList = this.ServiceRepository.RolePositionDescriptionRepository().List().OrderByDescending(rpd=>rpd.LastModifiedDate).Where(rdpd =>
                    (rdpd.StatusId == (int)Enums.StatusValue.Imported ||
                     rdpd.StatusId == (int)Enums.StatusValue.Approved) &&
                    rdpd.RolePositionDescId > 0
                    ).ToList();

            var trimRecordSrv = ServiceRepository.TrimRecordRepository();

            foreach(var rpd in rdPdLiveList) {

                try {

                    trimRecordSrv.SynchRolePosDescription(rpd.RolePositionDescId, comment, force);
                    OnSuccess?.Invoke(this, new RolePosDescriptionArg(rpd, "Success"));
                    }
                catch(Exception ex) {
                    OnError?.Invoke(this, new RolePosDescriptionArg(rpd, ex.Message));
                    }

                }

            }
        }

    }
