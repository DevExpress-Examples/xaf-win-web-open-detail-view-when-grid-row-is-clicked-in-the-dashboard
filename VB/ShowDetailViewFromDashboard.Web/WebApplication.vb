Imports System
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Web
Imports DevExpress.ExpressApp.Xpo

Namespace ShowDetailViewFromDashboard.Web

    ' For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    Public Partial Class ShowDetailViewFromDashboardAspNetApplication
        Inherits WebApplication

        Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule

        Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule

        Private module3 As ShowDetailViewFromDashboard.Module.ShowDetailViewFromDashboardModule

        Private module4 As ShowDetailViewFromDashboard.Module.Web.ShowDetailViewFromDashboardAspNetModule

        Private dashboardsModule As DevExpress.ExpressApp.Dashboards.DashboardsModule

        Private dashboardsAspNetModule As DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule

        Public Sub New()
            InitializeComponent()
            LinkNewObjectToParentImmediately = False
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxGridListEditor.AllowFilterControlHierarchy = True
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxGridListEditor.MaxFilterControlHierarchyDepth = 3
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxCriteriaPropertyEditor.AllowFilterControlHierarchyDefault = True
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxCriteriaPropertyEditor.MaxHierarchyDepthDefault = 3
        End Sub

        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
            args.ObjectSpaceProvider = New XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), True)
            args.ObjectSpaceProviders.Add(New NonPersistentObjectSpaceProvider(TypesInfo, Nothing))
        End Sub

        Private Function GetDataStoreProvider(ByVal connectionString As String, ByVal connection As Data.IDbConnection) As IXpoDataStoreProvider
            Dim application As System.Web.HttpApplicationState = If(System.Web.HttpContext.Current IsNot Nothing, System.Web.HttpContext.Current.Application, Nothing)
            Dim dataStoreProvider As IXpoDataStoreProvider = Nothing
            If application IsNot Nothing AndAlso application("DataStoreProvider") IsNot Nothing Then
                dataStoreProvider = TryCast(application("DataStoreProvider"), IXpoDataStoreProvider)
            Else
                If Not String.IsNullOrEmpty(connectionString) Then
                    connectionString = DevExpress.Xpo.XpoDefault.GetConnectionPoolString(connectionString)
                    dataStoreProvider = New ConnectionStringDataStoreProvider(connectionString, True)
                ElseIf connection IsNot Nothing Then
                    dataStoreProvider = New ConnectionDataStoreProvider(connection)
                End If

                If application IsNot Nothing Then
                    application("DataStoreProvider") = dataStoreProvider
                End If
            End If

            Return dataStoreProvider
        End Function

        Private Sub ShowDetailViewFromDashboardAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)
#If EASYTEST
            e.Updater.Update();
            e.Handled = true;
#Else
            If System.Diagnostics.Debugger.IsAttached Then
                e.Updater.Update()
                e.Handled = True
            Else
                Dim message As String = "The application cannot connect to the specified database, " & "because the database doesn't exist, its version is older " & "than that of the application or its schema does not match " & "the ORM data model structure. To avoid this error, use one " & "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article."
                If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
                    message += Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
                End If

                Throw New InvalidOperationException(message)
            End If
#End If
        End Sub

        Private Sub InitializeComponent()
            module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
            module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
            module3 = New ShowDetailViewFromDashboard.Module.ShowDetailViewFromDashboardModule()
            module4 = New ShowDetailViewFromDashboard.Module.Web.ShowDetailViewFromDashboardAspNetModule()
            dashboardsModule = New DevExpress.ExpressApp.Dashboards.DashboardsModule()
            dashboardsAspNetModule = New DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            ' dashboardsModule
            '
            dashboardsModule.DashboardDataType = GetType(DevExpress.Persistent.BaseImpl.DashboardData)
            ' 
            ' ShowDetailViewFromDashboardAspNetApplication
            ' 
            Me.ApplicationName = "ShowDetailViewFromDashboard"
            Me.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema
            Me.Modules.Add(module1)
            Me.Modules.Add(module2)
            Me.Modules.Add(module3)
            Me.Modules.Add(module4)
            Me.Modules.Add(dashboardsModule)
            Me.Modules.Add(dashboardsAspNetModule)
             ''' Cannot convert AssignmentExpressionSyntax, System.NullReferenceException: Object reference not set to an instance of an object.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''             this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ShowDetailViewFromDashboardAspNetApplication_DatabaseVersionMismatch)
'''  CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        End Sub
    End Class
End Namespace
