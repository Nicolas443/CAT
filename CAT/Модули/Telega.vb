Imports Telegram.Bot
Imports Telegram.Bot.Exceptions
Imports Telegram.Bot.Polling
Imports Telegram.Bot.Args
Imports Telegram.Bot.Extensions.Polling
Imports Telegram.Bot.Types
Imports Telegram.Bot.Types.Enums
Imports Telegram.Bot.Types.ReplyMarkups
Imports System.Net
Imports Un4seen.Bass
Module Telega
    Public token As String = "6524850683:AAEl9z9MotpJZqMGV_HsUwaPQtZDUhraBS4"
    Public chatid As String '= "5058332632"
    Public Настройки As System.Xml.Linq.XDocument

    Public botclient As TelegramBotClient
    Public Sub Bot()
        Dim логи As String
        Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
        логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value
        Try

            token = Настройки.Document.Element("Setting").Element("Telegram").Attribute("Token").Value
            botclient = New TelegramBotClient(token)
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  Error_sending_mess_to_TG " & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  Error_sending_mess_to_TG " & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try


    End Sub
    Public Async Sub Send_text(ByVal msg As String)
        Dim логи As String
        Try

            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            chatid = Настройки.Document.Element("Setting").Element("Telegram").Attribute("ChatID").Value
            логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value
            Await botclient.SendTextMessageAsync(chatid, msg)

        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  Error_sending_mess_to_TG " & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  Error_sending_mess_to_TG " & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try


    End Sub

    Public Async Sub Get_text()
        Dim s, d
        Dim логи As String
        ServicePointManager.Expect100Continue = True
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Try

            Настройки = System.Xml.Linq.XDocument.Load("Настройки.xml")
            логи = Настройки.Document.Element("Setting").Element("logs").Attribute("dir").Value
            While True
                s = botclient.GetUpdatesAsync.Result
                For Each ss In s

                    If ss.Message.text = "Стоп" Then
                        '  FMediaControl.Stop()

                        d = botclient.GetUpdatesAsync(ss.Id + 1)

                        ' Form1.Стоп()
                        '  Dim fgfg As Form1.VolumeTimerDelegate
                        '  fgfg = New Form1.VolumeTimerDelegate(AddressOf Form1.VolumeTimerUpdate)
                        ' fgfg.Invoke()

                        'Form1.Invoke(New Form1.VolumeTimerDelegate(AddressOf Form1.VolumeTimerUpdate), 1)
                        '  Form1.Invoke(New Form1.LabelUpdateDelegate(AddressOf Form1.LabelUpdate), Now & "  " & "sdfdsf")
                        '  Form1.Invoke(New Form1.VolumeTimerDelegate(AddressOf Form1.VolumeTimerUpdate), 0)

                        'Form1.VolumeCheck.Stop()
                        'Bass.BASS_SampleFree(Play.stream_music)
                        'Bass.BASS_ChannelStop(Play.stream_music)

                        'Bass.BASS_SampleFree(Play.stream2)
                        'Bass.BASS_ChannelStop(Play.stream2)

                        'Bass.BASS_Free()
                        'Form1.VolumeCheck.Stop()
                    End If
                    If ss.Message.text = "Старт" Then
                        '  Form1.TG.Stop()
                        'd = botclient.GetUpdatesAsync(ss.Id + 1)
                        'Play.Play("Music")
                        'Form1.VolumeCheck.Start()
                    End If


                Next
            End While

        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  Error_sending_mess_to_TG " & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  Error_sending_mess_to_TG " & ex.Message.ToString & " file: " & play_file_name & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try


    End Sub


End Module
