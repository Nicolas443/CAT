Module Save

    Public Sub SaveMainMusicList()
        Dim smml As SaveFileDialog = New SaveFileDialog
        Dim r As Integer
        smml.Filter = "Текстовые файлы (*.txt)|*.txt"
        If smml.ShowDialog = DialogResult.OK Then
            If Form1.MusicList.Items.Count >= 1 Then
                System.IO.File.WriteAllText(smml.FileName, Form1.MusicList.Items(0) & vbNewLine, System.Text.Encoding.Default)
                For r = 1 To Form1.MusicList.Items.Count - 1
                    System.IO.File.AppendAllText(smml.FileName, Form1.MusicList.Items(r) & vbNewLine, System.Text.Encoding.Default)
                Next
            End If



        End If
    End Sub


    Public Sub OpenMainMusicList()
        Dim omml As OpenFileDialog = New OpenFileDialog
        Dim r As Integer
        Dim МузПлейлист As Array
        omml.Filter = "Текстовые файлы (*.txt)|*.txt"

        If omml.ShowDialog = DialogResult.OK Then
            Form1.MusicList.Items.Clear()
            МузПлейлист = System.IO.File.ReadAllLines(omml.FileName, System.Text.Encoding.Default)
            For r = 0 To МузПлейлист.Length - 1
                Form1.MusicList.Items.Add(МузПлейлист(r))
            Next


        End If


    End Sub


    Public Sub SaveDynList()
        Dim smml As SaveFileDialog = New SaveFileDialog
        Dim r As Integer
        smml.Filter = "Текстовые файлы (*.txt)|*.txt"
        If smml.ShowDialog = DialogResult.OK Then
            If Form1.ListBox1.Items.Count >= 1 Then
                System.IO.File.WriteAllText(smml.FileName, Form1.ListBox1.Items(0) & vbNewLine, System.Text.Encoding.Default)
                For r = 1 To Form1.ListBox1.Items.Count - 1
                    System.IO.File.AppendAllText(smml.FileName, Form1.ListBox1.Items(r) & vbNewLine, System.Text.Encoding.Default)
                Next
            End If



        End If
    End Sub

    Public Sub OpenDynList()
        Dim omml As OpenFileDialog = New OpenFileDialog
        Dim r As Integer
        Dim МузПлейлист As Array
        omml.Filter = "Текстовые файлы (*.txt)|*.txt"

        If omml.ShowDialog = DialogResult.OK Then
            Form1.ListBox1.Items.Clear()
            МузПлейлист = System.IO.File.ReadAllLines(omml.FileName, System.Text.Encoding.Default)
            For r = 0 To МузПлейлист.Length - 1
                Form1.ListBox1.Items.Add(МузПлейлист(r))
            Next


        End If
    End Sub


End Module
