Module PreloadADVPlaylist
    Public Function CheckPlaylist()
        Dim CureDate As String
        Dim adv_patch As String = SCHSetting.dir
        Dim xe As XElement
        Dim PLTimeSEC As Integer
        Dim NEXTPLTimeSEC As Integer
        Dim diff As Integer
        Dim m As Integer
        Dim n As Integer
        Dim ADV As String
        Dim TMP_PL As New ArrayList
        Dim PLTime As String
        Dim NextPLTime As String


        Dim r As Integer
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        Dim Плейлист As Array
        Dim adv_dir As String = ADVSetting.dir
        Dim MInTimeDist As Integer = ADVSetting.MinTimeDist
        Dim ВременныйПлейлист As XDocument

        ВременныйПлейлист = <?xml version="1.0"?>
                            <плейлист>
                            </плейлист>


        CureDate = Format(Now, "yyyy") & Format(Now, "MM") & Format(Now, "dd")
        '  System.IO.File.AppendAllText("fff.txt", Now & "." & Now.Millisecond & "  ", System.Text.Encoding.Default)


        If System.IO.File.Exists(adv_patch & "\" & CureDate & ".txt") = True Then
            Плейлист = System.IO.File.ReadAllLines(adv_patch & "\" & CureDate & ".txt", System.Text.Encoding.Default)

            For r = 1 To Плейлист.Length - 1
                If Плейлист(r) <> "" Then
                    TMP_PL.Add(Плейлист(r))
                End If
            Next

            For r = 0 To TMP_PL.Count - 1
                Try
                    ' If Плейлист(r) <> "" Then
                    If TMP_PL(r).ToString.Split("=").Length = 3 Then
                        PLTime = TMP_PL(r).ToString.Split("=")(1)
                        ADV = adv_dir & "\" & TMP_PL(r).ToString.Split("=")(2) & ".wav"
                        If (r + 1) <= TMP_PL.Count - 1 Then
                            NextPLTime = TMP_PL(r + 1).ToString.Split("=")(1)
                        End If
                    End If
                    If Плейлист(r).ToString.Split("=").Length = 2 Then
                        ADV = adv_dir & "\" & TMP_PL(r).ToString.Split("=")(1) & ".wav"
                        PLTime = TMP_PL(r).ToString.Split("=")(0)
                        If (r + 1) <= TMP_PL.Count - 1 Then
                            NextPLTime = TMP_PL(r + 1).ToString.Split("=")(0)
                        End If
                    End If
                    PLTimeSEC = Mid(PLTime, 1, 2) * 3600 + Mid(PLTime, 3, 2) * 60 + Mid(PLTime, 5, 2)
                    NEXTPLTimeSEC = Mid(NextPLTime, 1, 2) * 3600 + Mid(NextPLTime, 3, 2) * 60 + Mid(NextPLTime, 5, 2)
                    diff = NEXTPLTimeSEC - PLTimeSEC
                    m = ВременныйПлейлист.Document.Element("плейлист").Elements.Count

                    If ВременныйПлейлист.Document.Element("плейлист").Element("block_" & m) Is Nothing Then
                        xe = New XElement("block_" & m, PLTime)
                        ВременныйПлейлист.Document.Element("плейлист").Add(xe)
                    End If
                    n = ВременныйПлейлист.Document.Element("плейлист").Element("block_" & m).Attributes.Count

                    ВременныйПлейлист.Document.Element("плейлист").Element("block_" & m).SetAttributeValue("val_" & n, ADV)

                    If r <= TMP_PL.Count - 1 Then


                        While diff <= MInTimeDist

                            r += 1

                            If r <= TMP_PL.Count - 1 Then
                                If TMP_PL(r).ToString.Split("=").Length = 3 Then
                                    PLTime = TMP_PL(r).ToString.Split("=")(1)
                                    ADV = adv_dir & "\" & TMP_PL(r).ToString.Split("=")(2) & ".wav"
                                    If (r + 1) < TMP_PL.Count Then
                                        NextPLTime = TMP_PL(r + 1).ToString.Split("=")(1)
                                    End If
                                End If
                                If TMP_PL(r).ToString.Split("=").Length = 2 Then
                                    ADV = adv_dir & "\" & TMP_PL(r).ToString.Split("=")(1) & ".wav"
                                    PLTime = TMP_PL(r).ToString.Split("=")(0)
                                    If (r + 1) < TMP_PL.Count Then
                                        NextPLTime = TMP_PL(r + 1).ToString.Split("=")(0)
                                    End If
                                End If
                                PLTimeSEC = Mid(PLTime, 1, 2) * 3600 + Mid(PLTime, 3, 2) * 60 + Mid(PLTime, 5, 2)
                                NEXTPLTimeSEC = Mid(NextPLTime, 1, 2) * 3600 + Mid(NextPLTime, 3, 2) * 60 + Mid(NextPLTime, 5, 2)
                                diff = NEXTPLTimeSEC - PLTimeSEC

                                If ВременныйПлейлист.Document.Element("плейлист").Element("block_" & m) Is Nothing Then
                                    xe = New XElement("block_" & m, PLTime)
                                    Form1.ВременныйПлейлист.Document.Element("плейлист").Add(xe)
                                End If
                                n = ВременныйПлейлист.Document.Element("плейлист").Element("block_" & m).Attributes.Count

                                ВременныйПлейлист.Document.Element("плейлист").Element("block_" & m).SetAttributeValue("val_" & n, ADV)

                            Else
                                Exit While
                            End If


                        End While
                    End If

                    'End If
                Catch ex As Exception
                    '  MsgBox(r)
                End Try

            Next

        End If


        r = 0
        Return ВременныйПлейлист
    End Function

End Module
