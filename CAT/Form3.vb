Imports Un4seen.Bass
Imports Un4seen.Bass.AddOn.Mix
Imports Un4seen.Bass.Misc
Public Class Form3

    Dim Настройки As System.Xml.Linq.XDocument
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'CheckSetting.БылКлик = False
        'CheckSetting.Check()
        ReadSetting()
    End Sub


    Public Sub ReadSetting()

        Dim Profile As XDocument
        Profile = GenerlaSetting.ProfileLoad

        RealTime.Checked = Profile.Document.Element("Setting").Element("music").Attribute("RealTimeVolumeRegulation").Value
        ADVRealTime.Checked = Profile.Document.Element("Setting").Element("adv").Attribute("RealTimeVolumeRegulation").Value
        AlarmRealTime.Checked = Profile.Document.Element("Setting").Element("alarm").Attribute("RealTimeVolumeRegulation").Value

        MusicVolSCH.Items.Clear()
        ADVVolSCH.Items.Clear()
        AlarmVOLSch.Items.Clear()
        JingleVOLSch.Items.Clear()


        For Each elem In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            MusicVolSCH.Items.Add(elem.Value.ToString.Split("|")(0))
            MusicVolSCH.Items(MusicVolSCH.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next
        For Each elem In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            ADVVolSCH.Items.Add(elem.Value.ToString.Split("|")(0))
            ADVVolSCH.Items(ADVVolSCH.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next
        For Each elem In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            AlarmVOLSch.Items.Add(elem.Value.ToString.Split("|")(0))
            AlarmVOLSch.Items(AlarmVOLSch.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next
        For Each elem In Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes
            JingleVOLSch.Items.Add(elem.Value.ToString.Split("|")(0))
            JingleVOLSch.Items(JingleVOLSch.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next


        MusicDefaultVolume.Value = Profile.Document.Element("Setting").Element("music").Attribute("volume").Value
        ADVDefaultVolume.Value = Profile.Document.Element("Setting").Element("adv").Attribute("volume").Value
        AlarmDefaultVolume.Value = Profile.Document.Element("Setting").Element("alarm").Attribute("volume").Value
        JingleDefaultVolume.Value = Profile.Document.Element("Setting").Element("Jingle").Attribute("volume").Value

    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ГромкостьРекламы.Value = TrackBar1.Value
    End Sub

    Private Sub ГромкостьРекламы_ValueChanged(sender As Object, e As EventArgs) Handles ГромкостьРекламы.ValueChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                times.Value = times.Value.Split("|")(0) & "|" & ГромкостьРекламы.Value
                '  Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, times.Value.Split("|")(1) / 100)
            End If
        Next

        ADVVolSCH.Items.Clear()
        For Each elem In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            ADVVolSCH.Items.Add(elem.Value.ToString.Split("|")(0))
            ADVVolSCH.Items(ADVVolSCH.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next


        '  Profile.Document.Element("Setting").Element("adv").SetAttributeValue("volume", ГромкостьРекламы.Value)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)

        If ADVRealTime.Checked = True Then
            VolumeSet.VolumeSet("ADV")
            ' Bass.BASS_ChannelSetAttribute(stream_adv, BASSAttribute.BASS_ATTRIB_VOL, (ГромкостьРекламы.Value / 100))
        End If


        TrackBar1.Value = ГромкостьРекламы.Value
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        AlarmVolume.Value = TrackBar2.Value
    End Sub

    Private Sub AlarmVolume_ValueChanged(sender As Object, e As EventArgs) Handles AlarmVolume.ValueChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                times.Value = times.Value.Split("|")(0) & "|" & AlarmVolume.Value
                '  Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, times.Value.Split("|")(1) / 100)
            End If
        Next

        AlarmVOLSch.Items.Clear()
        For Each elem In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            AlarmVOLSch.Items.Add(elem.Value.ToString.Split("|")(0))
            AlarmVOLSch.Items(AlarmVOLSch.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next


        '  Profile.Document.Element("Setting").Element("adv").SetAttributeValue("volume", ГромкостьРекламы.Value)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)

        If AlarmRealTime.Checked = True Then
            VolumeSet.VolumeSet("Alarm")
            ' Bass.BASS_ChannelSetAttribute(stream_adv, BASSAttribute.BASS_ATTRIB_VOL, (ГромкостьРекламы.Value / 100))
        End If

        TrackBar2.Value = AlarmVolume.Value
    End Sub

    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs)
        JingleVolume.Value = TrackBar3.Value
    End Sub

    Private Sub JingleVolume_ValueChanged(sender As Object, e As EventArgs)
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("volume_jingle", JingleVolume.Value)

        If System.IO.File.Exists(ProfileName) = True Then
            Profile.Save(ProfileName)
        Else
            Profile.Save("Настройки.xml")
        End If

        If RealTime.Checked = True Then
            Bass.BASS_ChannelSetAttribute(stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, JingleVolume.Value / 100)
        End If

        TrackBar3.Value = JingleVolume.Value
    End Sub

    Private Sub ДобавитьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ДобавитьToolStripMenuItem.Click
        Form2.Событие = "Music"
        Form2.Изменение = False

        Form2.ShowDialog()
    End Sub

    Private Sub ИзменитьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ИзменитьToolStripMenuItem.Click
        Form2.Событие = "Music"
        Form2.X = MusicVolSCH.FocusedItem.Index
        Form2.Изменение = True
        Form2.ShowDialog()
    End Sub

    Private Sub УдалитьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles УдалитьToolStripMenuItem.Click
        Dim r As Integer
        r = MusicVolSCH.FocusedItem.Index
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each elem In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            If elem.Value.ToString = MusicVolSCH.Items(r).Text & "|" & MusicVolSCH.Items(r).SubItems(1).Text Then
                '   attr = elem.Name.ToString
                elem.Remove()
                Exit For
            End If
        Next
        MusicVolSCH.Items(r).Remove()
        ' Настройки.Save("Настройки.xml")
        Profile.Save(ProfileName)
    End Sub

    Private Sub RealTime_CheckedChanged(sender As Object, e As EventArgs) Handles RealTime.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("music").SetAttributeValue("RealTimeVolumeRegulation", RealTime.Checked)
        If System.IO.File.Exists(ProfileName) = True Then
            Profile.Save(ProfileName)
        Else
            Profile.Save("Настройки.xml")
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MusicVolSCH.SelectedIndexChanged

    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Form2.Событие = "ADV"
        Form2.Изменение = False
        Form2.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Form2.Событие = "ADV"
        Form2.X = ADVVolSCH.FocusedItem.Index
        Form2.Изменение = True
        Form2.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Form2.Событие = "Alarm"
        Form2.Изменение = False
        Form2.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        Form2.Событие = "Alarm"
        Form2.X = AlarmVOLSch.FocusedItem.Index
        Form2.Изменение = True
        Form2.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах

        Dim ProfileName As String
        Dim Profile As XDocument
        Dim ЕстьВремя As Boolean
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                ГромкостьРекламы.Value = times.Value.Split("|")(1)
                ЕстьВремя = True
                '  Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, times.Value.Split("|")(1) / 100)
            End If

        Next

        If ЕстьВремя = False Then
            ГромкостьРекламы.Value = Profile.Document.Element("Setting").Element("adv").Attribute("volume").Value
        End If
        ЕстьВремя = False
    End Sub

    Private Sub ADVRealTime_CheckedChanged(sender As Object, e As EventArgs) Handles ADVRealTime.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("adv").SetAttributeValue("RealTimeVolumeRegulation", RealTime.Checked)
        If System.IO.File.Exists(ProfileName) = True Then
            Profile.Save(ProfileName)
        Else
            Profile.Save("Настройки.xml")
        End If
    End Sub

    Private Sub ГромкостьМузыки_ValueChanged(sender As Object, e As EventArgs) Handles ГромкостьМузыки.ValueChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                times.Value = times.Value.Split("|")(0) & "|" & ГромкостьМузыки.Value
                '  Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, times.Value.Split("|")(1) / 100)
            End If
        Next

        MusicVolSCH.Items.Clear()
        For Each elem In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            MusicVolSCH.Items.Add(elem.Value.ToString.Split("|")(0))
            MusicVolSCH.Items(MusicVolSCH.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
        Next


        '  Profile.Document.Element("Setting").Element("adv").SetAttributeValue("volume", ГромкостьРекламы.Value)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)

        If RealTime.Checked = True Then
            VolumeSet.VolumeSet("Music")
            ' Bass.BASS_ChannelSetAttribute(stream_adv, BASSAttribute.BASS_ATTRIB_VOL, (ГромкостьРекламы.Value / 100))
        End If




        TrackBar4.Value = ГромкостьМузыки.Value
    End Sub

    Private Sub TrackBar4_Scroll(sender As Object, e As EventArgs) Handles TrackBar4.Scroll
        ГромкостьМузыки.Value = TrackBar4.Value
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        Dim ЕстьВремя As Boolean

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                ГромкостьМузыки.Value = times.Value.Split("|")(1)
                ЕстьВремя = True
                '  Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, times.Value.Split("|")(1) / 100)
            End If
        Next

        If ЕстьВремя = False Then
            ГромкостьМузыки.Value = Profile.Document.Element("Setting").Element("music").Attribute("volume").Value
        End If
        ЕстьВремя = False



    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        Dim ЕстьВремя As Boolean

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                AlarmVolume.Value = times.Value.Split("|")(1)
                ЕстьВремя = True
                '  Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, times.Value.Split("|")(1) / 100)
            End If
        Next

        If ЕстьВремя = False Then
            AlarmVolume.Value = Profile.Document.Element("Setting").Element("alarm").Attribute("volume").Value
        End If
        ЕстьВремя = False

    End Sub

    Private Sub AlarmRealTime_CheckedChanged(sender As Object, e As EventArgs) Handles AlarmRealTime.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("RealTimeVolumeRegulation", RealTime.Checked)
        If System.IO.File.Exists(ProfileName) = True Then
            Profile.Save(ProfileName)
        Else
            Profile.Save("Настройки.xml")
        End If
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        Dim r As Integer
        r = ADVVolSCH.FocusedItem.Index
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        For Each elem In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            If elem.Value.ToString = ADVVolSCH.Items(r).Text & "|" & ADVVolSCH.Items(r).SubItems(1).Text Then
                '   attr = elem.Name.ToString
                elem.Remove()
                Exit For
            End If
        Next
        ADVVolSCH.Items(r).Remove()
        '  Настройки.Save("Настройки.xml")
        Profile.Save(ProfileName)
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem6.Click
        Dim r As Integer
        r = AlarmVOLSch.FocusedItem.Index
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        For Each elem In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            If elem.Value.ToString = AlarmVOLSch.Items(r).Text & "|" & AlarmVOLSch.Items(r).SubItems(1).Text Then
                '   attr = elem.Name.ToString
                elem.Remove()
                Exit For
            End If
        Next
        AlarmVOLSch.Items(r).Remove()
        'Настройки.Save("Настройки.xml")
        Profile.Save(ProfileName)
    End Sub

    Private Sub MusicDefaultVolume_ValueChanged(sender As Object, e As EventArgs) Handles MusicDefaultVolume.ValueChanged
        TrackBar5.Value = MusicDefaultVolume.Value
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("music").SetAttributeValue("volume", MusicDefaultVolume.Value)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)
    End Sub

    Private Sub TrackBar5_Scroll(sender As Object, e As EventArgs) Handles TrackBar5.Scroll

        MusicDefaultVolume.Value = TrackBar5.Value

    End Sub

    Private Sub ADVDefaultVolume_ValueChanged(sender As Object, e As EventArgs) Handles ADVDefaultVolume.ValueChanged
        TrackBar6.Value = ADVDefaultVolume.Value

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("adv").SetAttributeValue("volume", ADVDefaultVolume.Value)

        Profile.Save(ProfileName)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    
        'Else
        '    Profile.Save("Настройки.xml")
        'End If

    End Sub

    Private Sub TrackBar6_Scroll(sender As Object, e As EventArgs) Handles TrackBar6.Scroll
        ADVDefaultVolume.Value = TrackBar6.Value
    End Sub

    Private Sub AlarmDefaultVolume_ValueChanged(sender As Object, e As EventArgs) Handles AlarmDefaultVolume.ValueChanged
        TrackBar7.Value = AlarmDefaultVolume.Value
        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", AlarmDefaultVolume.Value)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)
    End Sub

    Private Sub TrackBar7_Scroll(sender As Object, e As EventArgs) Handles TrackBar7.Scroll
        AlarmDefaultVolume.Value = TrackBar7.Value
    End Sub

    Private Sub MusicSilence_ValueChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem7.Click
        Form2.Событие = "Jingle"
        Form2.Изменение = False
        Form2.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem8.Click
        Form2.Событие = "Jingle"
        Form2.X = JingleVOLSch.FocusedItem.Index
        Form2.Изменение = True
        Form2.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem9_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem9.Click
        Dim r As Integer
        r = JingleVOLSch.FocusedItem.Index
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each elem In Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes
            If elem.Value.ToString = JingleVOLSch.Items(r).Text & "|" & JingleVOLSch.Items(r).SubItems(1).Text Then
                '   attr = elem.Name.ToString
                elem.Remove()
                Exit For
            End If
        Next
        JingleVOLSch.Items(r).Remove()
        ' Настройки.Save("Настройки.xml")
        Profile.Save(ProfileName)
    End Sub

    Private Sub JingleDefaultVolume_ValueChanged(sender As Object, e As EventArgs) Handles JingleDefaultVolume.ValueChanged
        TrackBar3.Value = JingleDefaultVolume.Value
        Dim ProfileName As String
        Dim Profile As XDocument
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("Jingle").SetAttributeValue("volume", JingleDefaultVolume.Value)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)
    End Sub

    Private Sub TrackBar3_Scroll_1(sender As Object, e As EventArgs) Handles TrackBar3.Scroll


        JingleDefaultVolume.Value = TrackBar3.Value
    End Sub

    Private Sub GroupBox5_Enter(sender As Object, e As EventArgs) Handles GroupBox5.Enter

    End Sub

    Private Sub JingleVOLSch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles JingleVOLSch.SelectedIndexChanged

    End Sub
End Class