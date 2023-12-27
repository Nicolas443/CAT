Imports System.IO

Module Song
    Public Function GetNextSong()
        Dim Profile As XDocument
        Dim time_dir_start, time_dir_stop As Date
        Dim r As Integer
        Dim NextSong As String
        Dim ЕстьПесня As Boolean
        Dim v As Integer

        ' Try
        Try
            Profile = GenerlaSetting.ProfileLoad
            While Form1.MusicList.Items.Count <> 0
                NextSong = Form1.MusicList.Items(0)
                For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Elements
                    If NextSong.Contains(elem.Attribute("dir_patch").Value.ToString) = True Then
                        For r = 0 To elem.Attribute("Time").Value.ToString.Split(";").Count - 1
                            time_dir_start = Now.ToString("yyyy.MM.dd") & " " & elem.Attribute("Time").Value.ToString.Split(";")(r).Split("-")(0)
                            time_dir_stop = Now.ToString("yyyy.MM.dd") & " " & elem.Attribute("Time").Value.ToString.Split(";")(r).Split("-")(1)
                            If Now >= time_dir_start And Now <= time_dir_stop Then
                                Form1.MusicList.Items.RemoveAt(0)
                                Return NextSong
                            End If

                        Next
                    End If
                Next

                Form1.MusicList.Items.RemoveAt(0)
                v += 1
            End While
        Catch ex As Exception
            If Form1.MusicList.Items.Count > 0 Then
                Return Form1.MusicList.Items(0)
            End If


        End Try



        '    While Form1.MusicList.Items.Count <> 0

        '        Dim llkl As String = Form1.MusicList.Items(0)
        '        For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements
        '            If Form1.MusicList.Items.Count <> 0 Then
        '                If Form1.MusicList.Items(0).Contains(elem.Attribute("dir_patch").Value.ToString) = True Then
        '                    For r = 0 To elem.Attribute("Time").Value.ToString.Split(";").Count - 1
        '                        time_dir_start = Now.ToString("yyyy.MM.dd") & " " & elem.Attribute("Time").Value.ToString.Split(";")(r).Split("-")(0)
        '                        time_dir_stop = Now.ToString("yyyy.MM.dd") & " " & elem.Attribute("Time").Value.ToString.Split(";")(r).Split("-")(1)
        '                        If Now >= time_dir_start And Now <= time_dir_stop Then
        '                            play_file_name = Form1.MusicList.Items(0)

        '                            Form1.MusicList.Items.RemoveAt(0)
        '                            Return play_file_name
        '                            Exit While
        '                        Else
        '                            Form1.MusicList.Items.RemoveAt(0)
        '                        End If
        '                    Next

        '                End If
        '            End If

        '        Next


        '    End While
        'Catch ex As Exception
        '    play_file_name = Form1.MusicList.Items(0)
        '    Form1.MusicList.Items.RemoveAt(0)
        '    Return play_file_name
        'End Try




    End Function

    Public Function CheckDynDir()
        Dim Profile As XDocument
        Dim time_start As Date
        Dim time_stop As Date
        Dim ВсеДир As New ArrayList
        Profile = GenerlaSetting.ProfileLoad

        For Each elem In Profile.Document.Element("Setting").Element("music").Element("dyn_dir_2").Elements

            time_start = Now.Date.ToString("yyyy.MM.dd") & " " & elem.Attribute("Time").Value.ToString.Split("-")(0)
            time_stop = Now.Date.ToString("yyyy.MM.dd") & " " & elem.Attribute("Time").Value.ToString.Split("-")(1)

            If Now >= time_start And Now <= time_stop Then
                ' GenDynSong()
                ' Form1.DynDir.Stop()
                ВсеДир.Add(elem.Attribute("dir_patch").Value)
                ' Form1.DynDir.Stop()


            End If

        Next

        Return GenDynSong(ВсеДир)

    End Function



    Public Function GenDynSong(ByVal dir As ArrayList)
        Dim ПромежуточныйПлейлист As XDocument
        Dim Profile As XDocument
        Dim ПесниВТекущейПапке As New ArrayList
        Dim ВсеПесни As New ArrayList
        Dim xe As XElement
        Dim i As Integer
        Dim y As Integer
        Dim t As Integer

        Profile = GenerlaSetting.ProfileLoad
        Try
            ' Логи = Profile.Document.Element("Setting").Element("logs").Attribute("dir").Value
            '  Домен = Profile.Document.Element("Setting").Element("Domen").Attribute("Name").Value & Настройки.Document.Element("Setting").Element("Domen").Attribute("SubDomen").Value

            '  ВсеПесни = New ArrayList

            ПромежуточныйПлейлист = New XDocument
            ПромежуточныйПлейлист = <?xml version="1.0"?>
                                    <плейлист>
                                    </плейлист>

            If dir.Count <> 0 Then
                For y = 0 To dir.Count - 1
                    ' For Each songs In Profile.Document.Element("Setting").Element("music").Element("dyn_dir").Elements
                    ПесниВТекущейПапке.Clear()
                    If System.IO.Directory.Exists(dir(y)) = True Then
                        ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dir(y), "*.mp3", SearchOption.AllDirectories))
                        ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dir(y), "*.wav", SearchOption.AllDirectories))
                        ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dir(y), "*.wma", SearchOption.AllDirectories))
                        ПесниВТекущейПапке.AddRange(System.IO.Directory.GetFiles(dir(y), "*.aac", SearchOption.AllDirectories))
                        ВсеПесни.Insert(ВсеПесни.Count, ПесниВТекущейПапке)

                        'If ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i) Is Nothing Then
                        '    xe = New XElement("dir" & i, "")
                        '    ПромежуточныйПлейлист.Document.Element("плейлист").Add(xe)
                        'End If
                        'ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).SetAttributeValue("Вес", "100")


                        For j = 0 To ПесниВТекущейПапке.Count - 1
                            t = ПромежуточныйПлейлист.Document.Element("плейлист").Elements.Count
                            ''If ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Element("song" & j) Is Nothing Then
                            xe = New XElement("song" & t, "")
                            ПромежуточныйПлейлист.Document.Element("плейлист").Add(xe)
                            '  'End If
                            ' ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Element("song" & j).SetAttributeValue("songpatch", ПесниВТекущейПапке(j))
                            ' ПромежуточныйПлейлист.Document.Element("плейлист").Element("dir" & i).Element("song" & j).SetAttributeValue("Use", "0")
                            '  ВсегоПесен += 1
                            ПромежуточныйПлейлист.Document.Element("плейлист").Element("song" & t).SetAttributeValue("songpatch", ПесниВТекущейПапке(j))
                        Next

                        i += 1
                    End If

                Next
            End If


            ' ВсеПесни.Insert(ВсеПесни.Count, ПесниВТекущейПапке)

            Dim rnd As Random

            rnd = New Random
            Dim h As Integer = rnd.Next(0, ПромежуточныйПлейлист.Document.Element("плейлист").Elements.Count - 1)
            Dim l As Integer


            'For l = 0 To ВсеПесни(0)
            '    If l = h Then
            '        l = 0
            '        Return ВсеПесни(l)
            '    End If


            'Next

            For Each elem In ПромежуточныйПлейлист.Document.Element("плейлист").Elements
                If l = h Then
                    Return elem.Attribute("songpatch").Value
                    l = 0
                End If

                l += 1
            Next

            'For l = 0 To ПромежуточныйПлейлист.Document.Element("плейлист").Elements.Count - 1
            '    If l = h Then

            '    End If


            'Next





        Catch ex As Exception
            'If System.IO.Directory.Exists(Логи) = True Then
            '    System.IO.File.AppendAllText(Логи & "\" & Домен & "_" & Now.Date & ".err", Now & " Error_get_song_list " & ex.Message & vbNewLine, System.Text.Encoding.Default)
            'End If
        End Try

        Return ""
    End Function



    Public Function GetDinSongName(ByVal Удалить As Boolean)
        Dim r As Integer
        Dim t_start As Date
        Dim t_stop As Date
        Dim ЕстьПесня As Boolean
        Dim Песня As String = ""
        Dim n As Integer


        For r = 0 To Form1.ListBox1.Items.Count - 1
            t_start = Now.Date.ToString("yyyy.MM.dd") & " " & Form1.ListBox1.Items(r).ToString.Split("|")(1).Split("-")(0)
            t_stop = Now.Date.ToString("yyyy.MM.dd") & " " & Form1.ListBox1.Items(r).ToString.Split("|")(1).Split("-")(1)

            If Now >= t_start And Now <= t_stop Then
                '  Form1.ListView1.Items(r)
                ЕстьПесня = True
                Песня = Form1.ListBox1.Items(r).ToString.Split("|")(0)
                n = r
                Exit For
            End If

        Next

        If ЕстьПесня = True Then
            If Удалить = True Then
                Form1.ListBox1.Items.RemoveAt(n)
            End If
            Return Песня
        Else
            Return Nothing
        End If

        ЕстьПесня = False
    End Function

    Public Function GetStaticdirSongName()
        Dim Песня As String
        If Form1.MusicList.Items.Count >= 1 Then
            If MusicSetting.MusicPause = False Then
                Песня = Form1.MusicList.Items(0)
                Form1.MusicList.Items.RemoveAt(0)
                Return Form1.MusicList.Items(0)

            End If
        End If


        Return ""
    End Function
End Module
