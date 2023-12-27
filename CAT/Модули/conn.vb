Imports System.Net
Imports System.IO
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Text


Module conn
#Region "Variables"
    Public setting As System.Xml.Linq.XDocument
    Public A1реквест As HttpWebRequest
    Public А1респонз As HttpWebResponse
    Public A1Cookies As String
    Public Ответ As String
    Public allDone As New ManualResetEvent(False)
    Dim Поток As System.Threading.Thread
    Dim Filename As String
    Dim РоликиДляСкачки As Array
    Dim z As Integer
    Dim responseStream As Stream
    Dim md5 As String
    Public Form1 As Form1
    Public timer1 As New System.Windows.Forms.Timer
    Public ByteArr As Byte()
    Public myStreamReader As System.IO.StreamReader
    Public ИсходныйТекст As String
#End Region
    Public Sub timer1_Tick()
        ' init_session()
    End Sub

    Public Function init_session()
        Dim Domen, SubDomen, serial, domainid, Логи, sQueryString, куки As String
        Логи = ""
        Try
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11

            setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
            Domen = setting.Document.Element("Setting").Element("Domen").Attribute("Name").Value
            SubDomen = setting.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
            serial = setting.Document.Element("Setting").Element("Domen").Attribute("serial").Value
            domainid = Domen & SubDomen
            Логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value


            Запрос("https://217.72.153.37/traff/t2/chk.php", "us=cat&pw=l5G983AQnhf&subm=Вход", "", "1")

            A1реквест.GetRequestStream.Close()
            A1реквест.GetResponse.Close()


            For Each cook In А1респонз.Cookies
                куки = cook.value
            Next


            Запрос("https://217.72.153.37//traff/t2/start.php", "domainid=18d&rol=Array&usr=cat&pwd=l5G983AQnhf", куки, "2")
            A1реквест.GetRequestStream.Close()
            A1реквест.GetResponse.Close()

            sQueryString = "domainid=" & domainid & "&rol=Array&op=get_task&timeout=120&status=paused=no|playing=no|version=alpha 1.0|serial=" & serial & "|cur=" & System.IO.Path.GetFileName(Form1.play_file_name) & "|active=yes|ini=s_x.ini|"
            ИсходныйТекст = Запрос("https://217.72.153.37/traff/cat/index.php", sQueryString, куки, "3")


            Ответ = ИсходныйТекст.Split("|")(0)


            If System.IO.Directory.Exists(Логи) = False Then
                If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
                End If
                Логи = Application.StartupPath & "\Logs\"
            End If


            If System.IO.Directory.Exists(Логи) = True Then
                System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".txt", Now & " " & ИсходныйТекст & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".txt", Now & " " & ИсходныйТекст & vbNewLine, System.Text.Encoding.Default)
            End If


            Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), Now & "  " & ИсходныйТекст)

            If Ответ = "op=session_end" Then
                close(куки)
            End If
            If Ответ = "op=set_sch" Then
                get_sch(куки, ИсходныйТекст)
            End If
            If Ответ = "op=validate_sch" Then
                validate_sch(куки, ИсходныйТекст)
            End If
            If Ответ = "op=load_file" Then
                load_file(куки, ИсходныйТекст)
            End If

            A1реквест.GetRequestStream.Close()
            A1реквест.GetResponse.Close()
        Catch ex As Exception

            If Логи = "" Then
                System.IO.File.AppendAllText(Now.Date & ".err", Now & " error init_session" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".err", Now & " error init_session" & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try


        Return ИсходныйТекст

    End Function

    Public Function get_sch(ByVal куки As String, ByVal Ответ As String)
        Dim schbase64 As String = ""
        Dim sch As String
        Dim Дата As String
        Dim ДатаBase64 As String
        Dim Плейлист As Array
        Dim r As Integer
        Dim sQueryString As String
        Dim ByteArr() As Byte
        Dim ИсходныйТекст As String
        Dim логи As String
        Dim PlayListPatch As String
        логи = ""
        Try
            setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

            schbase64 = Ответ.Split("|")(2).Split("=")(2)
            While (schbase64.Length Mod 4) <> 0
                schbase64 = schbase64 & "="
            End While
            ' If (schbase64.Length Mod 4) <> 0 Then

            ' End If

            Дата = Ответ.Split("|")(2).Split("=")(1)
            ДатаBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Дата))

            sch = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(schbase64))
            Плейлист = sch.Split(ControlChars.CrLf)


            PlayListPatch = setting.Document.Element("Setting").Element("sch").Attribute("dir").Value

            System.IO.File.WriteAllText(PlayListPatch & "\" & Дата & ".txt", Плейлист(0), System.Text.Encoding.Default)



            For r = 1 To Плейлист.Length - 1
                System.IO.File.AppendAllText(PlayListPatch & "\" & Дата & ".txt", Плейлист(r), System.Text.Encoding.Default)
            Next

            логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value

            If System.IO.Directory.Exists(логи) = False Then
                If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
                End If
                логи = Application.StartupPath & "\Logs\"
            End If

            If System.IO.File.Exists(PlayListPatch & "\" & Дата & ".txt") = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " Получено расписание на " & Дата & vbNewLine, System.Text.Encoding.Default)
            End If



            A1реквест = HttpWebRequest.Create("https://217.72.153.37/traff/cat/index.php")
            A1реквест.Method = "POST"
            A1реквест.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:9.0.1) Gecko/20100101 Firefox/9.0.1"
            A1реквест.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            A1реквест.Headers.Add("Accept-Language", "en-us,en;q=0.5")
            A1реквест.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7")
            A1реквест.ContentType = "application/x-www-form-urlencoded"
            A1реквест.AllowAutoRedirect = False
            A1реквест.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" & куки)


            sQueryString = "rol=Array&op=set_sch_resp&" & "dat0=" & ДатаBase64 & "&" & "err=b2s="

            ByteArr = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sQueryString)
            Dim myStreamReader As New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.Default)
            A1реквест.ContentLength = ByteArr.Length()
            A1реквест.GetRequestStream().Write(ByteArr, 0, ByteArr.Length)


            А1респонз = A1реквест.GetResponse()
            myStreamReader = New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.Default)
            ИсходныйТекст = myStreamReader.ReadToEnd


            Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), Now & "  " & ИсходныйТекст)
            Ответ = ИсходныйТекст.Split("|")(0)
            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & "  " & ИсходныйТекст & vbNewLine, System.Text.Encoding.Default)
            If Ответ = "op=validate_sch" Then
                validate_sch(куки, ИсходныйТекст)
            Else
                close(куки)
            End If
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & " error get_sch" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & " error get_sch" & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try

        A1реквест.GetRequestStream.Close()
        A1реквест.GetResponse.Close()

        close(куки)
    End Function
    Public Function close(ByVal куки As String)
        Dim sQueryString As String
        Dim ByteArr As Byte()

        A1реквест = HttpWebRequest.Create("https://217.72.153.37/ext.php")
        A1реквест.Method = "POST"
        A1реквест.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:9.0.1) Gecko/20100101 Firefox/9.0.1"
        A1реквест.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        A1реквест.Headers.Add("Accept-Language", "en-us,en;q=0.5")
        A1реквест.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7")
        A1реквест.ContentType = "application/x-www-form-urlencoded"
        A1реквест.AllowAutoRedirect = False
        A1реквест.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" & куки)
        sQueryString = "domainid=18d&rol=Array"
        ByteArr = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sQueryString)
        A1реквест.ContentLength = ByteArr.Length()
        A1реквест.GetRequestStream().Write(ByteArr, 0, ByteArr.Length)
        А1респонз = A1реквест.GetResponse()




    End Function
    Public Function validate_sch(ByVal куки As String, ByVal ответ As String)
        Dim Дата As String
        Dim РоликиНаДень As ArrayList
        Dim r As Integer
        Dim md5 As String
        Dim reklama_str_of_day As String = ""
        Dim dat0 As String
        Dim dat0base64 As String
        Dim ИсходныйТекст As String
        Dim myStreamReader As New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.Default)
        Dim логи As String = ""
        Dim adv_patch As String

        Try
            setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
            adv_patch = setting.Document.Element("Setting").Element("adv").Attribute("dir").Value
            логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value
            Дата = ответ.Split("|")(1).Split("=")(1)
            РоликиНаДень = get_reklama_of_day(Дата)


            For r = 0 To РоликиНаДень.Count - 1
                If System.IO.File.Exists(adv_patch & "\" & РоликиНаДень(r) & ".wav") = True Then
                    md5 = get_md5(adv_patch & "\" & РоликиНаДень(r) & ".wav").ToLower
                Else
                    md5 = "-1"
                End If
                reklama_str_of_day = reklama_str_of_day & РоликиНаДень(r) & "=" & md5 & "|"
            Next

            dat0 = Дата & "=" & reklama_str_of_day


            Dim data As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(dat0)
            dat0base64 = System.Convert.ToBase64String(data)

            '
            Dim sQueryString As String
            Dim ByteArr As Byte()
            A1реквест = HttpWebRequest.Create("https://217.72.153.37/traff/cat/index.php")
            A1реквест.Method = "POST"
            A1реквест.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:9.0.1) Gecko/20100101 Firefox/9.0.1"
            A1реквест.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            A1реквест.Headers.Add("Accept-Language", "en-us,en;q=0.5")
            A1реквест.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7")
            A1реквест.ContentType = "application/x-www-form-urlencoded"
            A1реквест.AllowAutoRedirect = False
            A1реквест.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" & куки)
            sQueryString = "rol=Array&op=validate_sch_resp&dat0=" & dat0base64
            ByteArr = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sQueryString)
            A1реквест.ContentLength = ByteArr.Length()
            A1реквест.GetRequestStream().Write(ByteArr, 0, ByteArr.Length)
            А1респонз = A1реквест.GetResponse()


            myStreamReader = New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.Default)
            ИсходныйТекст = myStreamReader.ReadToEnd



            ответ = ИсходныйТекст.Split("|")(0)
            логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value

            If System.IO.Directory.Exists(логи) = False Then
                If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
                End If
                логи = Application.StartupPath & "\Logs\"
            End If

            System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & "  " & ИсходныйТекст & vbNewLine, System.Text.Encoding.Default)

            Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), Now & "  " & ИсходныйТекст)
            If ответ = "op=load_file" Then
                load_file(куки, ИсходныйТекст)
            End If

            If ответ = "op=session_end" Then
                close(куки)
            End If

            A1реквест.GetRequestStream.Close()
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  Error validate_sch" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  Error validate_sch" & vbNewLine, System.Text.Encoding.Default)
            End If

        End Try

        A1реквест.GetResponse.Close()

    End Function
    Public Function get_reklama_of_day(ByVal Дата As String)
        Dim r, n As Integer
        Dim Плейлист As Array
        Dim file_name As String
        Dim ЕстьТакой As Boolean = False
        Dim РоликиНаДень As New ArrayList
        Dim логи As String
        Dim adv_patch As String
        Dim PlayListPatch As String

        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value
        adv_patch = setting.Document.Element("Setting").Element("adv").Attribute("dir").Value
        PlayListPatch = setting.Document.Element("Setting").Element("sch").Attribute("dir").Value

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
    Public Function load_file(ByRef Куки As String, ByVal Ответ As String)
        Dim r As Integer
        Dim lenght As Integer
        Dim p As Integer
        Dim load_file_resp As String = ""
        Dim load_file_resp_base64 As String
        Dim логи As String
        Dim sQueryString As String
        Dim ByteArr As Byte()
        Dim adv_patch As String

        РоликиДляСкачки = Ответ.Split("|")


        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value
        adv_patch = setting.Document.Element("Setting").Element("adv").Attribute("dir").Value
        '   Form1.Timer3.Stop()
        If System.IO.Directory.Exists(логи) = False Then
            If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
            End If
            логи = Application.StartupPath & "\Logs\"
        End If

        For r = 2 To РоликиДляСкачки.Length - 1
            If РоликиДляСкачки(r).ToString.Contains("url") = True Then
                Try
                    Filename = РоликиДляСкачки(r).ToString.Split("=")(3)
                    Filename = System.IO.Path.GetFileName(Filename)

                    A1реквест = HttpWebRequest.Create("https://217.72.153.37/traff/script/SFDL.php?xPathToFile=" & РоликиДляСкачки(r).ToString.Split("=")(3))
                    A1реквест.Method = "POST"
                    A1реквест.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" & Куки)
                    A1реквест.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:9.0.1) Gecko/20100101 Firefox/9.0.1"
                    A1реквест.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
                    A1реквест.Headers.Add("Accept-Language", "en-us,en;q=0.5")
                    A1реквест.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7")
                    A1реквест.ContentType = "application/x-www-form-urlencoded"
                    sQueryString = "domainid=18d&rol=Array&op=get_task&timeout=120&status=paused=no|playing=no|version=1.0.4.50|serial=18d_1081872615_2214695837_|cur=|active=yes|ini=s_x.ini|"
                    ByteArr = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sQueryString)
                    A1реквест.ContentLength = ByteArr.Length()
                    A1реквест.GetRequestStream().Write(ByteArr, 0, ByteArr.Length)
                    А1респонз = A1реквест.GetResponse()
                    lenght = А1респонз.ContentLength
                    responseStream = А1респонз.GetResponseStream



                    Dim fs As New FileStream(adv_patch & "\" & Filename, FileMode.Create)
                    Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), "Качаем ролик " & Filename)
                    responseStream.CopyTo(fs)
                    fs.Close()
                    System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & "  Загружен ролик " & Filename & vbNewLine, System.Text.Encoding.Default)


                    md5 = get_md5(adv_patch & "\" & Filename).ToLower
                    Dim data As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(md5)
                    Dim nnn As String = Filename & "=" & md5
                    Dim nnn_base64 As String
                    Dim data1 As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(nnn)
                    nnn_base64 = System.Convert.ToBase64String(data1)
                    load_file_resp_base64 = System.Convert.ToBase64String(data1)
                    load_file_resp = load_file_resp & "file" & p.ToString & "=" & nnn_base64 & "&"
                    p += 1
                    z += 1
                Catch ex As Exception
                    If System.IO.Directory.Exists(логи) = True Then
                        System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "   Error load file " & Filename & vbNewLine, System.Text.Encoding.Default)
                    Else
                        System.IO.File.AppendAllText(Now.Date & ".err", Now & "   Error load file " & Filename & vbNewLine, System.Text.Encoding.Default)
                    End If
                    GoTo next_file
                End Try

            End If
