using Orchard.Environment.Extensions;
using Orchard.UI.Navigation;
using Orchard;
using Orchard.Localization;
namespace Wkong.SchedulingTask {
    [OrchardFeature("Wkong.SchedulingTask.UI")]
    public class AdminMenu :  INavigationProvider {

        public string MenuName { get { return "admin"; } }
        public Localizer T { get; set; }
        public void GetNavigation(NavigationBuilder builder) {
            builder
                .Add(T("定时任务"), "15.0", item =>
                {
                    item.Action("List", "Admin", new { area = "Wkong.SchedulingTask" });
                    item.LinkToFirstChild(false);
                });
        }
    }
}