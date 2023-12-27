Imports Un4seen.Bass

Module CheckSetting
    Public БылКлик As Boolean
    Public Sub Check()
        Dim xe As XElement
        Dim AudioDev As New ArrayList

        If System.IO.Directory.Exists("Templates") = False Then
            System.IO.Directory.CreateDirectory("Templates")
        End If



        If System.IO.File.Exists("Настройки.xml") = False Then
            Настройки = New XDocument
            Настройки = <?xml version="1.0"?>
                        <Setting>
                        </Setting>

            xe = New XElement("adv", "")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("volume", "0")
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("dir", "")
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("audio_dev", "")
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("MinTimeDist", "2")

            xe = New XElement("sch", "")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("sch").SetAttributeValue("dir", "")

            xe = New XElement("news", "")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("news").SetAttributeValue("dir", "")
            Настройки.Document.Element("Setting").Element("news").SetAttributeValue("volume", "0")
            Настройки.Document.Element("Setting").Element("news").SetAttributeValue("audio_dev", "")

            xe = New XElement("Domen", "")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("Domen").SetAttributeValue("Name", "")
            Настройки.Document.Element("Setting").Element("Domen").SetAttributeValue("SubDomen", "")
            Настройки.Document.Element("Setting").Element("Domen").SetAttributeValue("serial", "")

            xe = New XElement("logs", "")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("logs").SetAttributeValue("dir", "")

            xe = New XElement("db", "")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("db").SetAttributeValue("file", "")

            xe = New XElement("alarm")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", "0")
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("dir", "")
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("dir", "")
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("audio_dev", "")
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("ClearDir", False)
            'Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("Jingle", "")
            'Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume_jingle", "1")

            xe = New XElement("Jingle")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("Jingle").SetAttributeValue("Jingle", "")
            Настройки.Document.Element("Setting").Element("Jingle").SetAttributeValue("volume", "100")
            Настройки.Document.Element("Setting").Element("Jingle").SetAttributeValue("Use", "False")
            xe = New XElement("volume_sch")
            Настройки.Element("Setting").Element("Jingle").Add(xe)



            xe = New XElement("music")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("volume", "0")
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("audio_dev", "")
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("killtimeout", "0")
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("PlayListTimeCheck", "0")
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("CrossFade", "0")
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("CrossFade", "0")

            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("GenNewMusList", "False")
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("TimeGenNewMusPL", "00:00:00")

            xe = New XElement("main_dir")
            Настройки.Element("Setting").Element("music").Add(xe)

            xe = New XElement("dyn_dir")
            Настройки.Element("Setting").Element("music").Add(xe)

            xe = New XElement("dyn_dir_2")
            Настройки.Element("Setting").Element("music").Add(xe)

            xe = New XElement("Mix")
            Настройки.Element("Setting").Element("music").Add(xe)
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("FadeOutTime", "5000")
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseFadeOut", "0")
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("CrossFadeTime", "5000")
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseCrossFade", "0")


            xe = New XElement("volume_sch")
            Настройки.Element("Setting").Element("music").Add(xe)

            xe = New XElement("music_to_music_fade")
            Настройки.Element("Setting").Element("music").Add(xe)
            Настройки.Document.Element("Setting").Element("music").Element("music_to_music_fade").SetAttributeValue("value", "4000")

            xe = New XElement("music_to_adv_fade")
            Настройки.Element("Setting").Element("music").Add(xe)
            Настройки.Document.Element("Setting").Element("music").Element("music_to_adv_fade").SetAttributeValue("value", "4000")

            xe = New XElement("Telegram")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("Telegram").SetAttributeValue("Token", "")
            Настройки.Document.Element("Setting").Element("Telegram").SetAttributeValue("ChatID", "")

            xe = New XElement("timeoff")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Start", "23:59:00")
            Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Stop", "00:01:00")

            xe = New XElement("Silence")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("Silence").SetAttributeValue("value", "0")

            'xe = New XElement("MouseMove")
            'Настройки.Element("Setting").Add(xe)
            'Настройки.Document.Element("Setting").Element("MouseMove").SetAttributeValue("Value", "0")


            xe = New XElement("GeneralSetting")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("RealTimeVolumeRegulation", "true")
            ' Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("Regulation", "Log")
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("PlayingMode", "Stereo")

            Настройки.Save("Настройки.xml")

        End If
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        '//////////////////////ADV/////////////////////////
        If Настройки.Document.Element("Setting").Element("adv").Attribute("MinTimeDist") Is Nothing Then
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("MinTimeDist", "2")
        End If
        'If Настройки.Document.Element("Setting").Element("adv").Attribute("SilenceLevel") Is Nothing Then
        '    Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("SilenceLevel", "0")
        'End If
        If Настройки.Document.Element("Setting").Element("adv").Element("volume_sch") Is Nothing Then
            xe = New XElement("volume_sch")
            Настройки.Element("Setting").Element("adv").Add(xe)
        End If
        If Настройки.Document.Element("Setting").Element("adv").Attribute("RealTimeVolumeRegulation") Is Nothing Then
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("RealTimeVolumeRegulation", "false")
        End If
        If Настройки.Document.Element("Setting").Element("adv").Attribute("volume").Value < 0 Then
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("volume", "100")
        End If
        If Настройки.Document.Element("Setting").Element("adv").Attribute("LostADVTime") Is Nothing Then
            Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("LostADVTime", "0")
        End If


        '///////////////////////Music//////////////////
        If Настройки.Document.Element("Setting").Element("music").Attribute("mode") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("mode", "broadcast")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("OpenLastPL") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("OpenLastPL", "false")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("GenMode") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("GenMode", "PercentMode")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("SilenceLevel") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("SilenceLevel", "0")
        End If
        If Настройки.Document.Element("Setting").Element("alarm").Attribute("RealTimeVolumeRegulation") Is Nothing Then
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("RealTimeVolumeRegulation", "false")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("RealTimeVolumeRegulation") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("RealTimeVolumeRegulation", "false")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("OpenLastPL") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("OpenLastPL", "false")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("mode") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("mode", "broadcast")
        End If



        If Настройки.Document.Element("Setting").Element("alarm").Attribute("UseJingle") Is Nothing Then
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("UseJingle", "1")
        End If



        If Настройки.Document.Element("Setting").Element("alarm").Attribute("ClearDir") Is Nothing Then
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("ClearDir", False)
        End If

        If Настройки.Document.Element("Setting").Element("music").Element("Mix") Is Nothing Then
            xe = New XElement("Mix")
            Настройки.Element("Setting").Element("music").Add(xe)
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("FadeOutTime", "5000")
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseFadeOut", "0")
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("CrossFadeTime", "5000")
            Настройки.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseCrossFade", "0")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("CheckRepeat") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("CheckRepeat", "1")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("CkeckAutor") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("CkeckAutor", "false")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("AutorTimeCheck") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("AutorTimeCheck", "0")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("Разделитель") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("Разделитель", "^")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("volume").Value < 0 Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("volume", "100")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("GenNewMusList") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("GenNewMusList", "False")
        End If
        If Настройки.Document.Element("Setting").Element("music").Attribute("TimeGenNewMusPL") Is Nothing Then
            Настройки.Document.Element("Setting").Element("music").SetAttributeValue("TimeGenNewMusPL", "00:00:00")
        End If

        '////////////////////Alarm//////////////////

        'If Настройки.Document.Element("Setting").Element("alarm").Attribute("SilenceLevel") Is Nothing Then
        '    Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("SilenceLevel", "0")
        'End If
        If Настройки.Document.Element("Setting").Element("alarm").Element("volume_sch") Is Nothing Then
            xe = New XElement("volume_sch")
            Настройки.Element("Setting").Element("alarm").Add(xe)
            '  Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("RealTimeVolumeRegulation", "true")
        End If
        If Настройки.Document.Element("Setting").Element("alarm") Is Nothing Then
            xe = New XElement("alarm", "")
            Настройки.Document.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", "100")
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("dir", Application.StartupPath & "\" & "alarm")
            If System.IO.Directory.Exists(Application.StartupPath & "\" & "alarm") = False Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\" & "alarm")
            End If
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("audio_dev", AudioDev(0))
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("Jingle", "Не выбрано")
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume_jingle", "0")
        End If

        If Настройки.Document.Element("Setting").Element("alarm").Attribute("volume").Value < 0 Then
            Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", "100")
        End If

        If Настройки.Document.Element("Setting").Element("Jingle") Is Nothing Then
            xe = New XElement("Jingle", "")
            Настройки.Document.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("Jingle").SetAttributeValue("Jingle", "")
            Настройки.Document.Element("Setting").Element("Jingle").SetAttributeValue("volume", "100")
            Настройки.Document.Element("Setting").Element("Jingle").SetAttributeValue("Use", False)

            xe = New XElement("volume_sch")
            Настройки.Element("Setting").Element("Jingle").Add(xe)

        End If



        '////////////////////////GeneralSetting/////////////////////////////
        If Настройки.Document.Element("Setting").Element("GeneralSetting") Is Nothing Then
            xe = New XElement("GeneralSetting")
            Настройки.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("RealTimeVolumeRegulation", "true")

        End If
        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("CurentProfile", "настройки.xml")
        End If
        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("MusicPause") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("MusicPause", False)
        End If
        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("ProfileLoading") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("ProfileLoading", "1")
        End If
        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("Regulation", "Log")
        End If
        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("StartNewTrack", False)
        End If
        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("NewTrackTime", "0")
        End If

        'If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("SilenceLevel") Is Nothing Then
        '    Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("SilenceLevel", "0")
        'End If


        '//////////////////Telegram////////////////
        If Настройки.Document.Element("Setting").Element("Telegram") Is Nothing Then
            xe = New XElement("Telegram", "")
            Настройки.Document.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("Telegram").SetAttributeValue("Token", "")
            Настройки.Document.Element("Setting").Element("Telegram").SetAttributeValue("ChatID", "")
        End If

        '////////////////////timeoff////////////

        If Настройки.Document.Element("Setting").Element("timeoff") Is Nothing Then
            xe = New XElement("timeoff", "")
            Настройки.Document.Element("Setting").Add(xe)
            Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Start", "23:59:00")
            Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Stop", "00:01:00")
        End If

        '///////////////Silence////////////
        If Настройки.Document.Element("Setting").Element("Silence").Attribute("Value") Is Nothing Then
            Настройки.Document.Element("Setting").Element("Silence").SetAttributeValue("Value", "100")
        End If



        For Each elem In Настройки.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            If elem.Value.Split("|")(1) < 0 Then
                elem.Value = elem.Value.Split("|")(0) & "|110"
            End If
        Next

        For Each elem In Настройки.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            If elem.Value.Split("|")(1) < 0 Then
                elem.Value = elem.Value.Split("|")(0) & "|100"
            End If
        Next

        For Each elem In Настройки.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            If elem.Value.Split("|")(1) < 0 Then
                elem.Value = elem.Value.Split("|")(0) & "|100"
            End If
        Next
        For Each elem In Настройки.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes
            If elem.Value.Split("|")(1) < 0 Then
                elem.Value = elem.Value.Split("|")(0) & "|100"
            End If
        Next

        If Настройки.Document.Element("Setting").Element("Silence").Attribute("Value").Value < 0 Then
            Настройки.Document.Element("Setting").Element("Silence").SetAttributeValue("Value", "100")
        End If

        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("PLayingMode") Is Nothing Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("PLayingMode", "Stereo")
        End If


        If Настройки.Document.Element("Setting").Element("music").Element("dyn_dir_2") Is Nothing Then
            xe = New XElement("dyn_dir_2")
            Настройки.Document.Element("Setting").Element("music").Add(xe)
        End If



        Настройки.Save("Настройки.xml")
    End Sub


    Public Sub ChangeRegMetod()
        Dim Profile As XDocument
        Dim ГромкостьПоУмолчнию As String = ""
        Dim min As Integer
        Dim max As Integer
        Dim value As Integer

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        If БылКлик = False Then
            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                min = 0
                max = 100

            End If
            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                min = -60
                max = 0

            End If

        End If


        If БылКлик = True Then
            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                '   Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("Regulation", "Line")
                ГромкостьПоУмолчнию = "100"
                min = 0
                max = 100


            End If
            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                ' Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("Regulation", "Log")
                ГромкостьПоУмолчнию = "0"
                min = -60
                max = 0

            End If


            For Each times In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
                times.Value = times.Value.Split("|")(0) & "|" & ГромкостьПоУмолчнию
            Next
            For Each times In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
                times.Value = times.Value.Split("|")(0) & "|" & ГромкостьПоУмолчнию
            Next
            For Each times In Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes
                times.Value = times.Value.Split("|")(0) & "|" & ГромкостьПоУмолчнию
            Next
            For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
                times.Value = times.Value.Split("|")(0) & "|" & ГромкостьПоУмолчнию
            Next

            Profile.Document.Element("Setting").Element("adv").SetAttributeValue("volume", ГромкостьПоУмолчнию)
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("volume", ГромкостьПоУмолчнию)
            Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", ГромкостьПоУмолчнию)
            Profile.Document.Element("Setting").Element("Jingle").SetAttributeValue("volume", ГромкостьПоУмолчнию)
        End If

        ' Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, False)

        Form3.ГромкостьМузыки.Minimum = min
        Form3.ГромкостьМузыки.Maximum = max
        ' Form3.ГромкостьМузыки.Value = value



        setting.MusicTestVolume.Minimum = min
        setting.MusicTestVolume.Maximum = max

        setting.TrackBar4.Minimum = min
        setting.TrackBar4.Maximum = max


            setting.ADVTestVolume.Minimum = min
            setting.ADVTestVolume.Maximum = max

            setting.TrackBar5.Minimum = min
            setting.TrackBar5.Maximum = max

            setting.AlarmTextVolume.Minimum = min
            setting.AlarmTextVolume.Maximum = max

            setting.TrackBar6.Minimum = min
            setting.TrackBar6.Maximum = max




        Form3.ГромкостьМузыки.Maximum = max
        Form3.ГромкостьМузыки.Minimum = min


        Form3.ГромкостьРекламы.Maximum = max
        Form3.ГромкостьРекламы.Minimum = min

        Form3.MusicDefaultVolume.Maximum = max
        Form3.MusicDefaultVolume.Minimum = min
        Form3.JingleDefaultVolume.Maximum = max
        Form3.JingleDefaultVolume.Minimum = min

        Form3.AlarmDefaultVolume.Maximum = max
        Form3.AlarmDefaultVolume.Minimum = min

        Form3.AlarmVolume.Minimum = min
        Form3.AlarmVolume.Maximum = max



        Form3.ADVDefaultVolume.Maximum = max
        Form3.ADVDefaultVolume.Minimum = min



        Form3.TrackBar4.Maximum = max
        Form3.TrackBar4.Minimum = min

        Form3.TrackBar5.Maximum = max
        Form3.TrackBar5.Minimum = min

        Form3.TrackBar6.Maximum = max
        Form3.TrackBar6.Minimum = min

        Form3.TrackBar1.Maximum = max
        Form3.TrackBar1.Minimum = min

        Form3.TrackBar7.Maximum = max
        Form3.TrackBar7.Minimum = min

        Form3.TrackBar2.Maximum = max
        Form3.TrackBar2.Minimum = min

        Form3.TrackBar3.Maximum = max
        Form3.TrackBar3.Minimum = min

        Form3.TrackBar8.Maximum = max
        Form3.TrackBar8.Minimum = min

        Form2.Громкость.Minimum = min
        Form2.Громкость.Maximum = max
        Form2.TrackBar1.Minimum = min
        Form2.TrackBar1.Maximum = max

        Profile.Save(ProFileName)


            БылКлик = False
    End Sub


End Module
