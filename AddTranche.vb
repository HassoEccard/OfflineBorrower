Public Class fmAddTranche

    Private iCurrentLoanID As Integer

    Public Sub SetUp(iLoanID As Integer)
        Dim dr As DataRow
        Dim iNumTranches, i, iCounter, iParentID As Integer

        iCurrentLoanID = iLoanID
    End Sub

    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim strConn As String
        Dim iRes As Integer
        Dim sEmail, sBusiness As String
        Dim rRate As Double
        Dim iTerm, iAmount, iRate As Integer
        Dim dLastDate As DateTime

        dLastDate = Now.AddMonths(1)

        txtError.Visible = True
        Try
            iTerm = CInt(edTerm.Text)
        Catch ex As Exception
            iTerm = 0
        End Try
        iAmount = GenDB.CurrencyStringPoundsToPence(edAmount.Text)

        rRate = GenDB.fnDBDoubleField(edRate.Text)
        iRate = CInt(rRate * 100)

        If iTerm = 0 Then
            txtError.Text = "Term can't be empty"
            Exit Sub
        End If

        If iAmount = 0 Then
            txtError.Text = "Amount can't be empty"
            Exit Sub
        End If

        If iRate = 0 Then
            txtError.Text = "The rate can't be empty"
            Exit Sub
        End If
        'Comments

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()
        trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand
        Cmd.Connection = MyConn
        Cmd.CommandType = Data.CommandType.StoredProcedure
        Cmd.CommandText = "loan_tranche"

        Cmd.Parameters.Clear()

        Cmd.Parameters.Add("@I_NEW_AMOUNT", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iAmount
        Cmd.Parameters.Add("@I_NEW_TERM", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iTerm
        Cmd.Parameters.Add("@I_LOANID", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iCurrentLoanID
        Cmd.Parameters.Add("@I_FIXEDRATE", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iRate
        Cmd.Parameters.Add("@I_AUCTIONEND", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = dLastDate
        Cmd.Transaction = trans

        Cmd.ExecuteScalar()

        trans.Commit()

        Cmd = Nothing
        MyConn.Close()
        MyConn = Nothing

        txtError.Text = "New Tranche Created"
    End Sub

    Private Sub fmAddTranche_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtError.Visible = False
    End Sub
End Class