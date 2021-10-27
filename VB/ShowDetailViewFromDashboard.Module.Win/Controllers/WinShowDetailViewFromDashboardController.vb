Imports System
Imports System.Linq
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardCommon.ViewerData
Imports DevExpress.DashboardWin
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Dashboards.Win
Imports DevExpress.Persistent.Base
Imports ShowDetailViewFromDashboard.Module.BusinessObjects

Namespace ShowDetailViewFromDashboard.Module.Win.Controllers
	Public Class WinShowDetailViewFromDashboardController
		Inherits ObjectViewController(Of DetailView, IDashboardData)

		Private ReadOnly openDetailViewAction As ParametrizedAction

		Public Sub New()
			openDetailViewAction = New ParametrizedAction(Me, "Dashboard_OpenDetailView", "Dashboard", GetType(String))
			openDetailViewAction.Caption = "OpenDetailView"
			openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject
			AddHandler openDetailViewAction.Execute, AddressOf OpenDetailViewAction_Execute
		End Sub

		Private Sub DashboardViewerViewItem_ControlCreated(ByVal sender As Object, ByVal e As EventArgs)
			Dim dashboardViewerViewItem As WinDashboardViewerViewItem = TryCast(sender, WinDashboardViewerViewItem)
			AddHandler dashboardViewerViewItem.Viewer.DashboardItemDoubleClick, AddressOf Viewer_DashboardItemDoubleClick
		End Sub

		Private Function IsGridDashboardItem(ByVal dashboard As Dashboard, ByVal dashboardItemName As String) As Boolean
			Dim dashboardItem As DashboardItem = dashboard.Items.SingleOrDefault(Function(item) item.ComponentName = dashboardItemName)
			Return TypeOf dashboardItem Is GridDashboardItem
		End Function

		Private Shared Function GetOid(ByVal e As DashboardItemMouseActionEventArgs) As String
			Dim axisPoint As AxisPoint = e.GetAxisPoint()
			If axisPoint Is Nothing Then
				Return Nothing
			End If
			Dim data As MultiDimensionalData = e.Data.GetSlice(axisPoint)
			Dim descriptor As MeasureDescriptor = data.GetMeasures().SingleOrDefault(Function(item) item.DataMember = "Oid")
			Dim measureValue As MeasureValue = data.GetValue(descriptor)
			Return measureValue.Value.ToString()
		End Function

		Private Sub Viewer_DashboardItemDoubleClick(ByVal sender As Object, ByVal e As DashboardItemMouseActionEventArgs)
			Dim dashboard As Dashboard = DirectCast(sender, DashboardViewer).Dashboard
			Dim oid As Object
			If (IsGridDashboardItem(dashboard, e.DashboardItemName) AndAlso openDetailViewAction.Enabled.ResultValue AndAlso openDetailViewAction.Active.ResultValue AndAlso Guid.TryParse(GetOid(e), oid)) Then
				openDetailViewAction.DoExecute(oid)
			End If
		End Sub

		Private Sub OpenDetailViewAction_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs)
			Dim oid As Object = e.ParameterCurrentValue
			Dim objectSpace As IObjectSpace = Application.CreateObjectSpace(GetType(Contact))
			Dim contact As Contact = objectSpace.FindObject(Of Contact)(New BinaryOperator(NameOf(contact.Oid), oid))
			If contact IsNot Nothing Then
				e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View)
			End If
		End Sub

		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			Dim dashboardViewerViewItem As WinDashboardViewerViewItem = TryCast(View.FindItem("DashboardViewer"), WinDashboardViewerViewItem)
			If dashboardViewerViewItem IsNot Nothing Then
				If dashboardViewerViewItem.Viewer IsNot Nothing Then
					AddHandler dashboardViewerViewItem.Viewer.DashboardItemDoubleClick, AddressOf Viewer_DashboardItemDoubleClick
				End If
				AddHandler dashboardViewerViewItem.ControlCreated, AddressOf DashboardViewerViewItem_ControlCreated
			End If
		End Sub

		Protected Overrides Sub OnDeactivated()
			Dim dashboardViewerViewItem As WinDashboardViewerViewItem = TryCast(View.FindItem("DashboardViewer"), WinDashboardViewerViewItem)
			If dashboardViewerViewItem IsNot Nothing Then
				RemoveHandler dashboardViewerViewItem.ControlCreated, AddressOf DashboardViewerViewItem_ControlCreated
			End If
			MyBase.OnDeactivated()
		End Sub
	End Class
End Namespace
