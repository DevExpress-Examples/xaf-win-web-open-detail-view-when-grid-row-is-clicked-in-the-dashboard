Imports System
Imports System.ComponentModel
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Win
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.ExpressApp.Xpo

Namespace ShowDetailViewFromDashboard.Win

    ' For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWinWinApplicationMembersTopicAll.aspx
    Public Partial Class ShowDetailViewFromDashboardWindowsFormsApplication
        Inherits WinApplication

        Public Sub New()
            InitializeComponent()
            LinkNewObjectToParentImmediately = False
        End Sub

        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
            args.ObjectSpaceProviders.Add(New XPObjectSpaceProvider(args.ConnectionString, args.Connection, False))
            args.ObjectSpaceProviders.Add(New NonPersistentObjectSpaceProvider(TypesInfo, Nothing))
        End Sub

        Private Sub ShowDetailViewFromDashboardWindowsFormsApplication_CustomizeLanguagesList(ByVal sender As Object, ByVal e As CustomizeLanguagesListEventArgs)
            Dim userLanguageName As String = Threading.Thread.CurrentThread.CurrentUICulture.Name
            If Not Equals(userLanguageName, "en-US") AndAlso e.Languages.IndexOf(userLanguageName) = -1 Then
                e.Languages.Add(userLanguageName)
            End If
        End Sub

        Private Sub ShowDetailViewFromDashboardWindowsFormsApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)
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
    End Class
End Namespace
