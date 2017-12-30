using System.Collections.Generic;
using System.Linq;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards
{
    public class Dashboard
    {
        public string DashboardTitle { get; set; }
        public List<DashboardSection> DashboardSections { get; set; }

        public static void AddSection(ref Dashboard dashboard, string sectionTitle, params DashboardItem[] dashboardItems)
        {
            if (dashboard == null) dashboard = new Dashboard();
            if (dashboard.DashboardSections == null) dashboard.DashboardSections = new List<DashboardSection>();
            dashboard.DashboardSections.Add(new DashboardSection
            {
                Title = sectionTitle,
                DashboardItems = dashboardItems.ToList(),
                Status = UiStatus.Visible
                
            });
        }
    }
}
