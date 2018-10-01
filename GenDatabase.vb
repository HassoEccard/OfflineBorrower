Public Class GenDatabase

    Public Shared dsLoans As DataSet
    Public Shared dsLoanSets As DataSet
    Public Shared LatestDate As DateTime

    Public Shared Sub LoanLoans()
        Dim strConn As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim sSQL As String
        Dim i, iCounter As Integer
        Dim dr As DataRow
        Dim CurDate As DateTime

        sSQL = "select *
                 from loans l, accounts a, users u
                 where a.accountid = l.accountid
                    and l.isactive = 0
                    and l.LoanStatus > 0
                    and u.userid = a.userid
                 order by LoanID"

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()

        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(sSQL, MyConn)

        Adaptor.Fill(dsLoans)

        ' Get latest updated date
        LatestDate = Now.AddYears(-2)

        iCounter = dsLoans.Tables(0).Rows.Count

        For i = 0 To iCounter - 1
            dr = dsLoans.Tables(0).Rows(i)
            CurDate = GenDB.fnDBDateField(dr("LastUpdated"))

            If CurDate > LatestDate Then
                LatestDate = CurDate
            End If
        Next i

        MyConn.Close()
        Adaptor = Nothing
        MyConn = Nothing

        sSQL = "select *
                 from loan_sets
                where isactive = 0
                order by loanSetID"

        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MyConn.Open()

        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(sSQL, MyConn)

        Adaptor.Fill(dsLoanSets)

        MyConn.Close()
        Adaptor = Nothing
        MyConn = Nothing
    End Sub


    Public Shared Function UpdateLoans(dt As DateTime)
        Dim strConn As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim sSQL As String
        Dim ds As dataset
        Dim i, j, j1, a, b As Integer
        Dim bFound As Boolean
        Dim dr, dr1 As DataRow
        Dim iCounter, iLoanCounter As Integer

        sSQL = "select *
                 from loans l
                 where LastUpdated > @p1"

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        Adaptor.SelectCommand.Parameters.Add("@p1", FirebirdSql.Data.FirebirdClient.FbDbType.TimeStamp).Value = LatestDate

        ds = New dataset

        MyConn.Open()

        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(sSQL, MyConn)

        Adaptor.Fill(ds)

        iCounter = ds.Tables(0).Rows.Count
        iLoanCounter = dsLoans.Tables(0).Rows.Count

        For i = 0 To iCounter - 1
            dr = ds.Tables(0).Rows(i)
            j = GenDB.fnDBIntField(dr("LoanID"))

            a = 0
            bFound = False
            While (a < iLoanCounter) And (Not bFound)
                dr1 = dsLoans.Tables(0).Rows(a)
                j1 = GenDB.fnDBIntField(dr1("LoanID"))

                If j = j1 Then
                    bFound = True
                End If
                a += 1
            End While

            If bFound Then
                ' dsLoans.Tables(0).Rows(a). = dr1
            End If
        Next i


        ds = Nothing
        MyConn.Close()
        Adaptor = Nothing
        MyConn = Nothing
    End Function


    Public Shared Function GotoLoanIDIdx(iLoanID As Integer) As Integer
        Dim i, j, iCounter As Integer
        Dim dr As DataRow
        Dim bFound As Boolean

        iCounter = dsLoans.Tables(0).Rows.Count

        i = 0
        bFound = False
        While i <= iCounter - 1 And Not bFound
            dr = dsLoans.Tables(0).Rows(i)
            j = GenDB.fnDBIntField(dr("LoanID"))
            If j = iLoanID Then
                bFound = True
            Else
                i += 1
            End If
        End While

        If bFound Then
            GotoLoanIDIdx = i
        Else
            GotoLoanIDIdx = -1
        End If
    End Function

    Public Shared Function GotoLoanSetIDIdx(iLoanID As Integer) As Integer
        Dim i, j, iCounter As Integer
        Dim dr As DataRow
        Dim bFound As Boolean

        iCounter = dsLoanSets.Tables(0).Rows.Count

        i = 0
        bFound = False
        While i <= iCounter - 1 And Not bFound
            dr = dsLoans.Tables(0).Rows(i)
            j = GenDB.fnDBIntField(dr("LoanSetID"))
            If j = iLoanID Then
                bFound = True
            Else
                i += 1
            End If
        End While

        If bFound Then
            GotoLoanSetIDIdx = i
        Else
            GotoLoanSetIDIdx = -1
        End If
    End Function

    ' Get the last tranche number for the parent.
    Public Shared Function GetMaxTrancheValue(iLoanID_Idx) As Integer
        Dim iMax As Integer = 0
        Dim dr As DataRow
        Dim iParentLoanID, iRes As Integer
        Dim i, j, k, iCount As Integer

        ' Is the LoanIDX a parent or chilc
        dr = dsLoans.Tables(0).Rows(iLoanID_Idx)

        If GenDB.fnDBIntField(dr("Loan_Parent_ID")) = 0 Then ' it is a parent
            iParentLoanID = GenDB.fnDBIntField(dr("LoanID"))
        Else
            iParentLoanID = GenDB.fnDBIntField(dr("Loan_Parent_ID"))
        End If

        j = dsLoans.Tables(0).Rows.Count
        iCount = 0
        For i = 0 To j - 1
            If dsLoans.Tables(0).Rows(i)("Loan_Parent_ID") = iParentLoanID Then
                k = CInt(dsLoans.Tables(0).Rows(i)("Tranche_Number"))
                If k > iMax Then
                    iMax = k
                End If
                iCount += 1
            End If
        Next i

        If iCount > iMax Then
            iRes = iCount
        Else
            iRes = iMax
        End If

        GetMaxTrancheValue = iRes
    End Function


End Class
