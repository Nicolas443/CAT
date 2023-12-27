Imports Un4seen.Bass
Imports Un4seen.Bass.AddOn.Mix
Imports Un4seen.Bass.Misc
Module Mix
    Public ШагКросФейда As Double
    Public xx As Integer
    Dim u As Integer
    Public MMM As New ArrayList
    Public ШагИзмененияГромкости As Double

    Public Sub CrossFade1()
#Region "Variables5"
        Dim cur_volume As Double
        Dim cur_volume2 As Double
        Dim CFT As Timer = Form1.CrossFade
        Dim max_vol As Double
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        Dim time As String
        Dim cur_time As String
        Dim Profile As XDocument
        Dim Тишина As Double
        Dim ЕстьВремя As Boolean
#End Region




        ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream_music, BASSMode.BASS_POS_BYTE)
        ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream_music, ТекущаяПозиция_byte)


        Dim SSS As Integer = (MusicSetting.CrossFadeTime / 2) / CFT.Interval
        Profile = GenerlaSetting.ProfileLoad


        Тишина = 0.05

        For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                max_vol = (times.Value.Split("|")(1)) / 100
                ЕстьВремя = True
            End If
        Next

        If ЕстьВремя = False Then
            max_vol = MusicSetting.GetVolume
        End If
        ЕстьВремя = False


        Bass.BASS_ChannelGetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, cur_volume)
        Bass.BASS_ChannelGetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, cur_volume2)

        '  ШагИзмененияГромкости = ""

        If u >= xx Then
            ' КроссФейдНачался = False
            Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, 0)
            Bass.BASS_ChannelStop(stream_music)
            Bass.BASS_StreamFree(stream_music)
            stream_music = 0
            ОсновнойПоток = 2
            Form1.ProgressBar1.Maximum = Хронометраж2
            Play.Хронометраж = Хронометраж2
            КроссФейдНачался = False
            Form1.CrossFade.Stop()
            u = 0
        Else
            Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, (cur_volume - ((Play.НачальнаяГромкость)) / SSS))
        End If

        If Play.play_file_name <> Nothing Then
            If cur_volume2 <= max_vol Then
                '  Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, cur_volume2 + (max_vol) / (SSS))
                Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, cur_volume2 + (max_vol) / (SSS))
            End If
        End If


        u += 1

    End Sub

    Public Sub CrossFade2()
#Region "Variables7"
        Dim cur_volume As Double
        Dim cur_volume2 As Double
        Dim CFT As Timer = Form1.CrossFade
        Dim max_vol As Double
        Dim time_s_1 As Integer 'Начало временного дипазона в секундах
        Dim time_s_2 As Integer 'Окончание временного дипазона в секундах
        Dim cur_time_s As Integer 'Текущее время в секундах
        Dim time As String
        Dim cur_time As String
        Dim Profile As XDocument
        Dim Тишина As Double
        Dim ЕстьВремя As Boolean
#End Region


        ТекущаяПозиция_byte = Bass.BASS_ChannelGetPosition(stream2, BASSMode.BASS_POS_BYTE)
        ТекущаяПозиция = Bass.BASS_ChannelBytes2Seconds(stream2, ТекущаяПозиция_byte)


        Dim SSS As Integer = (MusicSetting.CrossFadeTime / 2) / CFT.Interval
        Profile = GenerlaSetting.ProfileLoad

        Тишина = 0.05

        For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                max_vol = (times.Value.Split("|")(1)) / 100
                ЕстьВремя = True
            End If
        Next
        If ЕстьВремя = False Then
            max_vol = MusicSetting.GetVolume
        End If
        ЕстьВремя = False

        ' If ШагКросФейда < SSS Then
        Bass.BASS_ChannelGetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, cur_volume)
        Bass.BASS_ChannelGetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, cur_volume2)


        If u >= xx Then
            Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, 0)
            Bass.BASS_ChannelStop(stream2)
            Bass.BASS_StreamFree(stream2)
            ОсновнойПоток = 1
            Form1.ProgressBar1.Maximum = Хронометраж
            stream2 = 0
            КроссФейдНачался = False
            Form1.CrossFade2.Stop()
            u = 0
        Else
            Bass.BASS_ChannelSetAttribute(stream2, BASSAttribute.BASS_ATTRIB_VOL, cur_volume2 - (Play.НачальнаяГромкость) / SSS)
        End If

        If cur_volume <= max_vol Then
            Bass.BASS_ChannelSetAttribute(stream_music, BASSAttribute.BASS_ATTRIB_VOL, cur_volume + (max_vol) / (SSS))
        End If


        u += 1
    End Sub

    Public Sub FadeIN()
        Dim КонечнаяГромкость As Double = (MusicSetting.GetVolume)
        Dim ТекущаяГромкость As Double
        Dim Поток As Integer

        Dim FO_Timer As Timer = Form1.musicfadeout
        Dim Тишина As Double

        Тишина = 0.05

        If stream_music <> 0 Then Поток = Play.stream_music
        If stream2 <> 0 Then Поток = Play.stream2

        Bass.BASS_ChannelGetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, ТекущаяГромкость)
        Bass.BASS_ChannelSetAttribute(Поток, BASSAttribute.BASS_ATTRIB_VOL, ТекущаяГромкость + (КонечнаяГромкость - Тишина) / (MusicSetting.FadeOutTime / FO_Timer.Interval))

        If Math.Round(ТекущаяГромкость, 2) >= Math.Round(КонечнаяГромкость, 2) Then
            Play.НачальнаяГромкость = КонечнаяГромкость
            Form1.FadeIN.Stop()
        End If
    End Sub


End Module
