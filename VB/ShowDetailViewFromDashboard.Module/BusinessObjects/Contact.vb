Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace ShowDetailViewFromDashboard.Module.BusinessObjects

    <DefaultClassOptions>
    Public Class Contact
        Inherits Person

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class
End Namespace
