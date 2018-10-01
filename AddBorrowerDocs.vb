Imports System.Net
Imports System.IO

Public Class fmAddBorrowerDocs
    Private Shared iCurrentLoanID, iNumTranches As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            edFile.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Public Shared Sub SetUp(iLoanID As Integer)
        Dim dr As DataRow
        Dim iNumTranches, i, iCounter, iParentID As Integer

        iCurrentLoanID = iLoanID
        iCounter = GenDatabase.dsLoans.Tables(0).Rows.Count
        iNumTranches = 0

        fmAddBorrowerDocs.ListBox1.Items.Clear()
        For i = 0 To iCounter - 1
            dr = GenDatabase.dsLoans.Tables(0).Rows(i)
            iParentID = GenDB.fnDBIntField(dr("Loan_Parent_ID"))

            If iParentID = iCurrentLoanID Then
                fmAddBorrowerDocs.ListBox1.Items.Add(GenDB.fnDBStringField(dr("Business_Name")))
                iNumTranches += 1
            End If
        Next

        fmAddBorrowerDocs.ProgressBar1.Maximum = iNumTranches
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

    End Sub


    ' Get the number of tranches
    ' For each, upload the file
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Dim sDesc, sFile As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim strConn, sSQL, sDocName, sExt As String
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim dr As DataRow
        Dim ftpRequest As FtpWebRequest
        Dim ftpResponse As FtpWebResponse
        Dim iMaxID, iFileType, iNumTranches, i, iCounter, iParentID, iTheLoanID, iTranchCount As Integer


        If iCurrentLoanID < 1 Then
            txtError.Visible = True
            txtError.Text = "No Loan Selected"
            Exit Sub
        End If

        txtError.Visible = False
        btnUpload.Enabled = False

        Cursor = Cursors.WaitCursor
        Application.DoEvents()

        iCounter = GenDatabase.dsLoans.Tables(0).Rows.Count
        iTranchCount = 0

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()

        Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)

        For i = 0 To iCounter - 1
            dr = GenDatabase.dsLoans.Tables(0).Rows(i)
            iParentID = GenDB.fnDBIntField(dr("Loan_Parent_ID"))

            If iParentID = iCurrentLoanID Then

                iTheLoanID = GenDB.fnDBIntField(dr("LoanID"))

                sSQL = "select max(loan_file_id) as maxid from loan_files"
                Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)
                Cmd.Transaction = Trans

                Reader = Cmd.ExecuteReader()

                If Reader.Read Then
                    iMaxID = GenDB.fnDBIntField(Reader("maxid")) + 1
                Else
                    iMaxID = 1
                End If

                Cmd = Nothing

                ' .................................................................................................................

                sDesc = edDescription.Text
                sFile = edFile.Text
                iFileType = ddFileType.SelectedIndex

                sExt = Path.GetExtension(sFile)

                sDocName = GenDB.GetBorrowerDoc(iMaxID, iTheLoanID, sExt)

                ftpRequest = DirectCast(FtpWebRequest.Create(New Uri("ftp://134.213.141.12/borrowerdocs/" & sDocName)), FtpWebRequest)
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile
                ftpRequest.Proxy = Nothing
                ftpRequest.UseBinary = True
                ftpRequest.KeepAlive = False
                ftpRequest.EnableSsl = True
                ftpRequest.Credentials = New NetworkCredential(FTPUser, FTPPassword)

                'Selection of file to be uploaded
                Dim ff As New FileInfo(sFile)
                'e.g.: c:\\Test.txt
                Dim fileContents As Byte() = New Byte(ff.Length - 1) {}

                Using fr As FileStream = ff.OpenRead()
                    fr.Read(fileContents, 0, Convert.ToInt32(ff.Length))
                End Using

                Using writer As Stream = ftpRequest.GetRequestStream()
                    writer.Write(fileContents, 0, fileContents.Length)
                End Using

                ftpResponse = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)

                ftpResponse.Close()
                ftpResponse.Dispose()

                sSQL = "insert into loan_files (description, filename, loanid, originalfilename, FileType) VALUES (@p1, @p2, @p3, @p4, @p5)"

                Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)
                Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)
                Cmd.Transaction = Trans
                Cmd.Parameters.Clear()
                Cmd.Parameters.Add("p1", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sDesc
                Cmd.Parameters.Add("p2", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sDocName
                Cmd.Parameters.Add("p3", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iTheLoanID
                Cmd.Parameters.Add("p4", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = sFile
                Cmd.Parameters.Add("p5", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iFileType

                Cmd.ExecuteReader()

                iTranchCount += 1
                ProgressBar1.Value = iTranchCount

                Cursor = Cursors.WaitCursor
                Application.DoEvents()
            End If
        Next

        Try
            Trans.Commit()
        Catch ex As Exception
            Trans.Rollback()
        End Try

        MyConn.Close()
        MyConn = Nothing

        Cursor = Cursors.Default
        btnUpload.Enabled = True

    End Sub
End Class