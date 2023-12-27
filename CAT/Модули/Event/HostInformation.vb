Module HostInformation
    Public Sub HostInfo()
        Dim strHostName As String

        Dim strIPAddress As Array
        Dim r As Integer


        strHostName = System.Net.Dns.GetHostName()

        strIPAddress = System.Net.Dns.GetHostByName(strHostName).AddressList

        Form1.IPADDR.Items.Clear()

        Form1.Label4.Text = strHostName
        For r = 0 To strIPAddress.Length - 1
            Form1.IPADDR.Items.Add(System.Net.Dns.GetHostByName(strHostName).AddressList(r).ToString())
        Next
    End Sub

End Module
