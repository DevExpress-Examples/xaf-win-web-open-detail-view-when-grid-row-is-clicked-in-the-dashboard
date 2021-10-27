Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Dashboards
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.BaseImpl
Imports System
Imports System.Reflection
Imports ShowDetailViewFromDashboard.Module.BusinessObjects

Namespace ShowDetailViewFromDashboard.Module.DatabaseUpdate

    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub

        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            Dim contactMary As Contact = ObjectSpace.FindObject(Of Contact)(CriteriaOperator.Parse("FirstName == 'Mary' && LastName == 'Tellitson'"))
            If contactMary Is Nothing Then
                contactMary = ObjectSpace.CreateObject(Of Contact)()
                contactMary.FirstName = "Mary"
                contactMary.LastName = "Tellitson"
                contactMary.Email = "mary_tellitson@md.com"
                contactMary.Birthday = New DateTime(1980, 11, 27)
            End If

            Dim contactJohn As Contact = ObjectSpace.FindObject(Of Contact)(CriteriaOperator.Parse("FirstName == 'John' && LastName == 'Nilsen'"))
            If contactJohn Is Nothing Then
                contactJohn = ObjectSpace.CreateObject(Of Contact)()
                contactJohn.FirstName = "John"
                contactJohn.LastName = "Nilsen"
                contactJohn.Email = "john_nilsen@md.com"
                contactJohn.Birthday = New DateTime(1981, 10, 3)
            End If

            Dim contactAndrew As Contact = ObjectSpace.FindObject(Of Contact)(CriteriaOperator.Parse("FirstName == 'Andrew' && LastName == 'Fuller'"))
            If contactAndrew Is Nothing Then
                contactAndrew = ObjectSpace.CreateObject(Of Contact)()
                contactAndrew.FirstName = "Andrew"
                contactAndrew.LastName = "Fuller"
                contactAndrew.Email = "andrewfuller@example.com"
                contactAndrew.Birthday = New DateTime(1952, 3, 19)
            End If

            Dim contactRobert As Contact = ObjectSpace.FindObject(Of Contact)(CriteriaOperator.Parse("FirstName == 'Robert' && LastName == 'King'"))
            If contactRobert Is Nothing Then
                contactRobert = ObjectSpace.CreateObject(Of Contact)()
                contactRobert.FirstName = "Robert"
                contactRobert.LastName = "King"
                contactRobert.Email = "robertking@example.com"
                contactRobert.Birthday = New DateTime(1975, 4, 25)
            End If

            Dim assembly As Assembly = Assembly.GetExecutingAssembly()
            DashboardsModule.AddDashboardDataFromResources(Of DashboardData)(ObjectSpace, "Contacts", assembly, "ShowDetailViewFromDashboard.Module.Dashboards.Contacts.xml")
            ObjectSpace.CommitChanges()
        End Sub
    End Class
End Namespace
