Public Class About
    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Версия программы: " & Application.ProductVersion
        Label2.Text = "Организация:  " & Application.CompanyName
        Label3.Text = "Домен: " & SettingDomen.Name & SettingDomen.SubDomen
        Label4.Text = "Серийный номер: " & SettingDomen.Serial

    End Sub
End Class