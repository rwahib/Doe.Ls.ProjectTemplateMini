using System.ComponentModel.DataAnnotations;

namespace Doe.Ls.EntityBase.Models
{
    public class UserInfo
    {
        private int _schoolId;

        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string SurName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        /// <remarks/>

        public string StaffId
        {
            get;
            set;
        }

        /// <remarks/>
        public virtual int SchoolId
        {
            get
            {
                if (_schoolId == 0)
                    _schoolId = Cnt.UnknownSchoolCode;
                return _schoolId;
            }
            set
            {
                _schoolId = (value == 0) ? Cnt.UnknownSchoolCode : value;
            }
        }

        /// <remarks/>
        public string SchoolName
        {
            get;
            set;
        }

        public string DepartmentId
        {
            get;
            set;
        }

        /// <remarks/>
        public string DepartmentName
        {
            get;
            set;
        }
        public string[] Groups
        {
            get;
            set;
        }

        /// <remarks/>
        public string[] MemberOf
        {
            get;
            set;
        }

        public string[] Roles
        {
            get;
            set;
        }

        /// <remarks/>
        public string Path
        {
            get;
            set;
        }
        public override string ToString()
        {
            return $"{UserName}:{DisplayName} {Email}";
        }

        public string Phone { get; set; }

        public virtual string CurrentRole { get; set; }
    }
}
