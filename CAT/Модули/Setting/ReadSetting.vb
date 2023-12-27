
Imports Un4seen.Bass

Public Module ADVSetting
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")
    Public ProFileName As String
    ''' <summary>
    ''' Уровень звука рекламы
    ''' </summary>
    ''' <returns></returns>
    Public Function Volume()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Return Profile.Document.Element("Setting").Element("adv").Attribute("volume").Value
    End Function
    ''' <summary>
    ''' Путь к рекламным ролимкам
    ''' </summary>
    ''' <returns></returns>
    Public Function dir()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("adv").Attribute("dir").Value
    End Function
    ''' <summary>
    ''' Аудиоустройство для вывода рекламы
    ''' </summary>
    ''' <returns></returns>
    Public Function audio_dev()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("adv").Attribute("audio_dev").Value
    End Function

    Public Function MinTimeDist()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("adv").Attribute("MinTimeDist").Value
    End Function

    Public Function GetVolume()
        Dim time As String
        Dim cur_time_s As Integer
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim ЕстьВремя As Boolean


        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("adv").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            ' cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                ЕстьВремя = True
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                '  Return (times.Value.Split("|")(1) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                Return (times.Value.Split("|")(1)) / 100
                'End If


            End If
        Next

        If ЕстьВремя = False Then
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100
            'End If
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
            Return (Profile.Document.Element("Setting").Element("adv").Attribute("volume").Value) / 100
            'End If
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100

        End If
        ЕстьВремя = False
    End Function



End Module

Public Module SCHSetting
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")
    ''' <summary>
    ''' Папка с рекламными расписаниями
    ''' </summary>
    ''' <returns></returns>
    Public Function dir()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("sch").Attribute("dir").Value
    End Function
End Module



Public Module NEWSSetting
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")
    ''' <summary>
    ''' Уровень звука новостей
    ''' </summary>
    ''' <returns></returns>
    Public Function Volume()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("news").Attribute("volume").Value
    End Function
    ''' <summary>
    ''' Путь к файлам новостей
    ''' </summary>
    ''' <returns></returns>
    Public Function dir()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("news").Attribute("dir").Value
    End Function
    ''' <summary>
    ''' Аудиоустройство для вывода новостей
    ''' </summary>
    ''' <returns></returns>
    Public Function audio_dev()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("news").Attribute("audio_dev").Value
    End Function

End Module


Public Module SettingDomen
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")
    ''' <summary>
    ''' Имя домена
    ''' </summary>
    ''' <returns></returns>
    Public Function Name()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value
    End Function
    ''' <summary>
    ''' Субдомен
    ''' </summary>
    ''' <returns></returns>
    Public Function SubDomen()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
    End Function
    ''' <summary>
    ''' Серийный номер
    ''' </summary>
    ''' <returns></returns>
    Public Function Serial()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("Domen").Attribute("serial").Value
    End Function
End Module


Public Module SettingLog
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")
    ''' <summary>
    ''' Путь к логам
    ''' </summary>
    ''' <returns></returns>
    Public Function DirPatch()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
    End Function

End Module


Public Module SettingDB
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")
    ''' <summary>
    ''' Путь к базе данных
    ''' </summary>
    ''' <returns></returns>
    Public Function DBFile()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("db").Attribute("file").Value
    End Function

End Module

Public Module SettingAlarm
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")

    ''' <summary>
    ''' Уровень звука объявлений
    ''' </summary>
    ''' <returns></returns>
    Public Function Volume()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("alarm").Attribute("volume").Value
    End Function
    Public Function GetVolume()
        Dim time As String
        Dim cur_time_s As Integer
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim ЕстьВремя As Boolean


        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("alarm").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            ' cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                ЕстьВремя = True
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                '  Return (times.Value.Split("|")(1) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                Return (times.Value.Split("|")(1)) / 100
                'End If


            End If
        Next

        If ЕстьВремя = False Then
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100
            'End If
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
            Return (Profile.Document.Element("Setting").Element("alarm").Attribute("volume").Value) / 100
            'End If
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100

        End If
        ЕстьВремя = False
    End Function

    ''' <summary>
    ''' Путь к объявлениям
    ''' </summary>
    ''' <returns></returns>
    Public Function Dir()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("alarm").Attribute("dir").Value
    End Function
    ''' <summary>
    ''' АудиоУстройство для вывода объявлений
    ''' </summary>
    ''' <returns></returns>
    Public Function AudioDev()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("alarm").Attribute("audio_dev").Value
    End Function
    ''' <summary>
    ''' Джингг перед объявлениями
    ''' </summary>
    ''' <returns></returns>
    Public Function Jingle()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("Jingle").Attribute("Jingle").Value
    End Function

    ''' <summary>
    ''' Громкость джингла
    ''' </summary>
    ''' <returns></returns>
    Public Function JingleVolume()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("Jingle").Attribute("volume").Value
    End Function



    Public Function UseJingle() As Boolean
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Return Profile.Document.Element("Setting").Element("Jingle").Attribute("Use").Value
        'If Profile.Document.Element("Setting").Element("Jingle").Attribute("UseJingle").Value = "1" Then
        '    Return True
        'End If
        'If Profile.Document.Element("Setting").Element("alarm").Attribute("UseJingle").Value = "0" Then
        '    Return False
        'End If

    End Function


    Public Function GetJingleVolume()
        Dim time As String
        Dim cur_time_s As Integer
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim ЕстьВремя As Boolean


        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("Jingle").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            ' cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                ЕстьВремя = True
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                '  Return (times.Value.Split("|")(1) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                Return (times.Value.Split("|")(1)) / 100
                'End If


            End If
        Next

        If ЕстьВремя = False Then
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100
            'End If
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
            Return (Profile.Document.Element("Setting").Element("Jingle").Attribute("volume").Value) / 100
            'End If
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100

        End If
        ЕстьВремя = False
    End Function

