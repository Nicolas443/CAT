Imports System.IO

Module ADV
    Public Сработка As ArrayList = New ArrayList
    Public ADV_Playlist As ArrayList = New ArrayList
    Public НеНайдено As ArrayList = New ArrayList
    Public НулевойРазмер As ArrayList = New ArrayList

    Public Sub ADV()
        Dim CureDate As String
        Dim логи As String = ""
        Dim adv_patch As String
        Dim killtimeout As Integer
        Dim Домен As String
        Dim PlayList As XDocument = New XDocument
        Dim PLTime As String
        Dim CureTime As String
        Dim fileinfo As FileInfo

        ' Dim mmm As Single = CSng(Math.Pow(10.0, -30 / 20))

        Try

            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            CureDate = Format(Now, "yyyy") & Format(Now, "MM") & Format(Now, "dd")
            логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value
            adv_patch = Настройки.Document.Element("Setting").Element("sch").Attribute("dir").Value
            Домен = Настройки.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
            Try
                killtimeout = Настройки.Document.Element("Setting").Element("music").Attribute("killtimeout").Value
            Catch ex As Exception
                Настройки.Document.Element("Setting").Element("music").SetAttributeValue("killtimeout", "1000")
                Настройки.Save("Настройки.xml")
                Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
                killtimeout = Настройки.Document.Element("Setting").Element("music").Attribute("killtimeout").Value
            End Try


            If System.IO.File.Exists(adv_patch & "\" & CureDate & ".txt") = True Then
                PlayList = PreloadADVPlaylist.CheckPlaylist
            Else
                Exit Sub
            End If
            If Now.Hour = "00" And Now.Minute = "00" And Now.Second = "00" Then Сработка.Clear()
            If Now.Hour = "00" And Now.Minute = "00" And Now.Second = "00" Then НеНайдено.Clear()
            If Now.Hour = "00" And Now.Minute = "00" And Now.Second = "00" Then НулевойРазмер.Clear()



            If PlayList.Document.Element("плейлист").Elements.Count = 0 Then
                If System.IO.Directory.Exists(логи) = True Then
                    System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Временный плейлист пуст" & vbNewLine, System.Text.Encoding.Default)
                End If
            End If

            For Each elem In PlayList.Document.Element("плейлист").Elements
                PLTime = elem.FirstNode.ToString
                CureTime = Now.ToString("HHmmss")
                If CureTime = PLTime Then
                    If Сработка.Contains(PLTime) = False Then
                        For Each attr In elem.Attributes
                            If System.IO.File.Exists(attr.Value) = True Then
                                fileinfo = My.Computer.FileSystem.GetFileInfo(attr.Value)
                                If Math.Round(fileinfo.Length / (1024 * 1024), 2) > 0 Then
                                    Сработка.Add(PLTime)
                                    ADV_Playlist.Add(attr.Value & "|1|" & PLTime)
                                    Form1.Media_info.Start()
                                End If
                                If Math.Round(fileinfo.Length / (1024 * 1024), 2) = 0 Then
                                    If НулевойРазмер.Contains(attr.Value) = False Then
                                        НулевойРазмер.Add(attr.Value)
                                        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error get size " & System.IO.Path.GetFileName(attr.Value) & vbNewLine, System.Text.Encoding.Default)
                                    End If
                                End If


                                    ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Math.Round(FileInfo.Length / (1024 * 1024), 2))
                                    ' Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("size", Math.Round(fileinfo.Length / (1024 * 1024), 2))


                                    ' Eventlist.Add(attr.Value & "|" & "adv" & "|" & "1")

                                Else
                                If System.IO.Directory.Exists(логи) = True Then
                                    If НеНайдено.Contains(PLTime) = False Then
                                        НеНайдено.Add(PLTime)
                                        System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".ADV", Now & " Not Found " & System.IO.Path.GetFileName(attr.Value) & vbNewLine, System.Text.Encoding.Default)
                                    End If
                                    ' System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Not Found " & System.IO.Path.GetFileName(attr.Value) & vbNewLine, System.Text.Encoding.Default)

                                End If
                            End If
                        Next

                    End If

                End If

            Next

            '  System.IO.File.AppendAllText(Now.Date & ".time", Now.ToString("HH:mm:ss") & "." & Now.Millisecond & vbNewLine, System.Text.Encoding.Default)

            ' ПредыдущаяСекунда = ТекущаяСекунда
            '   System.IO.File.AppendAllText(Now.Date & ".curtime", Now.ToString("HH:mm:ss.") & Now.Millisecond & vbNewLine, System.Text.Encoding.Default)

            If ADV_Playlist.Count > 0 Then
                Play.ВремяADV = True
                Play.ВремяМузыки = False
            End If
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " adv_check_error. " & ex.Message & vbNewLine, System.Text.Encoding.Default)
            End If

        End Try
    End Sub
    Public Sub Check()
        Dim Дата As String = Form1.DateTimePicker1.Value.Date.ToString("yyyyMMdd")
        Dim РоликиНаДень As ArrayList = GetADVOfDay(Дата)
        Dim adv_dir As String
        Dim r As Integer
        Dim FileInfo As System.IO.FileInfo
        Dim логи As String
        Dim Домен As String
        Dim Плейлист As Array
        Dim PlayListPatch As String
        Dim Status As Boolean
        Dim Check_rez As XDocument
        Dim xe As XElement
        Dim count As Integer


        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        adv_dir = ADVSetting.dir

        логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value
        Домен = Настройки.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value

        PlayListPatch = Настройки.Document.Element("Setting").Element("sch").Attribute("dir").Value

        Check_rez = New XDocument
        Check_rez = <?xml version="1.0"?>
                    <Check_rez>
                    </Check_rez>




        If System.IO.File.Exists(PlayListPatch & "\" & Дата & ".txt") = False Then
            MsgBox("Плейлист на " & Form1.DateTimePicker1.Value.ToString("dd.MM.yyyy") & " не найден")
        End If

        If System.IO.File.Exists(логи & "\" & Домен & "_" & Form1.DateTimePicker1.Value.Date & ".ADV") = False Then
            MsgBox("Лог рекламы на " & Form1.DateTimePicker1.Value.ToString("dd.MM.yyyy") & " не найден")
        End If


        If System.IO.File.Exists(PlayListPatch & "\" & Дата & ".txt") = True Then
            Плейлист = System.IO.File.ReadAllLines(PlayListPatch & "\" & Дата & ".txt", System.Text.Encoding.Default)

            For r = 1 To Плейлист.Length - 1
                count = Check_rez.Document.Element("Check_rez").Elements.Count
                xe = New XElement("adv_" & count, "")
                Check_rez.Document.Element("Check_rez").Add(xe)
                If Плейлист(r).ToString.Split("=").Length = 3 Then
                    Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("time", Плейлист(r).ToString.Split("=")(1))
                    Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("adv", Плейлист(r).ToString.Split("=")(2))
                    '  ListView1.Items.Add(Плейлист(r).ToString.Split("=")(1))
                    ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Плейлист(r).ToString.Split("=")(2))
                    If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(2) & ".wav") = True Then
                        FileInfo = My.Computer.FileSystem.GetFileInfo(adv_dir & "\" & Плейлист(r).ToString.Split("=")(2) & ".wav")
                        ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Math.Round(FileInfo.Length / (1024 * 1024), 2))
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("size", Math.Round(FileInfo.Length / (1024 * 1024), 2))
                    Else
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("size", "0")
                        'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("0")
                    End If
                    If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(2) & ".wav") = True Then
                        Status = ChecADVLog(Плейлист(r).ToString.Split("=")(2) & ".wav", Плейлист(r).ToString.Split("=")(1))
                    Else
                        Status = False
                    End If


                    If Status = True Then
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Status", "Вышел")
                        ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Вышел")
                    End If
                    If Status = False Then
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Status", "Не вышел")
                    End If
                    If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(2) & ".wav") = True Then
                        ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Yes")
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Exist", "Yes")
                    Else
                        'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("No")
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Exist", "No")
                    End If

                End If
                'If Плейлист(r).ToString.Split("=").Length = 2 Then
                '    ListView1.Items.Add(Плейлист(r).ToString.Split("=")(0))
                '    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Плейлист(r).ToString.Split("=")(1))
                '    If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav") = True Then
                '        FileInfo = My.Computer.FileSystem.GetFileInfo(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav")
                '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Math.Round(FileInfo.Length / (1024 * 1024), 2))

                '    Else
                '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("0")
                '    End If

                '    If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav") = True Then
                '        Status = ChecADVLog(Плейлист(r).ToString.Split("=")(1) & ".wav", Плейлист(r).ToString.Split("=")(0))
                '    Else
                '        Status = False
                '    End If
                '    If Status = True Then
                '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Вышел")
                '    End If
                '    If Status = False Then
                '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Не вышел")
                '    End If
                '    If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(2) & ".wav") = True Then
                '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Yes")
                '    Else
                '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("No")
                '    End If
                'End If

            Next


        End If
        '  Me.Invoke(New Form1.MusicListUpdate(AddressOf Me.MusicListAdd), Микс.Item(Микс.Count - 1))
        '  Form1.Invoke(New Form1.ADVCheckDelegate(AddressOf Form1.advcheckupdate), Check_rez)

    End Sub

    Private Function ChecADVLog(ByVal Ролик As String, ByVal ВремяВПлейлисте As String)
        Dim логи As String
        Dim Домен As String
        Dim ADV_LOG As Array
        Dim Плейлист As Array
        Dim PlayListPatch As String
        Dim Дата As String = Form1.DateTimePicker1.Value.Date.ToString("yyyyMMdd")
        Dim ВремяВЛоге As String
        Dim ВремяПлейлистаВСек As Integer
        Dim ВремяЛогеВСек As Integer
        Dim x As Integer
        Dim РоликВЛоге As String


        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value
        Домен = Настройки.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value


        PlayListPatch = Настройки.Document.Element("Setting").Element("sch").Attribute("dir").Value
        Плейлист = System.IO.File.ReadAllLines(PlayListPatch & "\" & Дата & ".txt", System.Text.Encoding.Default)

        'If System.IO.File.Exists(логи & "\" & Домен & "_" & DateTimePicker1.Value.Date & ".ADV") = False Then
        '    MsgBox("Лог рекламы на " & DateTimePicker1.Value.ToString("dd.MM.yyyy") & " не найден")
        'End If
        If System.IO.File.Exists(логи & "\" & Домен & "_" & Form1.DateTimePicker1.Value.Date & ".ADV") = True Then
            ADV_LOG = System.IO.File.ReadAllLines(логи & "\" & Домен & "_" & Form1.DateTimePicker1.Value.Date & ".ADV", System.Text.Encoding.Default)
            ВремяПлейлистаВСек = Mid(ВремяВПлейлисте, 1, 2) * 3600 + Mid(ВремяВПлейлисте, 3, 2) * 60 + Mid(ВремяВПлейлисте, 5, 2)


            For x = 0 To ADV_LOG.Length - 1
                ВремяВЛоге = ADV_LOG(x).ToString.Split(" ")(1)
                ВремяЛогеВСек = ВремяВЛоге.Split(":")(0) * 3600 + ВремяВЛоге.Split(":")(1) * 60 + ВремяВЛоге.Split(":")(2)
                РоликВЛоге = System.IO.Path.GetFileName(ADV_LOG(x).ToString.Split(" ")(3))
                If РоликВЛоге = РоликВЛоге Then
                    If ВремяЛогеВСек >= ВремяПлейлистаВСек And ВремяЛогеВСек <= ВремяПлейлистаВСек + 60 Then
                        Return True
                    End If
                End If

            Next
        End If






        Return False
    End Function

    Private Function GetADVOfDay(ByVal Дата As String) As ArrayList

        Dim r, n As Integer
        Dim Плейлист As Array
        Dim file_name As String
        Dim ЕстьТакой As Boolean = False
        Dim РоликиНаДень As New ArrayList
        Dim логи As String
        Dim adv_patch As String
        Dim PlayListPatch As String
        Dim ProFileName As String
        Dim Profile As XDocument




        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


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
    ''' <summary>
    ''' Ищем не вышедшие ролики за указанное количество минут
    ''' </summary>
    ''' <returns></returns>

    Public Function GetLostADV() As ArrayList
        Dim Profile As XDocument
        Dim dbpatch As String
        Dim H As Integer
        Dim dt As DataTable
        Dim r, p As Integer
        Dim Плейлист As Array
        Dim adv_patch As String = SCHSetting.dir
        Dim CureDate As String
        Dim LostADV As New ArrayList
        Dim PlTimeSec As Integer
        Dim PLTime As String = ""
        Dim CurTimesec As Integer
        Dim L As Integer
        Dim Вышел As Boolean
        Dim ВремяВБазе As Date
        Dim ВремяВБазеSec As Integer
        Dim НазваниеВБазе As String
        Dim НазваниеВПлейлисте As String
        Dim adv_dir As String = ADVSetting.dir


        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
        H = Profile.Document.Element("Setting").Element("adv").Attribute("LostADVTime").Value





        CureDate = Format(Now, "yyyy") & Format(Now, "MM") & Format(Now, "dd")

        If System.IO.File.Exists(adv_patch & "\" & CureDate & ".txt") = True Then
            dt = DataBase.GetLastADV(dbpatch, H * (-1))
            Плейлист = System.IO.File.ReadAllLines(adv_patch & "\" & CureDate & ".txt", System.Text.Encoding.Default)
            CurTimesec = Now.ToString("HH") * 3600 + Now.ToString("mm") * 60 + Now.ToString("ss")
            For p = 1 To Плейлист.Length - 1
                L = Плейлист(p).ToString.Split("=").Length
                If L = 3 Then
                    PLTime = Плейлист(p).ToString.Split("=")(1)
                    НазваниеВПлейлисте = Плейлист(p).ToString.Split("=")(2) & ".wav"
                End If
                If L = 2 Then
                    PLTime = Плейлист(p).ToString.Split("=")(0)
                    НазваниеВПлейлисте = Плейлист(p).ToString.Split("=")(1) & ".wav"
                End If

                PlTimeSec = Mid(PLTime, 1, 2) * 3600 + Mid(PLTime, 3, 2) * 60 + Mid(PLTime, 5, 2)

                If PlTimeSec >= CurTimesec - H * 60 And PlTimeSec <= CurTimesec Then
                    For r = 0 To dt.Rows.Count - 1
                        ВремяВБазе = dt(r)(2)
                        ВремяВБазеSec = ВремяВБазе.ToString("HH") * 3600 + ВремяВБазе.ToString("mm") * 60 + ВремяВБазе.ToString("ss")
                        НазваниеВБазе = dt(r)(5).ToString.Trim
                        If ВремяВБазеSec >= PlTimeSec And ВремяВБазеSec <= PlTimeSec + 30 Then
                            Вышел = True
                        End If
                    Next
                    If Вышел = False Then
                        LostADV.Add(PLTime & "|" & adv_dir & "\" & НазваниеВПлейлисте)
                    End If
                    Вышел = False
                End If
            Next


            For r = 0 To LostADV.Count - 1
                Сработка.Add(LostADV(r).ToString.Split("|")(0))
                ADV_Playlist.Add(LostADV(r).ToString.Split("|")(1) & "|0|" & LostADV(r).ToString.Split("|")(0))
            Next


            Return LostADV
        End If


    End Function

End Module
