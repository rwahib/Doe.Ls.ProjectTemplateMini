using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
    {
    public partial class Enums
        {
        public static class Cnt
        {
            public const string MessageFilePath = @"~/App_Data/MessageList.xml";
            public const string NaValue="NA";
            public const int Na = -1;
            public const string System = "System";
            public const long KB = 1024;
            public const string TBA = "TBA";
            public const string NotProvided = "[Not provided]";
            public const int AllAssociations = -1;
            public const string NotPublished = "[Not published]";

            public const string DefaultPositionNo = "XXXX00";
            public const string Custom = "Custom";

            public const int GenericCode = -3000;
            public const string Generic = "Generic";

            public const string Nil = "Nil";
            public const string CacheId = "cache_id";
        }

        public static class QueryName
            {
            public const string StartDate = "sd";
            public const string EndDate = "ed";
            }

        public enum WizardStepStatus
            {
            NotStarted,
            InProgress,
            Completed
            }

        public enum CacheRegion
            {
            Default,
            Position,
            Messages
            }

        public enum LogActions
            {
            [Description("Log in")]
            LogIn ,
            [Description("Log out")]
            LogOut,
            [Description("Modify user access")]
            ModifiyUserRole,
[Description("Trim publisher")]
            TrimPublisher
            }

        public class Privilege
            {
            private bool _canRead;
            private bool _canEdit;
            private bool _canDelete;
            private bool _canCreate;
            private bool _canApprove;
            private bool _canSubmit;
            private bool _canPerformActions;
            private bool _canDownload;

            public static Privilege AccessDeniedPrivilege
                {
                get
                    {
                    var priv = new Privilege();
                    priv.SetAll(false);
                    return priv;
                    }
                }
            public static Privilege FullContolPrivilege
                {
                get
                    {
                    var priv = new Privilege();
                    priv.SetAll(true);
                        priv.FullControl = true;
                    return priv;
                    }
                }
            public static Privilege ReadOnlyPrivilege
                {
                get
                    {
                    var priv = Privilege.AccessDeniedPrivilege;
                    priv._canRead = true;
                    return priv;
                    }
                }
            public static Privilege ModifyPrivilege
                {
                get
                    {
                    var priv = AccessDeniedPrivilege;
                    priv._canRead = true;
                    priv._canEdit = true;
                    return priv;
                    }
                }
            public bool FullControl { get; set; }

            public bool CanRead
                {
                get { return _canRead || FullControl; }
                set { _canRead = value; }
                }

            public bool CanEdit
                {
                get { return _canEdit || FullControl; }
                set { _canEdit = value; }
                }

            public bool CanDelete
                {
                get { return _canDelete || FullControl; }
                set { _canDelete = value; }
                }

            public bool CanCreate
                {
                get { return _canCreate || FullControl; }
                set { _canCreate = value; }
                }

            public bool CanApprove
                {
                get { return _canApprove || FullControl; }
                set { _canApprove = value; }
                }
            public bool CanSubmit
                {
                get { return _canSubmit || FullControl; }
                set { _canSubmit = value; }
                }

            public bool CanPerformActions
            {
                get { return _canPerformActions || FullControl; ; }
                set { _canPerformActions = value; }
            }
            public bool CanDownload
                {
                get { return _canDownload || FullControl; ; }
                set { _canDownload = value; }
                }

            public void SetAll(bool flag = true)
                {
                _canEdit = flag;
                _canRead = flag;
                _canEdit = flag;
                _canDelete = flag;
                _canCreate = flag;
                _canApprove = flag;
                _canSubmit = flag;
                _canPerformActions = flag;
                _canDownload = flag;
                }

            public override string ToString()
                {
                return this.JsonSerialise();
                }

            public override bool Equals(object obj)
            {
                return this == (Privilege) obj;
            }

            protected bool Equals(Privilege other)
            {
                if(object.Equals(other, null)) return false;

                return _canRead == other._canRead && _canEdit == other._canEdit && _canDelete == other._canDelete && _canCreate == other._canCreate && _canApprove == other._canApprove && _canSubmit == other._canSubmit && _canPerformActions == other._canPerformActions && _canDownload == other._canDownload && FullControl == other.FullControl;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = _canRead.GetHashCode();
                    hashCode = (hashCode*397) ^ _canEdit.GetHashCode();
                    hashCode = (hashCode*397) ^ _canDelete.GetHashCode();
                    hashCode = (hashCode*397) ^ _canCreate.GetHashCode();
                    hashCode = (hashCode*397) ^ _canApprove.GetHashCode();
                    hashCode = (hashCode*397) ^ _canSubmit.GetHashCode();
                    hashCode = (hashCode*397) ^ _canPerformActions.GetHashCode();
                    hashCode = (hashCode*397) ^ _canDownload.GetHashCode();
                    hashCode = (hashCode*397) ^ FullControl.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(Privilege first, Privilege second)
            {
                
                if (object.Equals(first, null) || object.Equals(second, null)) return false;

                return first._canRead == second._canRead && first._canEdit == second._canEdit && first._canDelete == second._canDelete && first._canCreate == second._canCreate && first._canApprove == second._canApprove && first._canSubmit == second._canSubmit && first._canPerformActions == second._canPerformActions && first._canDownload == second._canDownload && first.FullControl == second.FullControl;
                }

            public static bool operator !=(Privilege b, Privilege c)
            {
                return !(b == c);
            }

            public static Privilege operator + (Privilege first, Privilege second)
            {

                return new Privilege
                {
                    CanRead = first.CanRead || second.CanRead,
                    CanCreate = first.CanCreate || second.CanCreate,
                    CanEdit = first.CanEdit || second.CanEdit,
                    CanPerformActions = first.CanPerformActions || second.CanPerformActions,
                    CanSubmit = first.CanSubmit || second.CanSubmit,
                    FullControl = first.FullControl || second.FullControl,
                    CanDelete = first.CanDelete || second.CanDelete,
                    CanApprove = first.CanApprove || second.CanApprove,
                    CanDownload = first.CanDownload || second.CanDownload,
                };
            }
            public static Privilege operator -(Privilege first, Privilege second)
                {

                return new Privilege
                    {
                    CanRead = first.CanRead && ! second.CanRead,
                    CanCreate = first.CanCreate && ! second.CanCreate,
                    CanEdit = first.CanEdit && ! second.CanEdit,
                    CanPerformActions = first.CanPerformActions && ! second.CanPerformActions,
                    CanSubmit = first.CanSubmit && ! second.CanSubmit,
                    FullControl = first.FullControl && ! second.FullControl,
                    CanDelete = first.CanDelete && ! second.CanDelete,
                    CanApprove = first.CanApprove && ! second.CanApprove,
                    CanDownload = first.CanDownload && ! second.CanDownload,
                    };
                }
            }
        public enum NodeType
            {
            Assistant = 1,
            Regular = 0,
            Adviser = 2
            }



        public enum PositionDescWizardStep
            {
            Overview,
            SelectionCriteria,
            LinkedPositions,
            Summary,
            Actions,
            History,
            Trim
            }

        public enum RoleDescWizardStep
            {
            BasicDetails,
            RolePrimaryPurpose,
            KeyAccountbilities,
            Budget,
            EssentialRequirements,
            CapabilityFramework,
            UpdateCapabilityFramework,
            LinkedPositions,
            Summary,
            Actions,
            KeyRelationships,
            UpdateKeyRelationships,
            History,
            Trim
            }

        public enum GradeType
            {
            NSBTS = 0,
            PSNE = 1,
            PSSE = 2
            }

        public enum ScopeType
            {
            Internal = 10,
            External = 20,
            Ministerial = 30
            }

       
        public enum ValidationTasksStatus
            {
            Completed=0,
            Incomplete=10            
            }

        public enum DescriptionType
            {
            Position = 1,
            Role = 2,
            None = -1
            }

        public enum PositionWizardStep
            {
            BasicDetails,
            MoreInfo,
            CostCentre,
            Actions,
            Summary,
            History
            }

        public enum PositionType
            {

            T
            }

        public enum CapablityLevel
            {
            Adept = 1,
            Intermediate = 2,
            Advanced = 3,
            Foundational = 4,
            HighlyAdvanced = 5
            }

        public enum CapablityGroup
            {
            PersonalAttributes = 1,
            Relationships = 2,
            Results = 3,
            BusinessEnablers = 4,
            PeopleManagement = 5,
            OccupationSpecific = 6
            }

 		public enum DirectReportDefault
        {
            Nil
        }
        public enum PSSEType
        {
            PSSE1 = 1,
            PSSE1GD = 2,
            PSSE2 = 3,
            PSSE3 = 4
        }

        public enum ActionType
        {
            Create = 1,
            Update = 2,
            Delete = 3
         }

        public enum Division
        {
            ES = 1,
            TL = 2
        }

    }
    }
