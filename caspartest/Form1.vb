Imports Svt.Caspar

Public Class Form1
    Dim aa As New CasparDevice
    Private g_int_ChannelNumber As Integer = 1
    Private g_int_PlaylistLayer As Integer = 1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        aa.Connect("127.0.0.1", 5250)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If o.ShowDialog = DialogResult.OK Then
            TextBox1.Text = Replace(Replace(o.FileName, ":", ":\"), "\", "/")
        End If
        aa.SendString("Play " & g_int_ChannelNumber & "-" & g_int_PlaylistLayer & " " & """" & TextBox1.Text & """")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        aa.SendString("stop 1-1")
    End Sub
End Class

