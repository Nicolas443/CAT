Imports System.IO
Imports System.Threading
Imports System.Data.OleDb
Imports Un4seen.Bass
Imports Un4seen.Bass.Misc
Imports Un4seen.Bass.AddOn.Mix
Imports Un4seen.Bass.AddOn.Enc
Imports System.Data.SQLite
Imports Un4seen.BassAsio

Public Class Form1
#Region "Variables3"
    Public Настройки As System.Xml.Linq.XDocument
    Public НеИспользованныеПесни As XDocument

    Public Delegate Sub LabelUpdateDelegate(ByVal Value As String)
    Public Delegate Sub MusicListUpdate(ByVal Value As String)
    Public Delegate Sub MusicListUpdateClear(ByVal Value As String)
    Public Delegate Sub ListBox1ListUpdateClear(ByVal Value As String)


    Public Delegate Sub PBUpdate(ByVal Value As Integer)
    Public Delegate Sub ADVCheckDelegate(ByVal Value As XDocument)
    Public Delegate Sub DinPl(ByVal Value As String)


    Dim Сработка As New ArrayList
    Dim TempPlaylist As ArrayList
    Public ТекущийЦиклПесен As ArrayList
    Public ВсеПесни As ArrayList
    Public TempAlarmList As ArrayList = New ArrayList

    Public LostADV As ArrayList

    Dim iii As Integer

    Public Поток_conn As Thread

    Public БазаПустая As Boolean
    Dim file() As String

    Public ВременныйПлейлист As XDocument


    Public play_file_name As String
    Public conn_db As OleDb.OleDbConnection
    Public conn_t_sate As Boolean

    Public ПромежуточныйПлейлист As System.Xml.Linq.XDocument
    Public ПромежуточныйПлейлист_дин As System.Xml.Linq.XDocument

    Public CureTime, PLTime As String

    Public alarm_pl As Array
    Public alarm_pl_jingle As ArrayList


    Public alarm_file As String
    Public alarmpl_ As ArrayList

    Public th As Thread
    Public th2 As Thread

    Public ПлейлистОбъявлений As New ArrayList
    Public Обявления As System.Xml.Linq.XDocument
    Dim connect As OleDbConnection
    Dim cmd As OleDb.OleDbCommand

    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
