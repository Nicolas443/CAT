
Imports Un4seen.Bass
Imports Un4seen.Bass.AddOn.Mix
Imports Un4seen.Bass.Misc
Imports System.Data.SQLite
Module Play
#Region "Variables4"
    Public play_file_name As String
    '  Dim CrossFadeTime As Integer

    Dim Настройки As System.Xml.Linq.XDocument
    Public Хронометраж As Double
    Public ТекущаяПозиция As Double
    Public Хронометраж_byte As Double
    Public ПоследняяПесня As String
    Public ПесняНаПаузе As Boolean
    Public LastPos As Double
    Public LastHandlle As Integer
    Public ПесняПодготовлена As Boolean
    Public Шаг As Double
    Dim db_param As ArrayList
    Public ИграетДинПесня As Boolean
    Public СтавитьНаПаузу As Boolean

    Dim Остаток As Double

    Public db_th1 As System.Threading.Thread
    Public db_th2 As System.Threading.Thread

    Public ТекущаяПозиция_byte As Double

    Public ОсновнойПоток As Integer

    Public ФактическоеВремяНачалаРекламы As Date
    Public ФактическоеВремяНачалаРекламы_sec As Double

    Public Хронометраж2 As Double
    Public ТекущаяПозиция2 As Double
    Public Хронометраж_byte2 As Double
    Public ТекущаяПозиция_byte2 As Double

    Public КроссФейдНачался As Boolean
    Dim ШагКросФейда As Integer

    Public cur_vol As Double
    Public cur_vol_byte As Double
    Public cur_vol2 As Double
    Public cur_vol2_byte As Double

    Public ШагИзмененияГромкости As Double
    Public КоличествоШаговИзмененияГромкости As Integer
    Public НачальнаяГромкость As Double
    Public НачальнаяГромкость_byte As Double
    Public t As Double
    Public ВремяМузыки As Boolean
    Public ВремяADV As Boolean
    Public ВремяAlarm As Boolean
    Public ВремяJingle As Boolean
    Public ДелатьKillTimeOut As Boolean
    Public time_of_alarm As String
    Public alarm_file As String
    Dim KT_Count As Integer
    Public Объявление_Играется As Boolean
    Public Реклама_Играется As Boolean
    Public JingleИграется As Boolean
    Public НомерОбъявления As Integer
    Public Jingle As String
    Public ФейдНачался As Boolean
    Public Пауза As Boolean
    Public stream As Integer
    Public stream_adv As Integer
    Public stream_alarm As Integer
    Public stream_music As Integer
    Public stream_music_2 As Integer
    Public БлокОбъявленийИграется As Boolean
    Public ОбъявлениеПослеADV As Boolean
    Public НачальныйОстатокГромкости As Double
    Public stream2 As Integer
    Public ПредыдущаяПозиция As Double
    Public ДелатьKTO As Boolean
    Public НеЗапланированное As Boolean
    Public ВсёОстановлено As Boolean
    Public LastId, LastMusicID, LastADVId, LastAlarmId As Integer
    Public LastStreaam As String
    Public ПесняИзДинДир As Boolean
    Public ПесняИзСтатДир As Boolean
    Public ДинДирекорияОстановлена As Boolean
    Public ДинПесня As String
#End Region

    Public Sub Play(ByVal Событие As String)
#Region "Variables"
        Dim dev_number As Integer
        Dim rnd As New Random
        Dim логи As String
        Dim dbpatch As String
        Dim Домен As String
        Dim hr As Integer
        Dim Profile As XDocument
        Dim time_dir_start As Date
        Dim time_dir_stop As Date

