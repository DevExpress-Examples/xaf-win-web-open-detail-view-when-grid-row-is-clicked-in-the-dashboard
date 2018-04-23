using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.DashboardWin;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.Base;
using ShowDetailViewFromDashboard.Module.BusinessObjects;
using System;
using System.Linq;

namespace ShowDetailViewFromDashboard.Module.Win.Controllers
{
    public class WinShowDetailViewFromDashboardController : ObjectViewController<DetailView, IDashboardData>
    {
        private ParametrizedAction openDetailViewAction;

        protected override void OnActivated()
        {
            base.OnActivated();
            WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
            if (dashboardViewerViewItem != null)
            {
                if (dashboardViewerViewItem.Viewer != null)
                {
                    dashboardViewerViewItem.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
                }
                dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
            }
        }

        private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
        {
            WinDashboardViewerViewItem dashboardViewerViewItem = sender as WinDashboardViewerViewItem;
            dashboardViewerViewItem.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
        }
        private bool IsGridDashboardItem(Dashboard dashboard, string dashboardItemName)
        {
            DashboardItem dashboardItem = dashboard.Items.SingleOrDefault(item => item.ComponentName == dashboardItemName);
            return dashboardItem is GridDashboardItem;
        }
        private static string GetOid(DashboardItemMouseActionEventArgs e)
        {
            MultiDimensionalData data = e.Data.GetSlice(e.GetAxisPoint());
            MeasureDescriptor descriptor = data.GetMeasures().SingleOrDefault(item => item.DataMember == "Oid");
            MeasureValue measureValue = data.GetValue(descriptor);
            return measureValue.Value.ToString();
        }
        private void Viewer_DashboardItemDoubleClick(object sender, DashboardItemMouseActionEventArgs e)
        {
            Dashboard dashboard = ((DashboardViewer)sender).Dashboard;

            if (IsGridDashboardItem(dashboard, e.DashboardItemName) &&
                openDetailViewAction.Enabled && openDetailViewAction.Active)
            {
                openDetailViewAction.DoExecute(GetOid(e));
            }
        }

        private void OpenDetailViewAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Contact));
            Contact contact = objectSpace.FindObject<Contact>(new BinaryOperator("Oid", e.ParameterCurrentValue.ToString()));
            if (contact != null)
            {
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View);
            }
        }

        protected override void OnDeactivated()
        {
            WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
            if (dashboardViewerViewItem != null)
            {
                dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
            }
            base.OnDeactivated();
        }
        public WinShowDetailViewFromDashboardController()
        {
            openDetailViewAction = new ParametrizedAction(this, "Dashboard_OpenDetailView", "Dashboard", typeof(string));
            openDetailViewAction.Caption = "OpenDetailView";
            openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            openDetailViewAction.Execute += OpenDetailViewAction_Execute;
        }
    }
}
