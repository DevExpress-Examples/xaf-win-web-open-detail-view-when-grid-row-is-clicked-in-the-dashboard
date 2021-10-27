﻿Namespace ShowDetailViewFromDashboard.Win
    Partial Public Class ShowDetailViewFromDashboardWindowsFormsApplication
        ''' <summary> 
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary> 
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
            Me.module2 = New DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule()
            Me.module3 = New ShowDetailViewFromDashboard.Module.ShowDetailViewFromDashboardModule()
            Me.module4 = New ShowDetailViewFromDashboard.Module.Win.ShowDetailViewFromDashboardWindowsFormsModule()
            Me.dashboardsModule = New DevExpress.ExpressApp.Dashboards.DashboardsModule()
            Me.dashboardsWindowsFormsModule = New DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            ' dashboardsModule
            '
            Me.dashboardsModule.DashboardDataType = GetType(DevExpress.Persistent.BaseImpl.DashboardData)
            ' 
            ' ShowDetailViewFromDashboardWindowsFormsApplication
            ' 
            Me.ApplicationName = "ShowDetailViewFromDashboard"
            Me.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema
            Me.Modules.Add(Me.module1)
            Me.Modules.Add(Me.module2)
            Me.Modules.Add(Me.module3)
            Me.Modules.Add(Me.module4)
            Me.Modules.Add(Me.dashboardsModule)
            Me.Modules.Add(Me.dashboardsWindowsFormsModule)
            Me.UseOldTemplates = False

            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub

        #End Region

        Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
        Private module2 As DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule
        Private module3 As ShowDetailViewFromDashboard.Module.ShowDetailViewFromDashboardModule
        Private module4 As ShowDetailViewFromDashboard.Module.Win.ShowDetailViewFromDashboardWindowsFormsModule
        Private dashboardsModule As DevExpress.ExpressApp.Dashboards.DashboardsModule
        Private dashboardsWindowsFormsModule As DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule
    End Class
End Namespace
