using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Doe.Ls.EntityBase.Models;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks
    {
    public class UiPropertyItem
        {
        public UiPropertyItem(string propertyName, string containerClasses = "", string propertyAttributes = "")
            {
            PropertyName = propertyName;
            ContainerClasses = containerClasses;
            PropertyAttributes = propertyAttributes;
            }

        public string PropertyName { get; private set; }
        public string ContainerClasses { get; set; }
        public string PropertyAttributes { get; set; }
        public IEnumerable<SelectListItemExtension> PropertyValueList { get; set; }
        public void HideProperty()
            {

            this.ContainerClasses += "hidden";

            }
        public void ShowProperty()
            {

            this.ContainerClasses.Replace("hidden", "");

            }
        public bool IsHidden()
            {

            if(this == null) return true;
            return this.ContainerClasses.Contains("hidden");

            }
        public bool IsVisible()
            {
            return !IsHidden();

            }
        public static UiPropertyItem GetProperty(IEnumerable<UiPropertyItem> propList, string propertyName)
            {
            var prop = propList.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            return prop ?? new UiPropertyItem(propertyName, "hidden", "");
            }

        }
    }