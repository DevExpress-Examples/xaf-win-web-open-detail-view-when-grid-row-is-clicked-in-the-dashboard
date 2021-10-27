;#DropDB ShowDetailViewFromDashboardEasyTest

#Application ShowDetailViewFromDashboard.Win
#Application ShowDetailViewFromDashboard.Web
#IfDef ShowDetailViewFromDashboard.Win

*Action About...
 
*Action OK

#EndIf

#IfDef ShowDetailViewFromDashboard.Web
!Action Navigation(My Details)
#EndIf
