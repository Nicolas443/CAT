Imports System.Data.OleDb
Imports System.Runtime.InteropServices

Module DataBase
    Public Sub CreateDB(ByVal dbpatch As String)
        Dim cat As New ADOX.Catalog()

        cat.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch)



        'create the empty table in the DB file
        Dim conn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
        conn.Open()
        Dim cmd As New OleDb.OleDbCommand("", conn)
        cmd.CommandText = "CREATE TABLE Log (ID int identity, Домен CHAR, StartDate Date, StopDate Date, Событие CHAR, Название CHAR);"
        cmd.ExecuteNonQuery()
        cmd.Dispose()


        conn.Close()
        conn.Dispose()

        Marshal.ReleaseComObject(cat)
        cat = Nothing
    End Sub

    Public Sub UpdateDB(ByVal dbpatch As String, ByVal ID As Integer)
        Dim conn As OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim adapter As OleDbDataAdapter
        Dim dt As New DataTable

        If System.IO.File.Exists(dbpatch) = True Then
            conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
            conn.Open()
            cmd = New OleDb.OleDbCommand("", conn)
        End If

        cmd.CommandText = "UPDATE Log SET StopDate=@StopDate WHERE ID=@ID"

        cmd.Parameters.Add("@StopDate", Now.ToString)
        cmd.Parameters.Add("@ID", ID)


        cmd.ExecuteNonQuery()

        conn.Close()
    End Sub

    Public Sub AddNewRecord(ByVal dbpatch As String, ByVal Домен As String, ByVal логи As String, ByVal play_file_name As String, ByVal Событие As String, ByVal StartDate As Date, ByVal StopDate As Date)
        Try
            Dim conn As OleDbConnection
            Dim cmd As OleDb.OleDbCommand
            Dim adapter As OleDbDataAdapter
            Dim dt As New DataTable

            If System.IO.File.Exists(dbpatch) = True Then
                conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
                conn.Open()
                cmd = New OleDb.OleDbCommand("", conn)
            End If

            If System.IO.File.Exists(dbpatch) = True Then
                ' cmd.CommandText = "INSERT INTO Log (Домен, Дата, Событие, Название) values (" & "18d" & "," & "" & Now & "," & "play_music" & "," & System.IO.Path.GetFileName(play_file_name) & ");"
                '  cmd.CommandText = "INSERT INTO Log (Домен, Дата, Время, Событие, Название, Dir) values (" & "'" & Домен & "'" & "," & "'" & Now.Date & "'" & "," & "'" & Now.Hour.ToString & ":" & Now.Minute.ToString & ":" & Now.Second.ToString & "'" & "," & "'" & Событие & "'" & "," & "'" & System.IO.Path.GetFileName(play_file_name).Replace("'", "''") & "'" & "," & "'" & System.IO.Path.GetDirectoryName(play_file_name) & "'" & ");"
                cmd.CommandText = "INSERT INTO Log (Домен, StartDate, StopDate, Событие, Название) values (" & "'" & Домен & "'" & "," & "'" & StartDate & "'" & "," & "'" & StopDate & "'" & "," & "'" & Событие & "'" & "," & "'" & System.IO.Path.GetFileName(play_file_name).Replace("'", "''").Trim & "'" & ");"
                cmd.ExecuteNonQuery()
                conn.Close()


                conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
                conn.Open()
                cmd = New OleDb.OleDbCommand("", conn)

                cmd.CommandText = "SELECT * FROM Log"
                adapter = New OleDbDataAdapter(cmd)
                adapter.Fill(dt)

                Play.LastId = dt(dt.Rows.Count - 1)(0)
                cmd.ExecuteNonQuery()
                conn.Close()

            End If
        Catch ex As Exception
            If System.IO.Directory.Exists(логи) = True Then
                System.IO.File.AppendAllText(логи & "\" & Now.Date & ".err", Now & "  error write database  " & ex.Message.ToString & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  error write database  " & ex.Message.ToString & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try
    End Sub


    Public Function GetLastADV(ByVal dbpatch As String, ByVal H As Integer) As DataTable
        Dim conn As OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim adapter As OleDbDataAdapter
        Dim dt As New DataTable
        Dim connect As OleDbConnection
        If System.IO.File.Exists(dbpatch) = True Then
            conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
            conn.Open()
            cmd = New OleDb.OleDbCommand("", conn)
        End If


        connect = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & dbpatch & ";Persist Security Info=True")
        connect.Open()
        cmd = New OleDb.OleDbCommand("", connect)
        cmd.CommandText = "SELECT * FROM Log WHERE StartDate between @Dstart and @Dend and Событие='ADV'"

        cmd.Parameters.AddWithValue("@Dstart", DateAdd(DateInterval.Minute, H, Now).ToString)
        cmd.Parameters.AddWithValue("@Dend", Now.ToString)

        adapter = New OleDbDataAdapter(cmd)
        adapter.Fill(dt)
        cmd.ExecuteNonQuery()
        connect.Close()

        Return dt

    End Function


    Public Sub ccc(ByVal s As String)

        MsgBox(s)

    End Sub

    Public Sub AddNewRecTH(ByVal Param As ArrayList)
        Try
            Dim conn As OleDbConnection
            Dim cmd As OleDb.OleDbCommand
            Dim adapter As OleDbDataAdapter
            Dim dt As New DataTable

            If System.IO.File.Exists(Param(0)) = True Then
                conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Param(0) & ";Persist Security Info=True")
                conn.Open()
                cmd = New OleDb.OleDbCommand("", conn)
            End If

            If System.IO.File.Exists(Param(0)) = True Then
                ' cmd.CommandText = "INSERT INTO Log (Домен, Дата, Событие, Название) values (" & "18d" & "," & "" & Now & "," & "play_music" & "," & System.IO.Path.GetFileName(play_file_name) & ");"
                '  cmd.CommandText = "INSERT INTO Log (Домен, Дата, Время, Событие, Название, Dir) values (" & "'" & Домен & "'" & "," & "'" & Now.Date & "'" & "," & "'" & Now.Hour.ToString & ":" & Now.Minute.ToString & ":" & Now.Second.ToString & "'" & "," & "'" & Событие & "'" & "," & "'" & System.IO.Path.GetFileName(play_file_name).Replace("'", "''") & "'" & "," & "'" & System.IO.Path.GetDirectoryName(play_file_name) & "'" & ");"
                cmd.CommandText = "INSERT INTO Log (Домен, StartDate, StopDate, Событие, Название) values (" & "'" & Param(1) & "'" & "," & "'" & Param(5) & "'" & "," & "'" & Param(6) & "'" & "," & "'" & Param(4) & "'" & "," & "'" & System.IO.Path.GetFileName(play_file_name).Replace("'", "''").Trim & "'" & ");"
                cmd.ExecuteNonQuery()
                conn.Close()


                conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Param(0) & ";Persist Security Info=True")
                conn.Open()
                cmd = New OleDb.OleDbCommand("", conn)

                cmd.CommandText = "SELECT * FROM Log"
                adapter = New OleDbDataAdapter(cmd)
                adapter.Fill(dt)

                Play.LastId = dt(dt.Rows.Count - 1)(0)
                cmd.ExecuteNonQuery()
                conn.Close()

            End If
        Catch ex As Exception
            If System.IO.Directory.Exists(Param(2)) = True Then
                System.IO.File.AppendAllText(Param(2) & "\" & Now.Date & ".err", Now & "  error write database  " & ex.Message.ToString & vbNewLine, System.Text.Encoding.Default)
            Else
                System.IO.File.AppendAllText(Now.Date & ".err", Now & "  error write database  " & ex.Message.ToString & vbNewLine, System.Text.Encoding.Default)
            End If
        End Try
    End Sub

    Public Sub UpdateRecTH(ByVal Param As ArrayList)
        Dim conn As OleDbConnection
        Dim cmd As OleDb.OleDbCommand
        Dim adapter As OleDbDataAdapter
        Dim dt As New DataTable

        If System.IO.File.Exists(Param(0)) = True Then
            conn = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Param(0) & ";Persist Security Info=True")
            conn.Open()
            cmd = New OleDb.OleDbCommand("", conn)
        End If

        cmd.CommandText = "UPDATE Log SET StopDate=@StopDate WHERE ID=@ID"

        cmd.Parameters.Add("@StopDate", Now.ToString)
        cmd.Parameters.Add("@ID", Param(1))


        cmd.ExecuteNonQuery()

        conn.Close()
    End Sub
End Module
