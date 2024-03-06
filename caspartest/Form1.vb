Imports Svt.Caspar
Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class Form1

    Private parentedProcess As Process

    Public Sub New()
        InitializeComponent()
    End Sub


    Dim aa As New CasparDevice
    Private g_int_ChannelNumber As Integer = 1
    Private g_int_PlaylistLayer As Integer = 1
    Private mediaDuration As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        aa.Connect("127.0.0.1", 5250)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If o.ShowDialog = DialogResult.OK Then
            TextBox1.Text = Replace(Replace(o.FileName, ":", ":\"), "\", "/")
        End If
        aa.SendString($"Play {g_int_ChannelNumber}-{g_int_PlaylistLayer} ""{TextBox1.Text}""")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        aa.SendString("stop 1-1")
    End Sub

    Private Sub Cmdshowcasparcgwindow_Click(sender As Object, e As EventArgs) Handles Cmdshowcasparcgwindow.Click
        Try
            Dim p1() As Process = Process.GetProcessesByName("casparcg")
            Dim iprocess As Integer
            For iprocess = 0 To p1.GetUpperBound(0)
                If p1(iprocess).MainWindowTitle = "Screen consumer [1|1080p5000]" Then
                    Exit For
                End If
            Next iprocess

            parentedProcess = p1(iprocess)
            SetParent(parentedProcess.MainWindowHandle, Panel1.Handle)
            SendMessage(parentedProcess.MainWindowHandle, 274, 61488, 0)

            ' Embed the "Console Window Host" into Panel2
            Dim consoleProcesses() As Process = Process.GetProcessesByName("Console Window Host")
            If consoleProcesses.Length > 0 Then
                SetParent(consoleProcesses(0).MainWindowHandle, Panel2.Handle)
                SendMessage(consoleProcesses(0).MainWindowHandle, 274, 61488, 0)
            End If
        Catch ex As Exception
        End Try
    End Sub



    <DllImport("user32.dll")>
    Private Shared Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As IntPtr
    End Function

    Private Sub Cmdoutcasparcgwindow_Click(sender As Object, e As EventArgs) Handles Cmdoutcasparcgwindow.Click
        Try
            SetParent(parentedProcess.MainWindowHandle, IntPtr.Zero)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            SetParent(parentedProcess.MainWindowHandle, IntPtr.Zero)
        Catch ex As Exception
        End Try
    End Sub
End Class