next_file:
        Next

        Try
            load_file_resp = "rol=Array&op=load_file_resp&" & load_file_resp & "err=b2s="
            A1реквест = HttpWebRequest.Create("https://217.72.153.37/traff/cat/index.php")
            A1реквест.Method = "POST"
            A1реквест.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:9.0.1) Gecko/20100101 Firefox/9.0.1"
            A1реквест.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            A1реквест.Headers.Add("Accept-Language", "en-us,en;q=0.5")
            A1реквест.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7")
            A1реквест.ContentType = "application/x-www-form-urlencoded"
            A1реквест.AllowAutoRedirect = False
            A1реквест.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" & Куки)
            sQueryString = load_file_resp
            ByteArr = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sQueryString)
            A1реквест.ContentLength = ByteArr.Length()
            A1реквест.GetRequestStream().Write(ByteArr, 0, ByteArr.Length)

            А1респонз = A1реквест.GetResponse()

            Dim myStreamReader As New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.Default)
            Dim ИсходныйТекст As String
            myStreamReader = New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.Default)
            ИсходныйТекст = myStreamReader.ReadToEnd


            A1реквест.GetRequestStream.Close()
            A1реквест.GetResponse.Close()

            Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), ИсходныйТекст)
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "   Error validate md5 checksumm " & Filename & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "   Error validate md5 checksumm " & Filename & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try



        close(Куки)

        '   Form1.Timer3.Start()


    End Function
    Public Function get_md5(ByVal filename As String) As String
        Using md5 As MD5 = MD5.Create()
            Using stream = File.OpenRead(filename)
                Return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty)
            End Using
        End Using
    End Function

    Public Function Запрос(ByRef url As String, ByVal sQueryString As String, ByVal куки As String, ByVal шаг As String)
        Dim логи As String = ""
        Try

            setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
            логи = setting.Document.Element("Setting").Element("logs").Attribute("dir").Value

            If System.IO.Directory.Exists(логи) = False Then
                If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
                End If
                логи = Application.StartupPath & "\Logs\"
            End If

            A1реквест = HttpWebRequest.Create(url)
            A1реквест.Method = "POST"
            A1реквест.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:9.0.1) Gecko/20100101 Firefox/9.0.1"
            A1реквест.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            A1реквест.Headers.Add("Accept-Language", "en-us,en;q=0.5")
            A1реквест.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7")
            A1реквест.ContentType = "application/x-www-form-urlencoded"
            A1реквест.AllowAutoRedirect = False

            If шаг = "1" Then
                A1реквест.CookieContainer = New CookieContainer
            End If
            If куки <> "" Then
                A1реквест.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=" & куки)
            End If

            ByteArr = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(sQueryString)
            A1реквест.ContentLength = ByteArr.Length()
            A1реквест.GetRequestStream().Write(ByteArr, 0, ByteArr.Length)
            А1респонз = A1реквест.GetResponse()
            myStreamReader = New System.IO.StreamReader(А1респонз.GetResponseStream, System.Text.Encoding.UTF8)

            ИсходныйТекст = myStreamReader.ReadToEnd
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  Error init session" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  Error init session" & vbNewLine, System.Text.Encoding.Default)
            End If

        End Try
        Return ИсходныйТекст
    End Function

End Module
