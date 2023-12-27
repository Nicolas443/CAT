Imports System.Text.RegularExpressions

Public Class Form2
    Public настройки As System.Xml.Linq.XDocument
    Public Изменение As Boolean
    Public X As Integer
    Public Событие As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim attribute_count As Integer
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim r As Integer

        настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProfileName = настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value


        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        'If Regex.IsMatch(time1.Text, "^(20[0-9]{2}[0-1][0-2][0-3]\d_[0-2]\d[0-5]\d)$") Then


        'End If
        If Regex.IsMatch(time1.Text, "^([0-2]\d:[0-5]\d:[0-5]\d)$") = False Or Regex.IsMatch(time2.Text, "^([0-2]\d:[0-5]\d:[0-5]\d)$") = False Then
            MsgBox("Время должно соответсовать формату HH:MM:SS", MsgBoxStyle.Critical)
            Exit Sub
        End If


        If Изменение = True Then
            If Событие = "Music" Then
                Form3.MusicVolSCH.Items(X).Text = time1.Text & "-" & time2.Text
                Form3.MusicVolSCH.Items(X).SubItems(1).Text = Громкость.Value
                Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes.Remove
                For r = 0 To Form3.MusicVolSCH.Items.Count - 1
                    attribute_count = Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes.Count
                    Profile.Document.Element("Setting").Element("music").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, Form3.MusicVolSCH.Items(r).Text & "|" & Form3.MusicVolSCH.Items(r).SubItems(1).Text)
                Next
            End If

            If Событие = "ADV" Then
                Form3.ADVVolSCH.Items(X).Text = time1.Text & "-" & time2.Text
                Form3.ADVVolSCH.Items(X).SubItems(1).Text = Громкость.Value
                Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes.Remove
                For r = 0 To Form3.ADVVolSCH.Items.Count - 1
                    attribute_count = Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes.Count
                    Profile.Document.Element("Setting").Element("adv").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, Form3.ADVVolSCH.Items(r).Text & "|" & Form3.ADVVolSCH.Items(r).SubItems(1).Text)
                Next
            End If

            If Событие = "Alarm" Then
                Form3.AlarmVOLSch.Items(X).Text = time1.Text & "-" & time2.Text
                Form3.AlarmVOLSch.Items(X).SubItems(1).Text = Громкость.Value
                Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes.Remove
                For r = 0 To Form3.AlarmVOLSch.Items.Count - 1
                    attribute_count = Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes.Count
                    Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, Form3.AlarmVOLSch.Items(r).Text & "|" & Form3.AlarmVOLSch.Items(r).SubItems(1).Text)
                Next
            End If
            If Событие = "Jingle" Then
                Form3.JingleVOLSch.Items(X).Text = time1.Text & "-" & time2.Text
                Form3.JingleVOLSch.Items(X).SubItems(1).Text = Громкость.Value
                Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes.Remove
                For r = 0 To Form3.JingleVOLSch.Items.Count - 1
                    attribute_count = Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes.Count
                    Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, Form3.JingleVOLSch.Items(r).Text & "|" & Form3.JingleVOLSch.Items(r).SubItems(1).Text)
                Next
            End If

        End If

        If Изменение = False Then
            If Событие = "Music" Then
                Form3.MusicVolSCH.Items.Add(time1.Text & "-" & time2.Text)
                Form3.MusicVolSCH.Items(Form3.MusicVolSCH.Items.Count - 1).SubItems.Add(Громкость.Value)
                attribute_count = Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes.Count
                Profile.Document.Element("Setting").Element("music").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, time1.Text & "-" & time2.Text & "|" & Громкость.Value)
            End If
            If Событие = "ADV" Then
                Form3.ADVVolSCH.Items.Add(time1.Text & "-" & time2.Text)
                Form3.ADVVolSCH.Items(Form3.ADVVolSCH.Items.Count - 1).SubItems.Add(Громкость.Value)
                attribute_count = Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes.Count
                Profile.Document.Element("Setting").Element("adv").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, time1.Text & "-" & time2.Text & "|" & Громкость.Value)
            End If
            If Событие = "Alarm" Then
                Form3.AlarmVOLSch.Items.Add(time1.Text & "-" & time2.Text)
                Form3.AlarmVOLSch.Items(Form3.AlarmVOLSch.Items.Count - 1).SubItems.Add(Громкость.Value)
                attribute_count = Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes.Count
                Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, time1.Text & "-" & time2.Text & "|" & Громкость.Value)
            End If
            If Событие = "Jingle" Then
                Form3.JingleVOLSch.Items.Add(time1.Text & "-" & time2.Text)
                Form3.JingleVOLSch.Items(Form3.JingleVOLSch.Items.Count - 1).SubItems.Add(Громкость.Value)
                attribute_count = Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes.Count
                Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").SetAttributeValue("val" & attribute_count + 1, time1.Text & "-" & time2.Text & "|" & Громкость.Value)
            End If

        End If


        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
        Profile.Save(ProfileName)




        If Изменение = True Then
            If Form3.RealTime.Checked = True Then
                '  Play.volume_set("Music")
                VolumeSet.VolumeSet("Music")
            End If

        End If

        Изменение = False
        Me.Close()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        'silence = настройки.Document.Element("Setting").Element("Silence").Attribute("value").Value
        'If My.Settings.DShowLIb = True Then
        '    TrackBar1.Maximum = 0
        '    TrackBar1.Minimum = silence
        '    TrackBar1.Value = 0
        '    Громкость.Maximum = 0
        '    Громкость.Minimum = silence
        '    Громкость.Value = 0
        'End If
        'If My.Settings.BassNet = True Then
        '    TrackBar1.Maximum = 100
        '    TrackBar1.Minimum = 0
        '    TrackBar1.Value = 100

        '    Громкость.Maximum = 100
        '    Громкость.Minimum = 0
        '    Громкость.Value = 100
        'End If

        If Изменение = True Then
            If Событие = "Music" Then
                time1.Text = Form3.MusicVolSCH.SelectedItems.Item(0).Text.ToString.Split("-")(0) 'Диапазон времени изменения громкости(начало)
                time2.Text = Form3.MusicVolSCH.SelectedItems.Item(0).Text.ToString.Split("-")(1) 'Диапазон времени изменения громкости (оконание)
                Громкость.Value = Form3.MusicVolSCH.SelectedItems.Item(0).SubItems(1).Text
                TrackBar1.Value = Form3.MusicVolSCH.SelectedItems.Item(0).SubItems(1).Text
            End If
            If Событие = "ADV" Then
                time1.Text = Form3.ADVVolSCH.SelectedItems.Item(0).Text.ToString.Split("-")(0) 'Диапазон времени изменения громкости(начало)
                time2.Text = Form3.ADVVolSCH.SelectedItems.Item(0).Text.ToString.Split("-")(1) 'Диапазон времени изменения громкости (оконание)
                Громкость.Value = Form3.ADVVolSCH.SelectedItems.Item(0).SubItems(1).Text
                TrackBar1.Value = Form3.ADVVolSCH.SelectedItems.Item(0).SubItems(1).Text
            End If
            If Событие = "Alarm" Then
                time1.Text = Form3.AlarmVOLSch.SelectedItems.Item(0).Text.ToString.Split("-")(0) 'Диапазон времени изменения громкости(начало)
                time2.Text = Form3.AlarmVOLSch.SelectedItems.Item(0).Text.ToString.Split("-")(1) 'Диапазон времени изменения громкости (оконание)
                Громкость.Value = Form3.AlarmVOLSch.SelectedItems.Item(0).SubItems(1).Text
                TrackBar1.Value = Form3.AlarmVOLSch.SelectedItems.Item(0).SubItems(1).Text
            End If
            If Событие = "Jingle" Then
                time1.Text = Form3.JingleVOLSch.SelectedItems.Item(0).Text.ToString.Split("-")(0) 'Диапазон времени изменения громкости(начало)
                time2.Text = Form3.JingleVOLSch.SelectedItems.Item(0).Text.ToString.Split("-")(1) 'Диапазон времени изменения громкости (оконание)
                Громкость.Value = Form3.JingleVOLSch.SelectedItems.Item(0).SubItems(1).Text
                TrackBar1.Value = Form3.JingleVOLSch.SelectedItems.Item(0).SubItems(1).Text
            End If
        End If

    End Sub
    Private Sub Громкость_ValueChanged(sender As Object, e As EventArgs) Handles Громкость.ValueChanged
        TrackBar1.Value = Громкость.Value
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Громкость.Value = TrackBar1.Value
    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
End Class