#End Region


    'Dim lame As New EncoderLAME(Play.stream_music)
    'Dim icecast As New ICEcast(lame, True)
    'Dim _broadCast2 As BroadCastByVal dbpatch As String
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Public Structure SRC
        Public filename As String
        Public start As Double
        Public fadein As Double
        Public fadeout As Double
    End Structure
    Public Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        conn.Form1 = Me
        Dim dbpatch As String
        Dim Домен As String
        Dim app_patch As String
        Dim логи As String
        ADV_Playlist = New ArrayList
        '   ChangeProgBarColor(ProgressBar1, ProgressBarColor.Red)
        'Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, True)

        Me.AllowDrop = True

        CheckSetting.Check()

        LostADV = New ArrayList
        LostADV = ADV.GetLostADV()
        ' Setting_Check()
        ' Telega.Bot()
        adv_check.Start()
        ' Media_info.Start()
        VolumeCheck.Start()
        AlarmCheck.Start()
        Настройки = XDocument.Load("настройки.xml")

        Dim Profile As XDocument
        Dim ProfileName As String

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value


        Play.ДинДирекорияОстановлена = True
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile = XDocument.Load(ProfileName)
        'Else
        '    Profile = XDocument.Load("Настройки.xml")
        'End If




        'If Profile.Document.Element("Setting").Element("music").Attribute("mode").Value = "broadcast" Then
        '    BroadCastMode.Checked = True
        'End If
        'If Profile.Document.Element("Setting").Element("music").Attribute("mode").Value = "playlist" Then
        '    PlayListMode.Checked = True
        'End If

        ' If BroadCastMode.Checked = True Then
        Поток()
        Поток_DinSong()

        ' End If

        '  Voltimer = New System.Timers.Timer(1000)

        Templates.GetProfileList()

        ' GeneratePlaylist.GetAllSong()


        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        dbpatch = Настройки.Document.Element("Setting").Element("db").Attribute("file").Value

        Домен = Настройки.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
        app_patch = My.Application.Info.DirectoryPath
        логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value

        '  GetMusicPlaylist(логи)
        '
        Старт_Стоп_CONN()

        '  RandomPlaylist(10)


        ' th=New Thread (AddressOf get

        Me.Text = "CAT"

        Label2.Text = SettingDomen.Name & SettingDomen.SubDomen

        HostInformation.HostInfo()


        Profile = GenerlaSetting.ProfileLoad


        If Profile.Document.Element("Setting").Element("music").Attribute("OpenLastPL").Value = True Then
            PlayListMode.Checked = True
        End If

        If Profile.Document.Element("Setting").Element("music").Attribute("OpenLastPL").Value = False Then
            BroadCastMode.Checked = True
        End If

        Try

            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".txt", Now & " " & "Запуск плеера" & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".txt", Now & " " & "Запуск плеера" & vbNewLine, System.Text.Encoding.Default)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Старт()

    End Sub

    Public Sub advcheckupdate(ByVal value As XDocument)
        ListView3.Items.Clear()

        For Each elem In value.Elements("Check_rez").Elements
            'ListView1.Items.Add(elem.Attribute("time").Value)
            ''   ListView1.Items.Add(elem.Attribute("TimeFact").Value)
            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Attribute("TimeFact").Value)
            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Attribute("adv").Value)
            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Attribute("size").Value)
            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Attribute("Status").Value)
            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Attribute("Exist").Value)
            ListView3.Items.Add(elem.Attribute("time").Value)
            '   ListView1.Items.Add(elem.Attribute("TimeFact").Value)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(elem.Attribute("TimeFact").Value)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(elem.Attribute("adv").Value)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(elem.Attribute("size").Value)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(elem.Attribute("Status").Value)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(elem.Attribute("Exist").Value)
        Next


    End Sub
    Public Sub DinPlUpdate(ByVal value As String)
        'For Each elem In value.Document.Element("плейлист").Elements
        '    For Each song In elem.Elements
        '        ListView1.Items.Add(song.Attribute("songpatch").Value)
        '        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Attribute("TimeStart").Value & "-" & elem.Attribute("TimeStop").Value)
        '    Next


        'Next
        '   ListView1.Items.Add(value)
        ListBox1.Items.Add(value)

    End Sub



    Private Sub НастройкиToolStripMenuItem_Click(sender As Object, e As EventArgs)
        setting.ShowDialog()
    End Sub
    Public Sub Стоп()
        Dim Логи As String = SettingLog.DirPatch
        Try

            Bass.BASS_ChannelStop(Play.stream_music)
            Bass.BASS_StreamFree(Play.stream_music)

            Bass.BASS_ChannelStop(Play.stream2)
            Bass.BASS_StreamFree(Play.stream2)


            Bass.BASS_ChannelStop(Play.stream_adv)
            Bass.BASS_StreamFree(Play.stream_adv)

            Bass.BASS_ChannelStop(Play.stream_alarm)
            Bass.BASS_StreamFree(Play.stream_alarm)


            ADV.ADV_Playlist.Clear()




            Play.stream_music = 0
            Play.stream2 = 0
            Play.stream_alarm = 0
            Play.stream_adv = 0

            Play.ВремяADV = False
            Play.ВремяAlarm = False
            Play.ПесняНаПаузе = False





            Bass.BASS_Free()
            '   End If
            '   Media_info.Stop()
            VolumeCheck.Stop()
            ' AlarmCheck.Stop()
            ' adv_check.Stop()
            Label1.Text = "0"
            ChangeProgBarColor(ProgressBar1, ProgressBarColor.Green)
            ProgressBar1.Value = 0




            Label1.Text = "0"

            Play.ВсёОстановлено = True

            Play.Реклама_Играется = False
            Play.Объявление_Играется = False

            Me.Text = "CAT"

            If System.IO.Directory.Exists(Логи) = False Then

                If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                    System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
                End If
                Логи = Application.StartupPath & "\Logs\"
            End If

            System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".txt", Now & " " & "Нажата кнопка Stop" & vbNewLine, System.Text.Encoding.Default)




        Catch ex As Exception

        End Try

    End Sub
    Public Sub Старт()
        Dim Логи As String = SettingLog.DirPatch
        Bass.BASS_ChannelStop(Play.stream_music)
        Bass.BASS_StreamFree(Play.stream_music)
        Bass.BASS_ChannelStop(Play.stream_adv)
        Bass.BASS_StreamFree(Play.stream_adv)
        Bass.BASS_ChannelStop(Play.stream_alarm)
        Bass.BASS_StreamFree(Play.stream_alarm)

        Bass.BASS_Free()
        VolumeCheck.Start()
        Play.Play("Music")
        Play.ВремяADV = False
        Play.ВремяAlarm = False
        Play.ВремяМузыки = True
        AlarmCheck.Start()
        adv_check.Start()

        'Play.ВсёОстановлено = False


        '  MediaInfo.Play("Music", "Music")
        If System.IO.Directory.Exists(Логи) = False Then

            If System.IO.Directory.Exists(Application.StartupPath & "\Logs") = False Then
                System.IO.Directory.CreateDirectory(Application.StartupPath & "\Logs")
            End If
            Логи = Application.StartupPath & "\Logs\"
        End If


        System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".txt", Now & " " & "Нажата кнопка PLay" & vbNewLine, System.Text.Encoding.Default)
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Profile As XDocument
        Dim dbpatch As String
        Dim ПлейлистОбъяв As Array
        Dim alarm_mess_patch As String
        Dim r As Integer
        Dim Домен As String
        Dim Логи As String
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
        Логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value

        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value

        If Play.stream_music <> 0 Or Play.stream2 <> 0 Then
            DataBase.UpdateDB(dbpatch, LastMusicID)
        End If
        If Play.stream_adv <> 0 Then
            DataBase.UpdateDB(dbpatch, LastADVId)
        End If
        If Play.stream_alarm <> 0 Then
            DataBase.UpdateDB(dbpatch, LastAlarmId)
        End If
        Try
            If Profile.Document.Element("Setting").Element("alarm").Attribute("ClearDir").Value = True Then
                'alarm_mess_patch = Настройки.Document.Element("Setting").Element("alarm").Attribute("dir").Value
                ' ПлейлистОбъяв = System.IO.Directory.GetFiles(alarm_mess_patch)

                For r = 0 To ПлейлистОбъявлений.Count - 1
                    System.IO.File.Delete(ПлейлистОбъявлений(r))
                Next
                AlarmList.Items.Clear()
                ПлейлистОбъявлений.Clear()
            End If
        Catch ex As Exception
            If System.IO.Directory.Exists(Логи) = True Then
                System.IO.File.AppendAllText(Логи & "\" & Домен & "_" & Now.Date & ".err", Now & "  " & "Error clear alarm dir" & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try

        Стоп()


    End Sub

    Public Sub GetListViewItem(ByVal Value As String)
        Play.play_file_name = MusicList.Items(0)
    End Sub
    Public Sub LabelUpdate(ByVal Value As String)
        RichTextBox1.Text = Value
    End Sub
    Private Sub adv_check_Tick(sender As Object, e As EventArgs) Handles adv_check.Tick
        ADV.ADV()



    End Sub
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        '  conn.init_session()
        Поток_conn = New Thread(AddressOf conn.init_session)
        Поток_conn.Start()
    End Sub
    Private Sub musicfadeout_Tick(sender As Object, e As EventArgs) Handles musicfadeout.Tick
        If Play.ВремяADV = True Then
            ' ВремяРекламы = False
            Play.Fade("ADV")
        End If
        If Play.ВремяМузыки = True Then
            Play.Fade("Music")
        End If
        If Play.ВремяAlarm = True Then
            Play.Fade("Alarm")
        End If
        If Play.ВремяJingle = True Then
            Play.Fade("Jingle")
        End If


    End Sub
    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        Поток_conn = New Thread(AddressOf conn.init_session)
        Поток_conn.Start()
    End Sub
    Private Sub НастройкиToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        setting.ShowDialog()
    End Sub
    Private Sub ВыходToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ВыходToolStripMenuItem.Click
        Dim Profile As XDocument
        Dim Логи As String

        Profile = GenerlaSetting.ProfileLoad

        Логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value

        If MsgBox("Хотите выйти?", MsgBoxStyle.YesNo, "Выход") = MsgBoxResult.Yes Then
            My.Settings.Save()
            Me.Close()
            Try
                If System.IO.Directory.Exists(Логи) = True Then
                    System.IO.File.AppendAllText(Логи & "\" & Now.Date & ".txt", Now & " " & "Закрыли плеер" & vbNewLine, System.Text.Encoding.Default)
                Else
                    System.IO.File.AppendAllText(Now.Date & ".txt", Now & " " & "Закрыли плеер" & vbNewLine, System.Text.Encoding.Default)
                End If
            Catch ex As Exception

            End Try

        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Старт_Стоп_CONN()
    End Sub
    Private Sub Старт_Стоп_CONN()
        If conn_t_sate = False Then
            conn_t_sate = True
            Button4.Text = "STOP-CONN"
            Timer3.Start()
            GoTo end_sub
        End If
        If conn_t_sate = True Then
            conn_t_sate = False
            Button4.Text = "START-CONN"
            Timer3.Stop()
            GoTo end_sub
        End If

end_sub:
    End Sub
    Private Sub ОПрограммеToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ОПрограммеToolStripMenuItem.Click
        About.ShowDialog()
    End Sub
    Private Sub VolumeCheck_Tick(sender As Object, e As EventArgs) Handles VolumeCheck.Tick
        StartStop.StartStop()
    End Sub
    Private Sub AlarmCheck_Tick(sender As Object, e As EventArgs) Handles AlarmCheck.Tick
        Dim alarm_mess_patch As String
        Dim Jingle As String
        Dim r, i As Integer
        Dim FileInfo As System.IO.FileInfo
        Dim ЕстьТакой As Boolean
        Dim Profile As XDocument


        Try
            Profile = GenerlaSetting.ProfileLoad


            alarm_mess_patch = Profile.Document.Element("Setting").Element("alarm").Attribute("dir").Value

            If Now.Hour = "00" And Now.Minute = "00" And Now.Second = "00" Then ПлейлистОбъявлений.Clear()

            If System.IO.Directory.Exists(alarm_mess_patch) = True Then


                alarm_pl = System.IO.Directory.GetFiles(alarm_mess_patch)
                Jingle = Profile.Document.Element("Setting").Element("Jingle").Attribute("Jingle").Value
                alarm_pl_jingle = New ArrayList

                If alarm_pl.Length > 0 Then
                    For r = 0 To alarm_pl.Length - 1

                        FileInfo = My.Computer.FileSystem.GetFileInfo(alarm_pl(r))
                        '   FileInfo.l
                        ' If FileInfo.Length > 0 Then


                        For i = 0 To TempAlarmList.Count - 1
                            If TempAlarmList(i).ToString.Split("|")(0) = alarm_pl(r) Then
                                If TempAlarmList(i).ToString.Split("|")(1) = FileInfo.Length Then
                                    If ПлейлистОбъявлений.Contains(alarm_pl(r)) = False Then

                                        ПлейлистОбъявлений.Add(alarm_pl(r))
                                        AlarmList.Items.Add(alarm_pl(r))
                                        Media_info.Start()
                                    End If
                                Else
                                    TempAlarmList(i) = TempAlarmList(i).ToString.Split("|")(0) & "|" & FileInfo.Length
                                End If
                                ЕстьТакой = True
                            End If
                        Next


                        If ЕстьТакой = False Then
                            TempAlarmList.Add(alarm_pl(r) & "|" & FileInfo.Length)
                        End If
                        ЕстьТакой = False
                        ' End If
                    Next

                End If

                If ПлейлистОбъявлений.Count > 0 Then
                    Play.ВремяAlarm = True
                End If
            End If
        Catch ex As Exception
            '  MsgBox(ex.Message)
        End Try

        'If alarm_pl.Length > 0 Then
        '    For r = 0 To alarm_pl.Length - 1
        '        If ПлейлистОбъявлений.Contains(alarm_pl(r)) = False Then
        '            ПлейлистОбъявлений.Add(alarm_pl(r))
        '            AlarmList.Items.Add(alarm_pl(r))
        '        End If
        '    Next
        'End If



        'killtimeout = Настройки.Document.Element("Setting").Element("music").Attribute("killtimeout").Value

        'Настройки.Document.Element("Setting").Element("music").SetAttributeValue("killtimeout", "1000")
        'Настройки.Save("Настройки.xml")
        'Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'killtimeout = Настройки.Document.Element("Setting").Element("music").Attribute("killtimeout").Value






    End Sub
    Private Sub Media_info_Tick(sender As Object, e As EventArgs) Handles Media_info.Tick
        Play.M_Info()
        ' MediaInfo.MediaInfo()
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs)
        Telega.Get_text()
    End Sub

    Private Sub TG_Tick(sender As Object, e As EventArgs) Handles TG.Tick
        'Telega.Get_text()
        'Telega.Bot()
        Dim TG_Thread As Thread
        TG_Thread = New Thread(AddressOf Telega.Get_text)
        '  TG_Thread.IsBackground = True
        TG_Thread.Start()
        'Telega.Get_text()
    End Sub


    Private Sub Button3_Click_7(sender As Object, e As EventArgs)
        Media_info.Start()
    End Sub

    Private Sub Timer7_Tick(sender As Object, e As EventArgs) Handles Timer7.Tick
        '  Label2.Text = Format(CInt(Now.Hour), "00") & ":" & Format(CInt(Now.Minute), "00") & ":" & Format(CInt(Now.Second), "00")
        Label3.Text = Now
    End Sub
    Private Sub Button6_Click_4(sender As Object, e As EventArgs)
        conn.init_session()
    End Sub
    Public Sub GetAllSong()
        Dim i, j As Integer
        Dim ВсегоПесен As Integer
        Dim ПесниВТекущейПапке As New ArrayList
        Dim xe As XElement
        Dim Profile As XDocument
        Dim Логи As String
        Dim Домен As String

        Profile = GenerlaSetting.ProfileLoad

        Try
            Логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
            Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value

            ВсеПесни = New ArrayList

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
        Catch ex As Exception
            If System.IO.Directory.Exists(Логи) = True Then
                System.IO.File.AppendAllText(Логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_get_song_list " & ex.Message & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try

        ВсеПесни.Clear()
        i = 0


        'DinPlaylist(ПромежуточныйПлейлист_дин)

        If Profile.Document.Element("Setting").Element("music").Attribute("GenMode").Value = "AKMode" Then
            GenPlaylistAK()
        End If
        If Profile.Document.Element("Setting").Element("music").Attribute("GenMode").Value = "PercentMode" Then
            GenPlaylist()
        End If



    End Sub
    Public Sub GetAllDinsong()
        Dim Profile As XDocument
        Dim ПесниВТекущейПапке As New ArrayList
        Dim ВсеПесни As New ArrayList
        Dim i As Integer
        Dim xe As XElement
        Dim НомерСлучайнойПесни As Integer
        Dim Rnd As New Random
        Dim e As Integer
        Dim M As Integer
        Dim lmlm As New ArrayList
        Dim Домен, Логи As String

        Profile = GenerlaSetting.ProfileLoad

        ПромежуточныйПлейлист_дин = New XDocument
        ПромежуточныйПлейлист_дин = <?xml version="1.0"?>
                                    <плейлист>
                                    </плейлист>

        Try
            Profile = GenerlaSetting.ProfileLoad
            Логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
            Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
            For Each songs In Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Elements
                ПесниВТекущейПапке.Clear()
                If System.IO.Directory.Exists(songs.Attribute("dir_patch").Value) = True Then
                    ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.mp3", SearchOption.AllDirectories))
                    ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.wav", SearchOption.AllDirectories))
                    ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.wma", SearchOption.AllDirectories))
                    ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(songs.Attribute("dir_patch").Value, "*.aac", SearchOption.AllDirectories))
                    ВсеПесни.Insert(ВсеПесни.Count, ПесниВТекущейПапке)

                    If ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i) Is Nothing Then
                        xe = New XElement("dir" & i, "")
                        ПромежуточныйПлейлист_дин.Document.Element("плейлист").Add(xe)
                        ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).SetAttributeValue("TimeStart", songs.Attribute("Time").Value.ToString.Split("-")(0))
                        ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).SetAttributeValue("TimeStop", songs.Attribute("Time").Value.ToString.Split("-")(1))
                    End If
                    ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).SetAttributeValue("Вес", songs.Attribute("Вес").Value)

                    For j = 0 To ПесниВТекущейПапке.Count - 1
                        If ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).Element("song" & j) Is Nothing Then
                            xe = New XElement("song" & j, "")
                            ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).Add(xe)
                        End If
                        ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).Element("song" & j).SetAttributeValue("songpatch", ПесниВТекущейПапке(j))
                        ПромежуточныйПлейлист_дин.Document.Element("плейлист").Element("dir" & i).Element("song" & j).SetAttributeValue("Use", "0")

                    Next

                    i += 1
                End If

            Next

            For Each elem1 In ПромежуточныйПлейлист_дин.Document.Element("плейлист").Elements
                If elem1.Elements.Count <> 0 Then