#End Region

        Profile = GenerlaSetting.ProfileLoad

        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
        Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Profile.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
        логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value


        Bass.BASS_ChannelStop(stream_adv)
        Bass.BASS_StreamFree(stream_adv)

        Bass.BASS_ChannelStop(stream_alarm)
        Bass.BASS_StreamFree(stream_alarm)

        Bass.BASS_ChannelStop(stream2)
        Bass.BASS_StreamFree(stream2)

        Bass.BASS_ChannelStop(stream_music)
        Bass.BASS_StreamFree(stream_music)


        stream_adv = 0
        stream_alarm = 0
        stream_music_2 = 0
        stream2 = 0

        Form1.ProgressBar1.Value = 0
        If Событие = "ADV" Then
            ВремяAlarm = False
            ВремяМузыки = False
            ВремяADV = True
            ВремяJingle = False

            dev_number = GenerlaSetting.GetDevNum(ADVSetting.audio_dev)

            If GenerlaSetting.playingMode = "Mono" Then
                hr = Bass.BASS_Init(dev_number, 44100, BASSInit.BASS_DEVICE_MONO, IntPtr.Zero)
            End If
            If GenerlaSetting.playingMode = "Stereo" Then
                hr = Bass.BASS_Init(dev_number, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
            End If


            If ADV.ADV_Playlist(0).ToString.Split("|")(1) = "1" Then
                ФактическоеВремяНачалаРекламы = Now
            End If
            Dim sss As String
            sss = ADV.ADV_Playlist(0).ToString.Split("|")(2)
            If ADV.ADV_Playlist(0).ToString.Split("|")(1) = "0" Then
                ФактическоеВремяНачалаРекламы = Now.Date & " " & Mid(sss, 1, 2) & ":" & Mid(sss, 3, 2) & ":" & Mid(sss, 5, 2)
            End If

            If ADV.ADV_Playlist.Count > 0 Then
                play_file_name = ADV.ADV_Playlist(0).ToString.Split("|")(0)
                ADV.ADV_Playlist.RemoveAt(0)
            End If


            stream_adv = Bass.BASS_StreamCreateFile(play_file_name, 0, 0, BASSFlag.BASS_DEFAULT)



            ТекущаяПозиция = 0
            ПредыдущаяПозиция = 0
            ' Bass.BASS_ChannelSetAttribute(stream_adv, BASSAttribute.BASS_ATTRIB_VOL, ADVSetting.Volume / 100)
            VolumeSet.VolumeSet("ADV")
            Bass.BASS_ChannelPlay(stream_adv, True)
            Хронометраж_byte = Bass.BASS_ChannelGetLength(stream_adv, BASSMode.BASS_POS_BYTE)
            Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream_adv, Хронометраж_byte)
            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "BASS_INIT=" & hr & vbNewLine, System.Text.Encoding.Default)
            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "stream_adv=" & stream_adv & vbNewLine, System.Text.Encoding.Default)



            hr = Bass.BASS_ChannelGetAttribute(stream_adv, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)

            Form1.ChangeProgBarColor(Form1.ProgressBar1, Form1.ProgressBarColor.Red)
            Реклама_Играется = True
        End If

        If Событие = "Music" Then
            ВремяAlarm = False
            ВремяМузыки = True
            ВремяADV = False
            ВремяJingle = False

            dev_number = GenerlaSetting.GetDevNum(MusicSetting.AudioDev)
            play_file_name = Nothing
            If GenerlaSetting.playingMode = "Mono" Then
                hr = Bass.BASS_Init(dev_number, 44100, BASSInit.BASS_DEVICE_MONO, IntPtr.Zero)
            End If
            If GenerlaSetting.playingMode = "Stereo" Then
                hr = Bass.BASS_Init(dev_number, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
            End If

            If Form1.MusicList.Items.Count = 0 And Form1.ListBox1.Items.Count = 0 Then
                Form1.Text = "CAT"
                Form1.Label1.Text = 0
                Form1.Media_info.Stop()
                play_file_name = ""
            End If




            If MusicSetting.UseCrossFade = True Then
                If Form1.MusicList.Items.Count >= 1 Then
                    If MusicSetting.MusicPause = False Then
                        If Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements.Count <> 0 Then
                            play_file_name = Form1.MusicList.Items(0)
                            Form1.MusicList.Items.RemoveAt(0)
                        End If

                    Else
                        If MusicSetting.UseCrossFade = "1" Then
                            If Остаток <= (MusicSetting.CrossFadeTime / 1000) / 2 + 10 Then
                                ПесняНаПаузе = False
                            End If
                        End If
                        If MusicSetting.UseFadeOUt = "1" Then
                            If Остаток <= (MusicSetting.FadeOutTime / 1000) + 10 Then
                                ПесняНаПаузе = False
                            End If
                        End If

                        If ПесняНаПаузе = True Then
                            play_file_name = ПоследняяПесня
                        Else
                            If Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements.Count <> 0 Then
                                play_file_name = Form1.MusicList.Items(0)
                                Form1.MusicList.Items.RemoveAt(0)
                            Else
                                play_file_name = Form1.MusicList.Items(0)
                                Form1.MusicList.Items.RemoveAt(0)
                            End If
                        End If


                    End If
                Else
                    If Form1.ListBox1.Items.Count >= 1 Then
                        play_file_name = Song.GetDinSongName(True)
                        If play_file_name <> Nothing Then
                            ИграетДинПесня = True
                        Else
                            ИграетДинПесня = False
                        End If
                    End If
                End If


            End If

            If MusicSetting.UseFadeOUt = True Then
                ' If MusicSetting.MusicPause = False Then
                If Остаток <= (MusicSetting.FadeOutTime / 1000) + 10 Then
                    ПесняНаПаузе = False
                End If
                If ПесняИзДинДир = False And ПесняИзСтатДир = False Then
                        If Form1.MusicList.Items.Count >= 1 Then
                            play_file_name = Form1.MusicList.Items(0)
                            Form1.MusicList.Items.RemoveAt(0)
                            ПесняИзСтатДир = True
                            GoTo Воспроизведение
                        Else
                            play_file_name = Song.GetStaticdirSongName
                            ПесняИзДинДир = True
                            ПесняИзСтатДир = False
                            GoTo Воспроизведение
                        End If
                    End If

                    If ПесняИзСтатДир = True And ПесняИзДинДир = False Then
                    play_file_name = Song.GetDinSongName(True)
                    ПесняИзДинДир = True
                        ПесняИзСтатДир = False
                        GoTo Воспроизведение
                    End If

                    If ПесняИзДинДир = True Then
                        If Form1.MusicList.Items.Count >= 1 Then
                            play_file_name = Form1.MusicList.Items(0)
                            Form1.MusicList.Items.RemoveAt(0)
                            ПесняИзДинДир = False
                            ПесняИзСтатДир = True
                        Else
                        play_file_name = Song.GetDinSongName(True)
                        ПесняИзДинДир = True
                            ПесняИзСтатДир = False
                        End If

                    End If
                End If

            'If MusicSetting.MusicPause = True Then
            '    If ПесняНаПаузе = True Then

            '    End If
            'End If


            '  End If

            If Song.GetDinSongName(False) = Nothing Then
                If ПоследняяПесня <> "" Then
                    For r = 0 To Form1.ListBox1.Items.Count - 1
                        If Form1.ListBox1.Items(r) = ПоследняяПесня Then
                            ПесняНаПаузе = False
                        End If
                    Next
                End If
            End If


Воспроизведение:


            If play_file_name = Nothing Then
                Exit Sub
            End If
            If Form1.MusicList.Items.Count = 0 Then
                If Form1.th.ThreadState = Threading.ThreadState.Running Then
                    Form1.th.Abort()
                End If
                Form1.Поток()
            End If
            If Form1.ListBox1.Items.Count = 0 Then
                If Form1.th2.ThreadState = Threading.ThreadState.Running Then
                    Form1.th2.Abort()
                End If

                Form1.Поток_DinSong()
            End If

            If MusicSetting.MusicPause = True Then
                If ПесняНаПаузе = True Then
                    If LastStreaam = "stream_music" Then
                        stream_music = Bass.BASS_StreamCreateFile(ПоследняяПесня, 0, 0, BASSFlag.BASS_DEFAULT)
                        Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, 0)
                        Form1.FadeIN.Start()
                        Bass.BASS_ChannelPlay(stream_music, True)
                        Bass.BASS_ChannelSetPosition(stream_music, LastPos)
                        ОсновнойПоток = 1

                    End If
                    If LastStreaam = "stream2" Then
                        stream2 = Bass.BASS_StreamCreateFile(ПоследняяПесня, 0, 0, BASSFlag.BASS_DEFAULT)
                        Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, 0)
                        Form1.FadeIN.Start()
                        Bass.BASS_ChannelPlay(stream2, True)
                        Bass.BASS_ChannelSetPosition(stream2, LastPos)
                        ОсновнойПоток = 2

                    End If

                    Form1.Text = System.IO.Path.GetFileNameWithoutExtension(ПоследняяПесня)
                    DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)
                    LastMusicID = LastId
                Else
                    stream_music = Bass.BASS_StreamCreateFile(play_file_name, 0, 0, BASSFlag.BASS_DEFAULT)
                    VolumeSet.VolumeSet("Music")
                    Bass.BASS_ChannelPlay(stream_music, True)
                    ОсновнойПоток = 1
                    DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)
                    LastMusicID = LastId
                End If
            Else
                stream_music = Bass.BASS_StreamCreateFile(play_file_name, 0, 0, BASSFlag.BASS_DEFAULT)
                VolumeSet.VolumeSet("Music")
                Bass.BASS_ChannelPlay(stream_music, True)
                ОсновнойПоток = 1
                DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)
                LastMusicID = LastId
            End If

            If stream_music <> 0 Then
                hr = Bass.BASS_ChannelGetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)
            End If
            If stream2 <> 0 Then
                hr = Bass.BASS_ChannelGetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)
            End If

            If System.IO.Directory.Exists(логи) = False Then
                If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
                End If
                логи = Application.StartupPath & "\Logs\"
            End If

            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "BASS_INIT=" & hr & vbNewLine, System.Text.Encoding.Default)
            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "stream_music=" & stream_music & vbNewLine, System.Text.Encoding.Default)



            If dev_number = -1 Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "Устройство " & MusicSetting.AudioDev & " не инициализировано или не найдено. Воспроизвдение на устройство по умолчанию" & vbNewLine, System.Text.Encoding.Default)
            End If
            If hr = 0 Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "Устройство" & MusicSetting.AudioDev & " не инициализировано или не найдено" & stream_music & vbNewLine, System.Text.Encoding.Default)
            End If

            If MusicSetting.MusicPause = True Then
                If ПесняНаПаузе = True Then
                    play_file_name = ПоследняяПесня
                    If LastStreaam = "stream_music" Then
                        Хронометраж_byte = Bass.BASS_ChannelGetLength(stream_music, BASSMode.BASS_POS_BYTE)
                        Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream_music, Хронометраж_byte)
                    End If
                    If LastStreaam = "stream2" Then
                        Хронометраж_byte = Bass.BASS_ChannelGetLength(stream2, BASSMode.BASS_POS_BYTE)
                        Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream2, Хронометраж_byte)
                    End If
                Else
                    Хронометраж_byte = Bass.BASS_ChannelGetLength(stream_music, BASSMode.BASS_POS_BYTE)
                    Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream_music, Хронометраж_byte)
                End If
            Else

                Хронометраж_byte = Bass.BASS_ChannelGetLength(stream_music, BASSMode.BASS_POS_BYTE)
                Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream_music, Хронометраж_byte)
            End If

            ПесняНаПаузе = False
            hr = Bass.BASS_ChannelGetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)



            Form1.ChangeProgBarColor(Form1.ProgressBar1, Form1.ProgressBarColor.Green)

            ' DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)
            '   LastMusicID = LastId
        End If


        If Событие = "Alarm" Then
            play_file_name = alarm_file
            ВремяAlarm = True
            ВремяМузыки = False
            ВремяADV = False


            dev_number = GenerlaSetting.GetDevNum(SettingAlarm.AudioDev)

            If GenerlaSetting.playingMode = "Mono" Then
                hr = Bass.BASS_Init(dev_number, 44100, BASSInit.BASS_DEVICE_MONO, IntPtr.Zero)
            End If
            If GenerlaSetting.playingMode = "Stereo" Then
                hr = Bass.BASS_Init(dev_number, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
            End If


            stream_alarm = Bass.BASS_StreamCreateFile(play_file_name, 0, 0, BASSFlag.BASS_DEFAULT)


            ТекущаяПозиция = 0
            ПредыдущаяПозиция = 0


            If System.IO.Path.GetFileName(SettingAlarm.Jingle) = System.IO.Path.GetFileName(play_file_name) Then
                VolumeSet.VolumeSet("Jingle")
            Else
                VolumeSet.VolumeSet("Alarm")
            End If


            Bass.BASS_ChannelPlay(stream_alarm, True)
            Хронометраж_byte = Bass.BASS_ChannelGetLength(stream_alarm, BASSMode.BASS_POS_BYTE)
            Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream_alarm, Хронометраж_byte)

            Form1.ProgressBar1.Value = 0

            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "BASS_INIT=" & hr & vbNewLine, System.Text.Encoding.Default)
            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "stream_alarm=" & stream_alarm & vbNewLine, System.Text.Encoding.Default)

            hr = Bass.BASS_ChannelGetAttribute(stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)

            Объявление_Играется = True
            Form1.ChangeProgBarColor(Form1.ProgressBar1, Form1.ProgressBarColor.Yellow)
            DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)
            LastAlarmId = LastId

        End If

        'Try
        '    If Событие = "Jingle" Then
        '        dev_number = Настройки.Document.Element("Setting").Element("alarm").Attribute("audio_dev").Value.Split("_")(0)
        '        play_file_name = Jingle
        '        ВремяAlarm = False
        '        ВремяМузыки = False
        '        ВремяJingle = True
        '        ВремяADV = False
        '    End If
        'Catch ex As Exception


        'End Try

        Try
            НачальныйОстатокГромкости = НачальнаяГромкость
            '  Form1.Media_info.Start()

        Catch ex As Exception
            Error_Log(play_file_name, Домен, логи, ex.Message.ToString, "Play_init")
            Bass_Stop()
            Play("Music")
        End Try


        Try

            Form1.Text = System.IO.Path.GetFileName(play_file_name)
        Catch ex As Exception
            Error_Log(play_file_name, Домен, логи, ex.Message.ToString, "Get_file_name")
        End Try


        Try

            If Событие = "ADV" Then
                'If ADV.ADV_Playlist(0).ToString.Split("|")(1) = "1" Then
                '    ФактическоеВремяНачалаРекламы = Now
                'End If
                'Dim sss As String
                'sss = ADV.ADV_Playlist(0).ToString.Split("|")(2)
                'If ADV.ADV_Playlist(0).ToString.Split("|")(1) = "0" Then
                '    ФактическоеВремяНачалаРекламы = Now.Date & " " & Mid(sss, 1, 2) & ":" & Mid(sss, 3, 2) & ":" & Mid(sss, 5, 2)
                'End If

                ФактическоеВремяНачалаРекламы_sec = ФактическоеВремяНачалаРекламы.Hour * 3600 + ФактическоеВремяНачалаРекламы.Minute * 60 + ФактическоеВремяНачалаРекламы.Second
                '  Form1.write_base(dbpatch, Домен, логи, play_file_name, Событие, ФактическоеВремяНачалаРекламы, Now)
                DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, ФактическоеВремяНачалаРекламы, Now)
                LastADVId = LastId
            Else
                ' Form1.write_base(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)

                '   DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, Событие, Now, Now)
            End If





        Catch ex As Exception
            Error_Log(play_file_name, Домен, логи, ex.Message.ToString, "Write_base")
        End Try




        Try
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & Событие, Now & "  " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            End If
        Catch ex As Exception

        End Try



        ''ОТправляем уведомления в телегу
        'If My.Settings.TG_Send_Mess = "True" Then
        '    If hr = 1 Then
        '        If Событие = "Music" And My.Settings.TG_Send_Mess_Music = "True" Then
        '            Telega.Send_text(System.IO.Path.GetFileName(play_file_name))
        '        End If
        '        If Событие = "ADV" And My.Settings.TG_Send_Mess_ADV = "True" Then
        '            Telega.Send_text(System.IO.Path.GetFileName(play_file_name))
        '        End If
        '        If Событие = "Jingle" And My.Settings.TG_Send_Mess_alarm = "True" Then
        '            Telega.Send_text(System.IO.Path.GetFileName(play_file_name))
        '        End If
        '        If Событие = "Alarm" And My.Settings.TG_Send_Mess_alarm = "True" Then
        '            Telega.Send_text(System.IO.Path.GetFileName(play_file_name))
        '        End If
        '    End If
        'End If


        '  ГрафЗапущен = True
        '  music_volume_set()


        Try
            Form1.ProgressBar1.Maximum = Math.Ceiling(Хронометраж)
            Form1.ProgressBar1.Minimum = 0
            'Form1.ProgressBar1.Value = 0
        Catch ex As Exception
            Error_Log(play_file_name, Домен, логи, "Error_get_dur=" & Хронометраж & " Err_code=" & ex.Message, Событие)
        End Try
        Form1.Media_info.Start()



        '  Catch ex As Exception
        'If System.IO.File.Exists(play_file_name) = False Then
        '    If System.IO.Directory.Exists(логи) = True Then
        '        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " " & Событие & " playing_error" & ex.Message.ToString & " file: " & play_file_name & " не найден" & vbNewLine, System.Text.Encoding.Default)
        '    Else
        '        System.IO.File.AppendAllText(Домен & "_" & Now.Date & ".err", Now & Событие & " playing_error" & ex.Message.ToString & " file: " & play_file_name & " не найден" & vbNewLine, System.Text.Encoding.Default)
        '    End If
        'Else
        '    If System.IO.Directory.Exists(логи) = True Then
        '        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & Событие & " playing_error" & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
        '    Else
        '        System.IO.File.AppendAllText(Домен & "_" & Now.Date & ".err", Событие & " playing_error" & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
        '    End If
        'End If
        'Play("Music")
        '  End Try



    End Sub
    ''' <summary>
    ''' Обработка информации о наступлении событий.
    ''' </summary>
    ''' 
    Public Sub SetEventStatus(ByVal MusicStatus As Boolean, ByVal ADVSattus As Boolean, ByVal AlarmStatus As Boolean, ByVal JingleSatus As Boolean)



    End Sub
    Public Sub Alarm_stop()
        Dim Джингл As String
        Dim Profile As XDocument

        Profile = GenerlaSetting.ProfileLoad
        'Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile = XDocument.Load(ProfileName)
        'Else
        '    Profile = XDocument.Load("Настройки.xml")
        'End If

        Джингл = Profile.Document.Element("Setting").Element("Jingle").Attribute("Jingle").Value

        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(alarm_file) & "/архив/") = False Then
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(alarm_file) & "/архив/")
        End If

        Try
            'If Form1.alarm_pl_jingle(НомерОбъявления).ToString.Split("|")(1) = "Alarm" Then
            '  If My.Settings.UseJingle = False And БлокОбъявленийИграется = True Then
            If alarm_file <> Джингл Then
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(alarm_file) & "/архив/" & System.IO.Path.GetFileName(alarm_file)) = False Then
                    System.IO.File.Move(alarm_file, System.IO.Path.GetDirectoryName(alarm_file) & "/архив/" & System.IO.Path.GetFileName(alarm_file))
                    Form1.ПлейлистОбъявлений.RemoveAt(0)
                    Form1.AlarmList.Items.RemoveAt(0)
                Else
                    Form1.ПлейлистОбъявлений.RemoveAt(0)
                    Form1.AlarmList.Items.RemoveAt(0)
                    System.IO.File.Delete(alarm_file)
                End If
            End If

            '  End If

            If ВремяADV = True Then
                Play("ADV")
                GoTo Конец
            End If

            If alarm_file <> Джингл Then
                If Form1.ПлейлистОбъявлений.Count >= 1 Then
                    alarm_file = Form1.ПлейлистОбъявлений(0)
                    Form1.musicfadeout.Stop()
                    Объявление_Играется = True
                    Play("Alarm")
                Else
                    ' Form1.ПлейлистОбъявлений.RemoveAt(0)
                    ' Form1.AlarmList.Items.RemoveAt(0)
                    Объявление_Играется = False
                    ' Play("Music")
                    ВремяAlarm = False
                    Music_OFF()
                    If ВсёОстановлено = False Then
                        Form1.VolumeCheck.Start()
                    End If
                    Form1.Media_info.Start()
                    Form1.ChangeProgBarColor(Form1.ProgressBar1, Form1.ProgressBarColor.Green)
                    Form1.ProgressBar1.Value = 0
                End If
            Else
                If Form1.ПлейлистОбъявлений.Count >= 1 Then
                    alarm_file = Form1.ПлейлистОбъявлений(0)
                    Form1.musicfadeout.Stop()
                    Объявление_Играется = True
                    Play("Alarm")
                Else
                    Form1.ПлейлистОбъявлений.RemoveAt(0)
                    Form1.AlarmList.Items.RemoveAt(0)
                    Объявление_Играется = False
                    ' Play("Music")
                    ВремяAlarm = False
                    Music_OFF()
                    If ВсёОстановлено = False Then
                        Form1.VolumeCheck.Start()
                    End If

                    Form1.Media_info.Start()

                    Form1.ProgressBar1.Value = 0
                    Form1.ChangeProgBarColor(Form1.ProgressBar1, Form1.ProgressBarColor.Green)

                End If

            End If
            ' If БлокОбъявленийИграется = True Then

            '   End If

