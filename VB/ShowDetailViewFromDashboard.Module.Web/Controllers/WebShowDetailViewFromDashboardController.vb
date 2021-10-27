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

                dashboardViewerViewItem.ControlCreated += AddressOf DashboardViewerViewItem_ControlCreated
            End If
        End Sub

        Private Sub DashboardViewerViewItem_ControlCreated(ByVal sender As Object, ByVal e As EventArgs)
            Dim dashboardViewerViewItem As WebDashboardViewerViewItem = TryCast(sender, WebDashboardViewerViewItem)
            CustomizeDashboardViewer(dashboardViewerViewItem.DashboardDesigner)
        End Sub

        Private Sub CustomizeDashboardViewer(ByVal dashboardControl As ASPxDashboard)
            Dim callbackManager As XafCallbackManager = CType(WebWindow.CurrentRequestPage, ICallbackManagerHolder).CallbackManager
            callbackManager.RegisterHandler(HandlerName, Me)
            Dim widgetScript As String = "function getOid(s, e) {{" & Microsoft.VisualBasic.Constants.vbCrLf & "                                        function findMeasure(measure) {{" & Microsoft.VisualBasic.Constants.vbCrLf & "                                            return measure.DataMember === 'Oid';" & Microsoft.VisualBasic.Constants.vbCrLf & "                                        }}" & Microsoft.VisualBasic.Constants.vbCrLf & "                                        if (e.ItemName.includes('gridDashboardItem')) {{" & Microsoft.VisualBasic.Constants.vbCrLf & "                                             var axisPoint = e.GetAxisPoint();" & Microsoft.VisualBasic.Constants.vbCrLf & "                                             if (!axisPoint) " & Microsoft.VisualBasic.Constants.vbCrLf & "                                                 return null;" & Microsoft.VisualBasic.Constants.vbCrLf & "                                             var itemData = e.GetData(),   " & Microsoft.VisualBasic.Constants.vbCrLf & "                                                dataSlice = itemData.GetSlice(axisPoint)," & Microsoft.VisualBasic.Constants.vbCrLf & "                                                oidMeasure = dataSlice.GetMeasures().find(findMeasure).Id," & Microsoft.VisualBasic.Constants.vbCrLf & "                                                measureValue = dataSlice.GetMeasureValue(oidMeasure);" & Microsoft.VisualBasic.Constants.vbCrLf & "                                                {0}" & Microsoft.VisualBasic.Constants.vbCrLf & "                                        }}" & Microsoft.VisualBasic.Constants.vbCrLf & "                                    }}"
            dashboardControl.ClientSideEvents.ItemClick = String.Format(widgetScript, callbackManager.GetScript(HandlerName, "measureValue.GetValue()"))
        End Sub

        Public Sub ProcessAction(ByVal parameter As String)
            openDetailViewAction.DoExecute(parameter)
        End Sub

        Private Sub OpenDetailViewAction_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs)
            Dim oid As Guid = Nothing
            Guid.TryParse(CStr(e.ParameterCurrentValue), oid)
            Dim objectSpace As IObjectSpace = Application.CreateObjectSpace(GetType([Module].BusinessObjects.Contact))
            Dim contact As [Module].BusinessObjects.Contact = objectSpace.FindObject(Of [Module].BusinessObjects.Contact)(New BinaryOperator(NameOf([Module].BusinessObjects.Contact.Oid), oid))
            If contact IsNot Nothing Then
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View)
            End If
        End Sub

        Protected Overrides Sub OnDeactivated()
            Dim dashboardViewerViewItem As WebDashboardViewerViewItem = TryCast(View.FindItem("DashboardViewer"), WebDashboardViewerViewItem)
            If dashboardViewerViewItem IsNot Nothing Then
                dashboardViewerViewItem.ControlCreated -= AddressOf DashboardViewerViewItem_ControlCreated
            End If

            MyBase.OnDeactivated()
        End Sub

        Public Sub New()
            openDetailViewAction = New ParametrizedAction(Me, "Dashboard_OpenDetailView", "Dashboard", GetType(String))
            openDetailViewAction.Caption = "OpenDetailView"
            openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            Me.openDetailViewAction.Execute += AddressOf OpenDetailViewAction_Execute
        End Sub
    End Class
End Namespace