www:
                    НомерСлучайнойПесни = Rnd.Next(0, elem1.Elements.Count - 1)

                    For Each elem In elem1.Elements
                        If e = НомерСлучайнойПесни Then
                            lmlm.Add(elem.Attribute("songpatch").Value & "|" & elem1.Attribute("TimeStart").Value & "-" & elem1.Attribute("TimeStop").Value)
                            elem.Remove()
                            e = 0

                            If elem1.Elements.Count > 0 Then
                                GoTo www
                            End If
                            Exit For
                        End If
                        e += 1
                    Next
                End If
            Next

            Me.Invoke(New ListBox1ListUpdateClear(AddressOf ListBoxListClear), "")
            DinPlaylist(lmlm)

        Catch ex As Exception
            If System.IO.Directory.Exists(Логи) = True Then
                System.IO.File.AppendAllText(Логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_gen_dyn_pl" & ex.Message & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try

    End Sub



    Public Sub DinPlaylist(ByVal value As ArrayList)
        Dim r As Integer

        For r = 0 To value.Count - 1
            Me.Invoke(New Form1.DinPl(AddressOf Me.DinPlUpdate), value(r))
        Next



        'For Each elem In value.Document.Element("плейлист").Elements
        '    For Each song In elem.Elements
        '        Me.Invoke(New Form1.DinPl(AddressOf Me.DinPlUpdate), song.Attribute("songpatch").Value & "|" & elem.Attribute("TimeStart").Value & "-" & elem.Attribute("TimeStop").Value)
        '    Next


        'Next




    End Sub



    Public Sub GenPlaylistAK()
        Dim min, max As Integer
        Dim FN As XElement
        Dim min_dirname As String
        Dim max_dirname As String
        Dim Коэффициенты As XDocument
        Dim xe As XElement
        Dim НомерСлучайнойПесни As Integer
        Dim e, r As Integer
        Dim lmlm As New ArrayList
        Dim rnd As New Random
        Dim ВсегоПесен As Integer
        Dim СуммаКоэффициентов As Integer
        Dim P As Integer
        Dim K As Integer
        Dim M As Integer
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim Логи As String
        Dim Домен As String

        Коэффициенты = New XDocument
        Коэффициенты = <?xml version="1.0"?>
                       <Коэффициенты>
                       </Коэффициенты>


        Try

            If ПромежуточныйПлейлист.Document.Element("плейлист").Elements.Count >= 1 Then
                Profile = GenerlaSetting.ProfileLoad
                Логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
                Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value

                FN = ПромежуточныйПлейлист.Document.Element("плейлист").FirstNode
                min = ПромежуточныйПлейлист.Document.Element("плейлист").Element(FN.Name.ToString).Elements.Count
                min_dirname = FN.Name.ToString
                For Each elem1 In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
                    If elem1.Elements.Count < min Then
                        min = elem1.Elements.Count
                        min_dirname = elem1.Name.ToString
                    End If
                Next
                max = ПромежуточныйПлейлист.Document.Element("плейлист").Element(FN.Name.ToString).Elements.Count
                max_dirname = FN.Name.ToString
                For Each elem1 In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
                    If elem1.Elements.Count > max Then
                        max = elem1.Elements.Count
                        max_dirname = elem1.Name.ToString
                    End If
                Next


                For Each elem1 In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
                    ВсегоПесен = ВсегоПесен + elem1.Elements.Count
                    xe = New XElement(elem1.Name.ToString, "")
                    Коэффициенты.Document.Element("Коэффициенты").Add(xe)
                    Коэффициенты.Document.Element("Коэффициенты").Element(elem1.Name.ToString).SetAttributeValue("value", Math.Round(elem1.Elements.Count / min, 0))
                    СуммаКоэффициентов = СуммаКоэффициентов + Math.Round(elem1.Elements.Count / min, 0)
                Next
                P = ВсегоПесен / СуммаКоэффициентов
                For K = 0 To P
                    For Each elem1 In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
                        If elem1.Elements.Count <> 0 Then
                            For r = 0 To Коэффициенты.Document.Element("Коэффициенты").Element(elem1.Name.ToString).Attribute("value").Value - 1
                                НомерСлучайнойПесни = rnd.Next(0, elem1.Elements.Count - 1)
                                For Each elem In elem1.Elements
                                    If e = НомерСлучайнойПесни Then
                                        lmlm.Add(elem.Attribute("songpatch").Value)
                                        elem.Remove()
                                        If elem1.Elements.Count = 0 Then
                                            Exit For
                                        End If
                                        e = 0
                                    End If
                                    e += 1
                                Next
                                e = 0
                                If elem1.Elements.Count = 0 Then
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                Next
                e = 0
                For Each elem1 In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
                    If elem1.Elements.Count <> 0 Then
www:
                        НомерСлучайнойПесни = rnd.Next(0, elem1.Elements.Count - 1)

                        For Each elem In elem1.Elements
                            If e = НомерСлучайнойПесни Then
                                M = rnd.Next(0, lmlm.Count - 1)
                                '  lmlm.Add(elem.Attribute("songpatch").Value)
                                lmlm.Insert(M, elem.Attribute("songpatch").Value)
                                elem.Remove()
                                e = 0

                                If elem1.Elements.Count > 0 Then
                                    GoTo www
                                End If
                                Exit For
                            End If
                            e += 1
                        Next
                    End If
                Next



                '  Me.MusicList.Items.Clear()
                Me.Invoke(New MusicListUpdateClear(AddressOf MusicListClear), "")


                For r = 0 To lmlm.Count - 1
                    '   MusicList.Items.Add(lmlm(r))
                    Me.Invoke(New MusicListUpdate(AddressOf MusicListAdd), lmlm.Item(r))
                Next
            End If


        Catch ex As Exception
            If System.IO.Directory.Exists(Логи) = True Then
                System.IO.File.AppendAllText(Логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_gen_AK_Playlist " & ex.Message & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try









    End Sub

    Public Sub GenPlaylist()
        Dim НомерСлучайнойПесни As Integer
        Dim randd As New Random()
        Dim e As Integer
        Dim Вероятности As New ArrayList
        Dim Диапазоны As New ArrayList
        Dim dir_name As String
        Dim random As Integer
        '  Dim UnUsedSong As New ArrayList
        Dim H As Integer
        Dim H_ИСП As Integer
        Dim dt As New DataTable
        Dim dbpatch As String
        Dim ProfileName As String
        Dim Profile As XDocument
        Dim Домен As String
        Dim логи As String
        Dim Микс As New ArrayList
        Dim НеиспользованныеПесни As XDocument
        Dim dt_исп As New DataTable



        ActivePlaylist = New ArrayList
        TempPlaylist = New ArrayList

        Profile = GenerlaSetting.ProfileLoad



        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
        H = Profile.Document.Element("Setting").Element("music").Attribute("PlayListTimeCheck").Value
        '   H_ИСП = Profile.Document.Element("Setting").Element("music").Attribute("AutorTimeCheck").Value


        логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
        Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value


        Try
            If System.IO.File.Exists(dbpatch) = True Then
                If MusicSetting.CheckRepeat = 1 Then
                    Try
                        Dim adapter As OleDbDataAdapter
                        connect = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
                        connect.Open()
                        cmd = New OleDb.OleDbCommand("", connect)
                        cmd.CommandText = "SELECT * FROM Log WHERE StartDate between @Dstart and @Dend and Событие='Music'"

                        cmd.Parameters.AddWithValue("@Dstart", DateAdd(DateInterval.Hour, H, Now).ToString)
                        cmd.Parameters.AddWithValue("@Dend", Now.ToString)

                        adapter = New OleDbDataAdapter(cmd)
                        adapter.Fill(dt)
                        cmd.ExecuteNonQuery()
                        connect.Close()
                        БазаПустая = False
                    Catch ex As Exception
                        'MsgBox("df")
                        БазаПустая = True
                    End Try

                End If
            End If

            'If MusicSetting.CheckAutor = True Then
            '    Dim adapter As OleDbDataAdapter
            '    connect = New OleDb.OleDbConnection("provider=microsoft.ace.oledb.12.0;data source=" & dbpatch & ";persist security info=true")
            '    connect.Open()
            '    cmd = New OleDb.OleDbCommand("", connect)
            '    cmd.CommandText = "select * from log where startdate between @dstart and @dend and событие='music'"

            '    cmd.Parameters.AddWithValue("@dstart", DateAdd(DateInterval.Hour, H_ИСП, Now).ToString)
            '    cmd.Parameters.AddWithValue("@dend", Now.ToString)

            '    adapter = New OleDbDataAdapter(cmd)
            '    adapter.Fill(dt_исп)
            '    cmd.ExecuteNonQuery()
            '    connect.Close()
            'End If
            '   UnUsedSong = GetUnUsedSong(ПромежуточныйПлейлист, dir_name, dt)

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

            If БазаПустая = False Then
                НеиспользованныеПесни = GetUnUsedSong(ПромежуточныйПлейлист, dt)
            Else
                НеиспользованныеПесни = ПромежуточныйПлейлист
            End If




            For iii = 0 To 99
                random = CInt(Int((Rnd() * 100) + 1))
                '  Me.Invoke(New PBUpdate(AddressOf Me.PB2Update), iii)
                For vvv = 0 To Диапазоны.Count - 1
                    If random >= Диапазоны(vvv).ToString.Split("|")(0).Split("-")(0) And random <= Диапазоны(vvv).ToString.Split("|")(0).Split("-")(1) Then
                        dir_name = Диапазоны(vvv).ToString.Split("|")(1)
                        Exit For
                    End If
                Next
                If НеиспользованныеПесни.Document.Element("плейлист").Element(dir_name).Elements.Count > 0 Then
                    НомерСлучайнойПесни = randd.Next(0, НеиспользованныеПесни.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                    For Each elem In НеиспользованныеПесни.Document.Element("плейлист").Element(dir_name).Elements
                        If e = НомерСлучайнойПесни Then
                            TempPlaylist.Add(elem.Attribute("songpatch").Value)
                            elem.Remove()
                            e = 0
                            '  Me.Invoke(New Form1.MusicListUpdate(AddressOf Me.MusicListAdd), TempPlaylist.Item(TempPlaylist.Count - 1))
                            Exit For
                        End If

                        e += 1
                    Next
                Else
                    НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                    For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                        If e = НомерСлучайнойПесни Then
                            TempPlaylist.Add(elem.Attribute("songpatch").Value)
                            '  elem.Remove()
                            e = 0
                            '  Me.Invoke(New Form1.MusicListUpdate(AddressOf Me.MusicListAdd), TempPlaylist.Item(TempPlaylist.Count - 1))
                            Exit For
                        End If
                        e += 1
                    Next

                End If

            Next

            If TempPlaylist.Count < 100 Then 'Дозаполняем плейлист,игнорируя повторы
                For iii = 0 To 99
                    random = CInt(Int((Rnd() * 100) + 1))
                    For vvv = 0 To Диапазоны.Count - 1
                        If random >= Диапазоны(vvv).ToString.Split("|")(0).Split("-")(0) And random <= Диапазоны(vvv).ToString.Split("|")(0).Split("-")(1) Then
                            dir_name = Диапазоны(vvv).ToString.Split("|")(1)
                            Exit For
                        End If
                    Next
                    НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                    For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                        If e = НомерСлучайнойПесни Then
                            TempPlaylist.Add(elem.Attribute("songpatch").Value)
                            elem.Remove()
                            e = 0
                            Exit For
                        End If
                        e += 1
                    Next
                    If TempPlaylist.Count >= 100 Then
                        Exit For
                    End If


                Next
            End If






            For r = 0 To TempPlaylist.Count - 1
                iii = randd.Next(0, TempPlaylist.Count - 1)
                Микс.Add(TempPlaylist(iii))
                TempPlaylist.RemoveAt(iii)
                Me.Invoke(New Form1.MusicListUpdate(AddressOf Me.MusicListAdd), Микс.Item(Микс.Count - 1))
            Next


        Catch ex As Exception

            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_gen_Playlist " & ex.Message & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try




    End Sub

    Public Function FindUnUsedAutor(ByVal ПромежуточныйПлейлист As XDocument, ByVal dt_исп As DataTable, ByVal dir_name As String)
        Dim КоличествоПопытокПоискаДругогоИсполнителя = 10
        Dim НомерСлучайнойПесни As Integer
        Dim СлучайнаяПесня As String = ""
        Dim randd As New Random
        Dim r As Integer
        Dim s As Integer
        Dim e As Integer
        Dim БылТакойАвтор As Boolean = False
        Dim Split_char As String
        Dim Profile As XDocument

        Profile = GenerlaSetting.ProfileLoad


        Split_char = Profile.Document.Element("Setting").Element("music").Attribute("Разделитель").Value

        Try
            For s = 0 To КоличествоПопытокПоискаДругогоИсполнителя - 1
                НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                    If e = НомерСлучайнойПесни Then
                        СлучайнаяПесня = elem.Attribute("songpatch").Value
                        For r = 0 To dt_исп.Rows.Count - 1
                            If dt_исп.Rows(r).Item(5).ToString.Trim.Split(Split_char)(0).Trim = System.IO.Path.GetFileNameWithoutExtension(СлучайнаяПесня).Split(Split_char)(0).Trim Then
                                БылТакойАвтор = True
                                Exit For
                            End If
                        Next
                        e = 0
                    End If
                    e += 1
                Next
                If БылТакойАвтор = False Then
                    Return СлучайнаяПесня
                End If
            Next

            e = 0
            If БылТакойАвтор = True Then
                НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
                For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                    If e = НомерСлучайнойПесни Then
                        e = 0
                        Return elem.Attribute("songpatch").Value
                    End If
                    e += 1
                Next
                БылТакойАвтор = False
            End If
        Catch ex As Exception
            НомерСлучайнойПесни = randd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements.Count - 1)
            For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Element(dir_name).Elements
                If e = НомерСлучайнойПесни Then
                    e = 0
                    Return elem.Attribute("songpatch").Value
                End If
                e += 1
            Next
        End Try





    End Function

    Private Function GetUnUsedSong(ByVal TMPPlaylist As System.Xml.Linq.XDocument, ByVal dt As DataTable) As XDocument
        Dim UnUsedSong As New ArrayList
        Dim БылаТакая As Boolean
        Dim xe As XElement
        Dim song_count As Integer
        Dim dt_исп As DataTable = New DataTable
        Dim H_ИСП As Integer
        Dim Profile As XDocument
        Dim dbpatch As String
        Dim Split_char As String


        НеИспользованныеПесни = New XDocument
        НеИспользованныеПесни = <?xml version="1.0"?>
                                <плейлист>
                                </плейлист>
        Profile = GenerlaSetting.ProfileLoad

        dbpatch = Profile.Document.Element("Setting").Element("db").Attribute("file").Value
        H_ИСП = Profile.Document.Element("Setting").Element("music").Attribute("AutorTimeCheck").Value
        Split_char = Profile.Document.Element("Setting").Element("music").Attribute("Разделитель").Value
        'ProgressBar2.Maximum = TMPPlaylist.Document.Element("плейлист").Elements.Count
        '  ProgressBar3.Maximum = dt.Rows.Count
        For Each elem In TMPPlaylist.Document.Element("плейлист").Elements
            'If iii < TMPPlaylist.Document.Element("плейлист").Elements.Count Then
            '    Me.Invoke(New PBUpdate(AddressOf Me.PB2Update), iii)
            'End If
            For Each dirs In elem.Elements
                For r = 0 To dt.Rows.Count - 1
                    ' Me.Invoke(New PB3Update(AddressOf Me.PB3Update3), r)
                    If dt.Rows(r).Item(5).ToString.Trim = System.IO.Path.GetFileName(dirs.Attribute("songpatch").Value) Then
                        'If dt.Rows(r).Item(5).ToString.Trim.Split("^")(0) = System.IO.Path.GetFileNameWithoutExtension(dirs.Attribute("songpatch").Value).Split("^")(0) Then
                        БылаТакая = True
                        Exit For
                        ' End If

                    End If
                Next
                If НеИспользованныеПесни.Document.Element("плейлист").Element(elem.Name.ToString) Is Nothing Then
                    xe = New XElement(elem.Name.ToString, "")
                    НеИспользованныеПесни.Document.Element("плейлист").Add(xe)
                End If
                If БылаТакая = False Then
                    song_count = НеИспользованныеПесни.Document.Element("плейлист").Element(elem.Name.ToString).Elements.Count
                    xe = New XElement("song_" & song_count, "")
                    НеИспользованныеПесни.Document.Element("плейлист").Element(elem.Name.ToString).Add(xe)
                    НеИспользованныеПесни.Document.Element("плейлист").Element(elem.Name.ToString).Element("song_" & song_count).SetAttributeValue("songpatch", dirs.Attribute("songpatch").Value)
                End If
                БылаТакая = False
                '  rrr += 1
            Next
            ' iii += 1
        Next
        '  НеИспользованныеПесни.Save("unused.xml")

        If MusicSetting.CheckAutor = True Then
            Dim adapter As OleDbDataAdapter
            connect = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
            connect.Open()
            cmd = New OleDb.OleDbCommand("", connect)
            cmd.CommandText = "SELECT * FROM Log WHERE StartDate between @Dstart and @Dend and Событие='Music'"

            cmd.Parameters.AddWithValue("@Dstart", DateAdd(DateInterval.Hour, H_ИСП, Now).ToString)
            cmd.Parameters.AddWithValue("@Dend", Now.ToString)

            adapter = New OleDbDataAdapter(cmd)
            adapter.Fill(dt_исп)
            cmd.ExecuteNonQuery()
            connect.Close()

            For Each elem In НеИспользованныеПесни.Document.Element("плейлист").Elements
                For Each dirs In elem.Elements
                    Try
                        For r = 0 To dt_исп.Rows.Count - 1
                            If dt_исп.Rows(r).Item(5).ToString.Trim.Split(Split_char)(0) = System.IO.Path.GetFileNameWithoutExtension(dirs.Attribute("songpatch").Value).Split(Split_char)(0) Then
                                dirs.Remove()
                            End If
                        Next

                    Catch ex As Exception

                    End Try
                Next

            Next

        End If


        If MusicSetting.CheckRepeat = "1" Then
            Return НеИспользованныеПесни
        Else
            Return TMPPlaylist
        End If


    End Function
    Public Sub MusicListAdd(ByVal value As String)
        MusicList.Items.Add(value)
    End Sub
    Public Sub MusicListClear(ByVal value As String)
        MusicList.Items.Clear()
       
    End Sub
    Public Sub ListBoxListClear(ByVal value As String)

        ListBox1.Items.Clear()
    End Sub



    Public Sub PB2Update(ByVal value As Integer)
        ProgressBar2.Value = value
    End Sub
    Public Sub Поток()
        Try
            If th.ThreadState = Threading.ThreadState.Running Then
                th.Abort()
            End If
        Catch ex As Exception

        End Try

        th = New Thread(AddressOf GetAllSong)
        th.Start()


    End Sub
    Public Sub Поток_DinSong()

        Try
            If th2.ThreadState = Threading.ThreadState.Running Then
                th2.Abort()
            End If
        Catch ex As Exception

        End Try

        th2 = New Thread(AddressOf GetAllDinsong)
        th2.Start()

    End Sub


    Private Sub НастройкиToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles НастройкиToolStripMenuItem.Click
        setting.ShowDialog()
    End Sub

    Private Sub KillTimer_Tick(sender As Object, e As EventArgs) Handles KillTimer.Tick
        Play.KillTime()
    End Sub



    Private Sub LoadProfile_Tick(sender As Object, e As EventArgs) Handles LoadProfile.Tick
        Templates.GetProfileList()
    End Sub
    Private Sub Button6_Click_5(sender As Object, e As EventArgs)

        ''With lame
        ''    .InputFile = Nothing
        ''    .OutputFile = Nothing
        ''    .LAME_Bitrate = CInt(EncoderLAME.BITRATE.kbps_48)
        ''    .LAME_Mode = EncoderLAME.LAMEMode.Mono
        ''    .LAME_TargetSampleRate = CInt(EncoderLAME.SAMPLERATE.Hz_44100)
        ''    .LAME_Quality = EncoderLAME.LAMEQuality.Quality
        ''End With





        ''With icecast
        ''    .ServerAddress = "10.6.0.12"
        ''    .ServerPort = 8000
        ''    .Password = "sintez"
        ''    .MountPoint = "point.mp3"
        ''    .Username = "source"
        ''    .StreamName = "VPS"
        ''    .PublicFlag = False
        ''End With

        ''_broadCast = New BroadCast(icecast)
        ''_broadCast.AutoReconnect = True
        ''AddHandler _broadCast.Notification, AddressOf OnBroadCast_Notification
        ''_broadCast.AutoConnect()
        'Dim hr As Integer = Bass.BASS_PluginLoad("f:\YandexDisk\Проекты\Cat Last Stable 04-06-2023\CAT\bin\Debug\bassenc_mp3.dll")


        'Dim aac As New EncoderMP3(Play.stream_music)
        'aac.InputFile = Nothing ' STDIN
        'aac.OutputFile = Nothing ' STDOUT
        ''  aac.F = CInt(EncoderFAAC.BITRATE.kbps_256)
        '' aac.bi = BaseEncoder.BITRATE.kbps_128



        'Dim icy2 As New ICEcast(aac)
        'icy2.ServerAddress = "10.6.0.12"
        'icy2.ServerPort = 8000
        'icy2.MountPoint = "point.mp3"
        'icy2.Password = "sintez"
        ''  icy2.PublicFlag = myStream(1).isPublic
        'icy2.StreamDescription = "VPS"
        '' icy2.StreamGenre = myStream(1).streamGenre
        ''  icy2.StreamName = myStream(1).streamName
        '' icy2.UpdateTitle(myStream(1).streamTitle, Nothing)

        '' use the BroadCast class to control streaming
        '_broadcast2 = New BroadCast(icy2)
        '_broadcast2.AutoReconnect = True
        '_broadcast2.ReconnectTimeout = 10
        'AddHandler _broadcast2.Notification, AddressOf OnBroadCast_Notification
        '_broadcast2.AutoConnect()





    End Sub
    Private Sub TextBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TextBox1.MouseDoubleClick
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox1.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        If TextBox1.Text <> "" Then
            Media_info.Start()
            ADV.ADV_Playlist.Add(TextBox1.Text & "|2|" & Now)
            Media_info.Start()
            Play.НеЗапланированное = True
            ВремяADV = True
            ' adv_file = TextBox1.Text
        End If
    End Sub
    Private Sub TextBox2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TextBox2.MouseDoubleClick
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox2.Text = OpenFileDialog1.FileName
        End If
    End Sub
    Private Sub TextBox3_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TextBox3.MouseDoubleClick
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox3.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub TextBox4_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TextBox4.MouseDoubleClick
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox4.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub TextBox5_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TextBox5.MouseDoubleClick
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox5.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If TextBox2.Text <> "" Then
            Media_info.Start()
            ADV.ADV_Playlist.Add(TextBox2.Text & "|2|" & Now)
            Media_info.Start()
            Play.НеЗапланированное = True
            ВремяADV = True
            ' adv_file = TextBox2.Text
        End If

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If TextBox3.Text <> "" Then
            Media_info.Start()
            ADV.ADV_Playlist.Add(TextBox3.Text & "|2|" & Now)
            Media_info.Start()
            Play.НеЗапланированное = True
            ВремяADV = True
            '  adv_file = TextBox3.Text
        End If

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If TextBox4.Text <> "" Then
            Media_info.Start()
            ADV.ADV_Playlist.Add(TextBox4.Text & "|2|" & Now)
            Media_info.Start()
            Play.НеЗапланированное = True
            ВремяADV = True
            '  adv_file = TextBox4.Text
        End If

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If TextBox5.Text <> "" Then
            Media_info.Start()
            ADV.ADV_Playlist.Add(TextBox5.Text & "|2|" & Now)
            Media_info.Start()
            Play.НеЗапланированное = True
            ВремяADV = True
            '  adv_file = TextBox5.Text
        End If

    End Sub

    Private Sub CrossFade_Tick(sender As Object, e As EventArgs) Handles CrossFade.Tick
        '   Play.CrossFade()
        Mix.CrossFade1()
    End Sub

    Private Sub CrossFade2_Tick(sender As Object, e As EventArgs) Handles CrossFade2.Tick
        '  Play.CrossFade2()
        Mix.CrossFade2()
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        Поток()

        ProgressBar2.Minimum = 0
        ProgressBar2.Maximum = 99

        MusicList.Items.Clear()
        ' ListView1.Items.Clear()
        ' ListBox1.Items.Clear()
    End Sub

    Private Sub TabPage3_Click(sender As Object, e As EventArgs) Handles TabPage3.Click

    End Sub

    Private Sub TabPage3_DragDrop(sender As Object, e As DragEventArgs) Handles TabPage3.DragDrop
        file = CType(e.Data.GetData(DataFormats.FileDrop), String())

    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        file = CType(e.Data.GetData(DataFormats.FileDrop), String())


        If file.Length > 5 Then
            MsgBox("Одновременно можно добавить только 5 файлов. Будут отображены только первые 5")
        End If

        If file.Length >= 5 Then
            TextBox1.Text = file(0)
            TextBox2.Text = file(1)
            TextBox3.Text = file(2)
            TextBox4.Text = file(3)
            TextBox5.Text = file(4)
        End If
        If file.Length = 4 Then
            TextBox1.Text = file(0)
            TextBox2.Text = file(1)
            TextBox3.Text = file(2)
            TextBox4.Text = file(3)
        End If
        If file.Length = 3 Then
            TextBox1.Text = file(0)
            TextBox2.Text = file(1)
            TextBox3.Text = file(2)
        End If
        If file.Length = 2 Then
            TextBox1.Text = file(0)
            TextBox2.Text = file(1)
        End If
        If file.Length = 1 Then
            TextBox1.Text = file(0)
        End If

    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Timer9_Tick(sender As Object, e As EventArgs) Handles HostInfo.Tick
        HostInformation.HostInfo()
    End Sub
    Private Sub УровниГромкостиToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles УровниГромкостиToolStripMenuItem.Click
        Form3.Show()
    End Sub
    Private Sub Button8_Click_2(sender As Object, e As EventArgs)
        Play.ВремяМузыки = True
        Play.ВремяADV = True
    End Sub

    Private Sub Button6_Click_8(sender As Object, e As EventArgs) Handles Button6.Click
        ListView3.Items.Clear()
        Dim adv_check_th As Thread

        adv_check_th = New Thread(AddressOf ADVCheck)
        adv_check_th.IsBackground = True
        adv_check_th.Start()
    End Sub
    Private Sub ADVCheck()
        Dim Дата As String = DateTimePicker1.Value.Date.ToString("yyyyMMdd")
        Dim РоликиНаДень As ArrayList = Play.GetReklamaOfDay(Дата)
        Dim adv_dir As String
        Dim r As Integer
        Dim FileInfo As System.IO.FileInfo
        Dim логи As String
        Dim Домен As String
        Dim Плейлист As Array
        Dim PlayListPatch As String
        Dim Status As String
        Dim Check_rez As XDocument
        Dim xe As XElement
        Dim count As Integer

        Try
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
                MsgBox("Плейлист на " & DateTimePicker1.Value.ToString("dd.MM.yyyy") & " не найден")
                Exit Sub
            End If

            'If System.IO.File.Exists(логи & "\" & Домен & "_" & DateTimePicker1.Value.Date & ".ADV") = False Then
            '    MsgBox("Лог рекламы на " & DateTimePicker1.Value.ToString("dd.MM.yyyy") & " не найден")
            '    Exit Sub
            'End If


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
                            Status = "False|XX:XX:XX"
                        End If


                        If Status.Split("|")(0) = True Then
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Status", "Вышел")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("TimeFact", Status.Split("|")(1))
                            ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Вышел")
                        End If
                        If Status.Split("|")(0) = False Then
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Status", "Не вышел")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("TimeFact", Status.Split("|")(1))
                        End If
                        If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(2) & ".wav") = True Then
                            ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Yes")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Exist", "Yes")
                        Else
                            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("No")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Exist", "No")
                        End If

                    End If
                    If Плейлист(r).ToString.Split("=").Length = 2 Then
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("time", Плейлист(r).ToString.Split("=")(0))
                        Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("adv", Плейлист(r).ToString.Split("=")(1))
                        '  ListView1.Items.Add(Плейлист(r).ToString.Split("=")(1))
                        ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Плейлист(r).ToString.Split("=")(2))
                        If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav") = True Then
                            FileInfo = My.Computer.FileSystem.GetFileInfo(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav")
                            ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Math.Round(FileInfo.Length / (1024 * 1024), 2))
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("size", Math.Round(FileInfo.Length / (1024 * 1024), 2))
                        Else
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("size", "0")
                            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("0")
                        End If
                        If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav") = True Then
                            Status = ChecADVLog(Плейлист(r).ToString.Split("=")(1) & ".wav", Плейлист(r).ToString.Split("=")(0))
                        Else
                            Status = "False|XX:XX:XX"
                        End If


                        If Status.Split("|")(0) = True Then
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Status", "Вышел")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("TimeFact", Status.Split("|")(1))
                            ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Вышел")
                        End If
                        If Status.Split("|")(0) = False Then
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Status", "Не вышел")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("TimeFact", Status.Split("|")(1))
                        End If
                        If System.IO.File.Exists(adv_dir & "\" & Плейлист(r).ToString.Split("=")(1) & ".wav") = True Then
                            ' ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("Yes")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Exist", "Yes")
                        Else
                            'ListView1.Items(ListView1.Items.Count - 1).SubItems.Add("No")
                            Check_rez.Document.Element("Check_rez").Element("adv_" & count).SetAttributeValue("Exist", "No")
                        End If

                    End If

                Next


            End If
            Me.Invoke(New Form1.ADVCheckDelegate(AddressOf Me.advcheckupdate), Check_rez)
        Catch ex As Exception

        End Try



    End Sub


    Private Sub ADV_TIMER_Tick(sender As Object, e As EventArgs) Handles ProfileVolumeTimer.Tick
        ProfileVolumeSet()
    End Sub



    Private Sub Button8_Click_4(sender As Object, e As EventArgs)
        Bass.BASS_ChannelPause(Play.stream_music)
    End Sub

    Private Sub Button13_Click_1(sender As Object, e As EventArgs)
        Bass.BASS_ChannelStart(Play.stream_music)
    End Sub

    Private Sub Timer10_Tick(sender As Object, e As EventArgs) Handles Timer10.Tick
        ' adv_check.
        If Play.КроссФейдНачался = False Then
            musicfadeout.Start()
            Timer10.Stop()
        End If

    End Sub

    Private Sub FadeIN_Tick(sender As Object, e As EventArgs) Handles FadeIN.Tick
        Mix.FadeIN()
    End Sub



    Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles Button8.Click
        SaveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt"
        If SaveFileDialog1.ShowDialog = DialogResult.OK Then
            SaveFileDialog1.AddExtension = True
            For r = 0 To ListView3.Items.Count - 1

                System.IO.File.AppendAllText(SaveFileDialog1.FileName, ListView3.Items(r).Text & " " & ListView3.Items(r).SubItems(2).Text & " " & ListView3.Items(r).SubItems(3).Text & " " & ListView3.Items(r).SubItems(4).Text & " " & ListView3.Items(r).SubItems(5).Text & vbNewLine, System.Text.Encoding.Default)
            Next
        End If
    End Sub

    Private Sub BroadCastMode_CheckedChanged(sender As Object, e As EventArgs) Handles BroadCastMode.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim Profile As XDocument
        Dim ProFileName As String


        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        If BroadCastMode.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("OpenLastPL", "false")
        End If



        'If System.IO.File.Exists(ProFileName) = True Then
        '    ' Profile = XDocument.Load(ProFileName)
        '    Profile.Save(ProFileName)
        'Else
        '    Profile.Save("Настройки.xml")
        '    '  Profile = XDocument.Load("Настройки.xml")
        'End If


    End Sub

    Private Sub PlayListMode_CheckedChanged(sender As Object, e As EventArgs) Handles PlayListMode.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim Profile As XDocument
        Dim ProFileName As String


        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        If PlayListMode.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("OpenLastPL", "true")
        End If



        If System.IO.File.Exists(ProFileName) = True Then
            ' Profile = XDocument.Load(ProFileName)
            Profile.Save(ProFileName)
        Else
            Profile.Save("Настройки.xml")
            '  Profile = XDocument.Load("Настройки.xml")
        End If
    End Sub

    Private Sub CheckProfile_Tick(sender As Object, e As EventArgs) Handles CheckProfile.Tick
        Templates.GetProfileList()
    End Sub

    Private Sub Button13_Click_2(sender As Object, e As EventArgs)
        'GeneratePlaylist.GetAllSong()
        Поток()

        ProgressBar2.Minimum = 0
        ProgressBar2.Maximum = 99

        MusicList.Items.Clear()
    End Sub

    Private Sub Timer11_Tick(sender As Object, e As EventArgs) Handles Timer11.Tick
        If Play.КроссФейдНачался = False Then
            musicfadeout.Start()
            Timer11.Stop()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Play.Пауза = True
    End Sub

    Private Sub State_Tick(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs)
        '  Bass.BASS_ChannelSlideAttribute(Play.stream_music, BASSAttribute.BASS_ATTRIB_VOL, 0F, 3000)

        'Dim mixerStream As Integer = BassMix.BASS_Mixer_StreamCreate(44100, 2, BASSFlag.BASS_SAMPLE_FLOAT)

        'Dim streamA As Integer = Bass.BASS_StreamCreateFile("f:\CAT!!!\music\06 - РУССКИЙ РОК\БИ-2\Би-2 - Научи меня быть счастливым.mp3", 0, 0,
        '                                        BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_FLOAT)

        'Dim streamB As Integer = Bass.BASS_StreamCreateFile("f:\CAT!!!\music\06 - РУССКИЙ РОК\ДЖАНГО\Джанго - Босая Осень (2010).mp3", 0, 0,
        '                                   BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_FLOAT)


        'Dim okA As Boolean = BassMix.BASS_Mixer_StreamAddChannel(mixerStream, streamA, BASSFlag.BASS_DEFAULT)
        'Dim okB As Boolean = BassMix.BASS_Mixer_StreamAddChannel(mixerStream, streamB, BASSFlag.BASS_DEFAULT)


        '' and play the mixer channel
        'Bass.BASS_ChannelPlay(mixerStream, False)
        Dim hr As Integer
        hr = Bass.BASS_Init(7, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)

        Dim mixer As Integer = BassMix.BASS_Mixer_StreamCreate(44100, 2, BASSFlag.BASS_SAMPLE_FLOAT)
        ' now we need some channels to plug them in...create two decoding sources
        Dim streamA As Integer = Bass.BASS_StreamCreateFile("f:\CAT!!!\music\06 - РУССКИЙ РОК\БИ-2\Би-2 - Научи меня быть счастливым.mp3", 0, 0,
                                      BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_FLOAT)
        Dim streamB As Integer = Bass.BASS_StreamCreateFile("f:\CAT!!!\music\06 - РУССКИЙ РОК\ДЖАНГО\Джанго - Босая Осень (2010).mp3", 0, 0,
                                      BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_FLOAT)
        ' finally we plug them into the mixer (no downmix, since we assume the sources to be stereo)
        Dim okA As Boolean = BassMix.BASS_Mixer_StreamAddChannel(mixer, streamA, BASSFlag.BASS_DEFAULT)
        Dim okB As Boolean = BassMix.BASS_Mixer_StreamAddChannel(mixer, streamB, BASSFlag.BASS_DEFAULT)
        ' and play it...
        Bass.BASS_ChannelPlay(mixer, False)


        Bass.BASS_ChannelSlideAttribute(streamB, BASSAttribute.BASS_ATTRIB_VOL, 0.0F, 3000)

        Bass.BASS_ChannelSlideAttribute(streamB, BASSAttribute.BASS_ATTRIB_VOL, 1.0F, 3000)

    End Sub

    Private Sub Button13_Click_3(sender As Object, e As EventArgs)
        ' Dim ch1 As Integer 
        Dim hr As Integer
        Dim n As Integer
        Dim SRC(2) As SRC
        Dim mixer As Double
        Dim Хрон_b As Double
        Dim Хрон As Double
        Dim ch1, ch2 As Integer


        SRC(0).filename = "f:\CAT!!!\music\2\3.mp3"
        SRC(1).filename = "f:\CAT!!!\music\2\2.mp3"

        SRC(0).fadein = "5"
        SRC(1).fadein = "5"

        SRC(0).fadeout = "5"
        SRC(1).fadeout = "5"

        SRC(0).start = "1"
        SRC(1).start = "1"
        'Dim ch1 As Integer
        'Dim ch2 As Integer


        hr = Bass.BASS_Init(7, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)

        ch1 = Bass.BASS_StreamCreateFile("f:\CAT!!!\music\2\3.mp3", 0, 0, BASSFlag.BASS_DEFAULT)
        ch2 = Bass.BASS_StreamCreateFile("f:\CAT!!!\music\2\2.mp3", 0, 0, BASSFlag.BASS_DEFAULT)

        ''mixer = BassMix.BASS_Mixer_StreamCreate(44100, 2, BASSFlag.BASS_MIXER_END)
        Bass.BASS_ChannelPlay(ch1, True)
        '  Bass.BASS_ChannelSetAttribute(ch1, BASSAttribute.BASS_ATTRIB_VOL, 1)
        ' Bass.BASS_ChannelSlideAttribute(ch1, BASSAttribute.BASS_SLIDE_LOG, 0F, 4000)



        '  Bass.BASS_ChannelPlay(ch2, False)
        '   Bass.BASS_ChannelSetAttribute(ch2, BASSAttribute.BASS_ATTRIB_VOL, 0)
        Bass.BASS_ChannelSlideAttribute(ch1, BASSAttribute.BASS_ATTRIB_VOL, 0F, 5000)

        ' Bass.BASS_ChannelSlideAttribute(ch2, BASSAttribute.BASS_ATTRIB_VOL & BASSAttribute.BASS_SLIDE_LOG, 1, 4000)


        ''For n = 0 To 1
        ''    Dim decoder1 As Integer = Bass.BASS_StreamCreateFile(SRC(0).filename, 0, 0, BASSFlag.BASS_STREAM_DECODE)
        ''    Bass.BASS_ChannelPlay(decoder, False)
        ''    Dim start1 As Long = Bass.BASS_ChannelSeconds2Bytes(mixer, 0.0) ' delay
        ''    Dim length1 As Long = Bass.BASS_ChannelSeconds2Bytes(mixer, 49.0) ' duration
        ''    add the channel
        ''BassMix.BASS_Mixer_StreamAddChannelEx(mixer, decoder1, BASSFlag.BASS_MIXER_CHAN_NORAMPIN, start1, length1)

        ''    Dim decoder2 As Integer = Bass.BASS_StreamCreateFile(SRC(1).filename, 0, 0, BASSFlag.BASS_STREAM_DECODE)
        ''    Bass.BASS_ChannelPlay(decoder, False)
        ''    Dim start2 As Long = Bass.BASS_ChannelSeconds2Bytes(mixer, 44.0) ' delay
        ''    Dim length2 As Long = Bass.BASS_ChannelSeconds2Bytes(mixer, 69.0) ' duration
        ''    add the channel
        ''BassMix.BASS_Mixer_StreamAddChannelEx(mixer, decoder2, BASSFlag.BASS_MIXER_CHAN_NORAMPIN, start2, length2)


        ''    Dim decoder As Integer = Bass.BASS_StreamCreateFile(SRC(n).filename, 0, 0, BASSFlag.BASS_DEFAULT)
        ''    Dim start As Integer = Bass.BASS_ChannelSeconds2Bytes(mixer, SRC(n).start)

        ''    BassMix.BASS_Mixer_StreamAddChannelEx(mixer, decoder, BASSFlag.BASS_MIXER_NORAMPIN, start, 2)
        ''    Хрон_b = Bass.BASS_ChannelGetLength(decoder, BASSMode.BASS_POS_BYTE)
        ''    Хрон = Bass.BASS_ChannelBytes2Seconds(decoder, Хрон_b)


        ''    Dim nodes(4) As BASS_MIXER_NODE
        ''    nodes(0).pos = 0
        ''    nodes(0).val = 0
        ''    nodes(0).pos = Bass.BASS_ChannelSeconds2Bytes(mixer, SRC(n).fadein)
        ''    nodes(1).val = 1
        ''    Nodes(2).pos = Bass.BASS_ChannelSeconds2Bytes(mixer, Хрон - SRC(n).fadeout)

        ''    If nodes(2).pos < nodes(1).pos Then
        ''        nodes(2).val = 1
        ''        nodes(3).pos = Bass.BASS_ChannelSeconds2Bytes(mixer, Хрон)
        ''        nodes(3).val = 0
        ''    End If
        ''    BassMix.BASS_Mixer_ChannelSetEnvelope(decoder, BASSMIXEnvelope.BASS_MIXER_ENV_PAN & BASSMIXEnvelope.BASS_MIXER_ENV_LOOP, nodes, 4)

        ''    Dim length As Double = Bass.BASS_ChannelBytes2Seconds(decoder, BASS_ChannelGetLength(decoder, BASS_POS_BYTE))


        ''Next

        ''Dim encoder As Integer = BassEnc.BASS_Encode_Start(mixer, "", BASSEncode.BASS_ENCODE_PCM, Nothing, IntPtr.Zero)

        ''hr = Bass.BASS_ChannelPlay(mixer, True)



        '   hr = Bass.bass_mi

    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs)

        'db_th = New System.Threading.Thread(New ParameterizedThreadStart(AddressOf DataBase.AddNewRecord))
        'db_th.IsBackground = True
        'db_th.Start("gfdgfdgdf")


        'SQLiteConnection.CreateFile("MyDatabase.sqlite")


        'Dim connectionString As String
        'connectionString = "Data Source=MyDatabase.sqlite;Version=3;"

        'Dim SQLiteConn As New SQLiteConnection(connectionString)

        'SQLiteConn.Open()


        'Dim sql As String = "Create Table highscores (name varchar(20), score int)"

        'Dim command As SQLiteCommand = New SQLiteCommand(sql, SQLiteConn)

        'command.ExecuteNonQuery()
        '  Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_GVOL_STREAM, 2500)



    End Sub

    Private Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        '  MsgBox(ListView2.FocusedItem.Text)
    End Sub

    'Private Sub Button14_Click(sender As Object, e As EventArgs)
    '    Dim TG_Thread As Thread
    '    TG_Thread = New Thread(AddressOf Telega.Get_text)
    '    '  TG_Thread.IsBackground = True
    '    TG_Thread.Start()
    'End Sub
    Public Function ChecADVLog(ByVal Ролик As String, ByVal ВремяВПлейлисте As String) As String
        Dim логи As String
        Dim Домен As String
        Dim ADV_LOG As Array
        Dim Плейлист As Array
        Dim PlayListPatch As String
        Dim Дата As String = DateTimePicker1.Value.Date.ToString("yyyyMMdd")
        Dim ВремяВЛоге As String
        Dim ВремяПлейлистаВСек As Integer
        Dim ВремяЛогеВСек As Integer
        Dim x As Integer
        Dim РоликВЛоге As String
        Dim Profile As XDocument


        Profile = GenerlaSetting.ProfileLoad

        логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
        Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value


        PlayListPatch = Profile.Document.Element("Setting").Element("sch").Attribute("dir").Value
        Плейлист = System.IO.File.ReadAllLines(PlayListPatch & "\" & Дата & ".txt", System.Text.Encoding.Default)

        'If System.IO.File.Exists(логи & "\" & Домен & "_" & DateTimePicker1.Value.Date & ".ADV") = False Then
        '    MsgBox("Лог рекламы на " & DateTimePicker1.Value.ToString("dd.MM.yyyy") & " не найден")
        'End If
        If System.IO.File.Exists(логи & "\" & Домен & "_" & DateTimePicker1.Value.Date & ".ADV") = True Then
            ADV_LOG = System.IO.File.ReadAllLines(логи & "\" & Домен & "_" & DateTimePicker1.Value.Date & ".ADV", System.Text.Encoding.Default)
            ВремяПлейлистаВСек = Mid(ВремяВПлейлисте, 1, 2) * 3600 + Mid(ВремяВПлейлисте, 3, 2) * 60 + Mid(ВремяВПлейлисте, 5, 2)


            For x = 0 To ADV_LOG.Length - 1
                ВремяВЛоге = ADV_LOG(x).ToString.Split(" ")(1)
                ВремяЛогеВСек = ВремяВЛоге.Split(":")(0) * 3600 + ВремяВЛоге.Split(":")(1) * 60 + ВремяВЛоге.Split(":")(2)
                РоликВЛоге = System.IO.Path.GetFileName(ADV_LOG(x).ToString.Split(" ")(3))
                If РоликВЛоге = РоликВЛоге Then
                    If ВремяЛогеВСек >= ВремяПлейлистаВСек And ВремяЛогеВСек <= ВремяПлейлистаВСек + 60 Then
                        Return "True|" & ВремяВЛоге
                    End If
                End If

            Next
        End If
        Return "False|XX:XX:XX"
    End Function

    Private Sub Button15_Click(sender As Object, e As EventArgs)
        'Dim ggg As New AddOn.Fx.BASS_BFX_AUTOWAH
        'ggg.f = 8000

        'Dim hr As Integer

        'hr = Bass.BASS_FXGetParameters(Play.stream_music, ggg)
        ' set_eq(1, 100, 100)
    End Sub

    Private Sub ListView2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView2.MouseDoubleClick
        ' MsgBox(ListView2.FocusedItem.Text)

        If System.IO.File.Exists("Templates\" & ListView2.FocusedItem.Text & ".profile") = True Then
            Process.Start("CAT Profile Editor.exe", "Templates\" & ListView2.FocusedItem.Text & ".profile")
        Else
            MsgBox("Профиль " & "Templates\" & ListView2.FocusedItem.Text & ".profile" & " не найден", MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub Button15_Click_1(sender As Object, e As EventArgs)
        'Dim jjj As New ArrayList

        'For Each Dir As String In System.IO.Directory.GetDirectories("f:\CAT!!!\music\06 - РУССКИЙ РОК\")
        '    Dim dirInfo As New System.IO.DirectoryInfo(Dir)
        '    '  ListBox1.Items.Add(dirInfo.Name)
        '    jjj.Add(dirInfo.Name)
        'Next
        Dim dfgdf As String
        dfgdf = Song.CheckDynDir()

    End Sub

    Private Sub DynDir_Tick(sender As Object, e As EventArgs) Handles DynDir.Tick
        'Song.CheckDynDir()
    End Sub

    Private Sub Button13_Click_4(sender As Object, e As EventArgs) Handles Button13.Click
        ListBox1.Items.Clear()
        Поток_DinSong()
    End Sub

    Private Sub Timer_gen_mus_pl_Tick(sender As Object, e As EventArgs) Handles Timer_gen_mus_pl.Tick
        Dim profile As XDocument = GenerlaSetting.ProfileLoad

        If profile.Document.Element("Setting").Element("music").Attribute("GenNewMusList").Value = True Then

            If Format(Now.Hour, "00") & ":" & Format(Now.Minute, "00") & ":" & Format(Now.Second, "00") = profile.Document.Element("Setting").Element("music").Attribute("TimeGenNewMusPL").Value Then
                Поток()
                Поток_DinSong()
            End If


        End If


    End Sub

    Private Sub TimerDynDir_Tick(sender As Object, e As EventArgs) Handles TimerDynDir.Tick

    End Sub

    Private Sub Timer_Level_Tick(sender As Object, e As EventArgs) Handles Timer_Level.Tick
        Dim Profile As XDocument = GenerlaSetting.ProfileLoad
        Dim t_start As Date
        Dim t_stop As Date
        Dim ЕстьВремя As Boolean
        ListView1.Items.Clear()
        ListView4.Items.Clear()
        ListView5.Items.Clear()

        ListView1.Items.Add("Музыка")
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(MusicSetting.GetVolume * 100)

        ListView1.Items.Add("Реклама")
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(ADVSetting.GetVolume * 100)

        ListView1.Items.Add("Объявления")
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(SettingAlarm.GetVolume * 100)

        ListView1.Items.Add("Джиннгл")
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(SettingAlarm.GetJingleVolume * 100)


        For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Elements
            t_start = Now.Date & " " & elem.Attribute("Time").Value.ToString.Split("-")(0)
            t_stop = Now.Date & " " & elem.Attribute("Time").Value.ToString.Split("-")(1)
            If Now >= t_start And Now <= t_stop Then
                ListView4.Items.Add(elem.Attribute("dir_patch").Value)
                ListView4.Items(ListView4.Items.Count - 1).SubItems.Add(elem.Attribute("Time").Value)
                '  ЕстьВремя = True
            End If
        Next
        For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements
            t_start = Now.Date & " " & elem.Attribute("Time").Value.ToString.Split("-")(0)
            t_stop = Now.Date & " " & elem.Attribute("Time").Value.ToString.Split("-")(1)
            '   If Now >= t_start And Now <= t_stop Then
            ListView5.Items.Add(elem.Attribute("dir_patch").Value)
            ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(elem.Attribute("Time").Value)
            ' ЕстьВремя = True
            '  End If
        Next




        ЕстьВремя = False

    End Sub

    Private Sub ОткрытьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ОткрытьToolStripMenuItem.Click
        Save.OpenMainMusicList()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Save.SaveDynList()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Save.OpenDynList()
    End Sub

    Private Sub УдалитьПрофильToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles УдалитьПрофильToolStripMenuItem.Click
        Templates.УдалитьПрофиль()
    End Sub

    Private Sub СохранитьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles СохранитьToolStripMenuItem.Click
        Save.SaveMainMusicList()
    End Sub
End Class




