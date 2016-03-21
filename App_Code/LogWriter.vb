Imports System
Imports System.IO
Imports System.Web
Imports Microsoft.VisualBasic



Public Class LogWriter

    Private maxLines As Integer = 100000
    Private linesToDump As Integer = 99500
    Private logFileName As String = "~\Logs\EDDM-Log.txt"
    Private fullPath As String = HttpContext.Current.Server.MapPath(logFileName)
    Private useShrinkFile As Boolean = True


    Public Sub RecordInLog(logEntry As String)


        ' This text is added only once to the file.  
        If Not File.Exists(fullPath) Then

            ' Create a file to write to. 
            Using myStreamWriter As StreamWriter = File.CreateText(fullPath)
                myStreamWriter.WriteLine("EDDM LogWriter")
                myStreamWriter.WriteLine("*****************************************************************")
            End Using

        End If

        ' This text is always added, making the file longer over time......
        Using myStreamWriter As StreamWriter = File.AppendText(fullPath)
            myStreamWriter.WriteLine("Entry at: " & DateTime.Now)
            myStreamWriter.WriteLine(logEntry)
            myStreamWriter.WriteLine("****")
        End Using



        'Old code from C# model that should be implemented
        'Dim logFileLines As Integer = 0
        'Dim logWrite As New StreamWriter(fullPath)


        'Using logWrite As StreamWriter = New StreamWriter(fullPath)
        '    logWrite.WriteLine(logEntry)
        'End Using



        'Need to fully qualify path
        'If Not (File.Exists(fullPath)) Then
        'CreateFile(fullPath)
        'End If

        'logFileLines = CountLogLines(fullPath)

        'If (useShrinkFile) Then
        '    If (logFileLines > maxLines) Then
        '        ShrinkFile(fullPath)
        '    End If
        'End If

        'logWrite = File.AppendText(fullPath)
        'logWrite.WriteLine(logEntry)
        'logWrite.Close()
        'logWrite.Dispose()


        'Catch objException As Exception

        '    logWrite.WriteLine("***" & DateTime.Now.ToLongDateString() & " " & DateTime.Now.ToLongTimeString() & "***")
        '    logWrite.WriteLine("The following errors occurred:")
        '    logWrite.WriteLine("Message: " & objException.Message)
        '    logWrite.WriteLine("Source: " & objException.Source)
        '    logWrite.WriteLine("Stack Trace: " & objException.StackTrace)
        '    logWrite.WriteLine("Target Site: " & objException.TargetSite.Name)

        'Finally

        '    logWrite.Close()
        '    logWrite.Dispose()

        'End Try

    End Sub



    Private Sub CreateFile(logFileName As String)

        Dim createFile As StreamWriter = Nothing
        createFile = File.CreateText(logFileName)

        createFile.WriteLine("LOG CREATED ON: " & DateTime.Now.ToLongDateString() & " " + DateTime.Now.ToLongTimeString())
        createFile.WriteLine("==========================================================================")
        createFile.Close()

    End Sub



    Private Function CountLogLines(logFileName As String) As Integer

        'Needs to a Using
        Dim numberOfLines As Integer = 0
        Dim logReader As StreamReader = New StreamReader(logFileName)

        While (logReader.ReadLine())
            numberOfLines += 1
        End While

        logReader.Close()
        logReader.Dispose()

        Return numberOfLines

    End Function



    Private Sub ShrinkFile(logFileName As String)

        Dim logLines As StreamReader = New StreamReader(logFileName)

        Dim firstLine As String = logLines.ReadLine()
        Dim secondLine As String = logLines.ReadLine()

        For looper As Integer = 0 To linesToDump
            logLines.ReadLine()
        Next

        Dim remainingContents As String = logLines.ReadToEnd()

        File.WriteAllText(logFileName, firstLine + Environment.NewLine & secondLine + Environment.NewLine & remainingContents)

        logLines.Close()

    End Sub



End Class
