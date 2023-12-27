Imports System.Runtime.InteropServices
Imports Un4seen.Bass
Imports System.IO
Imports System.Data.OleDb


Public Class setting
    Public setting As System.Xml.Linq.XDocument
    Dim ВсеПесни As ArrayList
    Dim PlayList As ArrayList
    Dim ПромежуточныйПлейлист As System.Xml.Linq.XDocument
    Dim sample_check As Integer

    Private Sub setting_ImeModeChanged(sender As Object, e As EventArgs) Handles Me.ImeModeChanged

    End Sub
    Private Sub setting_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim dir_count As Integer
        Dim r As Integer
        Dim xe As XElement
        Dim Summ As Integer
        Dim ProfileName As String
        Dim Profile As XDocument



        For r = 1 To Grid1.RowsCount - 1
            Summ = Summ + Grid1(r, 3).Value
        Next
        If Grid1.Rows.Count > 1 Then
            If Summ < 100 Then
                MsgBox("Сумма вероятностей должна быть обязательно равна 100%. Сейчас " & Summ)
                e.Cancel = True
                Exit Sub

            End If
        End If


        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        dir_count = Grid1.RowsCount - 1


        Настройки.Document.Element("Setting").Element("music").Element("dyn_dir").Elements.Remove


        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If



        Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements.Remove
        Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Elements.Remove
        'For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements

        '    elem.Remove()

        'Next

        '  Profile.Save(ProfileName)

        For r = 1 To Grid1.RowsCount - 1
            xe = New XElement("_" & r, "")
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Add(xe)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Element("_" & r).SetAttributeValue("dir_patch", Grid1(r, 1).Value)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Element("_" & r).SetAttributeValue("Use", "1")
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Element("_" & r).SetAttributeValue("Time", Grid1(r, 4).Value)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Element("_" & r).SetAttributeValue("Вес", Grid1(r, 3).Value)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Element("_" & r).SetAttributeValue("SongCount", Grid1(r, 2).Value)
        Next


        For r = 1 To Grid2.RowsCount - 1
            xe = New XElement("_" & r, "")
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Add(xe)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Element("_" & r).SetAttributeValue("dir_patch", Grid2(r, 1).Value)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Element("_" & r).SetAttributeValue("Use", "1")
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Element("_" & r).SetAttributeValue("Time", Grid2(r, 4).Value)
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Element("_" & r).SetAttributeValue("Вес", "100")
            Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Element("_" & r).SetAttributeValue("SongCount", Grid2(r, 2).Value)
        Next



        Profile.Save(ProfileName)
    End Sub




    Private Sub setting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Profile As XDocument
        '  DateTimePicker1.Value = Now.Date & " " & "00:00:12"

        Try

            Profile = GenerlaSetting.ProfileLoad

            Домен.Text = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value
            SubDomen.Text = Profile.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value
            SerialNumber.Text = Profile.Document.Element("Setting").Element("Domen").Attribute("serial").Value
            Логи.Text = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
            БазаДанных.Text = Profile.Document.Element("Setting").Element("db").Attribute("file").Value

            AlarmДжингл.Text = Profile.Document.Element("Setting").Element("Jingle").Attribute("Jingle").Value

            '  Silence = Profile.Document.Element("Setting").Element("Silence").Attribute("value").Value

            KillTimeOut.Value = Profile.Document.Element("Setting").Element("music").Attribute("killtimeout").Value

            adv_audio_dev.Items.Clear()


            get_dev_list()
            MUsicGridРазметка()



            ПутьКПлейлистам.Text = Profile.Document.Element("Setting").Element("sch").Attribute("dir").Value
            ПутьКРоликам.Text = Profile.Document.Element("Setting").Element("adv").Attribute("dir").Value

            '  main_music_volume.Value = setting.Document.Element("Setting").Element("music").Attribute("volume").Value


            'news_volume.Value = setting.Document.Element("Setting").Element("news").Attribute("volume").Value
            'AlarmVolume.Value = setting.Document.Element("Setting").Element("alarm").Attribute("volume").Value
            'JingleVolume.Value = setting.Document.Element("Setting").Element("alarm").Attribute("volume_jingle").Value
            'KillTimeOut.Value = setting.Document.Element("Setting").Element("music").Attribute("killtimeout").Value
            'ГромкостьРекламы.Value = setting.Document.Element("Setting").Element("adv").Attribute("volume").Value


            'TrackBar1.Value = setting.Document.Element("Setting").Element("adv").Attribute("volume").Value
            'TrackBar2.Value = setting.Document.Element("Setting").Element("alarm").Attribute("volume").Value



            AlarmMess.Text = Profile.Document.Element("Setting").Element("alarm").Attribute("dir").Value

            TrackRepeatInt.Value = Profile.Document.Element("Setting").Element("music").Attribute("PlayListTimeCheck").Value



            'ListView1.Items.Clear()
            'For Each elem In setting.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
            '    ListView1.Items.Add(elem.Value.ToString.Split("|")(0))
            '    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(elem.Value.ToString.Split("|")(1))
            'Next

            '    music_to_adv_fade.Value = setting.Document.Element("Setting").Element("music").Element("music_to_adv_fade").Attribute("value").Value


            TGToken.Text = Profile.Document.Element("Setting").Element("Telegram").Attribute("Token").Value
            TGChatID.Text = Profile.Document.Element("Setting").Element("Telegram").Attribute("ChatID").Value


            'Загружаем списко песен
            Dim НоваяЯчейка As SourceGrid.Cells.Cell
            For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements
                Dim Чб As New SourceGrid.Cells.CheckBox
                Dim ed As New SourceGrid.Cells.Editors.TextBox(GetType(String))
                Grid1.Rows.Insert(Grid1.Rows.Count)
                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("dir_patch").Value)
                Grid1(Grid1.RowsCount - 1, 1) = НоваяЯчейка


                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("SongCount").Value)
                Grid1(Grid1.RowsCount - 1, 2) = НоваяЯчейка

                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("Вес").Value)
                Grid1(Grid1.RowsCount - 1, 3) = НоваяЯчейка

                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("Time").Value)
                Grid1(Grid1.RowsCount - 1, 4) = НоваяЯчейка
                Grid1(Grid1.RowsCount - 1, 4).Editor = ed

                Grid1(Grid1.RowsCount - 1, 3).Editor = ed

                If elem.Attribute("Use").Value = "1" Then
                    Чб.Checked = True
                End If
                If elem.Attribute("Use").Value = "0" Then
                    Чб.Checked = False
                End If
                Grid1(Grid1.RowsCount - 1, 0) = Чб

            Next

            For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Elements
                Dim Чб As New SourceGrid.Cells.CheckBox
                Dim ed As New SourceGrid.Cells.Editors.TextBox(GetType(String))
                Grid2.Rows.Insert(Grid2.Rows.Count)
                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("dir_patch").Value)
                Grid2(Grid2.RowsCount - 1, 1) = НоваяЯчейка


                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("SongCount").Value)
                Grid2(Grid2.RowsCount - 1, 2) = НоваяЯчейка

                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("Вес").Value)
                Grid2(Grid2.RowsCount - 1, 3) = НоваяЯчейка

                НоваяЯчейка = New SourceGrid.Cells.Cell(elem.Attribute("Time").Value)
                Grid2(Grid2.RowsCount - 1, 4) = НоваяЯчейка
                Grid2(Grid2.RowsCount - 1, 4).Editor = ed

                Grid2(Grid2.RowsCount - 1, 3).Editor = ed

                If elem.Attribute("Use").Value = "1" Then
                    Чб.Checked = True
                End If
                If elem.Attribute("Use").Value = "0" Then
                    Чб.Checked = False
                End If
                Grid2(Grid2.RowsCount - 1, 0) = Чб

            Next



            UseJingle.Checked = Profile.Document.Element("Setting").Element("Jingle").Attribute("Use").Value


            If Profile.Document.Element("Setting").Element("music").Attribute("CheckRepeat").Value = "1" Then
                RepeatTrackLimit.Checked = True
            End If
            If Profile.Document.Element("Setting").Element("music").Attribute("CheckRepeat").Value = "0" Then
                RepeatTrackLimit.Checked = False
            End If

            UseCrossFade.Checked = MusicSetting.UseCrossFade
            UseFadeOut.Checked = MusicSetting.UseFadeOUt

            FadeOuTime.Value = MusicSetting.FadeOutTime
            CrossFadeTime.Value = MusicSetting.CrossFadeTime




            MinADVDistance.Value = Profile.Document.Element("Setting").Element("adv").Attribute("MinTimeDist").Value


            MusicPause.Checked = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("MusicPause").Value
            StartNewTrack.Checked = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("StartNewTrack").Value
            NewTrackTime.Text = Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("NewTrackTime").Value


            OpenLastPlaylist.Checked = Profile.Document.Element("Setting").Element("music").Attribute("OpenLastPL").Value

            CkeckAutor.Checked = MusicSetting.CheckAutor
            CheckAutorTime.Value = Profile.Document.Element("Setting").Element("music").Attribute("AutorTimeCheck").Value
            Разделитель.Text = Profile.Document.Element("Setting").Element("music").Attribute("Разделитель").Value

            If Profile.Document.Element("Setting").Element("music").Attribute("GenMode").Value = "AKMode" Then
                AKMode.Checked = True
            End If
            If Profile.Document.Element("Setting").Element("music").Attribute("GenMode").Value = "PercentMode" Then
                PercentMode.Checked = True
            End If

            LostADVTime.Value = Profile.Document.Element("Setting").Element("adv").Attribute("LostADVTime").Value


            УровенТИшины.Value = Profile.Document.Element("Setting").Element("Silence").Attribute("Value").Value
            ClearAlarmDir.Checked = Profile.Document.Element("Setting").Element("alarm").Attribute("ClearDir").Value

            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("PLayingMode").Value = "Mono" Then
                Mono.Checked = True
            End If
            If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("PLayingMode").Value = "Stereo" Then
                Stereo.Checked = True
            End If


            GenMusList.Checked = Profile.Document.Element("Setting").Element("music").Attribute("GenNewMusList").Value
            DateTimePicker1.Value = Now.Date & " " & Profile.Document.Element("Setting").Element("music").Attribute("TimeGenNewMusPL").Value
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation") = "Log" Then
            '    LogRegulation.Checked = True
            'End If
            'If Profile.Document.Element("Setting").Element("GeneralSetting").Attribute("Regulation") = "Line" Then
            '    LineRegulation.Checked = True
            'End If
            'Читаем Список шаблонов

        Catch ex As Exception
            MsgBox("Error load setting " & ex.Message)
        End Try

    End Sub
    Public Sub MUsicGridРазметка()
        Grid1.ColumnsCount = 5
        Grid1.RowsCount = 1
        Grid1.FixedRows = 1
        Grid1.FixedColumns = 1


        Dim Галка As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Use")
        Галка.AutomaticSortEnabled = False
        Grid1(0, 0) = Галка
        Grid1.Columns(0).Width = 35

        Dim Путь As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Путь")
        Путь.AutomaticSortEnabled = False
        Grid1(0, 1) = Путь
        Grid1.Columns(1).Width = 225

        Dim Количество As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Колич.")
        '   Количество.AutomaticSortEnabled = False
        Grid1(0, 2) = Количество
        Grid1.Columns(2).Width = 50

        Dim Вес As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Вес(%)")
        Вес.AutomaticSortEnabled = False
        Grid1(0, 3) = Вес
        Grid1.Columns(3).Width = 45


        Dim Время As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Время")
        Время.AutomaticSortEnabled = False
        Grid1(0, 4) = Время
        Grid1.Columns(4).Width = 135



        Grid2.ColumnsCount = 5
        Grid2.RowsCount = 1
        Grid2.FixedRows = 1
        Grid2.FixedColumns = 1


        Dim Галка2 As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Use")
        Галка2.AutomaticSortEnabled = False
        Grid2(0, 0) = Галка2
        Grid2.Columns(0).Width = 35

        Dim Путь2 As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Путь")
        Путь2.AutomaticSortEnabled = False
        Grid2(0, 1) = Путь2
        Grid2.Columns(1).Width = 225

        Dim Количество2 As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Колич.")
        '   Количество.AutomaticSortEnabled = False
        Grid2(0, 2) = Количество2
        Grid2.Columns(2).Width = 50

        Dim Вес2 As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Вес(%)")
        Вес2.AutomaticSortEnabled = False
        Grid2(0, 3) = Вес2
        Grid2.Columns(3).Width = 45


        Dim Время2 As SourceGrid.Cells.ColumnHeader = New SourceGrid.Cells.ColumnHeader("Время")
        Время.AutomaticSortEnabled = False
        Grid2(0, 4) = Время2
        Grid2.Columns(4).Width = 135


    End Sub
    Public Sub get_dev_list()

        Dim adv_audio As String
        Dim music_audio As String
        Dim news_audio As String
        Dim alarm_audio As String
        Dim bas_dev_info As New BASS_DEVICEINFO()
        Dim i As Integer
        adv_audio_dev.Items.Clear()
        MusicAudioDev.Items.Clear()
        AlarmAudioDev.Items.Clear()

        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        adv_audio = setting.Document.Element("Setting").Element("adv").Attribute("audio_dev").Value
        music_audio = setting.Document.Element("Setting").Element("music").Attribute("audio_dev").Value
        news_audio = setting.Document.Element("Setting").Element("news").Attribute("audio_dev").Value
        alarm_audio = setting.Document.Element("Setting").Element("alarm").Attribute("audio_dev").Value

        While (Bass.BASS_GetDeviceInfo(i, bas_dev_info))
            adv_audio_dev.Items.Add(bas_dev_info.name)
            MusicAudioDev.Items.Add(bas_dev_info.name)
            AlarmAudioDev.Items.Add(bas_dev_info.name)



            If adv_audio = bas_dev_info.name Then
                adv_audio_dev.Text = bas_dev_info.name
            End If
            If music_audio = bas_dev_info.name Then
                MusicAudioDev.Text = bas_dev_info.name
            End If
            If alarm_audio = bas_dev_info.name Then
                AlarmAudioDev.Text = bas_dev_info.name
            End If
            i += 1
        End While
        ' End If


    End Sub
    Private Sub MusicAudioDev_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MusicAudioDev.SelectedIndexChanged
        Dim Profile As XDocument

        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("music").SetAttributeValue("audio_dev", MusicAudioDev.Text)
        Profile.Save(ProFileName)
    End Sub

    Private Sub DefaultAudioDev_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ПутьКРоликам.MouseDoubleClick
        If FolderBrowserDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
            ПутьКРоликам.Text = FolderBrowserDialog2.SelectedPath
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles ПутьКРоликам.TextChanged
        Dim Profile As XDocument
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If



        Profile.Document.Element("Setting").Element("adv").SetAttributeValue("dir", ПутьКРоликам.Text)
        Profile.Save(ProFileName)


    End Sub

    Private Sub ПутьКПлейлистам_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ПутьКПлейлистам.MouseDoubleClick
        If FolderBrowserDialog3.ShowDialog = Windows.Forms.DialogResult.OK Then
            ПутьКПлейлистам.Text = FolderBrowserDialog3.SelectedPath
        End If
    End Sub

    Private Sub ПутьКПлейлистам_TextChanged(sender As Object, e As EventArgs) Handles ПутьКПлейлистам.TextChanged
        Dim Profile As XDocument
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("sch").SetAttributeValue("dir", ПутьКПлейлистам.Text)
        Profile.Save(ProFileName)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles adv_audio_dev.SelectedIndexChanged
        Dim Profile As XDocument

        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If



        Profile.Document.Element("Setting").Element("adv").SetAttributeValue("audio_dev", adv_audio_dev.Text)
        Profile.Save(ProFileName)



        ' Form1.Set_volume(main_music_volume.Value)
    End Sub



    Private Sub ДобавитьToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ДобавитьToolStripMenuItem2.Click
        Form2.ShowDialog()
    End Sub



    Private Sub Логи_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Логи.MouseDoubleClick
        If FolderBrowserDialog4.ShowDialog = Windows.Forms.DialogResult.OK Then
            Логи.Text = FolderBrowserDialog4.SelectedPath
        End If
    End Sub

    Private Sub Логи_TextChanged(sender As Object, e As EventArgs) Handles Логи.TextChanged
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        setting.Document.Element("Setting").Element("logs").SetAttributeValue("dir", Логи.Text)
        setting.Save("Настройки.xml")
    End Sub

    Private Sub БазаДанных_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles БазаДанных.MouseDoubleClick
        SaveFileDialog1.Filter = "Базы данных | *.accdb"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            БазаДанных.Text = SaveFileDialog1.FileName
        End If
    End Sub

    Private Sub БазаДанных_TextChanged(sender As Object, e As EventArgs) Handles БазаДанных.TextChanged
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        setting.Document.Element("Setting").Element("db").SetAttributeValue("file", БазаДанных.Text)
        setting.Save("Настройки.xml")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim dbpatch As String
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        dbpatch = setting.Document.Element("Setting").Element("db").Attribute("file").Value
        If System.IO.File.Exists(dbpatch) = True Then
            If MsgBox("База с таким именем уже сущетсвует. Пересоздать невозможно", MsgBoxStyle.Information) = MsgBoxResult.Yes Then
                Exit Sub
            End If
        Else
            DataBase.CreateDB(dbpatch)
        End If


    End Sub
    Public Sub del_db(ByVal dbpatch As String)
        Dim cat As New ADOX.Catalog()

        ' cat.Create("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" & dbpatch)
        ' cat = Nothing

        'create the empty table in the DB file
        Dim conn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" & dbpatch & ";Persist Security Info=True")
        conn.Open()
        Dim cmd As New OleDb.OleDbCommand("", conn)
        cmd.CommandText = "DROP DATABASE db"
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If FolderBrowserDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
            ПутьКРоликам.Text = FolderBrowserDialog2.SelectedPath
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If FolderBrowserDialog3.ShowDialog = Windows.Forms.DialogResult.OK Then
            ПутьКПлейлистам.Text = FolderBrowserDialog3.SelectedPath
        End If
    End Sub



    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If FolderBrowserDialog4.ShowDialog = Windows.Forms.DialogResult.OK Then
            Логи.Text = FolderBrowserDialog4.SelectedPath
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        SaveFileDialog1.Filter = "Базы данных | *.accdb"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            БазаДанных.Text = SaveFileDialog1.FileName
        End If
    End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs)
        Dim dbpatch As String
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

        dbpatch = setting.Document.Element("Setting").Element("db").Attribute("file").Value
        ' SQLiteConnection.CreateFile(dbpatch)

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)
        Dim dbpatch As String
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        dbpatch = setting.Document.Element("Setting").Element("db").Attribute("file").Value


        '  Dim myconnection As New SQLiteConnection(dbpatch)

        Dim myTableCreate As String = "CREATE TABLE MyTable(CustomerID INTEGER PRIMARY KEY ASC, FirstName VARCHAR(25));"

        '  Dim mycommand = New SQLiteCommand(myTableCreate)

        '   mycommand.CommandText = myTableCreate
        '  mycommand.ExecuteNonQuery()



    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs)
        '   setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        '  setting.Document.Element("Setting").Element("music").Element("music_to_music_fade").SetAttributeValue("value", music_to_music_fade.Value)
        ' setting.Save("Настройки.xml")
    End Sub


    Private Sub AlarmMess_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles AlarmMess.MouseDoubleClick
        If FolderBrowserDialog6.ShowDialog = Windows.Forms.DialogResult.OK Then
            AlarmMess.Text = FolderBrowserDialog6.SelectedPath
        End If
    End Sub

    Private Sub AlarmMess_TextChanged(sender As Object, e As EventArgs) Handles AlarmMess.TextChanged
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        setting.Document.Element("Setting").Element("alarm").SetAttributeValue("dir", AlarmMess.Text)
        setting.Save("Настройки.xml")
    End Sub

    Private Sub AlarmAudioDev_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AlarmAudioDev.SelectedIndexChanged
        Dim Profile As XDocument
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("audio_dev", AlarmAudioDev.Text)
        Profile.Save(ProFileName)
    End Sub

    Private Sub AlarmVolume_ValueChanged(sender As Object, e As EventArgs)
        'setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'setting.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", AlarmVolume.Value)
        'setting.Save("Настройки.xml")

        'TrackBar2.Value = AlarmVolume.Value


    End Sub

    Private Sub KillTimeOut_ValueChanged(sender As Object, e As EventArgs) Handles KillTimeOut.ValueChanged
        Dim Profile As XDocument


        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")


        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("music").SetAttributeValue("killtimeout", KillTimeOut.Value)
        Profile.Save(ProFileName)


    End Sub

    Private Sub Button8_Click_2(sender As Object, e As EventArgs) Handles Button8.Click
        If FolderBrowserDialog6.ShowDialog = Windows.Forms.DialogResult.OK Then
            AlarmMess.Text = FolderBrowserDialog6.SelectedPath
        End If
    End Sub

    Private Sub AlarmДжингл_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles AlarmДжингл.MouseDoubleClick
        OpenFileDialog1.Filter = "Аудио Файлы (*.mp3, *.wav, *.wma)|*.mp3;*.wav;*.wma|All files (*.*)|*.*"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            AlarmДжингл.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub AlarmДжингл_TextChanged(sender As Object, e As EventArgs) Handles AlarmДжингл.TextChanged
        Dim Profile As XDocument

        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("Jingle").SetAttributeValue("Jingle", AlarmДжингл.Text)
        Profile.Save(ProFileName)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button9_Click_1(sender As Object, e As EventArgs) Handles Button9.Click
        'setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ' setting.Document.Element("Setting").Element("alarm").SetAttributeValue("Jingle", AlarmДжингл.Text)
        ' setting.Save("Настройки.xml")
        OpenFileDialog1.Filter = "Аудио Файлы (*.mp3, *.wav, *.wma)|*.mp3;*.wav;*.wma|All files (*.*)|*.*"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            AlarmДжингл.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub TG_Send_Mess_CheckStateChanged(sender As Object, e As EventArgs) Handles TG_Send_Mess.CheckStateChanged
        If TG_Send_Mess.CheckState = CheckState.Unchecked Then
            TG_Send_Mess_ADV.Checked = False
            TG_Send_Mess_alarm.Checked = False
            TG_Send_Mess_Music.Checked = False
            TG_Send_Mess_Status.Checked = False

            TG_Send_Mess_ADV.Enabled = False
            TG_Send_Mess_alarm.Enabled = False
            TG_Send_Mess_Music.Enabled = False
            TG_Send_Mess_Status.Enabled = False
            Exit Sub
        End If
        If TG_Send_Mess.CheckState = CheckState.Checked Then
            TG_Send_Mess_ADV.Checked = True
            TG_Send_Mess_alarm.Checked = True
            TG_Send_Mess_Music.Checked = True
            TG_Send_Mess_Status.Checked = True

            TG_Send_Mess_ADV.Enabled = True
            TG_Send_Mess_alarm.Enabled = True
            TG_Send_Mess_Music.Enabled = True
            TG_Send_Mess_Status.Enabled = True
            Exit Sub
        End If


    End Sub

    Private Sub TGToken_TextChanged(sender As Object, e As EventArgs) Handles TGToken.TextChanged
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        setting.Document.Element("Setting").Element("Telegram").SetAttributeValue("Token", TGToken.Text)
        setting.Save("Настройки.xml")
    End Sub

    Private Sub TGChatID_TextChanged(sender As Object, e As EventArgs) Handles TGChatID.TextChanged
        setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        setting.Document.Element("Setting").Element("Telegram").SetAttributeValue("ChatID", TGChatID.Text)
        setting.Save("Настройки.xml")

    End Sub


    Private Sub ИзменитьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ИзменитьToolStripMenuItem.Click

    End Sub

    Private Sub ИзменитьToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ИзменитьToolStripMenuItem1.Click
        Form2.Изменение = True
        Form2.ShowDialog()
    End Sub

    Private Sub ДобавитьToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ДобавитьToolStripMenuItem1.Click

    End Sub

    Private Sub УдалитьToolStripMenuItem1_Click(sender As Object, e As EventArgs)

    End Sub



    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Telega.Bot()
        Telega.Send_text(Now & "_HELLO WORLD!!!")
    End Sub

    Private Sub JingleVolume_ValueChanged(sender As Object, e As EventArgs)
        'setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'setting.Document.Element("Setting").Element("alarm").SetAttributeValue("volume_jingle", JingleVolume.Value)
        'setting.Save("Настройки.xml")
        'TrackBar3.Value = JingleVolume.Value
    End Sub



    'Private Sub TimeOffStart_TextChanged(sender As Object, e As EventArgs) Handles TimeOffStart.TextChanged
    '    setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
    '    setting.Document.Element("Setting").Element("timeoff").SetAttributeValue("Start", TimeOffStart.Text)
    '    setting.Save("Настройки.xml")
    'End Sub

    'Private Sub TimeOffStop_TextChanged(sender As Object, e As EventArgs) Handles TimeOffStop.TextChanged
    '    setting = System.Xml.Linq.XDocument.Load("Настройки.xml")
    '    setting.Document.Element("Setting").Element("timeoff").SetAttributeValue("Stop", TimeOffStop.Text)
    '    setting.Save("Настройки.xml")
    'End Sub

    'Private Sub DShowLIb_Click(sender As Object, e As EventArgs)
    '    Dim Silence As String
    '    Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
    '    Silence = setting.Document.Element("Setting").Element("Silence").Attribute("value").Value
    '    'My.Settings.Save()
    '    'get_dev_list()
    '    ' BassNet.Checked = False
    '    ' DShowLIb.Checked = True
    '    get_dev_list()

    '    'If My.Settings.DShowLIb = True Then
    '    '    TrackBar1.Minimum = Silence
    '    '    TrackBar1.Maximum = -0

    '    '    TrackBar2.Minimum = Silence
    '    '    TrackBar2.Maximum = 0

    '    '    TrackBar3.Minimum = Silence
    '    '    TrackBar3.Maximum = 0

    '    '    ГромкостьРекламы.Maximum = 0
    '    '    ГромкостьРекламы.Minimum = Silence

    '    '    AlarmVolume.Maximum = 0
    '    '    AlarmVolume.Minimum = Silence

    '    '    JingleVolume.Maximum = 0
    '    '    JingleVolume.Minimum = Silence

    '    '    For Each times In Настройки.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
    '    '        times.SetValue(times.Value.ToString.Split("|")(0) & "|" & "0")
    '    '    Next
    '    'End If

    '    'If My.Settings.BassNet = True Then
    '    '    TrackBar1.Minimum = 0
    '    '    TrackBar1.Maximum = 100

    '    '    TrackBar2.Minimum = 0
    '    '    TrackBar2.Maximum = 100

    '    '    TrackBar3.Minimum = 0
    '    '    TrackBar3.Maximum = 100


    '    '    ГромкостьРекламы.Minimum = 0
    '    '    ГромкостьРекламы.Maximum = 100

    '    '    AlarmVolume.Minimum = 0
    '    '    AlarmVolume.Maximum = 100

    '    '    JingleVolume.Minimum = 0
    '    '    JingleVolume.Maximum = 100


    '    '    For Each times In Настройки.Document.Element("Setting").Element("music").Element("volume_sch").Attributes
    '    '        times.SetValue(times.Value.ToString.Split("|")(0) & "|" & "100")
    '    '    Next

    '    'End If
    '    'Настройки.Document.Element("Setting").Element("adv").SetAttributeValue("volume", ГромкостьРекламы.Value)
    '    'Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume", AlarmVolume.Value)
    '    'Настройки.Document.Element("Setting").Element("alarm").SetAttributeValue("volume_jingle", JingleVolume.Value)

    '    'Настройки.Save("Настройки.xml")


    'End Sub

    Private Sub DShowLIb_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub BassNet_CheckedChanged(sender As Object, e As EventArgs)

    End Sub


    Private Sub ДобавитьДиректориюToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ДобавитьДиректориюToolStripMenuItem.Click
        Dim Чб As New SourceGrid.Cells.CheckBox
        Dim НоваяЯчейка As SourceGrid.Cells.Cell
        Dim Время As SourceGrid.Cells.Editors.ComboBox = New SourceGrid.Cells.Editors.ComboBox(GetType(String), "00:00:00-23:59:59".Split(";"), True)
        Dim song_count As Integer
        Dim row_count As Integer
        Dim Вес As Integer
        Dim r As Integer

        row_count = Grid1.Rows.Count
        If row_count = 1 Then
            Вес = 100
        Else
            Вес = 0
        End If





        If MusicDirSelect.ShowDialog = DialogResult.OK Then
            Grid1.Rows.Insert(Grid1.RowsCount)
            song_count = get_song_count(MusicDirSelect.SelectedPath)
            НоваяЯчейка = New SourceGrid.Cells.Cell(MusicDirSelect.SelectedPath)
            Grid1(Grid1.RowsCount - 1, 1) = НоваяЯчейка


            НоваяЯчейка = New SourceGrid.Cells.Cell(song_count)
            Grid1(Grid1.RowsCount - 1, 2) = НоваяЯчейка






            НоваяЯчейка = New SourceGrid.Cells.Cell("")
            Grid1(Grid1.RowsCount - 1, 3) = НоваяЯчейка

            Вес = 100 / (Grid1.RowsCount - 1)
            For r = 1 To Grid1.RowsCount - 1
                Grid1(r, 3).Value = Вес
            Next



            НоваяЯчейка = New SourceGrid.Cells.Cell("00:00:00-23:59:59")
            Grid1(Grid1.RowsCount - 1, 4) = НоваяЯчейка
            '  Grid1(Grid1.RowsCount - 1, 4).Editor = ed




            Grid1(Grid1.RowsCount - 1, 0) = Чб

            Dim ed As New SourceGrid.Cells.Editors.TextBox(GetType(String))
            Grid1(Grid1.RowsCount - 1, 3).Editor = ed
            Grid1(Grid1.RowsCount - 1, 4).Editor = ed


            Чб.Checked = True
            Grid1.Update()
            Grid1.Refresh()


        End If

    End Sub

    Private Sub MusicDirSelect_HelpRequest(sender As Object, e As EventArgs) Handles MusicDirSelect.HelpRequest

    End Sub

    Public Function get_song_count(ByVal dirpatch As String)

        Dim ПесниВТекущейПапке As New ArrayList
        Try
            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dirpatch, "*.mp3", SearchOption.AllDirectories))
            ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dirpatch, "*.wav", SearchOption.AllDirectories))
            ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dirpatch, "*.wma", SearchOption.AllDirectories))
            ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dirpatch, "*.aac", SearchOption.AllDirectories))

            Return ПесниВТекущейПапке.Count
        Catch ex As Exception

        End Try

    End Function

    Private Sub Grid1_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles Grid1.PreviewKeyDown
        Dim r As Integer
        Dim Summ As Integer
        Dim ActiveRow As Integer = Grid1.Selection.ActivePosition.Row
        Dim ActiveCollumn As Integer = Grid1.Selection.ActivePosition.Column
        For r = 1 To Grid1.RowsCount - 1
            Summ = Summ + Grid1(r, 3).Value
        Next
        If e.KeyValue = 13 Then
            If Summ > 100 Then
                MsgBox("Общее число вероятностей попадания в директории не может быть больше 100%")
                Grid1(ActiveRow, 3).Value = 0
            End If
        End If


    End Sub

    Private Sub Grid1_Paint(sender As Object, e As PaintEventArgs) Handles Grid1.Paint

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TrackRepeatInt_ValueChanged(sender As Object, e As EventArgs) Handles TrackRepeatInt.ValueChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim ProfileName As String
        Dim Profile As XDocument

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If




        Profile.Document.Element("Setting").Element("music").SetAttributeValue("PlayListTimeCheck", TrackRepeatInt.Value)
        Profile.Save(ProfileName)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("настройки.xml")
        'End If

    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

    End Sub

    Private Sub CrossFadeTime_ValueChanged(sender As Object, e As EventArgs) Handles CrossFadeTime.ValueChanged
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("CrossFadeTime", CrossFadeTime.Value)
        Profile.Save(ProFileName)

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        SerialNumber.Text = Домен.Text & SubDomen.Text & "_" & GenSerial()
    End Sub
    Private Function GenSerial()
        Dim Символы As Array = "6,q,2,w,7,e,r,1,t,y,3,u,i,2,o,4,p,a,s,7,d,f,3,g,h,j,4,k,l,8,z,x,5,c,v,0,b,n,8,m,9".Split(",")
        Dim r As New Random
        Dim m As Integer
        Dim ID As String
        ID = ""
        System.Threading.Thread.Sleep(25)
        For m = 0 To 7
            ID = ID & Символы(r.Next(0, Символы.Length - 1))
        Next
        ID = ID & "-"
        For m = 9 To 12
            ID = ID & Символы(r.Next(0, Символы.Length - 1))
        Next
        ID = ID & "-"
        For m = 14 To 17
            ID = ID & Символы(r.Next(0, Символы.Length - 1))
        Next

        Return ID
    End Function

    Private Sub Домен_TextChanged(sender As Object, e As EventArgs) Handles Домен.TextChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Настройки.Document.Element("Setting").Element("Domen").SetAttributeValue("Name", Домен.Text)
        Настройки.Save("настройки.xml")
        Form1.Label2.Text = Домен.Text & SubDomen.Text
    End Sub

    Private Sub SubDomen_TextChanged(sender As Object, e As EventArgs) Handles SubDomen.TextChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Настройки.Document.Element("Setting").Element("Domen").SetAttributeValue("SubDomen", SubDomen.Text)
        Настройки.Save("настройки.xml")
        Form1.Label2.Text = Домен.Text & SubDomen.Text
    End Sub

    Private Sub SerialNumber_TextChanged(sender As Object, e As EventArgs) Handles SerialNumber.TextChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Настройки.Document.Element("Setting").Element("Domen").SetAttributeValue("serial", SerialNumber.Text)
        Настройки.Save("настройки.xml")
    End Sub

    Private Sub УдалитьДиректориюToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles УдалитьДиректориюToolStripMenuItem.Click
        Dim r As Integer

        r = Grid1.Selection.ActivePosition.Row

        If r = -1 Then
            MsgBox("Нужно выделить директорию, а потом удалять", MsgBoxStyle.Critical)
            Exit Sub
        End If


        Grid1.Rows.Remove(r)
    End Sub

    Private Sub TabPage17_Click(sender As Object, e As EventArgs) Handles TabPage17.Click

    End Sub

    Private Sub NightOnOf_CheckedChanged(sender As Object, e As EventArgs) Handles NightOnOf.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        If NightOnOf.Checked = True Then
            Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Use", "1")
        End If
        If NightOnOf.Checked = False Then
            Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Use", "0")
        End If
        Настройки.Save("настройки.xml")
    End Sub

    Private Sub TimeOffStart_MaskChanged(sender As Object, e As EventArgs) Handles TimeOffStart.MaskChanged
        'Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'Настройки.Document.Element("Setting").Element("timeoff").SetAttributeValue("Start", TimeOffStart.Text)
        'Настройки.Save("Настройки.xml")
    End Sub

    Private Sub TimeOffStart_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles TimeOffStart.MaskInputRejected

    End Sub

    Private Sub UseJingle_CheckedChanged(sender As Object, e As EventArgs) Handles UseJingle.CheckedChanged
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        'If UseJingle.Checked = True Then
        '    Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("UseJingle", "1")
        'End If
        'If UseJingle.Checked = False Then
        '    Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("UseJingle", "0")
        'End If

        Profile.Document.Element("Setting").Element("Jingle").SetAttributeValue("Use", UseJingle.Checked)
        Profile.Save(ProFileName)
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs)









    End Sub

    Private Shared Sub execCommand(ByVal conn As OleDbConnection, ByVal sqlText As String)
        Try
            Using command As OleDbCommand = New OleDbCommand()
                command.Connection = conn
                command.CommandText = sqlText
                command.ExecuteNonQuery()
            End Using
        Catch ex As Exception

        End Try


    End Sub

    Private Sub TimeOffStart_TextChanged(sender As Object, e As EventArgs) Handles TimeOffStart.TextChanged

    End Sub

    Private Sub ДобавитьШаблонToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ДобавитьШаблонToolStripMenuItem.Click
        If System.IO.Directory.Exists("Templates") = False Then
            System.IO.Directory.CreateDirectory("Templates")
        End If




    End Sub

    Private Sub RepeatTrackLimit_CheckedChanged(sender As Object, e As EventArgs) Handles RepeatTrackLimit.CheckedChanged
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim ProfileName As String
        Dim Profile As XDocument

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If



        If RepeatTrackLimit.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("CheckRepeat", "1")
        End If
        If RepeatTrackLimit.Checked = False Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("CheckRepeat", "0")
        End If




        Profile.Save(ProfileName)
    End Sub

    Private Sub Button10_Click_1(sender As Object, e As EventArgs) Handles Button10.Click
        AudioDev_Test(MusicSetting.AudioDev, (MusicTestVolume.Value) / 100)
    End Sub


    Public Sub AudioDev_Test(ByVal AudioDev As String, ByVal volume As Integer)
        Dim hr As Integer
        Dim i As Integer
        Dim dev_num As Integer = GenerlaSetting.GetDevNum(AudioDev)

        hr = Bass.BASS_Init(dev_num, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
        sample_check = Bass.BASS_SampleCreate(256, 440 * 64, 2, 1, BASSFlag.BASS_SAMPLE_LOOP Or BASSFlag.BASS_SAMPLE_OVER_POS)
        Dim data As Short() = New Short(1024) {}
        Dim channel As Integer


        For i = 0 To 1023
            data(i) = CShort(32767.0 * Math.Sin(i * 6.283185 / 64))
        Next
        hr = Bass.BASS_SampleSetData(sample_check, data)
        channel = Bass.BASS_SampleGetChannel(sample_check, False)

        hr = Bass.BASS_ChannelPlay(channel, True)


        Bass.BASS_ChannelSetAttribute(sample_check, BASSAttribute.BASS_ATTRIB_VOL, volume)
        '  Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, True)

    End Sub

    Private Sub Button11_Click_1(sender As Object, e As EventArgs) Handles Button11.Click
        Dev_Check_Stop()
    End Sub

    Public Sub Dev_Check_Stop()
        Bass.BASS_ChannelStop(sample_check)
        Bass.BASS_StreamFree(sample_check)
        '   Bass.BASS_Free()

    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click

        AudioDev_Test(SettingAlarm.AudioDev, (AlarmTextVolume.Value) / 100)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dev_Check_Stop()
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        AudioDev_Test(ADVSetting.audio_dev, (ADVTestVolume.Value + 100) / 100)
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Dev_Check_Stop()
    End Sub

    Private Sub FadeOuTime_ValueChanged(sender As Object, e As EventArgs) Handles FadeOuTime.ValueChanged
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("FadeOutTime", FadeOuTime.Value)
        Profile.Save(ProFileName)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles UseFadeOut.CheckedChanged
        Dim Profile As XDocument

        If UseFadeOut.Checked = True Then
            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

            ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
            If System.IO.File.Exists(ProFileName) = True Then
                Profile = XDocument.Load(ProFileName)
            Else
                Profile = XDocument.Load("Настройки.xml")
            End If

            Profile.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseFadeOut", "1")
            Profile.Save(ProFileName)

        Else
            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
            If System.IO.File.Exists(ProFileName) = True Then
                Profile = XDocument.Load(ProFileName)
            Else
                Profile = XDocument.Load("Настройки.xml")
            End If

            Profile.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseFadeOut", "0")
            Profile.Save(ProFileName)
        End If

    End Sub

    Private Sub UseCrossFade_CheckedChanged(sender As Object, e As EventArgs) Handles UseCrossFade.CheckedChanged
        Dim Profile As XDocument

        If UseCrossFade.Checked = True Then
            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
            If System.IO.File.Exists(ProFileName) = True Then
                Profile = XDocument.Load(ProFileName)
            Else
                Profile = XDocument.Load("Настройки.xml")
            End If
            Profile.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseCrossFade", "1")
            Profile.Save(ProFileName)
        Else
            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            ProFileName = setting.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
            If System.IO.File.Exists(ProFileName) = True Then
                Profile = XDocument.Load(ProFileName)
            Else
                Profile = XDocument.Load("Настройки.xml")
            End If

            Profile.Document.Element("Setting").Element("music").Element("Mix").SetAttributeValue("UseCrossFade", "0")
            Profile.Save(ProFileName)
        End If
    End Sub

    Private Sub TrackBar4_Scroll(sender As Object, e As EventArgs) Handles TrackBar4.Scroll
        MusicTestVolume.Value = TrackBar4.Value
    End Sub

    Private Sub MusicTestVolume_ValueChanged(sender As Object, e As EventArgs) Handles MusicTestVolume.ValueChanged
        TrackBar4.Value = MusicTestVolume.Value

        'If LineRegulation.Checked = True Then
        TestVolume((MusicTestVolume.Value) / 100)
        'End If

        'If LogRegulation.Checked = True Then
        '    TestVolume((MusicTestVolume.Value + 100) / 100)
        'End If

    End Sub

    Private Sub TrackBar5_Scroll(sender As Object, e As EventArgs) Handles TrackBar5.Scroll
        ADVTestVolume.Value = TrackBar5.Value
    End Sub

    Private Sub ADVTestVolume_ValueChanged(sender As Object, e As EventArgs) Handles ADVTestVolume.ValueChanged


        TrackBar5.Value = ADVTestVolume.Value
        'If LineRegulation.Checked = True Then
        TestVolume((ADVTestVolume.Value) / 100)
        'End If

        'If LogRegulation.Checked = True Then
        '    TestVolume((ADVTestVolume.Value + 100) / 100)
        'End If
    End Sub

    Private Sub TrackBar6_Scroll(sender As Object, e As EventArgs) Handles TrackBar6.Scroll
        AlarmTextVolume.Value = TrackBar6.Value
    End Sub

    Private Sub AlarmTextVolume_ValueChanged(sender As Object, e As EventArgs) Handles AlarmTextVolume.ValueChanged
        TrackBar6.Value = AlarmTextVolume.Value
        'If LineRegulation.Checked = True Then
        TestVolume((AlarmTextVolume.Value) / 100)
        'End If

        'If LogRegulation.Checked = True Then
        '    TestVolume((AlarmTextVolume.Value + 100) / 100)
        'End If
    End Sub


    Private Sub TestVolume(ByVal Value As Double)

        '
        Bass.BASS_ChannelSetAttribute(sample_check, BASSAttribute.BASS_ATTRIB_VOL, Value)
        ' Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, True)

        ' Bass.BASS_ChannelSlideAttribute(sample_check, BASSAttribute.BASS_SLIDE_LOG, Value / 100, 1)
        '
        ' Dim LC As Integer
        ' Dim RC As Integer
        ' Dim level As Integer = AddOn.Mix.BassMix.BASS_Mixer_ChannelGetLevel(sample_check)
        'Dim nnn As Double = Un4seen.Bass.Utils.LowWord32(level)

        'Dim kkk As Integer = Un4seen.Bass.Utils.LevelToDB(nnn, 65535)


        ' Bass.BASS_ChannelSlideAttribute(sample_check, BASSAttribute.BASS_SLIDE_LOG, Value / 100, 1)

    End Sub

    Private Sub TabPage5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub MinADVDistance_ValueChanged(sender As Object, e As EventArgs) Handles MinADVDistance.ValueChanged
        Dim ProfileName As String
        Dim Profile As XDocument
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("adv").SetAttributeValue("MinTimeDist", MinADVDistance.Value)
        Profile.Save(ProfileName)
    End Sub

    Private Sub MusicPause_CheckedChanged(sender As Object, e As EventArgs) Handles MusicPause.CheckedChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("MusicPause", MusicPause.Checked)
        Profile.Save(ProfileName)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CkeckAutor.CheckedChanged

        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("music").SetAttributeValue("CkeckAutor", CkeckAutor.Checked)
        Profile.Save(ProfileName)
        'If System.IO.File.Exists(ProfileName) = True Then
        '    Profile.Save(ProfileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If

    End Sub

    Private Sub CheckAutorTime_ValueChanged(sender As Object, e As EventArgs) Handles CheckAutorTime.ValueChanged
        Dim Profile As XDocument
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("music").SetAttributeValue("AutorTimeCheck", CheckAutorTime.Value)
        Profile.Save(ProFileName)
    End Sub

    Private Sub Разделитель_TextChanged(sender As Object, e As EventArgs) Handles Разделитель.TextChanged
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If



        Profile.Document.Element("Setting").Element("music").SetAttributeValue("Разделитель", Разделитель.Text)
        Profile.Save(ProFileName)
    End Sub

    Private Sub OpenLastPlaylist_CheckedChanged(sender As Object, e As EventArgs) Handles OpenLastPlaylist.CheckedChanged
        Dim Profile As XDocument
        Dim Настройки As XDocument
        Настройки = XDocument.Load("настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("music").SetAttributeValue("OpenLastPL", OpenLastPlaylist.Checked)
        '
        ' Try
        If System.IO.File.Exists(ProFileName) = True Then
            Profile.Save(ProFileName)
        Else
            Profile.Save("Настройки.xml")
        End If
        '  Catch ex As Exception

        '  End Try





    End Sub

    Private Sub AKMode_CheckedChanged(sender As Object, e As EventArgs) Handles AKMode.CheckedChanged
        Dim Profile As XDocument
        Dim Настройки As XDocument
        Настройки = XDocument.Load("настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        If AKMode.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("GenMode", "AKMode")
        End If
        If PercentMode.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("GenMode", "PercentMode")
        End If
        '
        ' Try
        Profile.Save(ProFileName)
        'If System.IO.File.Exists(ProFileName) = True Then
        '    Profile.Save(ProFileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs)

    End Sub





    Private Sub NumericUpDown1_ValueChanged_1(sender As Object, e As EventArgs)
        ' TrackBar4.Value = MusicTestVolume.Value

    End Sub

    Private Sub TextBox1_TextChanged_1(sender As Object, e As EventArgs) Handles MusicPLS.TextChanged

    End Sub

    Private Sub PercentMode_CheckedChanged(sender As Object, e As EventArgs) Handles PercentMode.CheckedChanged
        Dim Profile As XDocument
        Dim Настройки As XDocument
        Настройки = XDocument.Load("настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value

        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        If AKMode.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("GenMode", "AKMode")
        End If
        If PercentMode.Checked = True Then
            Profile.Document.Element("Setting").Element("music").SetAttributeValue("GenMode", "PercentMode")
        End If
        '
        ' Try

        Profile.Save(ProFileName)
        'If System.IO.File.Exists(ProFileName) = True Then
        '    Profile.Save(ProFileName)
        'Else
        '    Profile.Save("Настройки.xml")
        'End If
    End Sub

    Private Sub LostADVTime_ValueChanged(sender As Object, e As EventArgs) Handles LostADVTime.ValueChanged
        Dim Profile As XDocument
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If
        Profile.Document.Element("Setting").Element("adv").SetAttributeValue("LostADVTime", LostADVTime.Value)
        Profile.Save(ProFileName)
    End Sub

    Private Sub УровенТИшины_ValueChanged(sender As Object, e As EventArgs) Handles УровенТИшины.ValueChanged
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("Silence").SetAttributeValue("Value", УровенТИшины.Value)
        Profile.Save(ProFileName)

        TrackBar1.Value = УровенТИшины.Value
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        УровенТИшины.Value = TrackBar1.Value


    End Sub

    Private Sub ClearAlarmDir_CheckedChanged(sender As Object, e As EventArgs) Handles ClearAlarmDir.CheckedChanged
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProFileName) = True Then
            Profile = XDocument.Load(ProFileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        Profile.Document.Element("Setting").Element("alarm").SetAttributeValue("ClearDir", ClearAlarmDir.Checked)
        Profile.Save(ProFileName)
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs)
        'Form3.Timer1.Stop()
        'Form3.Timer2.Stop()
        'Form3.Timer3.Stop()
        'Dim Profile As XDocument

        'Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        'If System.IO.File.Exists(ProFileName) = True Then
        '    Profile = XDocument.Load(ProFileName)
        'Else
        '    Profile = XDocument.Load("Настройки.xml")
        'End If

        'If LineRegulation.Checked = True Then
        '    CheckSetting.БылКлик = True
        '    Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("Regulation", "Line")
        '    Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, False)
        'End If


        'Profile.Save(ProFileName)

        'CheckSetting.ChangeRegMetod()

        'Form3.Timer1.Start()
        'Form3.Timer2.Start()
        'Form3.Timer3.Start()
    End Sub

    Private Sub LogRegulation_CheckedChanged(sender As Object, e As EventArgs)
        'Form3.Timer1.Stop()
        'Form3.Timer2.Stop()
        'Form3.Timer3.Stop()

        'Dim Profile As XDocument

        'Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        'ProFileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        'If System.IO.File.Exists(ProFileName) = True Then
        '    Profile = XDocument.Load(ProFileName)
        'Else
        '    Profile = XDocument.Load("Настройки.xml")
        'End If
        'If LogRegulation.Checked = True Then
        '    CheckSetting.БылКлик = True
        '    Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("Regulation", "Log")
        '    Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, True)
        'End If


        'Profile.Save(ProFileName)

        'CheckSetting.ChangeRegMetod()
        'Form3.Timer1.Start()
        'Form3.Timer2.Start()
        'Form3.Timer3.Start()
    End Sub

    Private Sub LineRegulation_MouseClick(sender As Object, e As MouseEventArgs)
        '  CheckSetting.БылКлик = True
    End Sub

    Private Sub LogRegulation_MouseClick(sender As Object, e As MouseEventArgs)
        '   CheckSetting.БылКлик = True
    End Sub

    Private Sub StartNewTrack_CheckedChanged(sender As Object, e As EventArgs) Handles StartNewTrack.CheckedChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("StartNewTrack", StartNewTrack.Checked)
        Profile.Save(ProfileName)
    End Sub

    Private Sub NewTrackTime_TextChanged(sender As Object, e As EventArgs) Handles NewTrackTime.TextChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("NewTrackTime", NewTrackTime.Text)
        Profile.Save(ProfileName)
    End Sub

    Private Sub Mono_CheckedChanged(sender As Object, e As EventArgs) Handles Mono.CheckedChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        If Mono.Checked = True Then
            Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("PLayingMode", "Mono")
        End If
        If Stereo.Checked = True Then
            Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("PLayingMode", "Stereo")
        End If


        Profile.Save(ProfileName)




    End Sub

    Private Sub Stereo_CheckedChanged(sender As Object, e As EventArgs) Handles Stereo.CheckedChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If

        If Mono.Checked = True Then
            Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("PLayingMode", "Mono")
        End If
        If Stereo.Checked = True Then
            Profile.Document.Element("Setting").Element("GeneralSetting").SetAttributeValue("PLayingMode", "Stereo")
        End If

        Profile.Save(ProfileName)

    End Sub

    Private Sub РедактироватьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles РедактироватьToolStripMenuItem.Click
        Form4.ShowDialog()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        '  If FolderBrowserDialog7.ShowDialog = DialogResult.OK Then

        Dim Чб As New SourceGrid.Cells.CheckBox
            Dim НоваяЯчейка As SourceGrid.Cells.Cell
        '   Dim Время As SourceGrid.Cells.Editors.TextBox = New SourceGrid.Cells.Editors.TextBox(GetType(String), "00:00:00-23:59:59", True)
        Dim song_count As Integer
            Dim row_count As Integer
            Dim Вес As Integer
            Dim r As Integer

            row_count = Grid2.Rows.Count
        'If row_count = 1 Then
        '    Вес = 100
        'Else
        '    Вес = 0
        'End If





        If MusicDirSelect.ShowDialog = DialogResult.OK Then
            Grid2.Rows.Insert(Grid2.RowsCount)

            song_count = get_song_count(MusicDirSelect.SelectedPath)
            НоваяЯчейка = New SourceGrid.Cells.Cell(MusicDirSelect.SelectedPath)
            Grid2(Grid2.RowsCount - 1, 1) = НоваяЯчейка


            НоваяЯчейка = New SourceGrid.Cells.Cell(song_count)
            Grid2(Grid2.RowsCount - 1, 2) = НоваяЯчейка






            НоваяЯчейка = New SourceGrid.Cells.Cell("")
            Grid2(Grid2.RowsCount - 1, 3) = НоваяЯчейка

            'Вес = 100 / (Grid1.RowsCount - 1)
            'For r = 1 To Grid1.RowsCount - 1
            '    Grid1(r, 3).Value = Вес
            'Next



            НоваяЯчейка = New SourceGrid.Cells.Cell("00:00:00-23:59:59")
            Grid2(Grid2.RowsCount - 1, 4) = НоваяЯчейка
            '  Grid1(Grid1.RowsCount - 1, 4).Editor = ed




            Grid2(Grid2.RowsCount - 1, 0) = Чб

            Dim ed As New SourceGrid.Cells.Editors.TextBox(GetType(String))
            Grid2(Grid2.RowsCount - 1, 3).Editor = ed
            Grid2(Grid2.RowsCount - 1, 4).Editor = ed


            Чб.Checked = True
            Grid2.Update()
            Grid2.Refresh()


        End If
        ' End If
    End Sub


    Public Sub vcbcvbcb()
        MsgBox("sdfgfgdfg")
    End Sub
    Public Sub gdsfgfd(sender As Object, e As EventArgs)

    End Sub

    Private Sub Grid2_Paint(sender As Object, e As PaintEventArgs) Handles Grid2.Paint

    End Sub

    Private Sub Grid2_MouseClick(sender As Object, e As MouseEventArgs) Handles Grid2.MouseClick
        Dim r, c As Integer
        Dim n As Integer

        c = Grid2.Selection.ActivePosition.Column
        r = Grid2.Selection.ActivePosition.Column
        If r = 0 Then

            If c = 0 Then
                For n = 2 To Grid2.Rows.Count - 1
                    If Grid2.Rows(n).Visible = True Then
                        Grid2.Rows(n).Visible = False
                    Else
                        Grid2.Rows(n).Visible = True
                    End If

                Next
            End If
        End If



    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Dim r As Integer

        r = Grid2.Selection.ActivePosition.Row

        If r = -1 Then
            MsgBox("Нужно выделить директорию, а потом удалять", MsgBoxStyle.Critical)
            Exit Sub
        End If


        Grid2.Rows.Remove(r)
    End Sub

    Private Sub GenMusList_CheckedChanged(sender As Object, e As EventArgs) Handles GenMusList.CheckedChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("music").SetAttributeValue("GenNewMusList", GenMusList.Checked)
        Profile.Save(ProfileName)
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Dim ProfileName As String
        Dim Profile As XDocument

        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")

        ProfileName = Настройки.Document.Element("Setting").Element("GeneralSetting").Attribute("CurentProfile").Value
        If System.IO.File.Exists(ProfileName) = True Then
            Profile = XDocument.Load(ProfileName)
        Else
            Profile = XDocument.Load("Настройки.xml")
        End If


        Profile.Document.Element("Setting").Element("music").SetAttributeValue("TimeGenNewMusPL", DateTimePicker1.Value.ToString("HH:mm:ss"))
        Profile.Save(ProfileName)
    End Sub
End Class