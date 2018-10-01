Public Class fmLoanExtend
    Public Shared iLoanSetID As Integer


    Private Sub fmLoanExtend_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim MySQL, strConn As String
        Dim iRate As Integer

        dtDateStart.Value = Now
        dtDateEnd.Value = Now.AddMonths(1)
        txtError.Visible = False

        MySQL = "select first 1 l.fixed_rate
                 from loans l
                 where loansetid = " & iLoanSetID.ToString() &
                 " and isactive = 0"

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(MySQL, MyConn)

        Reader = Cmd.ExecuteReader()

        If Reader.Read Then
            iRate = GenDB.fnDBIntField(Reader("fixed_rate"))
        Else
            iRate = 0
        End If

        edExistingRate.Text = GenDB.fnInterestRateToPercent(iRate)

        Cmd = Nothing
        MyConn.Close()
        MyConn = Nothing

    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim MySQL, strConn As String
        Dim NewRate, iLoanID As Integer
        Dim ActiveDate, EndDate As DateTime

        btnGo.Enabled = False
        Cursor = Cursors.WaitCursor
        Application.DoEvents()

        ActiveDate = dtDateStart.Value
        EndDate = dtDateEnd.Value
        NewRate = GenDB.DisplayToInterestRate(edNewRate.Text)

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)

        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand
        Cmd.Connection = MyConn
        Cmd.Transaction = Trans

        Try
            Cmd.CommandType = Data.CommandType.StoredProcedure
            Cmd.CommandText = "EXTEND_LOAN2"
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add("@I_LOANSETID", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iLoanSetID
            Cmd.Parameters.Add("@I_NEWRATE", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = NewRate
            Cmd.Parameters.Add("@I_ACTIVEDATE", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = ActiveDate
            Cmd.Parameters.Add("@I_ENDDATE", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = EndDate

            Cmd.ExecuteReader()
            Trans.Commit()
        Catch
            Trans.Rollback()
        End Try

        Cmd = Nothing

        Cursor = Cursors.Default
    End Sub
End Class