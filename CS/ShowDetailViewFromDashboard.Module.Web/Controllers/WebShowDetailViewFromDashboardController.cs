using DevExpress.DashboardWeb;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.Persistent.Base;
using ShowDetailViewFromDashboard.Module.BusinessObjects;
using System;

namespace ShowDetailViewFromDashboard.Module.Web.Controllers {
    public class WebShowDetailViewFromDashboardController : ObjectViewController<DetailView, IDashboardData>, IXafCallbackHandler {
        private const string HandlerName = "WebShowDetailViewFromDashboardController";
        private readonly ParametrizedAction openDetailViewAction;

        protected override void OnActivated() {
            base.OnActivated();
            WebDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
            if (dashboardViewerViewItem != null) {
                if (dashboardViewerViewItem.DashboardDesigner != null) {
                    CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner);
                }
                dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
            }
        }
        private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e) {
            WebDashboardViewerViewItem dashboardViewerViewItem = sender as WebDashboardViewerViewItem;
            CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner);
        }
        private void CustomizeDashboardViewer(ASPxDashboard dashboardControl) {
            XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
            callbackManager.RegisterHandler(HandlerName, this);
            string widgetScript = @"function getOid(s, e) {{
                                        function findMeasure(measure) {{
                                            return measure.DataMember === 'Oid';
                                        }}
                                        if (e.ItemName.includes('gridDashboardItem')) {{
                                             var axisPoint = e.GetAxisPoint();
                                             if (!axisPoint) 
                                                 return null;
                                             var itemData = e.GetData(),   
                                                dataSlice = itemData.GetSlice(axisPoint),
                                                oidMeasure = dataSlice.GetMeasures().find(findMeasure).Id,
                                                measureValue = dataSlice.GetMeasureValue(oidMeasure);
                                                {0}
                                        }}
                                    }}";
            dashboardControl.ClientSideEvents.ItemClick = string.Format(widgetScript, callbackManager.GetScript(HandlerName, "measureValue.GetValue()"));
        }
        public void ProcessAction(string parameter) {
            openDetailViewAction.DoExecute(parameter);
        }
        private void OpenDetailViewAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            Guid.TryParse((string)e.ParameterCurrentValue, out var oid); 
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Contact));
            Contact contact = objectSpace.FindObject<Contact>(new BinaryOperator(nameof(Contact.Oid), oid));
            if (contact != null) {
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View);
            }
        }

        protected override void OnDeactivated() {
            WebDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
            if (dashboardViewerViewItem != null) {
                dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
            }
            base.OnDeactivated();
        }
        public WebShowDetailViewFromDashboardController() {
            openDetailViewAction = new ParametrizedAction(this, "Dashboard_OpenDetailView", "Dashboard", typeof(string));
            openDetailViewAction.Caption = "OpenDetailView";
            openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            openDetailViewAction.Execute += OpenDetailViewAction_Execute;
        }
    }
}