Конец:

        Catch ex As Exception
            '  MsgBox("")
            '  MsgBox(ex.Message & "   " & alarm_file & "  " & time_of_alarm.Replace(":", "-"))
        End Try



    End Sub
    Public Sub Pause()
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        Jingle = Настройки.Document.Element("Setting").Element("Jingle").Attribute("Jingle").Value
        If Пауза = True Then
            Form1.AlarmCheck.Stop()
            Form1.adv_check.Stop()
            ФейдНачался = False
            ВремяADV = False
            ВремяAlarm = False
            ВремяМузыки = False
            НомерОбъявления = 0
            Form1.musicfadeout.Stop()
        End If
        'If Пауза = False Then
        '    If ВремяAlarm = True Then
        '        Form1.musicfadeout.Stop()
        '        Form1.Media_info.Stop()
        '        Объявление_Играется = True
        '        Реклама_Играется = False
        '        If SettingAlarm.UseJingle = True Then
        '            alarm_file = Jingle
        '        Else
        '            alarm_file = Form1.ПлейлистОбъявлений(0)
        '        End If
        '        Play("Alarm")
        '    Else
        '        Play("Music")
        '    End If

        '    ФейдНачался = False
        '    Form1.Media_info.Start()
        '    Form1.musicfadeout.Stop()
        'End If
    End Sub
    Public Sub Error_Log(ByVal PLay_File_Name As String, Домен As String, ByVal ПутьКЛогам As String, ByVal EX As String, ByVal Событие As String)
        If System.IO.File.Exists(PLay_File_Name) = False Then
            If System.IO.Directory.Exists(ПутьКЛогам) = True Then
                System.IO.File.AppendAllText(ПутьКЛогам & "\" & Домен & "_" & Now.Date & ".err", Now & " " & Событие & " playing_error" & EX & " file: " & PLay_File_Name & " не найден" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Домен & "_" & Now.Date & ".err", Now & Событие & " playing_error" & EX & " file: " & PLay_File_Name & " не найден" & vbNewLine, System.Text.Encoding.Default)
            End If
        Else
            If System.IO.Directory.Exists(ПутьКЛогам) = True Then
                System.IO.File.AppendAllText(ПутьКЛогам & "\" & Домен & "_" & Now.Date & ".err", Now & Событие & " playing_error" & EX & " file: " & PLay_File_Name & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Домен & "_" & Now.Date & ".err", Событие & " playing_error" & EX & " file: " & PLay_File_Name & vbNewLine, System.Text.Encoding.Default)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Останавливаем воспроизведение и высвобождаем все ресурсы
    ''' </summary>
    Public Sub Bass_Stop()
        Bass.BASS_ChannelStop(stream_adv)
        Bass.BASS_StreamFree(stream_adv)

        Bass.BASS_ChannelStop(stream_alarm)
        Bass.BASS_StreamFree(stream_alarm)

        Bass.BASS_ChannelStop(stream_music)
        Bass.BASS_StreamFree(stream_music)

        Bass.BASS_ChannelStop(stream2)
        Bass.BASS_StreamFree(stream2)


        Bass.BASS_Free()



        Объявление_Играется = False
        Реклама_Играется = False

        'Реклама_Играется = False
        'Объявление_Играется = False
        ВремяADV = False
        'ВремяAlarm = False
        Form1.Media_info.Start()
    End Sub

    ''' <summary>
    ''' Обработка информации о наступлении событий.
    ''' </summary>
    Public Sub M_Info()
        Dim s1 As String = GenerlaSetting.OffNightStart
        Dim s2 As String = GenerlaSetting.OffNightStop
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim cur_time_s As Integer
        Dim SP_Music As String
        Dim SP_Music_2 As String
        Dim SP_Alarm As String
        Dim dbpatch As String
        Dim Домен As String
        Dim логи As String
        Dim hr As Integer
        Dim Profile As XDocument
        time_s_1 = s1.Split(":")(0) * 3600 + s1.Split(":")(1) * 60 + s1.Split(":")(2)
        time_s_2 = s2.Split(":")(0) * 3600 + s2.Split(":")(1) * 60 + s2.Split(":")(2)
        cur_time_s = Now.Hour * 3600 + Now.Minute * 60 + Now.Second

        Profile = GenerlaSetting.ProfileLoad


        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
        Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Profile.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
        логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value


        If stream_music <> 0 Or stream2 <> 0 Then
            If ОсновнойПоток = 1 Then
                ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_music, BASSMode.BASS_POS_BYTE)
                ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_music, ТекущаяПозиция_byte)
            End If
            If ОсновнойПоток = 2 Then
                ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream2, BASSMode.BASS_POS_BYTE)
                ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream2, ТекущаяПозиция_byte)
            End If
        End If

        'If ТекущаяПозиция = ПредыдущаяПозиция And Реклама_Играется = True And ВремяJingle = False And ТекущаяПозиция <> 0 And ПредыдущаяПозиция <> 0 And ТекущаяПозиция <> -1 And ПредыдущаяПозиция <> -1 Then
        '    MsgBox(Хронометраж)
        'End If

        If stream_adv <> 0 Then
            ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_adv, BASSMode.BASS_POS_BYTE)
            ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_adv, ТекущаяПозиция_byte)
        End If
        If stream_alarm <> 0 Then
            ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_alarm, BASSMode.BASS_POS_BYTE)
            ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_alarm, ТекущаяПозиция_byte)
        End If




        'If ВремяADV = True Or ВремяAlarm = True Then
        '    If Song.GetDinSongName(False) = Nothing Then
        '        If ИграетДинПесня = True Then
        '            СтавитьНаПаузу = False
        '        Else
        '            СтавитьНаПаузу = True
        '        End If
        '    Else
        '        СтавитьНаПаузу = True
        '    End If
        'End If



        ОбъявлениеПослеADV = False

        'Form1.Label5.Text = ТекущаяПозиция
        If stream_music = 0 And stream_adv = 0 And stream_alarm = 0 And stream2 = 0 Then
            ТекущаяПозиция_byte = 0
            ТекущаяПозиция = 0
            Хронометраж = 0
        End If

        'Если реклама во время выключенной музыки

        SP_Music = Bass.BASS_ChannelIsActive(stream_music)
        SP_Music_2 = Bass.BASS_ChannelIsActive(stream2)
        SP_Alarm = Bass.BASS_ChannelIsActive(stream_alarm)


        If ВремяADV = True And SP_Music = "0" And SP_Music_2 = "0" And Реклама_Играется = False And SP_Alarm = "0" And Объявление_Играется = False Then
            Реклама_Играется = True
            ' Form1.VolumeCheck.Stop() \\\\\\\\\\\\\\\\\\\\\\\\\
            Play("ADV")
        End If



        If ВремяAlarm = True And SP_Music = "0" And SP_Music_2 = "0" And Объявление_Играется = False And Реклама_Играется = False Then
            Объявление_Играется = True
            ' alarm_file = Get_First_Alarm_File_Name()
            alarm_file = Alarm.Get_First_Alarm_File_Name
            '  Form1.VolumeCheck.Stop() \\\\\\\\\\\\\\\\\\\\\\\
            Play("Alarm")
        End If



        If ВремяADV = True And Объявление_Играется = True Then
            If ТекущаяПозиция = ПредыдущаяПозиция Then
                Объявление_Играется = False
                Form1.Media_info.Stop()

                Bass.BASS_ChannelStop(stream_alarm)
                Bass.BASS_StreamFree(stream_alarm)
                Bass.BASS_Free()
                Реклама_Играется = True
                Объявление_Играется = False
                DataBase.UpdateDB(dbpatch, LastAlarmId)
                '   Form1.UpdateBase(dbpatch, LastId)
                Alarm_stop()

                GoTo Конец

            End If
            ПредыдущаяПозиция = ТекущаяПозиция
        End If

        If ВремяAlarm = True And Реклама_Играется = True Then 'Если реклама встык с объявами
            If ТекущаяПозиция = Хронометраж And Реклама_Играется = True Then
                Form1.musicfadeout.Stop()
                Form1.Media_info.Stop()
                Bass_Stop()
                Реклама_Играется = False
                ОбъявлениеПослеADV = True
                DataBase.UpdateDB(dbpatch, LastADVId)
                Pause()
                ' Play("Music")
            End If

        End If


        If ВремяМузыки = True Then 'если нет никаких событий, играем музыку с фейд аутом
            If Song.GetDinSongName(False) = Nothing Then
                If ИграетДинПесня = True Then
                    ДелатьKTO = False
                    Form1.Media_info.Stop()
                    Form1.musicfadeout.Start()
                    ФейдНачался = True
                    '    ДинДирекорияОстановлеа = True
                    ИграетДинПесня = False
                    СтавитьНаПаузу = False
                    '  ПесняНаПаузе = False
                End If
            Else
                СтавитьНаПаузу = True
            End If
            If MusicSetting.UseFadeOUt = True Then
                If Int((Хронометраж - ТекущаяПозиция)) * 1000 <= MusicSetting.FadeOutTime() And Хронометраж <> 0.0 And ТекущаяПозиция <> 0.0 Then
                    ДелатьKTO = True
                    Form1.Media_info.Stop()
                    Form1.musicfadeout.Start()
                    ФейдНачался = True

                End If
            End If
            If MusicSetting.UseCrossFade = True Then
                If Пауза = True And КроссФейдНачался = False Then
                    ДелатьKTO = True
                    Form1.Media_info.Stop()
                    Form1.musicfadeout.Start()
                    ФейдНачался = True


                End If
                If Пауза = True And КроссФейдНачался = True Then
                    Bass.BASS_ChannelStop(stream_music)
                    Bass.BASS_ChannelStop(stream2)
                    Bass.BASS_SampleFree(stream_music)
                    Bass.BASS_SampleFree(stream2)
                    stream2 = 0
                    stream_music = 0
                    Bass.BASS_Free()
                End If
                If Int((Хронометраж - ТекущаяПозиция)) * 1000 <= MusicSetting.CrossFadeTime() / 2 And Хронометраж <> 0 And ТекущаяПозиция <> 0 Then
                    If КроссФейдНачался = False Then
                        If SP_Music_2 = "0" Then

                            If Form1.ListBox1.Items.Count >= 1 Then
                                If Song.GetDinSongName(False) <> Nothing Then
                                    play_file_name = Song.GetDinSongName(True)
                                    ДинПесня = play_file_name
                                    ИграетДинПесня = True
                                Else
                                    ИграетДинПесня = False
                                End If
                            Else
                                play_file_name = ""
                                ДинПесня = play_file_name
                                ИграетДинПесня = False
                            End If


                            If Form1.MusicList.Items.Count >= 1 Then
                                If ИграетДинПесня = False Then
                                    play_file_name = Form1.MusicList.Items(0)
                                    Form1.MusicList.Items.RemoveAt(0)
                                End If

                            End If

                                'If ДинПесня = "" Or ДинПесня = Nothing Then
                                '    ДинДирекорияОстановлена = True


                                'End If
                                'If play_file_name = Nothing Then

                                '    Exit Sub
                                'End If
                                'End If
                                If Form1.MusicList.Items.Count = 0 Then
                                Form1.th.Abort()
                                Form1.Поток()
                            End If
                            If Form1.ListBox1.Items.Count = 0 Then
                                Form1.th2.Abort()
                                Form1.Поток_DinSong()
                            End If

                            If play_file_name <> Nothing Then

                                '   ДинДирекорияОстановлена = False
                                Form1.Text = System.IO.Path.GetFileName(play_file_name)


                                db_param = New ArrayList
                                db_param.Add(dbpatch)
                                db_param.Add(LastMusicID)


                                db_th1 = New System.Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DataBase.UpdateRecTH))
                                db_th1.Start(db_param)

                                db_param = New ArrayList
                                db_param.Add(dbpatch)
                                db_param.Add(Домен)
                                db_param.Add(логи)
                                db_param.Add(play_file_name)
                                db_param.Add("Music")
                                db_param.Add(Now)
                                db_param.Add(Now)


                                db_th2 = New System.Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DataBase.AddNewRecTH))
                                db_th2.Start(db_param)

                                '  DataBase.UpdateDB(dbpatch, LastMusicID)
                                '  DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, "Music", Now, Now)
                                LastMusicID = LastId
                                stream2 = Bass.BASS_StreamCreateFile(play_file_name, 0, 0, BASSFlag.BASS_DEFAULT)

                                Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, 0)
                                Bass.BASS_ChannelPlay(stream2, True)

                                hr = Bass.BASS_ChannelGetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)
                                Хронометраж_byte2 = Bass.BASS_ChannelGetLength(stream2, BASSMode.BASS_POS_BYTE)
                                Хронометраж2 = Bass.BASS_ChannelBytes2Seconds(stream2, Хронометраж_byte2)

                                If System.IO.Directory.Exists(логи) = True Then
                                    If stream2 <> 0 Then
                                        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & "Music", Now & "  " & play_file_name & vbNewLine, System.Text.Encoding.Default)
                                    Else
                                        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & "Music", Now & "  Error_play_" & play_file_name & vbNewLine, System.Text.Encoding.Default)
                                        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & "Music", Now & "  " & "Music_stream=" & stream2 & vbNewLine, System.Text.Encoding.Default)
                                    End If

                                End If
                            End If

                            Mix.xx = (MusicSetting.CrossFadeTime / 2) / 100

                            Form1.CrossFade.Start()

                        End If
                        If SP_Music = "0" Then


                            If Form1.MusicList.Items.Count >= 1 Then
                                play_file_name = Form1.MusicList.Items(0)
                                Form1.MusicList.Items.RemoveAt(0)
                                ИграетДинПесня = False
                                '    ДинДирекорияОстановлена = False
                            Else

                                If Form1.ListBox1.Items.Count >= 1 Then
                                    play_file_name = Song.GetDinSongName(True)
                                    If play_file_name = Nothing Then
                                        ДинДирекорияОстановлена = True
                                    Else
                                        ИграетДинПесня = True
                                    End If
                                Else
                                    Exit Sub

                                End If

                            End If



                            If Form1.MusicList.Items.Count = 0 Then
                                Form1.th.Abort()
                                Form1.Поток()
                            End If
                            If Form1.ListBox1.Items.Count = 0 Then
                                Form1.th2.Abort()
                                Form1.Поток_DinSong()
                            End If
                            If play_file_name = Nothing Then
                                Exit Sub
                            End If



                            Form1.Text = System.IO.Path.GetFileName(play_file_name)


                            db_param = New ArrayList
                            db_param.Add(dbpatch)
                            db_param.Add(LastMusicID)



                            db_th1 = New System.Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DataBase.UpdateRecTH))
                            db_th1.Start(db_param)

                            db_param = New ArrayList
                            db_param.Add(dbpatch)
                            db_param.Add(Домен)
                            db_param.Add(логи)
                            db_param.Add(play_file_name)
                            db_param.Add("Music")
                            db_param.Add(Now)
                            db_param.Add(Now)


                            db_th2 = New System.Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DataBase.AddNewRecTH))
                            db_th2.Start(db_param)


                            '  DataBase.UpdateDB(dbpatch, LastMusicID)

                            ' DataBase.AddNewRecord(dbpatch, Домен, логи, play_file_name, "Music", Now, Now)

                            LastMusicID = LastId

                            stream_music = Bass.BASS_StreamCreateFile(play_file_name, 0, 0, BASSFlag.BASS_DEFAULT)
                            '  Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, (GenerlaSetting.SilenceLevel) / 100)
                            Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, 0)
                            Bass.BASS_ChannelPlay(stream_music, True)


                            hr = Bass.BASS_ChannelGetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, НачальнаяГромкость)

                            '    Dim plpl As Integer = Form1.ListBox1.Items.Count
                            If System.IO.Directory.Exists(логи) = True Then
                                If stream_music <> 0 Then
                                    System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & "Music", Now & "  " & play_file_name & vbNewLine, System.Text.Encoding.Default)
                                Else
                                    System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & "Music", Now & "  Error_play_" & play_file_name & vbNewLine, System.Text.Encoding.Default)
                                    System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & "." & "Music", Now & "  " & "Music_stream=" & stream_music & vbNewLine, System.Text.Encoding.Default)
                                End If

                            End If


                            Хронометраж_byte = Bass.BASS_ChannelGetLength(stream_music, BASSMode.BASS_POS_BYTE)
                            Хронометраж = Bass.BASS_ChannelBytes2Seconds(stream_music, Хронометраж_byte)
                            Mix.xx = (MusicSetting.CrossFadeTime / 2) / 100
                            Form1.CrossFade2.Start()

                            Dim plpl As Integer = Form1.ListBox1.Items.Count
                        End If

                        КроссФейдНачался = True
                    End If
                End If
            End If
        End If


        If ВремяAlarm = True And Реклама_Играется = False And ОбъявлениеПослеADV = False And ВремяADV = False Then
            '///Штатный фейдаут+KillTimeOut\\\
            If MusicSetting.UseFadeOUt = True Then
                If (Int((Хронометраж - ТекущаяПозиция)) * 1000) > (Int(MusicSetting.FadeOutTime) + Int(MusicSetting.killtimeout)) And Объявление_Играется = False And ФейдНачался = False Then
                    ДелатьKTO = True
                    Form1.KillTimer.Start()
                    Form1.Media_info.Stop()
                End If
                '///KillTimeOut Игнорируем и делаем обычный фейд
                If (Int((Хронометраж - ТекущаяПозиция)) * 1000) - MusicSetting.killtimeout < MusicSetting.FadeOutTime And (Int((Хронометраж - ТекущаяПозиция)) * 1000) > MusicSetting.FadeOutTime And Объявление_Играется = False And ФейдНачался = False Then
                    ' If (Int((Хронометраж - ТекущаяПозиция)) * 1000) < MusicSetting.FadeOutTime Then
                    '  alarm_file = Get_First_Alarm_File_Name()
                    alarm_file = Alarm.Get_First_Alarm_File_Name()
                    ФейдНачался = True
                    ВремяМузыки = False
                    Объявление_Играется = True
                    ДелатьKTO = False
                    Form1.Media_info.Stop()
                    Form1.musicfadeout.Start()
                End If
            End If

            If MusicSetting.UseCrossFade = True And Объявление_Играется = False Then
                If Объявление_Играется = False Then
                    If КроссФейдНачался = False Then
                        Form1.Media_info.Stop()
                        Объявление_Играется = True
                        ФейдНачался = True
                        ВремяМузыки = False
                        ВремяAlarm = True
                        '  Form1.VolumeCheck.Stop()
                        'If Song.GetDinSongName(False) = Nothing Then
                        '    If ИграетДинПесня = True Then
                        '        СтавитьНаПаузу = False
                        '    Else
                        '        СтавитьНаПаузу = True
                        '    End If
                        'Else
                        '    СтавитьНаПаузу = True
                        'End If
                        Form1.musicfadeout.Start()
                    Else
                        alarm_file = Alarm.Get_First_Alarm_File_Name()
                        Form1.Media_info.Stop()
                        Form1.Timer11.Start()

                    End If
                End If
            End If


            '///Конец файла\\\
            If ТекущаяПозиция = ПредыдущаяПозиция And Объявление_Играется = True And ВремяJingle = False And ТекущаяПозиция <> 0 And ПредыдущаяПозиция <> 0 Then
                stream_alarm = 0
                Bass_Stop()
                Form1.Media_info.Stop()
                DataBase.UpdateDB(dbpatch, LastAlarmId)

                Alarm_stop()
                Form1.Label1.Text = "0"
            End If
            ПредыдущаяПозиция = ТекущаяПозиция

        End If



        If ВремяADV = True And Объявление_Играется = False Then
            '///Штатный фейдаут+KillTimeOut\\\
            If MusicSetting.UseFadeOUt = True Then
                If (Int((Хронометраж - ТекущаяПозиция)) * 1000) > (Int(MusicSetting.FadeOutTime) + Int(MusicSetting.killtimeout)) And Реклама_Играется = False And ФейдНачался = False Then
                    ДелатьKTO = True
                    Form1.KillTimer.Start()
                    Form1.Media_info.Stop()
                    'If Song.GetDinSongName(False) = Nothing Then
                    '    If ИграетДинПесня = True Then
                    '        ПесняНаПаузе = False
                    '    End If
                    'End If
                End If
                '///KillTimeOut Игнорируем и делаем обычный фейд
                If (Int((Хронометраж - ТекущаяПозиция)) * 1000) - MusicSetting.killtimeout < MusicSetting.FadeOutTime And (Int((Хронометраж - ТекущаяПозиция)) * 1000) > MusicSetting.FadeOutTime And Реклама_Играется = False And ФейдНачался = False Then
                    ' If (Int((Хронометраж - ТекущаяПозиция)) * 1000) < MusicSetting.FadeOutTime Then
                    ФейдНачался = True
                    ВремяМузыки = False
                    Реклама_Играется = True
                    ДелатьKTO = False
                    Form1.Media_info.Stop()
                    Form1.musicfadeout.Start()
                    'If Song.GetDinSongName(False) = Nothing Then
                    '    If ИграетДинПесня = True Then
                    '        ПесняНаПаузе = False
                    '    End If
                    'End If
                    'End If
                End If
            End If
            If MusicSetting.UseCrossFade = True And Реклама_Играется = False Then

                If Реклама_Играется = False Then
                    If КроссФейдНачался = False Then
                        Form1.Media_info.Stop()
                        Реклама_Играется = True
                        ФейдНачался = True
                        ВремяМузыки = False
                        ВремяADV = True
                        'If Song.GetDinSongName(False) = Nothing Then
                        '    If ИграетДинПесня = True Then
                        '        СтавитьНаПаузу = False
                        '    Else
                        '        СтавитьНаПаузу = True
                        '    End If
                        'Else
                        '    СтавитьНаПаузу = True
                        'End If
                        '  Form1.VolumeCheck.Stop()
                        Form1.musicfadeout.Start()
                    Else
                        Form1.Timer10.Start()
                    End If
                End If
            End If

            '///Конец файла\\\

            If ТекущаяПозиция = ПредыдущаяПозиция And Реклама_Играется = True And ВремяJingle = False And ТекущаяПозиция <> 0 And ПредыдущаяПозиция <> 0 And ТекущаяПозиция <> -1 And ПредыдущаяПозиция <> -1 Then
                ' MsgBox(stream_adv)

                If ТекущаяПозиция < Хронометраж Then
                    ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_adv, BASSMode.BASS_POS_BYTE)
                    ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_adv, ТекущаяПозиция_byte)
                End If
                Bass_Stop()
                stream_adv = 0
                DataBase.UpdateDB(dbpatch, LastADVId)
                '   Form1.UpdateBase(dbpatch, LastId)
                If ADV.ADV_Playlist.Count = 0 Then
                    If НеЗапланированное = False And ВсёОстановлено = False Then 'Реклама завершается штатно
                        Music_OFF()
                    End If
                    If НеЗапланированное = True And ВсёОстановлено = True Then
                        Bass.BASS_ChannelStop(stream_adv)
                        Bass.BASS_SampleFree(stream_adv)
                        Bass.BASS_Free()
                        НеЗапланированное = False
                    End If

                    If НеЗапланированное = True And ВсёОстановлено = False Then
                        Music_OFF()
                    End If
                    Form1.ChangeProgBarColor(Form1.ProgressBar1, Form1.ProgressBarColor.Green)
                    Form1.ProgressBar1.Value = 0
                    '  Form1.VolumeCheck.Start()
                Else
                    Form1.Media_info.Stop()
                    Реклама_Играется = True
                    Play("ADV")

                End If


                '  Pause()
            End If
            ПредыдущаяПозиция = ТекущаяПозиция
        End If

        If stream_music <> 0 Or stream2 <> 0 Then
            If ОсновнойПоток = 1 Then
                Form1.Label1.Text = Math.Round(Хронометраж - ТекущаяПозиция, 2)
            End If
            If ОсновнойПоток = 2 Then
                Form1.Label1.Text = Math.Round(Хронометраж2 - ТекущаяПозиция, 2)
            End If
        End If


        If stream_adv <> 0 Or stream_alarm <> 0 Then
            Form1.Label1.Text = Math.Round(Хронометраж - ТекущаяПозиция, 2)
        End If

        Try
            Form1.ProgressBar1.Value = ТекущаяПозиция
        Catch ex As Exception

        End Try


