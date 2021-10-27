Imports DevExpress.DashboardWeb
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Dashboards.Web
Imports DevExpress.ExpressApp.Web
Imports DevExpress.ExpressApp.Web.Templates
Imports DevExpress.Persistent.Base
Imports ShowDetailViewFromDashboard.Module.BusinessObjects
Imports System

Namespace ShowDetailViewFromDashboard.Module.Web.Controllers
	Public Class WebShowDetailViewFromDashboardController
		Inherits ObjectViewController(Of DetailView, IDashboardData)
		Implements IXafCallbackHandler

		Private Const HandlerName As String = "WebShowDetailViewFromDashboardController"
		Private ReadOnly openDetailViewAction As ParametrizedAction

		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			Dim dashboardViewerViewItem As WebDashboardViewerViewItem = TryCast(View.FindItem("DashboardViewer"), WebDashboardViewerViewItem)
			If dashboardViewerViewItem IsNot Nothing Then
				If dashboardViewerViewItem.DashboardDesigner IsNot Nothing Then
					CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner)
				End If
				AddHandler dashboardViewerViewItem.ControlCreated, AddressOf DashboardViewerViewItem_ControlCreated
			End If
		End Sub
		Private Sub DashboardViewerViewItem_ControlCreated(ByVal sender As Object, ByVal e As EventArgs)
			Dim dashboardViewerViewItem As WebDashboardViewerViewItem = TryCast(sender, WebDashboardViewerViewItem)
			CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner)
		End Sub
		Private Sub CustomizeDashboardViewer(ByVal dashboardControl As ASPxDashboard)
			Dim callbackManager As XafCallbackManager = DirectCast(WebWindow.CurrentRequestPage, ICallbackManagerHolder).CallbackManager
			callbackManager.RegisterHandler(HandlerName, Me)
			Dim widgetScript As String = "function getOid(s, e) {{
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
                                    }}"
			dashboardControl.ClientSideEvents.ItemClick = String.Format(widgetScript, callbackManager.GetScript(HandlerName, "measureValue.GetValue()"))
		End Sub
		Public Sub ProcessAction(ByVal parameter As String) Implements IXafCallbackHandler.ProcessAction
			openDetailViewAction.DoExecute(parameter)
		End Sub
		Private Sub OpenDetailViewAction_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs)
			Dim oid As Object
			If Not Guid.TryParse(CStr(e.ParameterCurrentValue), oid) Then
				Return
			End If
			Dim objectSpace As IObjectSpace = Application.CreateObjectSpace(GetType(Contact))
			Dim contact As Contact = objectSpace.FindObject(Of Contact)(New BinaryOperator(NameOf(contact.Oid), oid))
			If contact IsNot Nothing Then
				e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View)
			End If
		End Sub

		Protected Overrides Sub OnDeactivated()
			Dim dashboardViewerViewItem As WebDashboardViewerViewItem = TryCast(View.FindItem("DashboardViewer"), WebDashboardViewerViewItem)
			If dashboardViewerViewItem IsNot Nothing Then
				RemoveHandler dashboardViewerViewItem.ControlCreated, AddressOf DashboardViewerViewItem_ControlCreated
			End If
			MyBase.OnDeactivated()
		End Sub
		Public Sub New()
			openDetailViewAction = New ParametrizedAction(Me, "Dashboard_OpenDetailView", "Dashboard", GetType(String))
			openDetailViewAction.Caption = "OpenDetailView"
			openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject
			AddHandler openDetailViewAction.Execute, AddressOf OpenDetailViewAction_Execute
		End Sub
	End Class
End Namespace
