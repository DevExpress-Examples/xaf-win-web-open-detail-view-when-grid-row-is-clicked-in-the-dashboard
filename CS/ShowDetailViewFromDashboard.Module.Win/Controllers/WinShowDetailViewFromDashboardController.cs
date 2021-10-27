using System;
using System.Linq;
using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.DashboardWin;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.Base;
using ShowDetailViewFromDashboard.Module.BusinessObjects;

namespace ShowDetailViewFromDashboard.Module.Win.Controllers {
    public class WinShowDetailViewFromDashboardController : ObjectViewController<DetailView, IDashboardData> {
        private readonly ParametrizedAction openDetailViewAction;

        public WinShowDetailViewFromDashboardController() {
            openDetailViewAction = new ParametrizedAction(this, "Dashboard_OpenDetailView", "Dashboard", typeof(string));
            openDetailViewAction.Caption = "OpenDetailView";
            openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            openDetailViewAction.Execute += OpenDetailViewAction_Execute;
        }

        private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e) {
            WinDashboardViewerViewItem dashboardViewerViewItem = sender as WinDashboardViewerViewItem;
            dashboardViewerViewItem.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
        }

        private bool IsGridDashboardItem(Dashboard dashboard, string dashboardItemName) {
            DashboardItem dashboardItem = dashboard.Items.SingleOrDefault(item => item.ComponentName == dashboardItemName);
            return dashboardItem is GridDashboardItem;
        }

        private static string GetOid(DashboardItemMouseActionEventArgs e) {
            AxisPoint axisPoint = e.GetAxisPoint();
            if (axisPoint is null) {
                return null;
            }
            MultiDimensionalData data = e.Data.GetSlice(axisPoint);
            MeasureDescriptor descriptor = data.GetMeasures().SingleOrDefault(item => item.DataMember == "Oid");
            MeasureValue measureValue = data.GetValue(descriptor);
            return measureValue.Value.ToString();
        }

        private void Viewer_DashboardItemDoubleClick(object sender, DashboardItemMouseActionEventArgs e) {
            Dashboard dashboard = ((DashboardViewer)sender).Dashboard;
            if (IsGridDashboardItem(dashboard, e.DashboardItemName) &&
                    openDetailViewAction.Enabled && openDetailViewAction.Active
                    && Guid.TryParse(GetOid(e), out var oid)) {
                openDetailViewAction.DoExecute(oid);
            }
        }

        private void OpenDetailViewAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            object oid = e.ParameterCurrentValue;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Contact));
            Contact contact = objectSpace.FindObject<Contact>(new BinaryOperator(nameof(Contact.Oid), oid));
            if (contact != null) {
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View);
            }
        }

        protected override void OnActivated() {
            base.OnActivated();
            WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
            if (dashboardViewerViewItem != null) {
                if (dashboardViewerViewItem.Viewer != null) {
                    dashboardViewerViewItem.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
                }
                dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
            }
        }

        protected override void OnDeactivated() {
            WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
            if (dashboardViewerViewItem != null) {
                dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
            }
            base.OnDeactivated();
        }
    }
}