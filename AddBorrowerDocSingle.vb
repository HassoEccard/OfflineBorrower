Imports System.Net
Imports System.IO


Public Class fmAddBorrowerDocSingle

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            edFile.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

    End Sub
End Class