Конец:
    End Sub

    Public Sub KillTime()
        Dim KT_Timer As Timer = Form1.KillTimer

        ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_music, BASSMode.BASS_POS_BYTE)
        ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_music, ТекущаяПозиция_byte)

        Form1.Label1.Text = Math.Round(Хронометраж - ТекущаяПозиция, 3)
        Try
            Form1.ProgressBar1.Value = ТекущаяПозиция
        Catch ex As Exception

        End Try


        If KT_Count = MusicSetting.killtimeout / KT_Timer.Interval Then
            KT_Count = 0
            If ВремяAlarm = True Then
                ' alarm_file = Get_First_Alarm_File_Name()
                alarm_file = Alarm.Get_First_Alarm_File_Name()
            End If
            ФейдНачался = True
            ВремяМузыки = False
            ФейдНачался = True
            Объявление_Играется = True
            Form1.KillTimer.Stop()
            Form1.musicfadeout.Start()
        End If
        If MusicSetting.killtimeout <> 0 Then
            KT_Count += 1
        End If
    End Sub

    Public Sub Fade(ByVal Событие As String)
        Dim cur_volume As Double
        Dim FO_Timer As Timer = Form1.musicfadeout
        Dim hr As Integer
        Dim dbpatch As String
        Dim Поток As Integer
        Dim Profile As XDocument
        Dim Тишина As Double
        Dim MaxLevel As Double


        Profile = GenerlaSetting.ProfileLoad

        Тишина = (Profile.Document.Element("Setting").Element("Silence").Attribute("Value").Value) / 100
        MaxLevel = MusicSetting.GetVolume


        If MaxLevel <= Тишина Then Тишина = 0.05


        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value

        If stream_music <> 0 Then
            ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_music, BASSMode.BASS_POS_BYTE)
            ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_music, ТекущаяПозиция_byte)
            Поток = stream_music
        End If
        If stream2 <> 0 Then
            ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream2, BASSMode.BASS_POS_BYTE)
            ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream2, ТекущаяПозиция_byte)
            Поток = stream2
        End If
        If stream_adv <> 0 Then
            ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_adv, BASSMode.BASS_POS_BYTE)
            ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_adv, ТекущаяПозиция_byte)
        End If
        If stream_alarm <> 0 Then
            ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_alarm, BASSMode.BASS_POS_BYTE)
            ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_alarm, ТекущаяПозиция_byte)
        End If

        Try
            Form1.Label1.Text = Math.Round(Хронометраж - ТекущаяПозиция, 2)
            Form1.ProgressBar1.Value = ТекущаяПозиция
        Catch ex As Exception

        End Try

        If Событие = "Music" And stream_music = 0 And stream2 = 0 Then
            If StartStop.ВремяТИшины = True Then
                Form1.VolumeCheck.Start()
                Form1.musicfadeout.Stop()
                Play("Music")
                Exit Sub
            End If

        End If


        '  Тишина = 0.4
        Bass.BASS_ChannelGetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, cur_volume)

        If MusicSetting.UseFadeOUt = True Then
            If ДелатьKTO = True Then
                If cur_volume <= Тишина Then
                    Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, 0)
                    Bass_Stop()
                    If StartStop.ВремяТИшины = True Then
                        Form1.Label1.Text = "0"
                        Form1.Media_info.Stop()
                    End If
                    If Событие = "Alarm" Then
                        Объявление_Играется = True
                        alarm_file = Alarm.Get_First_Alarm_File_Name()
                        DataBase.UpdateDB(dbpatch, LastMusicID)
                        If MusicSetting.MusicPause = True Then
                            ПоследняяПесня = play_file_name
                            LastPos = ТекущаяПозиция
                            Остаток = Хронометраж - ТекущаяПозиция
                            ' If ФейдНачался = False Then
                            If stream_music <> 0 Then
                                    LastHandlle = stream_music
                                    LastStreaam = "stream_music"
                                End If
                                If stream2 <> 0 Then
                                    LastHandlle = stream2
                                    LastStreaam = "stream2"
                                End If
                            ' End If

                            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value = True Then
                                If Now.Minute = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value Then
                                    ПесняНаПаузе = False
                                Else
                                    ПесняНаПаузе = True
                                End If
                            Else
                                ПесняНаПаузе = True
                            End If

                        End If

                        Play("Alarm")
                    End If
                    If Событие = "ADV" Then
                        Реклама_Играется = True
                        DataBase.UpdateDB(dbpatch, LastADVId)
                        If MusicSetting.MusicPause = True Then
                            ПоследняяПесня = play_file_name
                            LastPos = ТекущаяПозиция
                            Остаток = Хронометраж - ТекущаяПозиция
                            '   If ФейдНачался = False Then
                            If stream_music <> 0 Then
                                    LastHandlle = stream_music
                                    LastStreaam = "stream_music"
                                End If
                                If stream2 <> 0 Then
                                    LastHandlle = stream2
                                    LastStreaam = "stream2"
                                End If
                            ' End If

                            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value = True Then
                                If Now.Minute = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value Then
                                    ПесняНаПаузе = False
                                Else
                                    ПесняНаПаузе = True
                                End If
                            Else
                                ПесняНаПаузе = True
                            End If

                        End If

                        Play("ADV")
                    End If
                    If Событие = "Music" Then
                        If Пауза = False Then
                            DataBase.UpdateDB(dbpatch, LastMusicID)
                            Form1.Text = "CAT"
                            Form1.Label1.Text = "0"
                            Play("Music")
                        End If
                    End If
                    ДелатьKTO = False
                    Form1.musicfadeout.Stop()
                    ФейдНачался = False
                Else
                    Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, cur_volume - (НачальнаяГромкость - Тишина) / (MusicSetting.FadeOutTime / FO_Timer.Interval))
                End If
            Else
                If (Хронометраж - ТекущаяПозиция) * 1000 <= MusicSetting.FadeOutTime Then
                    If cur_volume < Тишина Then
                        Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, 0)
                        Bass_Stop()
                        If StartStop.ВремяТИшины = True Then
                            Form1.Label1.Text = "0"
                            Form1.Media_info.Stop()
                        End If
                        If Событие = "Alarm" Then
                            Объявление_Играется = True
                            alarm_file = Alarm.Get_First_Alarm_File_Name()
                            DataBase.UpdateDB(dbpatch, LastMusicID)
                            If MusicSetting.MusicPause = True Then
                                ПоследняяПесня = play_file_name
                                LastPos = ТекущаяПозиция
                                Остаток = Хронометраж - ТекущаяПозиция
                                ' If ФейдНачался = False Then
                                If stream_music <> 0 Then
                                        LastHandlle = stream_music
                                        LastStreaam = "stream_music"
                                    End If
                                    If stream2 <> 0 Then
                                        LastHandlle = stream2
                                        LastStreaam = "stream2"
                                    End If
                                '  End If

                                If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value = True Then
                                    If Now.Minute = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value Then
                                        ПесняНаПаузе = False
                                    Else
                                        ПесняНаПаузе = True
                                    End If
                                Else
                                    ПесняНаПаузе = True
                                End If


                            End If
                            Play("Alarm")
                        End If
                        If Событие = "ADV" Then
                            DataBase.UpdateDB(dbpatch, LastMusicID)
                            Реклама_Играется = True

                            If MusicSetting.MusicPause = True Then
                                ПоследняяПесня = play_file_name
                                LastPos = ТекущаяПозиция
                                Остаток = Хронометраж - ТекущаяПозиция
                                '  If КроссФейдНачался = False Then
                                If stream_music <> 0 Then
                                        LastHandlle = stream_music
                                        LastStreaam = "stream_music"
                                    End If
                                    If stream2 <> 0 Then
                                        LastHandlle = stream2
                                        LastStreaam = "stream2"
                                    End If
                                '  End If

                                If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value = True Then
                                    If Now.Minute = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value Then
                                        ПесняНаПаузе = False
                                    Else
                                        ПесняНаПаузе = True
                                    End If
                                Else
                                    ПесняНаПаузе = True
                                End If

                            End If
                            Play("ADV")
                        End If
                        If Событие = "Music" Then
                            If Пауза = False Then
                                If StartStop.ВремяТИшины = False Then
                                    DataBase.UpdateDB(dbpatch, LastMusicID)
                                    Form1.Text = "CAT"
                                    Form1.Label1.Text = "0"
                                    Play("Music")
                                End If

                            End If
                        End If
                        ДелатьKTO = False
                        Form1.musicfadeout.Stop()
                        ФейдНачался = False
                    Else
                        hr = Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, cur_volume - (НачальнаяГромкость - Тишина) / (MusicSetting.FadeOutTime / FO_Timer.Interval))
                    End If

                End If
            End If
        End If



        If MusicSetting.UseCrossFade = True Then
            If cur_volume <= Тишина Then
                Bass.BASS_ChannelSetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, 0)
                Bass_Stop()
                If StartStop.ВремяТИшины = True Then
                    Form1.Label1.Text = "0"
                    Form1.Media_info.Stop()
                End If



                If Событие = "Alarm" Then
                    Объявление_Играется = True
                    '   alarm_file = Get_First_Alarm_File_Name()
                    DataBase.UpdateDB(dbpatch, LastMusicID)
                    alarm_file = Alarm.Get_First_Alarm_File_Name()
                    If MusicSetting.MusicPause = True Then
                        ПоследняяПесня = play_file_name
                        LastPos = ТекущаяПозиция
                        Остаток = Хронометраж - ТекущаяПозиция
                        ' If КроссФейдНачался = False Then
                        If stream_music <> 0 Then
                                LastHandlle = stream_music
                                LastStreaam = "stream_music"
                            End If
                            If stream2 <> 0 Then
                                LastHandlle = stream2
                                LastStreaam = "stream2"
                            End If
                        '  End If

                        If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value = True Then
                            If Now.Minute = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value Then
                                ПесняНаПаузе = False
                            Else
                                ПесняНаПаузе = True
                            End If
                        Else
                            ПесняНаПаузе = True
                        End If
                    End If
                    Play("Alarm")
                End If
                If Событие = "ADV" Then
                    DataBase.UpdateDB(dbpatch, LastMusicID)
                    Реклама_Играется = True
                    If MusicSetting.MusicPause = True Then
                        ПоследняяПесня = play_file_name
                        LastPos = ТекущаяПозиция
                        Остаток = Хронометраж - ТекущаяПозиция
                        ' If КроссФейдНачался = False Then
                        If stream_music <> 0 Then
                            LastHandlle = stream_music
                            LastStreaam = "stream_music"
                        End If
                        If stream2 <> 0 Then
                            LastHandlle = stream2
                            LastStreaam = "stream2"
                        End If
                        If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value = True Then
                            If Now.Minute = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value Then
                                ПесняНаПаузе = False
                            Else
                                ПесняНаПаузе = True
                            End If
                        Else
                            ПесняНаПаузе = True
                        End If
                        'If СтавитьНаПаузу = False Then
                        '    ПесняНаПаузе = False
                        'End If
                        ' End If
                    End If
                    If Song.GetDinSongName(False) = Nothing Then
                        If ИграетДинПесня = True Then
                            ПесняНаПаузе = False
                        End If
                    End If
                    Play("ADV")
                End If
                If Событие = "Music" Then
                    If StartStop.ВремяТИшины = False Then
                        DataBase.UpdateDB(dbpatch, LastMusicID)
                        Form1.Text = "CAT"
                        Form1.Label1.Text = "0"
                        Play("Music")
                    End If

                End If
                ДелатьKTO = False
                Form1.musicfadeout.Stop()
                ФейдНачался = False
            Else
                hr = Bass.BASS_ChannelSetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, cur_volume - (НачальнаяГромкость - Тишина) / (MusicSetting.FadeOutTime / FO_Timer.Interval))
            End If
        End If

    End Sub


    Public Sub Music_OFF()
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'Dim time As String
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim time_s_3 As Integer
        Dim cur_time_s As Integer
        Dim ProFileName As String
        Dim Profile As XDocument

        Profile = GenerlaSetting.ProfileLoad

        Dim s1 As String = Profile.Document.Element("Setting").Element("timeoff").Attribute("Start").Value
        Dim s2 As String = Profile.Document.Element("Setting").Element("timeoff").Attribute("Stop").Value

        Dim SP_Music As String
        Dim SP_ADV As String
        Dim SP_Alarm As String
        Dim Diff As Integer

        time_s_1 = s1.ToString.Split(":")(0) * 3600 + s1.ToString.Split(":")(1) * 60 + s1.ToString.Split(":")(2)
        time_s_2 = s2.ToString.Split(":")(0) * 3600 + s2.ToString.Split(":")(1) * 60 + s2.ToString.Split(":")(2)
        time_s_3 = 23 * 3600 + 59 * 60 + 59 'Время конца дня

        cur_time_s = Now.Hour * 3600 + Now.Minute * 60 + Now.Second


        SP_Music = Bass.BASS_ChannelIsActive(stream_music)
        SP_ADV = Bass.BASS_ChannelIsActive(stream_adv)
        SP_Alarm = Bass.BASS_ChannelIsActive(stream_alarm)
        Form1.Text = "CAT"
        If DateDiff(DateInterval.Hour, CDate(s1), CDate(s2)) < 0 Then 'Вычисляем Дату и время тишины
            Diff = DateDiff(DateInterval.Second, CDate(s1), CDate("23:59:59")) + DateDiff(DateInterval.Second, CDate("00:00:00"), CDate(s2)) + 1
            If cur_time_s >= time_s_1 Or cur_time_s <= time_s_2 Then
                Bass.BASS_ChannelStop(stream_music)
                Bass.BASS_ChannelStop(stream2)
                stream_music = 0
                stream2 = 0
                If SP_ADV = "0" Then
                    Form1.ProgressBar1.Value = 0
                    Form1.Label1.Text = "0"
                End If
            End If

            If cur_time_s > time_s_2 And cur_time_s < time_s_1 Then
                If SP_Music = "0" Then
                    If ВремяADV <> True And ВремяAlarm <> True Then
                        If Пауза = False Then
                            'If MusicSetting.MusicPause = True Then
                            '    If stream_music <> 0 Then
                            '        volume_set("Music")
                            '        Bass.BASS_ChannelStart(stream_music)

                            '    End If
                            '    If stream2 <> 0 Then
                            '        volume_set("Music")
                            '        Bass.BASS_ChannelStart(stream2)

                            '    End If
                            'End If
                            ' If MusicSetting.MusicPause = False Then
                            If ВсёОстановлено = False Then
                                Play("Music")
                            End If

                            '  End If

                        End If

                    End If
                End If
            End If

        End If

        '   Utils .GetNormalizationGain ("",0.5,-1,-1,)
        If DateDiff(DateInterval.Hour, CDate(s1), CDate(s2)) > 0 Then
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then 'Останавливаем музыку согласно планировщику
                Bass.BASS_ChannelStop(stream_music)
                Bass.BASS_ChannelStop(stream2)
                stream_music = 0
                stream2 = 0
                If SP_ADV = "0" Or SP_Alarm = "0" Then
                    Form1.ProgressBar1.Value = 0
                    Form1.Label1.Text = "0"
                End If
            Else
                If SP_Music = "0" Then
                    If ВремяADV <> True And ВремяAlarm <> True Then
                        If Пауза = False Then
                            If ВсёОстановлено = False Then
                                Play("Music")
                            End If


                        End If
                    End If
                End If
            End If
        End If
    End Sub
    ''' <summary>
    ''' Проверяем все ролики на указанную дату
    ''' </summary>
    Public Function GetReklamaOfDay(ByVal Дата As String)
        Dim r, n As Integer
        Dim Плейлист As Array
        Dim file_name As String
        Dim ЕстьТакой As Boolean = False
        Dim РоликиНаДень As New ArrayList
        Dim логи As String
        Dim adv_patch As String
        Dim PlayListPatch As String
        Dim Profile As XDocument


        Profile = GenerlaSetting.ProfileLoad


        логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
        adv_patch = Profile.Document.Element("Setting").Element("adv").Attribute("dir").Value
        PlayListPatch = Profile.Document.Element("Setting").Element("sch").Attribute("dir").Value

        If System.IO.Directory.Exists(логи) = False Then
            If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
            End If
            логи = Application.StartupPath & "\Logs\"
        End If

        Try
            Плейлист = System.IO.File.ReadAllLines(PlayListPatch & "\" & Дата & ".txt", System.Text.Encoding.Default)

            For r = 1 To Плейлист.Length - 1
                file_name = Плейлист(r).ToString.Split("=")(2)
                For n = 0 To РоликиНаДень.Count - 1
                    If РоликиНаДень(n) = file_name Then
                        ЕстьТакой = True
                        Exit For
                    End If
                Next
                If ЕстьТакой = False Then
                    РоликиНаДень.Add(file_name)
                End If
                ЕстьТакой = False
            Next
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  Error get_reklama_of_day" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  Error get_reklama_of_day" & vbNewLine, System.Text.Encoding.Default)
            End If

        End Try

        Return РоликиНаДень

    End Function
End Module
