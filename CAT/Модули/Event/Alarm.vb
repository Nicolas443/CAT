Module Alarm
    Public Sub Alarm()
        Dim alarm_mess_patch As String
        Dim killtimeout As Integer
        Dim Jingle As String
        Dim r As Integer
        Dim alarm_pl As Array
        Dim ПлейлистОбъявлений As New ArrayList
        Dim Настройки As XDocument


        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")


        alarm_mess_patch = Настройки.Document.Element("Setting").Element("alarm").Attribute("dir").Value

        If System.IO.Directory.Exists(alarm_mess_patch) = True Then
            alarm_pl = System.IO.Directory.GetFiles(alarm_mess_patch)
            Jingle = Настройки.Document.Element("Setting").Element("alarm").Attribute("Jingle").Value

            If alarm_pl.Length > 0 Then
                For r = 0 To alarm_pl.Length - 1
                    If Eventlist.Contains("2" & "|" & alarm_pl(r) & "|" & "alarm") = False Then
                        ' ПлейлистОбъявлений.Add(alarm_pl(r))
                        '  AlarmList.Items.Add(alarm_pl(r))
                        Eventlist.Add("2" & "|" & alarm_pl(r) & "|" & "alarm")
                    End If
                Next
            End If


            'Try
            '    killtimeout = Настройки.Document.Element("Setting").Element("music").Attribute("killtimeout").Value
            'Catch ex As Exception
            '    Настройки.Document.Element("Setting").Element("music").SetAttributeValue("killtimeout", "1000")
            '    Настройки.Save("Настройки.xml")
            '    Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            '    killtimeout = Настройки.Document.Element("Setting").Element("music").Attribute("killtimeout").Value
            'End Try

            'Try
            '    If ПлейлистОбъявлений.Count > 0 Then
            '        Play.ВремяAlarm = True
            '    End If

            'Catch ex As Exception

            'End Try
        End If
    End Sub
    Public Function Get_First_Alarm_File_Name()
        Dim Jingle As String
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim Profile As XDocument

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Jingle = Profile.Document.Element("Setting").Element("Jingle").Attribute("Jingle").Value
        If SettingAlarm.UseJingle = True Then
            alarm_file = Jingle
        Else
            alarm_file = Form1.ПлейлистОбъявлений(0)
        End If
        Return alarm_file
    End Function

End Module