End Module


Public Module MusicSetting
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")

    Public Function CheckAutor() As Boolean
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Return Profile.Document.Element("Setting").Element("music").Attribute("CkeckAutor").Value
    End Function




    Public Function MusicPause()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Return Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("MusicPause").Value
    End Function

    Public Function GetVolume()
        Dim time As String
        Dim cur_time_s As Integer
        Dim time_s_1 As Integer
        Dim time_s_2 As Integer
        Dim ЕстьВремя As Boolean


        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        For Each times In Profile.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            time = times.Value.Split("|")(0)
            cur_time_s = CInt(Format(Now, "HH")) * 3600 + CInt(Format(Now, "mm")) * 60 + CInt(Format(Now, "ss"))
            time_s_1 = time.Split("-")(0).Split(":")(0) * 3600 + time.Split("-")(0).Split(":")(1) * 60 + time.Split("-")(0).Split(":")(2)
            time_s_2 = time.Split("-")(1).Split(":")(0) * 3600 + time.Split("-")(1).Split(":")(1) * 60 + time.Split("-")(1).Split(":")(2)
            ' cur_time = Format(Now, "HH") & ":" & Format(Now, "mm") & ":" & Format(Now, "ss")
            If cur_time_s >= time_s_1 And cur_time_s < time_s_2 Then
                ЕстьВремя = True
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
                '  Return (times.Value.Split("|")(1) + 100) / 100
                'End If
                'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
                Return (times.Value.Split("|")(1)) / 100
                'End If


            End If
        Next

        If ЕстьВремя = False Then
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Log" Then
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100
            'End If
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation").Value = "Line" Then
            Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value) / 100
            'End If
            ' Return (Profile.Document.Element("Setting").Element("music").Attribute("volume").Value + 100) / 100

        End If
        ЕстьВремя = False

    End Function

    Public Function Volume()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("music").Attribute("volume").Value
    End Function
    Public Function AudioDev()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("music").Attribute("audio_dev").Value
    End Function

    ''' <summary>
    ''' Время, через которое нужно прибить музыку
    ''' </summary>
    ''' <returns></returns>
    Public Function killtimeout()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("music").Attribute("killtimeout").Value
    End Function


    ''' <summary>
    ''' Проверять или нет на повторы из базы
    ''' </summary>
    ''' <returns></returns>
    Public Function PlayListTimeCheck()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("music").Attribute("PlayListTimeCheck").Value
    End Function

    ''' <summary>
    ''' Время кроссфейда
    ''' </summary>
    ''' <returns></returns>
    Public Function CrossFadeTime()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("music").Element("Mix").Attribute("CrossFadeTime").Value
    End Function

    ''' <summary>
    ''' Время ФейдАута
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FadeOutTime()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("music").Element("Mix").Attribute("FadeOutTime").Value
    End Function



    ''' <summary>
    ''' Проверяет, нужно ли использовать Кроссфейд
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function UseCrossFade() As Boolean
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        If Profile.Document.Element("Setting").Element("music").Element("Mix").Attribute("UseCrossFade").Value = "0" Then
            Return False
        End If
        If Profile.Document.Element("Setting").Element("music").Element("Mix").Attribute("UseCrossFade").Value = "1" Then
            Return True
        End If
    End Function


    Public Function UseFadeOUt() As Boolean
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        If Profile.Document.Element("Setting").Element("music").Element("Mix").Attribute("UseFadeOut").Value = "0" Then
            Return False
        End If
        If Profile.Document.Element("Setting").Element("music").Element("Mix").Attribute("UseFadeOut").Value = "1" Then
            Return True
        End If
    End Function




    ''' <summary>
    ''' Проверяет, нужно ли использовать ФейдАут
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function CheckRepeat()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Return Profile.Document.Element("Setting").Element("music").Attribute("CheckRepeat").Value
    End Function




End Module

Public Module GenerlaSetting
    Public Sett As System.Xml.Linq.XDocument = System.Xml.Linq.XDocument.Load("Настройки.xml")

    Public Function OffNightStart()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("timeoff").Attribute("Start").Value
    End Function

    Public Function OffNightStop()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("timeoff").Attribute("Stop").Value
    End Function

    Public Function GetDevNum(ByVal Dev As String) As Integer
        Dim bas_dev_info As New Un4seen.Bass.BASS_DEVICEINFO()
        Dim i As Integer
        Dim Найдено As Boolean

        While (Bass.BASS_GetDeviceInfo(i, bas_dev_info))
            If Dev = bas_dev_info.name And Dev <> "No sound" Then
                Найдено = True
                Return i
            End If
            i += 1
        End While
        Return -1


    End Function
    Public Function SilenceLevel() As Integer
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Return Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("SilenceLevel").Value
    End Function

    Public Function ProfileLoad()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Return Profile
    End Function
    Public Function playingMode()
        Dim Profile As XDocument
        Sett = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Sett.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Return Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("PLayingMode").Value


    End Function

End Module








