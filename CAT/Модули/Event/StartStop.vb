

Imports Un4seen.Bass

Module StartStop
    Public ВремяТИшины As Boolean
    Public Sub StartStop()
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'Dim time As String
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim time_s_3 As Integer
        Dim cur_time_s As Integer
        Dim ProFileName As String
        Dim Profile As XDocument
        Dim dbpatch As String

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Dim s1 As String = Profile.Document.Element("Setting").Element("timeoff").Attribute("Start").Value
        Dim s2 As String = Profile.Document.Element("Setting").Element("timeoff").Attribute("Stop").Value

        Dim SP_Music As String
        Dim SP_Music2 As String
        Dim SP_ADV As String
        Dim SP_Alarm As String
        Dim Diff As Integer
        Dim Время As String
        Dim Время1, Время2 As String
        Dim ТекВремяСек As Integer

        time_s_1 = s1.ToString.Split(":")(0) * 3600 + s1.ToString.Split(":")(1) * 60 + s1.ToString.Split(":")(2)
        time_s_2 = s2.ToString.Split(":")(0) * 3600 + s2.ToString.Split(":")(1) * 60 + s2.ToString.Split(":")(2)
        time_s_3 = 23 * 3600 + 59 * 60 + 59 'Время конца дня



        cur_time_s = Now.Hour * 3600 + Now.Minute * 60 + Now.Second


        SP_Music = Bass.BASS_ChannelIsActive(Play.stream_music)
        SP_Music2 = Bass.BASS_ChannelIsActive(Play.stream2)
        SP_ADV = Bass.BASS_ChannelIsActive(Play.stream_adv)
        SP_Alarm = Bass.BASS_ChannelIsActive(Play.stream_alarm)

        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value

        If Play.ВремяМузыки = True Then
            For Each values In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
                Время = values.Value.Split("|")(0)
                ТекВремяСек = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
                Время1 = Время.Split("-")(0).Split(":")(0) * 3600 + Время.Split("-")(0).Split(":")(1) * 60 + Время.Split("-")(0).Split(":")(2)
                Время2 = Время.Split("-")(1).Split(":")(0) * 3600 + Время.Split("-")(1).Split(":")(1) * 60 + Время.Split("-")(1).Split(":")(2)
                If ТекВремяСек >= Время1 And ТекВремяСек < Время2 Then
                    If values.Value.Split("|")(1) = 0 Then

                        Bass.BASS_SampleFree(Play.stream_music)
                        Bass.BASS_SampleFree(Play.stream2)

                        Bass.BASS_ChannelFree(Play.stream_music)
                        Bass.BASS_ChannelFree(Play.stream2)

                        Bass.BASS_ChannelStop(Play.stream_music)
                        Bass.BASS_ChannelStop(Play.stream2)

                        Play.stream_music = 0
                        Play.stream2 = 0
                        Form1.Text = "CAT"
                        Form1.Label1.Text = "0"
                        Bass.BASS_Stop()
                        Bass.BASS_Free()
                        ВремяТИшины = True
                        DataBase.UpdateDB(dbpatch, Play.LastMusicID)
                        GoTo Конец

                    End If
                End If

            Next
        End If




        If DateDiff(DateInterval.Second, CDate(s1), CDate(s2)) < 0 Then 'Вычисляем Дату и время тишины
            Diff = DateDiff(DateInterval.Second, CDate(s1), CDate("23:59:59")) + DateDiff(DateInterval.Second, CDate("00:00:00"), CDate(s2)) + 1
            If cur_time_s >= time_s_1 Or cur_time_s <= time_s_2 Then
                'Bass.BASS_ChannelStop(Play.stream_music)
                'Bass.BASS_ChannelStop(Play.stream2)
                'Play.stream_music = 0
                'Play.stream2 = 0


                If Play.ВремяМузыки = True Then
                    If Play.ФейдНачался = False Then
                        DataBase.UpdateDB(dbpatch, Play.LastMusicID)
                        If ВремяТИшины = False Then
                            ВремяТИшины = True
                            Form1.musicfadeout.Start()
                            Play.ФейдНачался = True
                        End If

                    End If

                End If
                If SP_ADV = "0" And SP_Alarm = "0" Then
                    Form1.ProgressBar1.Value = 0
                    Form1.Label1.Text = "0"
                End If
            End If

            If cur_time_s > time_s_2 And cur_time_s < time_s_1 Then
                If SP_Music = "0" And SP_Music2 = "0" Then
                    If Play.ВремяADV <> True And Play.ВремяAlarm <> True Then
                        If Play.Пауза = False Then
                            ВремяТИшины = False
                            Play.Play("Music")
                            ' MediaInfo.Play("Music", "Music")
                        End If

                    End If
                End If
            End If

        End If


        If DateDiff(DateInterval.Second, CDate(s1), CDate(s2)) > 0 Then
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then 'Останавливаем музыку согласно планировщику


                If Play.ВремяМузыки = True Then
                    'Bass.BASS_ChannelStop(Play.stream_music)
                    'Bass.BASS_ChannelStop(Play.stream2)
                    'Play.stream_music = 0
                    'Play.stream2 = 0
                    DataBase.UpdateDB(dbpatch, Play.LastMusicID)
                    If Play.ФейдНачался = False Then
                        If ВремяТИшины = False Then
                            ВремяТИшины = True
                            Form1.musicfadeout.Start()
                            Play.ФейдНачался = True
                        End If

                    End If

                    If SP_ADV = "0" Then
                        Form1.ProgressBar1.Value = 0
                        Form1.Label1.Text = "0"
                    End If
                    Form1.Text = "CAT"
                End If



            Else
                If SP_Music = "0" And SP_Music2 = "0" Then
                    If Play.ВремяADV <> True And Play.ВремяAlarm <> True Then
                        If Play.Пауза = False Then
                            ВремяТИшины = False
                            Play.Play("Music")

                        End If

                    End If
                End If
            End If
        End If

Конец:
    End Sub

End Module
