using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.HTMLHelperExtensions
{
    public static class UiExtensions
    {
        public static void SetSelectedValue(this IEnumerable<SelectListItem> selectedItems, string selectedValue)
        {
            if (selectedItems == null) return;

            foreach (var item in selectedItems)
            {
                if (item.Value == selectedValue)
                {
                    item.Selected = true;
                    return;
                }
            }
        }

        public static void SetSelectedDefaultValue(this IEnumerable<SelectListItem> selectedItems)
        {
            if (selectedItems == null || !selectedItems.Any()) return;
            selectedItems.FirstOrDefault().Selected = true;
        }

    }
}
