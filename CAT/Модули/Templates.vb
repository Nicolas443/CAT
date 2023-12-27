

Imports System.Text.RegularExpressions
Imports Un4seen.Bass
''' <summary>
''' Читаем список шаблонов и загружаем нужный(в соотвествии с приоритетом)
''' </summary>
Module Templates
#Region "Variables2"
    Public СписокПрофилей As Array
    Public ГрузитьДефолтныйПрофиль As Boolean = True
    Public ggg As Integer
    Public Поток As Integer
    Public НачальнаяГромкость As Double
    Public КонечнаяГромкость As Double
    Public ТекущаяГромкость As Double
    Public ПрофильМеняется As Boolean
    Public НаСкоклькоУбавить As Double
    Public S As Double
    Public P1, P2, P3, P4, P5, P6 As New ArrayList
    Public P11, P22, P33, P44, P55, P66 As New ArrayList
#End Region

    Public Sub GetProfileList()
#Region "Variables1"
        Dim r, rr As Integer
        Dim date1 As Date
        Dim date2 As Date
        Dim y1, m1, d1, t1 As String
        Dim y2, m2, d2, t2 As String
        Dim ProfileName As String
        Dim Логи As String
        Dim max As Integer
        Dim PFName As String
        Dim PF_TimeStart As Integer
        Dim PF_TimeStop As Integer
        Dim cur_time As Integer
        Dim Есть As Boolean
#End Region

        Настройки = XDocument.Load("настройки.xml")
        If System.IO.Directory.Exists("Templates") = True Then
            СписокПрофилей = System.IO.Directory.GetFiles("Templates", "*.profile")
        End If


        Логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value


        '  Form1.ListView2.Items.Clear()
        If СписокПрофилей.Length = 0 Then
            Form1.ListView2.Items.Clear()
        End If
        Dim aaaa As String 
        For r = 0 To СписокПрофилей.Length - 1

            ProfileName = System.IO.Path.GetFileNameWithoutExtension(СписокПрофилей(r))
            For rr = 0 To Form1.ListView2.Items.Count - 1
                If rr <= Form1.ListView2.Items.Count - 1 Then
                    aaaa = Form1.ListView2.Items.Count
                    If Form1.ListView2.Items(rr).Text = ProfileName Then
                        Есть = True
                        ' Exit For
                    End If
                    If System.IO.File.Exists("Templates\" & Form1.ListView2.Items(rr).Text & ".profile") = False Then
                        Есть = True
                        Form1.ListView2.Items(rr).Remove()
                    End If
                End If

            Next
            If Есть = False Then
                Form1.ListView2.Items.Add(ProfileName)
                '  aaaa = Form1.ListView2.Items.Count
            End If
            Есть = False
        Next

        Try
            For r = 0 To СписокПрофилей.Length - 1
                ProfileName = System.IO.Path.GetFileNameWithoutExtension(СписокПрофилей(r).ToString)
                If Regex.IsMatch(ProfileName, "^(20[0-9]{2}[0-1][0-2][0-3]\d_[0-2]\d[0-5]\d)$") Then
                    If P1.Contains(ProfileName) = False Then
                        P1.Add(ProfileName)

                    End If
                End If

                If Regex.IsMatch(ProfileName, "^(20[0-9]{2}[0-1][0-2][0-3]\d_[0-2]\d[0-5]\d_20[0-3]{2}[0-1][0-2][0-3]\d_[0-2]\d[0-5]\d)$") Then
                    If P2.Contains(ProfileName) = False Then
                        P2.Add(ProfileName)
                    End If
                End If
                If Regex.IsMatch(ProfileName, "^([1-7]_[0-2]\d[0-5]\d)$") Then
                    If P3.Contains(ProfileName) = False Then
                        P3.Add(ProfileName)
                    End If
                End If
                If Regex.IsMatch(ProfileName, "^([1-7]_[0-2]\d[0-5]\d_[0-2]\d[0-5]\d)$") Then
                    If P4.Contains(ProfileName) = False Then
                        P4.Add(ProfileName)
                    End If
                End If
                If Regex.IsMatch(ProfileName, "^([0-2][0-9][0-5][0-9])$") Then
                    If P5.Contains(ProfileName) = False Then
                        P5.Add(ProfileName)
                    End If
                End If
                If Regex.IsMatch(ProfileName, "^([0-2][0-9][0-5][0-9]_[0-2][0-9][0-5][0-9])$") Then
                    If P6.Contains(ProfileName) = False Then
                        P6.Add(ProfileName)
                    End If
                End If
            Next



            If P1.Count > 0 Then 'На дату, только начало(конецв 23:59:59)
                For r = 0 To P1.Count - 1
                    y1 = P1(r).Split("_")(0).Substring(0, 4)
                    m1 = P1(r).Split("_")(0).Substring(4, 2)
                    d1 = P1(r).Split("_")(0).Substring(6, 2)
                    t1 = P1(r).Split("_")(1).Substring(0, 2) & ":" & P1(r).Split("_")(1).Substring(2, 2)
                    date1 = CDate(y1 & "." & m1 & "." & d1 & " " & t1)
                    date2 = CDate(y1 & "." & m1 & "." & d1 & " " & "23:59:59")
                    If Now >= date1 And Now <= date2 Then
                        If System.IO.File.Exists("Templates\" & P1(r) & ".profile") = True Then
                            LoadProfile("Templates\" & P1(r) & ".profile")
                            GoTo ПропуститьОстальные
                        Else
                            '   GoTo ПропуститьПрофиль1
                            GoTo ПропуститьОстальные
                        End If
                    End If
                Next
            End If

ПропуститьПрофиль1:

            If P2.Count > 0 Then 'На диапазон дат
                For r = 0 To P2.Count - 1
                    y1 = P2(r).Split("_")(0).Substring(0, 4)
                    m1 = P2(r).Split("_")(0).Substring(4, 2)
                    d1 = P2(r).Split("_")(0).Substring(6, 2)
                    t1 = P2(r).Split("_")(1).Substring(0, 2) & ":" & P2(r).Split("_")(1).Substring(2, 2)

                    y2 = P2(r).Split("_")(2).Substring(0, 4)
                    m2 = P2(r).Split("_")(2).Substring(4, 2)
                    d2 = P2(r).Split("_")(2).Substring(6, 2)
                    t2 = P2(r).Split("_")(3).Substring(0, 2) & ":" & P2(r).Split("_")(3).Substring(2, 2)


                    date1 = CDate(y1 & "." & m1 & "." & d1 & " " & t1)
                    date2 = CDate(y2 & "." & m2 & "." & d2 & " " & t2)
                    If Now >= date1 And Now <= date2 Then
                        If System.IO.File.Exists("Templates\" & P2(r).ToString & ".profile") = True Then
                            LoadProfile("Templates\" & P2(r).ToString & ".profile")
                            GoTo ПропуститьОстальные
                        Else
                            ' GoTo ПропуститьПрофиль2
                            GoTo ПропуститьОстальные
                        End If

                    End If
                Next

            End If

ПропуститьПрофиль2:



            If P3.Count > 0 Then 'На день недели, только начало
                For r = 0 To P3.Count - 1
                    PFName = P3(r).ToString
                    If DatePart(DateInterval.Weekday, Now, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) = PFName.ToString.Split("_")(0) Then
                        P33.Add(PFName)
                        '  Form1.ListView2.Items.Add(PFName)
                    End If
                Next

                If P33.Count > 0 Then
                    Dim u As String = P33(0).ToString.Split("_")(1)
                    max = u.Substring(0, 2) * 3600 + u.Substring(2, 2) * 60
                    PFName = P33(0).ToString
                    For r = 0 To P33.Count - 1
                        u = P33(r).ToString.Split("_")(1)
                        If (u.Substring(0, 2) * 3600 + u.Substring(2, 2) * 60) > max Then
                            max = u.Substring(0, 2) * 3600 + u.Substring(2, 2) * 60
                            PFName = P33(r).ToString

                        End If

                    Next


                    If DatePart(DateInterval.Weekday, Now, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) = PFName.ToString.Split("_")(0) Then
                        PF_TimeStart = PFName.Split("_")(1).Substring(0, 2) * 3600 + PFName.Split("_")(1).Substring(2, 2) * 60
                        If (Now.Hour * 3600 + Now.Minute * 60) >= PF_TimeStart And (Now.Hour * 3600 + Now.Minute * 60) < 86360 Then
                            If System.IO.File.Exists("Templates\" & PFName & ".profile") = True Then
                                LoadProfile("Templates\" & PFName & ".profile")
                                GoTo ПропуститьОстальные
                            Else
                                '  GoTo ПропуститьПрофиль3
                                GoTo ПропуститьОстальные
                            End If
                        End If
                    End If
                End If
            End If


ПропуститьПрофиль3:


            If P4.Count > 0 Then 'На день недели, начало и конец
                For r = 0 To P4.Count - 1
                    PFName = P4(r).ToString
                    If DatePart(DateInterval.Weekday, Now, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) = PFName.ToString.Split("_")(0) Then
                        P44.Add(PFName)
                        '  Form1.ListView2.Items.Add(PFName)
                    End If
                Next
                If P44.Count > 0 Then
                    For r = 0 To P44.Count - 1
                        PF_TimeStart = P44(r).Split("_")(1).Substring(0, 2) * 3600 + P44(r).Split("_")(1).Substring(2, 2) * 60
                        PF_TimeStop = P44(r).Split("_")(2).Substring(0, 2) * 3600 + P44(r).Split("_")(2).Substring(2, 2) * 60
                        If DatePart(DateInterval.Weekday, Now, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) = P44(r).ToString.Split("_")(0) Then
                            If (Now.Hour * 3600 + Now.Minute * 60) >= PF_TimeStart And (Now.Hour * 3600 + Now.Minute * 60) < PF_TimeStop Then
                                If System.IO.File.Exists("Templates\" & P44(r) & ".profile") = True Then
                                    LoadProfile("Templates\" & P44(r) & ".profile")
                                    '  GoTo ПропуститьПрофиль4
                                    GoTo ПропуститьОстальные
                                Else
                                    GoTo ПропуститьОстальные

                                End If
                            End If
                        End If
                    Next
                End If
            End If

ПропуститьПрофиль4:

            If P5.Count > 0 Then

                PFName = P5(0).ToString
                max = PFName.Substring(0, 2) * 3600 + PFName.Substring(2, 2) * 60
                For r = 0 To P5.Count - 1
                    If P5(r).Substring(0, 2) * 3600 + P5(r).Substring(2, 2) * 60 > max Then
                        max = P5(r).Substring(0, 2) * 3600 + P5(r).Substring(2, 2) * 60
                        PFName = P5(r).ToString
                    End If

                Next
                '  PFName = P5(r).ToString
                PF_TimeStart = PFName.Substring(0, 2) * 3600 + PFName.Substring(2, 2) * 60
                PF_TimeStop = 23 * 3600 + 59 * 60 + 59
                cur_time = Now.Hour * 3600 + Now.Minute * 60 + Now.Second
                If cur_time >= PF_TimeStart And cur_time <= PF_TimeStop Then
                    If System.IO.File.Exists("Templates\" & PFName & ".profile") = True Then
                        LoadProfile("Templates\" & PFName & ".profile")
                        '  GoTo ПропуститьПрофиль5
                        GoTo ПропуститьОстальные
                    Else
                        GoTo ПропуститьОстальные
                    End If
                End If


            End If

ПропуститьПрофиль5:

            If P6.Count > 0 Then
                For r = 0 To P6.Count - 1
                    PFName = P6(r).ToString
                    PF_TimeStart = PFName.ToString.Split("_")(0).Substring(0, 2) * 3600 + PFName.ToString.Split("_")(0).Substring(2, 2) * 60
                    PF_TimeStop = PFName.ToString.Split("_")(1).Substring(0, 2) * 3600 + PFName.ToString.Split("_")(1).Substring(2, 2) * 60
                    cur_time = Now.Hour * 3600 + Now.Minute * 60 + Now.Second
                    If cur_time >= PF_TimeStart And cur_time <= PF_TimeStop Then
                        If System.IO.File.Exists("Templates\" & P6(r) & ".profile") = True Then
                            LoadProfile("Templates\" & P6(r) & ".profile")
                            GoTo ПропуститьОстальные
                        Else
                            GoTo ПропуститьОстальные
                        End If
                    End If
                Next

            End If
        Catch ex As Exception
            ГрузитьДефолтныйПрофиль=True 
        End Try






ПропуститьОстальные:




        If ГрузитьДефолтныйПрофиль = True Then
            Form1.Label5.Text = "настройки.xml"
            ' Form1.Поток()
            If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value <> "настройки.xml" Then
                Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("CurentProfile", "настройки.xml")

                Настройки.Save("настройки.xml")

                If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("ProfileLoading") = "0" Then
                    Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("ProfileLoading", "1")
                    Настройки.Save("настройки.xml")
                    Form1.MusicList.Items.Clear()
                    Form1.ListBox1 .Items .Clear 
                    If Play.stream_music <> 0 Then
                        Поток = Play.stream_music
                    End If
                    If Play.stream2 <> 0 Then
                        Поток = Play.stream2
                    End If

                    If Play.ВремяМузыки = True Then
                        Form1.musicfadeout.Start()
                    End If
                    If Play.ВремяADV = True Then
                        VolumeSet.VolumeSet("ADV")
                    End If

                    If Play.ВремяAlarm = True Then
                        VolumeSet.VolumeSet("Alarm")
                    End If
                    If System.IO.Directory.Exists(Логи) = True Then
                        System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".txt", Now & " Загружен профиль " & "настройки.xml" & vbNewLine, System.Text.Encoding.Default)
                    End If
                    Form1.Поток()
                    Form1.Поток_DinSong()
                    Form1.musicfadeout.Start()
                End If
            End If


        End If
        ГрузитьДефолтныйПрофиль = True
        P1.Clear()
        P2.Clear()
        P3.Clear()
        P4.Clear()
        P5.Clear()
        P6.Clear()


        P11.Clear()
        P22.Clear()
        P33.Clear()
        P44.Clear()
        P55.Clear()
        P66.Clear()
    End Sub


    Public Sub ProfileVolumeSet()

        Dim Поток As Integer
        If stream_music <> 0 Then
            Поток = stream_music
        End If
        If stream2 <> 0 Then
            Поток = stream2
        End If

        '  If stream_music <> 0 Then
        Bass.BASS_ChannelGetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, ТекущаяГромкость)
        If КонечнаяГромкость < НачальнаяГромкость Then
            If Math.Round(ТекущаяГромкость, 2) > Math.Round(КонечнаяГромкость, 2) Then
                Bass.BASS_ChannelSetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, Math.Round(ТекущаяГромкость, 4) - Math.Round(НаСкоклькоУбавить / 20, 4))
            End If
        End If
        If КонечнаяГромкость > НачальнаяГромкость Then
            If Math.Round(ТекущаяГромкость, 2) <= Math.Round(КонечнаяГромкость, 2) Then
                Bass.BASS_ChannelSetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, Math.Round(ТекущаяГромкость, 4) - Math.Round(НаСкоклькоУбавить / 20, 4))
            End If
        End If


        'End If
        'If stream2 <> 0 Then
        '    Bass.BASS_ChannelGetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, ТекущаяГромкость)
        '    Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, Math.Round(ТекущаяГромкость, 2) - НаСкоклькоУбавить / 20)
        'End If

        If КонечнаяГромкость = НачальнаяГромкость Then
            Form1.ProfileVolumeTimer.Stop()
        End If
        If КонечнаяГромкость > НачальнаяГромкость Then
            If Math.Round(ТекущаяГромкость, 4) >= Math.Round(КонечнаяГромкость, 4) Then
                Form1.ProfileVolumeTimer.Stop()
            End If
        End If

        If КонечнаяГромкость < НачальнаяГромкость Then
            If Math.Round(ТекущаяГромкость, 4) <= Math.Round(КонечнаяГромкость, 4) Then
                Form1.ProfileVolumeTimer.Stop()
                If КонечнаяГромкость = 0.4 Then

                    Form1.Стоп()
                    'Bass.BASS_SampleFree(Play.stream_music)
                    'Bass.BASS_SampleFree(Play.stream2)

                    'Bass.BASS_ChannelFree(Play.stream_music)
                    'Bass.BASS_ChannelFree(Play.stream2)

                    'Bass.BASS_ChannelStop(Play.stream_music)
                    'Bass.BASS_ChannelStop(Play.stream2)

                    'stream_music = 0
                    'stream2 = 0

                    'Form1.Text = "CAT"
                    'Form1.Label1.Text = "0"

                    'Form1.VolumeCheck.Start()
                    ' Bass.BASS_Stop()
                    ' Bass.BASS_Free()
                End If
            End If

        End If
        '   ggg += 1
        'If stream_music <> 0 Then
        '    Bass.BASS_ChannelGetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, ТекущаяГромкость)
        '    Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, Math.Round(ТекущаяГромкость, 2) - НаСкоклькоУбавить / 20)
        'End If
        'If stream2 <> 0 Then
        '    Bass.BASS_ChannelGetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, ТекущаяГромкость)
        '    Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, Math.Round(ТекущаяГромкость, 2) - НаСкоклькоУбавить / 20)
        'End If




        '  Bass_Stop()
        ' stream_music = 0
        ' stream2 = 0
        ' Поток = 0

        'If Событие = "Alarm" Then
        '    Объявление_Играется = True
        '    alarm_file = Get_First_Alarm_File_Name()
        '    Play("Alarm")
        'End If
        'If Событие = "ADV" Then
        '    Form1.UpdateBase(dbpatch, LastId)
        '    Реклама_Играется = True
        '    Play("ADV")
        'End If
        'If Событие = "Music" Then
        '    Play("Music")
        'End If
        '  ДелатьKTO = False
        'Form1.musicfadeout.Stop()
        'дНачался = False

        '  hr = Bass.BASS_ChannelSetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, cur_volume - НачальнаяГромкость / (MusicSetting.FadeOutTime / FO_Timer.Interval))

    End Sub
    ''' <summary>
    ''' Загружаем профиль
    ''' </summary>
    ''' <returns></returns>
    Public Function Load() As XDocument
        Dim Profile As XDocument
        Dim Настройки As XDocument
        Настройки = XDocument.Load("настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Return Profile
    End Function

    Public Sub Save()
        Dim Profile As XDocument
        Dim Настройки As XDocument
        Настройки = XDocument.Load("настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value


        Profile.Save(ProFileName)
    End Sub


    Public Sub LoadProfile(ByVal PFName As String)

        ГрузитьДефолтныйПрофиль = False
        ' Play.ПесняНаПаузе = False



        If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value <> PFName Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("CurentProfile", PFName)
            Настройки.Save("настройки.xml")
            Play.ПесняНаПаузе = False

            '  If Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("ProfileLoading") = "0" Then
            Настройки.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("ProfileLoading", "0")
            Настройки.Save("настройки.xml")
            Form1.MusicList.Items.Clear()
            Form1.ListBox1.Items.Clear()

            Form1.Поток()
            Form1.Поток_DinSong()
            ' Form1.Media_info.Start()
            If Play.ВремяМузыки = True Then
                ' Bass.BASS_ChannelGetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)

                ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
                'If System.IO.File.Exists(ProFileName) = True Then
                '    Profile = XDocument.Load(ProFileName)
                'Else
                '    Profile = XDocument.Load("настройки.xml")
                'End If
                Form1.musicfadeout.Start()
            End If
            If Play.ВремяADV = True Then
                VolumeSet.VolumeSet("ADV")

            End If

            If Play.ВремяAlarm = True Then
                VolumeSet.VolumeSet("Alarm")

            End If
            'If System.IO.Directory.Exists(Логи) = True Then
            '    System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".txt", Now & " Загружен профиль " & P2(r).ToString & vbNewLine, System.Text.Encoding.Default)
            'End If
        End If
        Form1.Label5.Text = System.IO.Path.GetFileNameWithoutExtension(PFName)




    End Sub

    Public Sub УдалитьПрофиль()

        If MsgBox("Хотите удалить профиль " & Form1.ListView2.FocusedItem.Text & " ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                System.IO.File.Delete("Templates\" & Form1.ListView2.Items(Form1.ListView2.FocusedItem.Index).Text & ".profile")
            Catch ex As Exception
                MsgBox("Ошибка удаления", MsgBoxStyle.Critical)
            End Try


        End If

    End Sub
End Module
