Imports Un4seen.Bass

Module VolumeSet
    Public Sub VolumeSet(ByVal Событие As String)
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
#Region "Variables6"
        Dim time As String
        Dim cur_time As String
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        Dim логи As String
        Dim Домен As String
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim ЕстьВремя As Boolean
        Dim ГромкостьПоУмолчанию As Double
#End Region

        Profile = GenerlaSetting.ProfileLoad

        логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
        Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value

        If Событие = "ADV" Then
            For Each times In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
                time = times.Value.Split("|")(0)
                cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
                time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
                time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
                cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
                If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                    ' Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1) + 100) / 100)
                    'End If
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                    Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1)) / 100)
                    'End If

                    ЕстьВремя = True
                    Exit For
                End If
            Next
            If ЕстьВремя = False Then
                ' 'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                ' ГромкостьПоУмолчанию = ((ADVSetting.Volume) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                ГромкостьПоУмолчанию = ((ADVSetting.Volume)) / 100
                'End If
                Bass.BASS_ChannelSetAttribute(Play.stream_adv, BASSAttribute.BASS_ATTRIB_VOL, ГромкостьПоУмолчанию)
            End If
            ЕстьВремя = False

        End If

        If Событие = "Alarm" Then
            For Each times In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
                time = times.Value.Split("|")(0)
                cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
                time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
                time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
                cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
                If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                    '  'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                    Bass.BASS_ChannelSetAttribute(Play.stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1) + 100) / 100)
                    'End If
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                    Bass.BASS_ChannelSetAttribute(Play.stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1)) / 100)
                    'End If

                    ЕстьВремя = True
                    Exit For
                End If
            Next
            If ЕстьВремя = False Then
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                ' ГромкостьПоУмолчанию = ((SettingAlarm.Volume) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                ГромкостьПоУмолчанию = ((SettingAlarm.Volume)) / 100
                'End If
                Bass.BASS_ChannelSetAttribute(Play.stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, ГромкостьПоУмолчанию)
            End If
            ЕстьВремя = False
        End If
        If Событие = "Jingle" Then
            For Each times In Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes
                time = times.Value.Split("|")(0)
                cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
                time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
                time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
                cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
                If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                    ' Bass.BASS_ChannelSetAttribute(Play.stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1) + 100) / 100)
                    'End If
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                    Bass.BASS_ChannelSetAttribute(Play.stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1)) / 100)
                    'End If

                    ЕстьВремя = True
                    Exit For
                End If
            Next
            If ЕстьВремя = False Then
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                ' ГромкостьПоУмолчанию = ((SettingAlarm.JingleVolume) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                ГромкостьПоУмолчанию = ((SettingAlarm.JingleVolume)) / 100
                'End If


                Bass.BASS_ChannelSetAttribute(Play.stream_alarm, BASSAttribute.BASS_ATTRIB_VOL, ГромкостьПоУмолчанию)
            End If
            ЕстьВремя = False
        End If


        If Событие = "Music" Then
            Try
                For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
                    time = times.Value.Split("|")(0)
                    cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
                    time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
                    time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
                    cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
                    If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                        'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                        '  Bass.BASS_ChannelSetAttribute(Play.stream_music, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1) + 100) / 100)
                        ' Bass.BASS_ChannelSetAttribute(Play.stream2, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1) + 100) / 100)
                        'End If
                        'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                        Bass.BASS_ChannelSetAttribute(Play.stream_music, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1)) / 100)
                        Bass.BASS_ChannelSetAttribute(Play.stream2, BASSAttribute.BASS_ATTRIB_VOL, (times.Value.Split("|")(1)) / 100)

                        ' Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_GVOL_STREAM, 10000)
                        'End If
                        ЕстьВремя = True
                            Exit For
                        End If
                Next

                If ЕстьВремя = False Then
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                    ' ГромкостьПоУмолчанию = ((MusicSetting.Volume) + 100) / 100
                    'End If
                    'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                    ГромкостьПоУмолчанию = ((MusicSetting.Volume)) / 100
                    'End If

                    Bass.BASS_ChannelSetAttribute(Play.stream_music, BASSAttribute.BASS_ATTRIB_VOL, ГромкостьПоУмолчанию)
                    Bass.BASS_ChannelSetAttribute(Play.stream2, BASSAttribute.BASS_ATTRIB_VOL, ГромкостьПоУмолчанию)
                End If
                ЕстьВремя = False

            Catch ex As Exception
                If System.IO.Directory.Exists(логи) = True Then
                    System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_set_music_volume " & vbNewLine, System.Text.Encoding.Default)
                End If
            End Try
        End If
    End Sub

End Module
