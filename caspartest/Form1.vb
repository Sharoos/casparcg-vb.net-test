Imports Svt.Caspar
Imports System.Runtime.InteropServices

Public Class Form1

    Private parentedProcess As Process

    Public Sub New()
        InitializeComponent()
    End Sub


    Dim aa As New CasparDevice
    Private g_int_ChannelNumber As Integer = 1
    Private g_int_ChannelNumber1 As Integer = 2
    Private g_int_PlaylistLayer As Integer = 1
    Private mediaDuration As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openFileDialog1 As New OpenFileDialog()
        openFileDialog1.Title = "Locate casparcg.exe"
        openFileDialog1.Filter = "Executable Files (*.exe)|*.exe|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim casparCGPath As String = openFileDialog1.FileName
            Dim casparCGDirectory As String = IO.Path.GetDirectoryName(casparCGPath)

            ' Set the working directory to the directory containing CasparCG.exe
            Dim processStartInfo As New ProcessStartInfo(casparCGPath)
            processStartInfo.WorkingDirectory = casparCGDirectory

            Try
                Process.Start(processStartInfo) ' Open CasparCG.exe using Process.Start
            Catch ex As Exception
                MessageBox.Show("Error starting CasparCG.exe: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            ' Introduce a delay to allow CasparCG to fully start
            System.Threading.Thread.Sleep(10000) ' Adjust the delay as needed

            ' Connect to the server
            aa.Connect("127.0.0.1", 5250)

            Try
                ' Locate and manipulate windows
                Dim p1() As Process = Process.GetProcessesByName("casparcg")
                Dim iprocess As Integer
                For iprocess = 0 To p1.GetUpperBound(0)
                    If p1(iprocess).MainWindowTitle = "Screen consumer [2|PAL]" Then
                        Exit For
                    End If
                Next iprocess

                Dim parentedProcess As Process = p1(iprocess)
                SetParent(parentedProcess.MainWindowHandle, Panel1.Handle)
                SendMessage(parentedProcess.MainWindowHandle, 274, 61488, 0)

                ' Embed the "Console Window Host" into Panel2
                Dim consoleProcesses() As Process = Process.GetProcessesByName("CasparCG Server 2.4.0 1e25c7a Stable x64 ")
                If consoleProcesses.Length > 0 Then
                    SetParent(consoleProcesses(0).MainWindowHandle, Panel2.Handle)
                    SendMessage(consoleProcesses(0).MainWindowHandle, 274, 61488, 0)
                End If
                MoveWindowToSecondMonitor("casparcg", "Screen consumer [1|1080p5000]")
                Cursor.Position = New Point(Screen.PrimaryScreen.Bounds.Width \ 2, Screen.PrimaryScreen.Bounds.Height \ 2)
            Catch ex As Exception
                MessageBox.Show("Error manipulating windows: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        o.Title = "Select media to play"
        If o.ShowDialog = DialogResult.OK Then
            TextBox1.Text = Replace(Replace(o.FileName, ":", ":\"), "\", "/")
        End If
        aa.SendString($"Play {g_int_ChannelNumber}-{g_int_PlaylistLayer} ""{TextBox1.Text}""")
        aa.SendString($"Play {g_int_ChannelNumber1}-{g_int_PlaylistLayer} ""{TextBox1.Text}""")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        aa.SendString("stop 1-1")
        aa.SendString("stop 2-1")
    End Sub

    Private Sub Cmdshowcasparcgwindow_Click(sender As Object, e As EventArgs)

    End Sub

    <DllImport("user32.dll")>
    Private Shared Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
    End Function

    Private Sub Cmdoutcasparcgwindow_Click(sender As Object, e As EventArgs)
        Try
            SetParent(parentedProcess.MainWindowHandle, IntPtr.Zero)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            Dim myProcesses() As Process
            Dim myProcess As Process

            myProcesses = Process.GetProcessesByName("casparcg")
            For Each myProcess In myProcesses
                myProcess.Kill()
            Next

            myProcesses = Nothing
            myProcess = Nothing
        Catch ex As Exception
            MessageBox.Show("Error closing CasparCG.exe: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Integer, y As Integer, cx As Integer, cy As Integer, uFlags As UInteger) As Boolean
    End Function

    Private Const SWP_NOSIZE As UInteger = &H1
    Private Const SWP_NOZORDER As UInteger = &H4
    Private Const SWP_SHOWWINDOW As UInteger = &H40

    Private Sub MoveWindowToSecondMonitor(processName As String, windowTitle As String)
        For Each process As Process In Process.GetProcessesByName(processName)
            If process.MainWindowTitle = windowTitle AndAlso Screen.AllScreens.Length > 1 Then
                SetWindowPos(process.MainWindowHandle, IntPtr.Zero, Screen.AllScreens(1).WorkingArea.Left, Screen.AllScreens(1).WorkingArea.Top, 0, 0, SWP_NOSIZE Or SWP_NOZORDER Or SWP_SHOWWINDOW)
                Exit Sub
            End If
        Next
    End Sub

    Private Sub SomeMethod()

    End Sub

End Class