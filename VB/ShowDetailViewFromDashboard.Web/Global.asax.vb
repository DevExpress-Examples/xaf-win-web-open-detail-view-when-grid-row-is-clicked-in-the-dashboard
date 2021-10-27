Imports System
Imports System.Configuration
Imports System.Web
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security
Imports DevExpress.ExpressApp.Web
Imports DevExpress.Web

Namespace ShowDetailViewFromDashboard.Web

    Public Class [Global]
        Inherits HttpApplication

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
             ''' Cannot convert AssignmentExpressionSyntax, System.NullReferenceException: Object reference not set to an instance of an object.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAssignmentExpression(AssignmentExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''             ASPxWebControl.CallbackError += new System.EventHandler(this.Application_Error)
'''  WebApplication.EnableMultipleBrowserTabsSupport = True
#If EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#End If
        End Sub

        Protected Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
            Tracing.Initialize()
            WebApplication.SetInstance(Session, New ShowDetailViewFromDashboardAspNetApplication())
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit()
            WebApplication.Instance.SwitchToNewStyle()
            If ConfigurationManager.ConnectionStrings("ConnectionString") IsNot Nothing Then
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            End If

#If EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#End If
            If System.Diagnostics.Debugger.IsAttached AndAlso WebApplication.Instance.CheckCompatibilityType Is CheckCompatibilityType.DatabaseSchema Then
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways
            End If

            WebApplication.Instance.Setup()
            WebApplication.Instance.Start()
        End Sub

        Protected Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Protected Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Protected Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
            ErrorHandling.Instance.ProcessApplicationError()
        End Sub

        Protected Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
            WebApplication.LogOff(Session)
            WebApplication.DisposeInstance(Session)
        End Sub

        Protected Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

'#Region "Web Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
        End Sub
'#End Region
    End Class
End Namespace
