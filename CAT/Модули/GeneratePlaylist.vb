Imports System.Data.OleDb
Imports System.IO

Module GeneratePlaylist
    Dim Настройки As System.Xml.Linq.XDocument
    Dim ВсеПесни As ArrayList
    Dim PlayList As ArrayList
    Dim ПромежуточныйПлейлист As System.Xml.Linq.XDocument
    Public ActivePlaylist As ArrayList
    'Public TempPlaylist As ArrayList
    Dim connect As OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Public БазаПустая As Boolean
    Private Delegate Sub PBUpdate(ByVal Value As Integer)


    ''' <summary>
    ''' Получаем список всех песени во всех каталогах
    ''' </summary>
    Public Sub GetAllSong()
        Dim i, j As Integer
        Dim ВсегоПесен As Integer
        Dim ПесниВТекущейПапке As New ArrayList
        Dim xe As XElement
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        ВсеПесни = New ArrayList
        '  PlayList = New ArrayList
        ' Try
        ПромежуточныйПлейлист = New XDocument
        ПромежуточныйПлейлист = <?xml version="1.0"?>
                                <плейлист>
                                </плейлист>

        For Each songs In Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements
            ПесниВТекущейПапке.Clear()
            If System.IO.Directory.Exists(songs.Attribute("dir_patch").Value) = True Then
                ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.mp3", SearchOption.AllDirectories))
                ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.wav", SearchOption.AllDirectories))
                ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.wma", SearchOption.AllDirectories))
                ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.aac", SearchOption.AllDirectories))
                ВсеПесни.Insert(ВсеПесни.Count, ПесниВТекущейПапке)

                If ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i) Is Nothing Then
                    xe = New XElement("dir" & i, "")
                    ПромежуточныйПлейлист.Document.Element("плейлист").Add(xe)
                End If
                ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).SetAttributeValue("Вес", songs.Attribute("Вес").Value)

                For j = 0 To ПесниВТекущейПапке.Count - 1
                    If ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Element("song" & j) Is Nothing Then
                        xe = New XElement("song" & j, "")
                        ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Add(xe)
                    End If
                    ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Element("song" & j).SetAttributeValue("songpatch", ПесниВТекущейПапке(j))
                    ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Element("song" & j).SetAttributeValue("Use", "0")
                    ВсегоПесен += 1
                Next

                i += 1
            End If

        Next
        MusicPlaylist()
        ' GenPlaylist()
    End Sub
    ''' <summary>
    ''' Гненерируем плейлист в заивисомти от веса диреткорий
    ''' </summary>
   ' Public Function GenPlaylist()
    '    Dim НомерСлучайнойПесни As Integer
    '    Dim randd As New Random()
    '    Dim e As Integer
    '    Dim Вероятности As New ArrayList
    '    Dim Диапазоны As New ArrayList
    '    Dim dir_name As String
    '    Dim random As Integer
    '    Dim UnUsedSong As New ArrayList
    '    Dim H As Integer
    '    Dim dt As New DataTable
    '    Dim dbpatch As String
    '    Dim ProfileName As String
    '    Dim Profile As XDocument
    '    Dim Домен As String
    '    Dim логи As String


    '    ActivePlaylist = New ArrayList
    '    TempPlaylist = New ArrayList

    '    Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
    '    ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

    '    If System.IO.File.Exists(ProfileName) = True Then
    '        Profile = XDocument.Load(ProfileName)
    '    Else
    '        Profile = XDocument.Load("Настройки.xml")
    '    End If


    '    dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
    '    H = Profile.Document.Element("Setting").Element("music").Attribute("PlayListTimeCheck").Value

    '    логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value

    '    Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
    '    Try
    '        If System.IO.File.Exists(dbpatch) = True Then
    '            If MusicSetting.CheckRepeat = 1 Then
    '                Try
    '                    Dim adapter As OleDbDataAdapter
    '                    connect = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
    '                    connect.Open()
    '                    cmd = New OleDb.OleDbCommand("", connect)
    '                    cmd.CommandText = "SELECT * FROM Log WHERE StartDate between @Dstart and @Dend"

    '                    cmd.Parameters.AddWithValue("@Dstart", DateAdd(DateInterval.Hour, H, Now).ToString)
    '                    cmd.Parameters.AddWithValue("@Dend", Now.ToString)

    '                    adapter = New OleDbDataAdapter(cmd)
    '                    adapter.Fill(dt)
    '                    cmd.ExecuteNonQuery()
    '                    connect.Close()
    '                    БазаПустая = False
    '                Catch ex As Exception
    '                    'MsgBox("df")
    '                    БазаПустая = True
    '                End Try

    '            End If


    '            For Each var In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
    '                Вероятности.Add(var.Attribute("Вес").Value & "|" & var.Name.ToString)
    '            Next

    '            For y = 0 To Вероятности.Count - 1
    '                If Диапазоны.Count = 0 Then
    '                    Диапазоны.Add("0-" & Вероятности(y).ToString.Split("|")(0) & "|" & Вероятности(y).ToString.Split("|")(1))
    '                Else
    '                    Диапазоны.Add(Диапазоны(y - 1).ToString.Split("|")(0).Split("-")(1) + 1 & "-" & CDbl(Диапазоны(y - 1).ToString.Split("|")(0).Split("-")(1)) + CDbl(Вероятности(y).ToString.Split("|")(0)) & "|" & Вероятности(y).ToString.Split("|")(1))
    '                End If
    '            Next



    '            If ПромежуточныйПлейлист.Document.Element("плейлист").Elements.Count <> 0 Then
    '                For iii = 0 To 99
    '                    random = CInt(Int((Rnd() * 100) + 1))

    '                    '  Form1.Invoke(New Form1.PBUpdate(AddressOf Form1.PB2Update), Now & "  " & ИсходныйТекст)
    '                    '   Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), Now & "  " & ИсходныйТекст)
    '                    Form1.Invoke(New PBUpdate(AddressOf Form1.PB2Update), iii)
    '                    For vvv = 0 To Диапазоны.Count - 1
    '                        If random >= Диапазоны(vvv).ToString.Split("|")(0).Split("-")(0) And random <= Диапазоны(vvv).ToString.Split("|")(0).Split("-")(1) Then
    '                            dir_name = Диапазоны(vvv).ToString.Split("|")(1)
    '                            '   UnUsedSong = GetUnUsedSong(ПромежуточныйПлейлист, dir_name, dt)
    '                            Exit For
    '                        End If
    '                    Next





    '                    If MusicSetting.CheckRepeat = 1 And БазаПустая = False Then 'Проверяем трэк на повтор за указанное количсество часов
    '                        НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
    '                        For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
    '                            If e = НомерСлучайнойПесни Then
    '                                If CheckTrackRepeat(dt, elem.Attribute("songpatch").Value) = False Then
    '                                    TempPlaylist.Add(elem.Attribute("songpatch").Value)
    '                                Else
    '                                    ' GetUnUsedSong(ПромежуточныйПлейлист, dir_name, dt)
    '                                    '  TempPlaylist.Add("была такая")
    '                                    UnUsedSong = GetUnUsedSong(ПромежуточныйПлейлист, dir_name, dt)
    '                                    НомерСлучайнойПесни = randd.Next(0, UnUsedSong.Count - 1)
    '                                    TempPlaylist.Add(UnUsedSong(НомерСлучайнойПесни))
    '                                End If
    '                                e = 0
    '                                Exit For
    '                            End If
    '                            e += 1

    '                        Next
    '                    Else
    '                        НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
    '                        For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
    '                            If e = НомерСлучайнойПесни Then
    '                                TempPlaylist.Add(elem.Attribute("songpatch").Value)

    '                                e = 0
    '                                Exit For
    '                            End If
    '                            e += 1
    '                        Next
    '                    End If

    '                    Form1.Invoke(New Form1.MusicListUpdate(AddressOf Form1.MusicListAdd), TempPlaylist.Item(TempPlaylist.Count - 1))


    '                Next


    '                Dim okok As New Random(System.DateTime.Now.Millisecond)
    '                Dim ss As Integer
    '                For b = 0 To TempPlaylist.Count - 1
    '                    ss = okok.Next(0, TempPlaylist.Count - 1)
    '                    ActivePlaylist.Add(TempPlaylist(ss))
    '                    TempPlaylist.RemoveAt(ss)
    '                Next
    '                For b = 0 To ActivePlaylist.Count - 1
    '                    ' Me.Invoke(New Form1.MusicListUpdate(AddressOf Me.MusicListAdd), ActivePlaylist.Item(b))
    '                    ' Form1.Invoke(New Form1.ListBoxpdateDelegate(AddressOf Form1.ListBoxUpdate), ActivePlaylist.Item(b))

    '                    'Form1.Invoke(New Form1.ListBoxpdateDelegate(AddressOf Form1.ListBoxUpdate), ActivePlaylist.Item(b))
    '                    ' Form1.MusicList.Items.Add(ActivePlaylist.Item(b))
    '                    '   Form1.BeginInvoke(New ListBoxpdateDelegate(AddressOf Form1.ListBoxUpdate), "dfhdfh")
    '                    ' Form1.MusicList.Invoke(New Form1.ListBoxpdateDelegate(AddressOf Form1.ListBoxUpdate), "dfgdfgf")
    '                Next
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If System.IO.Directory.Exists(логи) = True Then
    '            System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_gen_music_playlist " & vbNewLine, System.Text.Encoding.Default)
    '        End If
    '    End Try
    'End Function


    Public Function CheckTrackRepeat(ByVal D_T As DataTable, ByVal filename As String) As Boolean
        Dim r As Integer
        Dim БылаТакая As Boolean


        For r = 0 To D_T.Rows.Count - 1
            If D_T(r)(5).ToString.Trim = System.IO.Path.GetFileName(filename) Then
                Return True
            End If

        Next

        Return False




    End Function

    Public Sub PB2Update(ByVal value As String)
        Form1.ProgressBar2.Value = value
    End Sub

    Public Sub HandPlaylist()

    End Sub


    Public Sub MusicPlaylist()
        Dim Profile As XDocument
        Dim dbpatch As String
        Dim H_Repeat As Integer
        Dim connect As OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim dt As New DataTable
        Dim Диапазоны As New ArrayList
        Dim Вероятности As New ArrayList
        Dim r As Integer
        Dim i As Integer
        Dim d As Integer
        Dim dir_name As String
        Dim НомерСлучайнойПесни As Integer
        Dim Rand As New Random
        Dim e As Integer
        Dim UnUsedSong As New ArrayList
        Dim TempPlaylist As New ArrayList




        Profile = Templates.Load

        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
        H_Repeat = Profile.Document.Element("Setting").Element("music").Attribute("PlayListTimeCheck").Value


        Try
            If System.IO.File.Exists(dbpatch) = True Then
                If MusicSetting.CheckRepeat = 1 Then
                    Dim adapter As OleDbDataAdapter
                    connect = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
                    connect.Open()
                    cmd = New OleDb.OleDbCommand("", connect)
                    cmd.CommandText = "SELECT * FROM Log WHERE StartDate between @Dstart and @Dend"

                    cmd.Parameters.AddWithValue("@Dstart", DateAdd(DateInterval.Hour, H_Repeat, Now).ToString)
                    cmd.Parameters.AddWithValue("@Dend", Now.ToString)

                    adapter = New OleDbDataAdapter(cmd)
                    adapter.Fill(dt)
                    cmd.ExecuteNonQuery()
                    connect.Close()
                    БазаПустая = False
                End If

            End If
        Catch ex As Exception
            БазаПустая = True
        End Try

        For Each var In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
            Вероятности.Add(var.Attribute("Вес").Value & "|" & var.Name.ToString)
        Next

        For y = 0 To Вероятности.Count - 1
            If Диапазоны.Count = 0 Then
                Диапазоны.Add("0-" & Вероятности(y).ToString.Split("|")(0) & "|" & Вероятности(y).ToString.Split("|")(1))
            Else
                Диапазоны.Add(Диапазоны(y - 1).ToString.Split("|")(0).Split("-")(1) + 1 & "-" & CDbl(Диапазоны(y - 1).ToString.Split("|")(0).Split("-")(1)) + CDbl(Вероятности(y).ToString.Split("|")(0)) & "|" & Вероятности(y).ToString.Split("|")(1))
            End If
        Next

        If ПромежуточныйПлейлист.Document.Element("плейлист").Elements.Count <> 0 Then
            For r = 0 To 99
                i = CInt(Int((Rnd() * 100) + 1))
                For d = 0 To Диапазоны.Count - 1
                    If i >= Диапазоны(d).ToString.Split("|")(0).Split("-")(0) And i <= Диапазоны(d).ToString.Split("|")(0).Split("-")(1) Then
                        dir_name = Диапазоны(d).ToString.Split("|")(1)
                        Exit For
                    End If
                Next
                If MusicSetting.CheckRepeat = 1 And БазаПустая = False Then
                    НомерСлучайнойПесни = Rand.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                    For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                        If e = НомерСлучайнойПесни Then
                            If CheckTrackRepeat(dt, elem.Attribute("songpatch").Value) = False Then
                                TempPlaylist.Add(elem.Attribute("songpatch").Value)
                                elem.Remove()
                                ' Exit For
                            Else
                                UnUsedSong = GetUnUsedSong(ПромежуточныйПлейлист, dir_name, dt)
                                НомерСлучайнойПесни = Rand.Next(0, UnUsedSong.Count - 1)
                                TempPlaylist.Add(UnUsedSong(НомерСлучайнойПесни))
                                '  Exit For
                            End If
                            e = 0
                            Exit For
                        End If
                        e += 1

                    Next
                Else
                    НомерСлучайнойПесни = Rand.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                    For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                        If e = НомерСлучайнойПесни Then
                            TempPlaylist.Add(elem.Attribute("songpatch").Value)
                            elem.Remove()
                            e = 0
                            Exit For
                        End If
                        e += 1
                    Next
                End If
                Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), Now & "  " & ИсходныйТекст)
                '   Form1.Invoke(New PBUpdate(AddressOf Form1.PB2Update), i)
            Next
        End If

    End Sub


    Private Function GetUnUsedSong(ByVal TMPPlaylist As XDocument, ByVal dir As String, ByVal dt As DataTable) As ArrayList
        Dim UnUsedSong As New ArrayList
        Dim БылаТакая As Boolean

        For Each elem In TMPPlaylist.Document.Element("плейлист").Element(dir).Elements
            For r = 0 To dt.Rows.Count - 1
                If dt.Rows(r).Item(5).ToString.Trim = System.IO.Path.GetFileName(elem.Attribute("songpatch").Value) Then
                    БылаТакая = True
                    Exit For
                End If
            Next
            If БылаТакая = False Then
                UnUsedSong.Add(elem.Attribute("songpatch").Value)
            End If
            If БылаТакая = True Then
                БылаТакая = False
            End If
        Next


        Return UnUsedSong

    End Function


    Private Sub form1pbbupdate()

    End Sub
End Module
