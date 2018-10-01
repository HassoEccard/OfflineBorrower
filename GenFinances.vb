Imports System.Text
Imports System.IO
Imports System.Configuration

Public Class GenFinances

    Public Shared Function GetServerName() As String
        Dim s, strConn As String

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString.ToUpper

        s = ""
        If strConn.Contains("DEVEL") Then
            s = "Development Server"
            Return s
        End If

        If s = "" Then
            If strConn.Contains("MAIN2") Then
                s = "Shadow Server"
                Return s
            End If
        End If

        If s = "" Then
            If strConn.Contains("MAIN") Then
                s = "Live Server"
                Return s
            End If
        End If

        GetServerName = s
    End Function

    Public Shared Function LogEntry(ByVal sEntry As String) As Integer
        Dim mydocpath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim sb As StringBuilder = New StringBuilder()
        Dim sFile As String

        sb.Append(Now.ToShortDateString & "=" & Now.ToLongTimeString & "=" & sEntry)
        sb.AppendLine()

        sFile = Directory.GetCurrentDirectory & "\logs\Finances-" & Now.ToString("yyyyMMdd") & ".log"
        Using outfile As StreamWriter = New StreamWriter(sFile, True)
            outfile.Write(sb.ToString())
        End Using

        LogEntry = 0
    End Function

    Public Shared Function ExecuteSimpleSQL(sSQL As String) As String
        Dim strConn, sResult As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()

        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)

        sResult = ""
        Try
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            sResult = "SimpleSQL error: " & ex.Message & vbNewLine & sSQL
        End Try

        MyConn.Close()
        Cmd = Nothing
        MyConn = Nothing

        ExecuteSimpleSQL = sResult
    End Function

    Public Shared Function GetBidTotal(dDate As Date) As Integer
        Dim strConn, sResult As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim sSQL As String
        Dim iTotal As Integer


        sSQL = "Select sum(b.amount) as thetotal
                from loans l, orders o, bids b
                where l.loanstatus = 1
                  And l.isactive = 0
                  And o.loanid = l.loanid
                  And b.orderid = o.orderid
                  And b.isactive = 0
                  And b.DateTimeCreated < @p1"

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()

        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)
        Cmd.Parameters.Add("@p1", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = dDate

        sResult = ""

        iTotal = 0
        Try
            Reader = Cmd.ExecuteReader()
            If Reader.Read Then
                iTotal = GenDB.fnDBIntField(Reader("thetotal"))
            End If
        Catch ex As Exception
            sResult = "GetBidTotal error: " & ex.Message & vbNewLine & sSQL
            iTotal = -1
        End Try

        MyConn.Close()
        Cmd = Nothing
        MyConn = Nothing

        GetBidTotal = iTotal
    End Function

    ' Use solicitor's transactions - subtract debts from credits for all transactions before the date
    Public Shared Function GetSolicitorsTotal(dDate As Date) As Integer
        Dim strConn, sResult As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim ds As DataSet
        Dim dr As DataRow
        Dim sSQL As String
        Dim iTotal, iTransType, iAmount, iCounter As Integer

        ds = New DataSet

        sSQL = "select amount, transtype
                from fin_trans f
                where f.accountid in (27,28,29,31,32,33,34,35,36,37,38,39,40)
                  and f.transtype in (1004, 1007)
                  And f.DateCreated < @p1"

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()

        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(sSQL, MyConn)
        Adaptor.SelectCommand.Parameters.Add("@p1", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = dDate

        Adaptor.Fill(ds)
        sResult = ""

        iCounter = ds.Tables(0).Rows.Count

        iTotal = 0

        For i = 0 To iCounter - 1
            dr = ds.Tables(0).Rows(i)
            iAmount = GenDB.fnDBIntField(dr("amount"))
            iTransType = GenDB.fnDBIntField(dr("TransType"))

            If (iTransType = 1007) Then
                iTotal -= iAmount
            Else
                iTotal += iAmount
            End If

        Next i

        MyConn.Close()
        Adaptor = Nothing
        MyConn = Nothing

        GetSolicitorsTotal = iTotal
    End Function

    Public Shared Function GetArrangementFeeTotal(dDate As Date) As Integer
        Dim strConn, sResult As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim sSQL As String
        Dim iTotal As Integer

        sSQL = "select first 1 amount
                from fin_bals f
                where f.accountid in (30)
                  And f.DateCreated < @p1
                order by f.fin_balid desc"

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()

        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)
        Cmd.Parameters.Add("@p1", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = dDate

        sResult = ""

        iTotal = 0
        Try
            Reader = Cmd.ExecuteReader()
            If Reader.Read Then
                iTotal = GenDB.fnDBIntField(Reader("amount"))
            End If
        Catch ex As Exception
            sResult = "GetArrangementFeeTotal error: " & ex.Message & vbNewLine & sSQL
            iTotal = -1
        End Try

        MyConn.Close()
        Cmd = Nothing
        MyConn = Nothing

        GetArrangementFeeTotal = iTotal
    End Function
End Class
