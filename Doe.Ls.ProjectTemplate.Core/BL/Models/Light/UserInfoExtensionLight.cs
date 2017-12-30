using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Light
    {
    /// <summary>
    /// This class for all user information conversion
    /// </summary>

    public class UserInfoExtensionLight
        {

        public string UserName { get; set; }


        public string Title { get; set; }


        public string FirstName { get; set; }


        public string SurName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }


        public string Phone { get; set; }
        
        public string CurrentRole { get; set; }
        public string DisplayRoles { get; set; }

        public static UserInfoExtensionLight MapFrom(UserInfoExtension u)
            {
            return new UserInfoExtensionLight
                {
                UserName = u.UserName,
                Email = u.Email,
                CurrentRole = u.CurrentRole,
                DisplayRoles = u.DisplayRoles(),
                Phone = u.Phone,
                SurName = u.SurName,
                Title = u.Title,
                FirstName = u.FirstName,
                DisplayName = u.DisplayName,
                
                };


            }

        public override string ToString()
        {
            return $"{Email}-{CurrentRole}-{DisplayRoles}";
        }
        }



    }
