Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1a
    Public Shared iSelectedLoanID, iSelectedLoanIDIdx, iLoanStatus As Integer
    Public Shared SelectedNode As TreeNode
    Private m_OldSelectNode As TreeNode

    Private Sub LoadLoanTree()
        Dim iParentID As Integer
        Dim i, j, k, iCounter, iLoanID, iAccID, iLoanSetID, a, b, iTemp As Integer
        Dim s As String
        Dim dr As DataRow
        Dim cnode As TreeNode
        Dim isFound As Boolean

        tvLoans.Nodes.Clear()
        i = 0
        iCounter = GenDatabase.dsLoans.Tables(0).Rows.Count

        While i < (iCounter)
            dr = GenDatabase.dsLoans.Tables(0).Rows(i)
            iParentID = GenDB.fnDBIntField(dr("Loan_Parent_ID"))
            iLoanSetID = GenDB.fnDBIntField(dr("LoanSetID"))
            s = Trim(dr("Business_Name").ToString)
            ' Find the node matching the LoanSetID from the array-tag index
            ' Loan_Set nodes have a negative tag value of the Loan_Set_ID

            b = tvLoans.Nodes.Count
            a = 0
            iTemp = iLoanSetID * -1
            isFound = False
            While a < (b) And isFound = False
                If (tvLoans.Nodes(a).Tag = iTemp) And (iTemp < 0) Then
                    isFound = True
                Else
                    a = a + 1
                End If
            End While

            If b > 0 Then
                If isFound Then
                    cnode = tvLoans.Nodes(a).Nodes.Add(dr("LoanID").ToString.PadLeft(5, "0") & "-" & Trim(dr("Trading_Name").ToString))
                    cnode.ContextMenuStrip = menuTranche
                Else
                    cnode = tvLoans.Nodes.Add(Trim(dr("Business_Name").ToString)) ' get the data from the loansets dataset
                    cnode.ContextMenuStrip = menuLoan
                    cnode.Tag = iTemp
                    cnode = cnode.Nodes.Add(dr("LoanID").ToString.PadLeft(5, "0") & "-" & Trim(dr("Trading_Name").ToString))
                    cnode.ContextMenuStrip = menuTranche
                End If
            Else
                cnode = tvLoans.Nodes.Add(Trim(dr("Business_Name").ToString))
                cnode.Tag = iTemp
                cnode.ContextMenuStrip = menuLoan
            End If

            'If iParentID = 0 Then
            '    ' Now find the borrower, if there isn't one, add it
            '    iAccID = GenDB.fnDBIntField(dr("AccountID"))
            '    ' cnode.

            '    cnode = tvLoans.Nodes.Add(dr("LoanID").ToString.PadLeft(5, "0") & "-" & Trim(dr("Business_Name").ToString))
            '    cnode.ContextMenuStrip = menuLoan
            'Else
            '    j = tvLoans.Nodes.Count
            '    k = 0
            '    s = tvLoans.Nodes(0).Text.Substring(0, 5)
            '    iLoanID = CInt(s)

            '    While (k < j - 1) And (iParentID <> iLoanID)
            '        k += 1
            '        s = tvLoans.Nodes(k).Text.Substring(0, 5)
            '        iLoanID = CInt(s)
            '    End While

            '    If (iParentID = iLoanID) Then
            '        cnode = tvLoans.Nodes(k).Nodes.Add(dr("LoanID").ToString.PadLeft(5, "0") & "-" & Trim(dr("Trading_Name").ToString))
            '        cnode.ContextMenuStrip = menuTranche
            '    End If
            'End If
            i += 1
        End While
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        GenDatabase.dsLoans = New DataSet
        GenDatabase.dsLoanSets = New DataSet
        GenDatabase.LoanLoans()
        LoadLoanTree()
    End Sub

    Private Sub AuctionBids()
        Dim strConn, MySQL As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim ds As DataSet

        'Cursor = Cursors.WaitCursor
        Application.DoEvents()


        If chIncludeCancelled.Checked Then
            MySQL = " select (trim(u.Lastname) || ', ' || trim(u.Firstname)) as thename, b.amount as theamount,  b.datetimecreated" &
                    " from users u, accounts a, bids b, orders o  " &
                    " where a.accountid = b.accountid and  " &
                    "      u.userid     = a.userid and " &
                    "      o.orderid    = b.orderid and " &
                    "      o.loanID     = " & iSelectedLoanID.ToString
        Else
            MySQL = " select (trim(u.Lastname) || ', ' || trim(u.Firstname)) as thename, b.amount as theamount, b.datetimecreated " &
                    " from users u, accounts a, bids b, orders o  " &
                    " where a.accountid = b.accountid and  " &
                    "      u.userid     = a.userid and " &
                    "      b.IsActive   = 0 and " &
                    "      o.orderid    = b.orderid and " &
                    "      o.loanID     = " & iSelectedLoanID.ToString
        End If

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        ds = New DataSet
        MyConn.Open()
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.Fill(ds)

        'ListAuctionBids.
        ListAuctionBids.DataSource = ds.Tables(0)
        ' DataGridView1.DataMember = "loans"

        MyConn.Close()
        MyConn = Nothing
    End Sub

    Private Sub grpboxIndividualLenderRegistrationDetails_Enter(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        SaveLoan()
    End Sub

    Private Sub SaveLoan()
        Dim strConn, MySQL As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim ds As DataSet
        Dim sPeople, sMsg As String
        Dim iTerm As Integer

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()

        MySQL = " update loans " &
                " set BUSINESS_NAME=?, " &
                "     COMPANY_REG_NO = ? , " &
                "     TRADING_NAME = ? , " &
                "     BUS_HOUSE_NAME = ? , " &
                "     BUS_ADDRESS1 = ? , " &
                "     BUS_ADDRESS2 = ? , " &
                "     BUS_ADDRESS3 = ? , " &
                "     BUS_POST_CODE = ? , " &
                "     CONTACT = ? , " &
                "     REG_HOUSE_NAME = ? , " &
                "     REG_ADDRESS1 = ? , " &
                "     REG_ADDRESS2 = ? , " &
                "     REG_ADDRESS3 = ? , " &
                "     REG_POST_CODE = ? , " &
                "     BUS_PHONE = ? , " &
                "     MOBILE_PHONE = ? , " &
                "     WEBSITE = ? , " &
                "     EMAIL = ? , " &
                "     PURPOSE_OF_LOAN = ? , " &
                "     BACKGROUND = ? , " &
                "     PEOPLE = ? , " &
                "     ANYTHINGELSE = ?,  " &
                "     TERM = ?,  " &
                "     MAXLOANAMOUNT = ?,  " &
                "     LOANTYPE = ?,  " &
                "     FIXED_RATE = ?,  " &
                "     DEAL_REF = ?, " &
                "     VISIBLE = ?, " &
                "     IPO_END = ?, " &
                "     DD_DATE = ?, " &
                "     DD_LASTDATE = ?, " &
                "     BIDLIVE = ?, " &
                "     DATE_OF_LAST_PAYMENT = ?, " &
                "     SECONDARY_TRADING = ?, " &
                "     FACILITY_AMOUNT = ?, " &
                "     AMOUNT_PREBID = ? " &
                " where loanid = ?"

        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(MySQL, MyConn)
        Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Cmd.Transaction = Trans

        sPeople = ""
        If lstPeople.Items.Count > 0 Then
            Dim PeopleRows As String = ""
            PeopleRows = "<table style=""width:310px;"">" & vbCrLf
            For Each lstItem In lstPeople.Items
                PeopleRows &= "<tr style=""background-color:#FFFFFF; border-bottom:0px;"">" & vbCrLf &
                                 " <td><strong>" & Split(lstItem, " - ")(0) & "</strong></td>" & vbCrLf
                If Split(lstItem, " - ").Count > 1 Then
                    PeopleRows &= "<td style = ""font-weight:inherit;"" >" & Split(lstItem, " - ")(1) & "</td>" & vbCrLf
                End If
                PeopleRows &= "</tr>" & vbCrLf
            Next
            PeopleRows &= "</table>"
            sPeople = PeopleRows
        End If

        Cmd.Parameters.Add("BUSINESS_NAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusinessName.Text
        Cmd.Parameters.Add("COMPANY_REG_NO", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edRegNumber.Text
        Cmd.Parameters.Add("TRADING_NAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edTradingName.Text
        Cmd.Parameters.Add("BUS_HOUSE_NAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusAdd1.Text
        Cmd.Parameters.Add("BUS_ADDRESS1", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusAdd2.Text
        Cmd.Parameters.Add("BUS_ADDRESS2", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusAdd3.Text
        Cmd.Parameters.Add("BUS_ADDRESS3", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusAdd4.Text
        Cmd.Parameters.Add("BUS_POST_CODE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusPC.Text
        Cmd.Parameters.Add("CONTACT", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edContactName.Text
        Cmd.Parameters.Add("REG_HOUSE_NAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edRegAdd1.Text
        Cmd.Parameters.Add("REG_ADDRESS1", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edRegAdd2.Text
        Cmd.Parameters.Add("REG_ADDRESS2", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edRegAdd3.Text
        Cmd.Parameters.Add("REG_ADDRESS3", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edRegAdd4.Text
        Cmd.Parameters.Add("REG_POST_CODE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edRegPC.Text
        Cmd.Parameters.Add("BUS_PHONE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edBusPhone.Text
        Cmd.Parameters.Add("MOBILE_PHONE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edMobile.Text
        Cmd.Parameters.Add("WEBSITE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edWebsite.Text
        Cmd.Parameters.Add("EMAIL", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edEmail.Text
        Cmd.Parameters.Add("PURPOSE_OF_LOAN", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edPurpose.Text
        Cmd.Parameters.Add("BACKGROUND", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edDescription.Text
        Cmd.Parameters.Add("PEOPLE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sPeople
        'Cmd.Parameters.Add("QANDA", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edQA.Text
        Cmd.Parameters.Add("ANYTHINGELSE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = ""
        'Cmd.Parameters.Add("COMPANY_RISK_AS_STARS", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = Trim(edStar/Rating.Text)
        'Cmd.Parameters.Add("SECURITY_GUARANTEES", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = Trim(edSecurityGuarantees.Text)
        Cmd.Parameters.Add("TERM", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.fnDBIntField(edTerm.Text)
        Cmd.Parameters.Add("MAXLOANAMOUNT", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.CurrencyStringPoundsToPence(edAmount.Text)
        Cmd.Parameters.Add("LOANTYPE", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = 3 ' ddLoanType.SelectedIndex
        Cmd.Parameters.Add("FIXED_RATE", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.DisplayToInterestRate(edFixedRate.Text)
        Cmd.Parameters.Add("DEAL_REF", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = "" ' Trim(edDealRef.Text)
        If chVisible.Checked Then
            Cmd.Parameters.Add("VISIBLE", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = 0
        Else
            Cmd.Parameters.Add("VISIBLE", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = 1
        End If

        Cmd.Parameters.Add("IPO_END", FirebirdSql.Data.FirebirdClient.FbDbType.Date).Value = GenDB.fnDBDateField(dtIPOEnds.Value)
        Cmd.Parameters.Add("DD_DATE", FirebirdSql.Data.FirebirdClient.FbDbType.Date).Value = GenDB.fnDBDateField(dtLoanStart.Value)
        Cmd.Parameters.Add("DD_LASTDATE", FirebirdSql.Data.FirebirdClient.FbDbType.Date).Value = GenDB.fnDBDateField(dtEndDate.Value)
        Cmd.Parameters.Add("BIDLIVE", FirebirdSql.Data.FirebirdClient.FbDbType.Date).Value = GenDB.fnDBDateField(dtBidLive.Value)
        Cmd.Parameters.Add("DATE_OF_LAST_PAYMENT", FirebirdSql.Data.FirebirdClient.FbDbType.Date).Value = GenDB.fnDBDateField(dtDateOfLastPayment.Value)

        If chSecTrading.Checked Then
            Cmd.Parameters.Add("SECONDARY_TRADING", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = 0
        Else
            Cmd.Parameters.Add("SECONDARY_TRADING", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = 1
        End If

        Cmd.Parameters.Add("FACILITY_AMOUNT", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.CurrencyStringPoundsToPence(edAmount.Text)
        Cmd.Parameters.Add("AMOUNT_PREBID", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.CurrencyStringPoundsToPence(edPreBidAmt.Text)
        ' Cmd.Parameters.Add("TRANCHE_NUMBER", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = Trim(edTrancheNum.Text)
        'Cmd.Parameters.Add("LOAN_PARENT_ID", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.fnDBIntField(edParentLoanID.Text)

        'Cmd.Parameters.Add("LOANSTATUS", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = GenDB.fnDBIntField(cbLoanStatus.SelectedIndex)

        Cmd.Parameters.Add("loanid", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iSelectedLoanID
        Try
            Cmd.ExecuteNonQuery()
            sMsg = ""
        Catch ex As Exception
            sMsg = ex.Message
            'GenExternals.LogEntry("Borrower edit exception: " & ex.Message)
        End Try

        MySQL = "update users 
                    set FIRSTNAME = ?,
                        LASTNAME = ?,
                        ADDRESS1 = ?,
                        ADDRESS2 = ?,
                        ADDRESS3 = ?,
                        ADDRESS4 = ?,
                        TOWN = ?,
                        COUNTY = ?,
                        COUNTRY = ?,
                        POSTCODE = ?
                        "
        Cmd.Parameters.Add("FIRSTNAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edFirstname.Text
        Cmd.Parameters.Add("LASTNAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edLastname.Text
        Cmd.Parameters.Add("ADDRESS1", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edAdd1.Text
        Cmd.Parameters.Add("ADDRESS2", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edAdd2.Text
        Cmd.Parameters.Add("ADDRESS3", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edAdd3.Text
        Cmd.Parameters.Add("ADDRESS4", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edAdd4.Text
        Cmd.Parameters.Add("TOWN", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edTown.Text
        Cmd.Parameters.Add("COUNTY", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edCounty.Text
        Cmd.Parameters.Add("COUNTRY", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edCountry.Text
        Cmd.Parameters.Add("POSTCODE", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = edPostCode.Text

        Try
            Cmd.ExecuteNonQuery()
            Trans.Commit()
            sMsg = ""
        Catch ex As Exception
            sMsg = ex.Message
            Trans.Rollback()
            'GenExternals.LogEntry("Borrower edit exception: " & ex.Message)
        End Try

        'GenAccounts.SetLoanWorkflow(5, iIdx)

        'PopulateListbox()

        MyConn.Close()
        MyConn = Nothing
        Cursor = Cursors.Default

        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("FIRSTNAME") = edFirstname.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("LASTNAME") = edLastname.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("ADDRESS1") = edAdd1.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("ADDRESS2") = edAdd2.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("ADDRESS3") = edAdd3.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("ADDRESS4") = edAdd4.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("TOWN") = edTown.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("COUNTY") = edCounty.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("COUNTRY") = edCountry.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("POSTCODE") = edPostCode.Text

        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUSINESS_NAME") = edBusinessName.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("COMPANY_REG_NO") = edRegNumber.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("TRADING_NAME") = edTradingName.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUS_HOUSE_NAME") = edBusAdd1.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUS_ADDRESS1") = edBusAdd2.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUS_ADDRESS2") = edBusAdd3.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUS_ADDRESS3") = edBusAdd4.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUS_POST_CODE") = edBusPC.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("CONTACT") = edContactName.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("REG_HOUSE_NAME") = edRegAdd1.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("REG_ADDRESS1") = edRegAdd2.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("REG_ADDRESS2") = edRegAdd3.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("REG_ADDRESS3") = edRegAdd4.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("REG_POST_CODE") = edRegPC.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BUS_PHONE") = edBusPhone.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("MOBILE_PHONE") = edMobile.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("WEBSITE") = edWebsite.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("EMAIL") = edEmail.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("PURPOSE_OF_LOAN") = edPurpose.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BACKGROUND") = edDescription.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("PEOPLE") = sPeople
        'GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("QANDA") = edQA.Text
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("ANYTHINGELSE") = ""
        'GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("COMPANY_RISK_AS_STARS") = Trim(edStar / Rating.Text)
        'GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("SECURITY_GUARANTEES") = Trim(edSecurityGuarantees.Text)
        Try
            iTerm = CInt(edTerm.Text)
        Catch ex As Exception
            iTerm = 0
        End Try
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("TERM") = iTerm
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("MAXLOANAMOUNT") = GenDB.CurrencyStringPoundsToPence(edAmount.Text)
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("LOANTYPE") = 3
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("FIXED_RATE") = GenDB.DisplayToInterestRate(edFixedRate.Text)
        'GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("DEAL_REF") = Trim(edDealRef.Text)

        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("IPO_END") = GenDB.fnDBDateField(dtIPOEnds.Value)
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("DD_DATE") = GenDB.fnDBDateField(dtLoanStart.Value)
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("DD_LASTDATE") = GenDB.fnDBDateField(dtEndDate.Value)
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("BIDLIVE") = GenDB.fnDBDateField(dtBidLive.Value)
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("DATE_OF_LAST_PAYMENT") = GenDB.fnDBDateField(dtDateOfLastPayment.Value)

        If chVisible.Checked Then
            GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("VISIBLE") = 0
        Else
            GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("VISIBLE") = 1
        End If

        If chSecTrading.Checked Then
            GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("SECONDARY_TRADING") = 0
        Else
            GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("SECONDARY_TRADING") = 1
        End If

        If chSecSelling.Checked Then
            GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("SECONDARY_SELL_OK") = 0
        Else
            GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("SECONDARY_SELL_OK") = 1
        End If

        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("FACILITY_AMOUNT") = GenDB.CurrencyStringPoundsToPence(edAmount.Text)
        GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("AMOUNT_PREBID") = GenDB.CurrencyStringPoundsToPence(edPreBidAmt.Text)
        'GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)("LOANSTATUS") = GenDB.fnDBIntField(cbLoanStatus.SelectedIndex)

        ' Update the business name in the tree node

        SelectedNode.Text = iSelectedLoanID.ToString.PadLeft(5, "0") & "-" & edBusinessName.Text
        MsgBox("Changes saved" & sMsg)
    End Sub

    Private Function LoadFileDocs()
        Dim MySQL, strConn As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Adaptor As FirebirdSql.Data.FirebirdClient.FbDataAdapter
        Dim ds As DataSet

        'Cursor = Cursors.WaitCursor
        Application.DoEvents()

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)

        MySQL = "select loan_file_id, trim(description) as description, DateCreated from loan_files where isactive=0 and filetype=0 and loanid=" & iSelectedLoanID.ToString

        ds = New DataSet
        MyConn.Open()
        Adaptor = New FirebirdSql.Data.FirebirdClient.FbDataAdapter(MySQL, MyConn)
        Adaptor.Fill(ds)

        DataGridView2.DataSource = ds.Tables(0)
        ' DataGridView1.DataMember = "loans"

        MyConn.Close()
        MyConn = Nothing

        'Cursor = Cursors.Default
    End Function

    Private Sub btnAddDoc_Click(sender As Object, e As EventArgs)
        Dim sDesc, sFile As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim strConn, sSQL, s, sDocName, sExt As String
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim ftpRequest As FtpWebRequest
        Dim ftpResponse As FtpWebResponse
        Dim iMaxID, iFileType As Integer

        If iSelectedLoanID < 1 Then
            MsgBox("You must select a loan before adding a file to upload")
            Exit Sub
        End If

        If fmAddBorrowerDocs.ShowDialog = Windows.Forms.DialogResult.OK Then
            Cursor = Cursors.WaitCursor
            Application.DoEvents()

            sSQL = "select max(loan_file_id) as maxid from loan_files"

            strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
            MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
            MyConn.Open()

            Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)

            Reader = Cmd.ExecuteReader()

            If Reader.Read Then
                iMaxID = GenDB.fnDBIntField(Reader("maxid")) + 1
            Else
                iMaxID = 1
            End If

            Cmd = Nothing

            ' .................................................................................................................

            sDesc = fmAddBorrowerDocs.edDescription.Text
            sFile = fmAddBorrowerDocs.edFile.Text
            iFileType = fmAddBorrowerDocs.ddFileType.SelectedIndex

            sExt = Path.GetExtension(sFile)

            sDocName = GenDB.GetBorrowerDoc(iMaxID, iSelectedLoanID, sExt)

            ftpRequest = DirectCast(FtpWebRequest.Create(New Uri("ftp://162.13.2.130/borrowerdocs/" & sDocName)), FtpWebRequest)
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile
            ftpRequest.Proxy = Nothing
            ftpRequest.UseBinary = True
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
            'Status.Text = ftpResponse.StatusDescription

            ftpResponse.Close()

            sSQL = "insert into loan_files (description, filename, loanid, originalfilename, FileType) VALUES (@p1, @p2, @p3, @p4, @p5)"

            Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)
            Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Cmd.Transaction = Trans
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add("p1", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sDesc
            Cmd.Parameters.Add("p2", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sDocName
            Cmd.Parameters.Add("p3", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iSelectedLoanID
            Cmd.Parameters.Add("p4", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = sFile
            Cmd.Parameters.Add("p5", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iFileType

            Cmd.ExecuteReader()

            Try
                Trans.Commit()
            Catch ex As Exception
                Trans.Rollback()
            End Try

            MyConn.Close()
            MyConn = Nothing

            'Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileDocs)
            LoadFileDocs()

            Cursor = Cursors.Default
        End If


    End Sub

    Private Sub btnRemoveDoc_Click(sender As Object, e As EventArgs)
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        Dim cnode, row, iBorrowerLoc As DialogResult
        Dim Desc As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim strConn, sSQL, s As String
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand

        Try
            row = DataGridView2.CurrentRow.Index
        Catch ex As Exception
            MsgBox("There is nothing to delete")
            Exit Sub
        End Try

        If MessageBox.Show("Confirm deletion of this file", "File uploaded", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Cursor = Cursors.WaitCursor
            Application.DoEvents()

            iBorrowerLoc = DataGridView2(0, row).Value

            'Desc = EditSingleText.TextBox1.Text

            strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
            MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
            MyConn.Open()

            Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand
            Cmd.Connection = MyConn

            sSQL = "update loan_files set isactive=@p1 where loan_file_id=@p2"

            Cmd.CommandType = Data.CommandType.Text
            Cmd.CommandText = sSQL
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add("p1", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = 1
            Cmd.Parameters.Add("p2", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iBorrowerLoc

            Cmd.Transaction = Trans
            Cmd.ExecuteReader()

            Trans.Commit()
            MyConn.Close()
            MyConn = Nothing

            LoadFileDocs()
            'Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileDocs)

            Cursor = Cursors.Default
        End If

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        btnAddPerson.Enabled = False
        lstPeople.Items.Add(edPersonName.Text & " - " & edPersonPosition.Text)
        btnAddPerson.Enabled = True
    End Sub

    Private Sub c_Click(sender As Object, e As EventArgs)
        btnRemovePerson.Enabled = False
        lstPeople.Items.RemoveAt(lstPeople.SelectedIndex)
        btnRemovePerson.Enabled = True
    End Sub


    Private Sub tabControl_DrawItem(sender As Object, e As DrawItemEventArgs) Handles tabControl.DrawItem
        Dim paddedBounds As Rectangle = tabControl.GetTabRect(e.Index)
        paddedBounds.Inflate(-7, -7)

        Select Case e.Index
            Case 0
                e.Graphics.FillRectangle(New SolidBrush(Color.OrangeRed), e.Bounds)
                tabControl.SelectedTab.Text = "Overview"
                'Dim x = tabControl.Font.Bold
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
            Case 1
                e.Graphics.FillRectangle(New SolidBrush(Color.Pink), e.Bounds)
                tabControl.SelectedTab.Text = "Company"
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
            Case 2
                e.Graphics.FillRectangle(New SolidBrush(Color.LightCoral), e.Bounds)
                tabControl.SelectedTab.Text = "Pitch Page"
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
            Case 3
                e.Graphics.FillRectangle(New SolidBrush(Color.IndianRed), e.Bounds)
                tabControl.SelectedTab.Text = "Pre-Bidding"
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
            Case 4
                e.Graphics.FillRectangle(New SolidBrush(Color.Tomato), e.Bounds)
                tabControl.SelectedTab.Text = "Auction"
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
            Case 5
                e.Graphics.FillRectangle(New SolidBrush(Color.LightSalmon), e.Bounds)
                tabControl.SelectedTab.Text = "Diary"
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
            Case 6
                e.Graphics.FillRectangle(New SolidBrush(Color.LightSalmon), e.Bounds)
                tabControl.SelectedTab.Text = "Diary"
                e.Graphics.DrawString(tabControl.SelectedTab.Text, Me.Font, SystemBrushes.HighlightText, paddedBounds)
        End Select

    End Sub

    Private Sub Form1a_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
    End Sub

    ' New tranche
    Private Sub menuAddTranche_Click(sender As Object, e As EventArgs) Handles menuAddTranche.Click
        fmAddTranche.Show()
    End Sub

    Private Sub btnAddBorrower_Click(sender As Object, e As EventArgs) Handles btnAddBorrower.Click
        Dim MySQL, strConn, sResult As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim ds As DataSet
        Dim iIndivid As Integer

        If AddBorrower.ShowDialog <> DialogResult.OK Then
            Exit Sub
        End If

        If Trim(AddBorrower.edBusinessName.Text) = "" Then
            MsgBox("It must have a business name")
            Exit Sub
        End If

        Cursor = Cursors.WaitCursor
        Application.DoEvents()

        iIndivid = 0

        strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
        MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
        MyConn.Open()
        Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)

        Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand
        Cmd.Connection = MyConn
        Cmd.Parameters.Clear()
        Cmd.CommandType = Data.CommandType.StoredProcedure
        Cmd.CommandText = "INSERT_BORROWER"
        Cmd.Parameters.Add("@EM", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = AddBorrower.edEmail.Text
        Cmd.Parameters.Add("@FNAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = AddBorrower.edFirstname.Text
        Cmd.Parameters.Add("@LNAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = AddBorrower.edLastname.Text
        Cmd.Parameters.Add("@BUSINESSNAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = AddBorrower.edBusinessName.Text
        Cmd.Parameters.Add("@INDIVID", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iIndivid
        Cmd.Parameters.Add("@COMPNYNA", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = AddBorrower.edBusinessName.Text
        Cmd.Parameters.Add("@COMPNYNUM", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = ""
        Cmd.Parameters.Add("@TEL", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = ""
        Cmd.Parameters.Add("@HH", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = ""
        Cmd.Parameters.Add("@PW", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = ""

        Cmd.Transaction = Trans

        sResult = ""
        Try
            Cmd.ExecuteNonQuery()
            Trans.Commit()
            sResult = "OK"
        Catch ex As Exception
            Trans.Rollback()
            sResult = ex.Message
        End Try

        Cursor = Cursors.Default
    End Sub

    Private Sub menuExtendLoan_Click(sender As Object, e As EventArgs) Handles menuExtendLoan.Click
        Dim reader As DataRow
        Dim s As String
        Dim iLoanSetID, iIdx As Integer

        If SelectedNode.Tag >= 0 Then
            MessageBox.Show("You must select a loan to extend")
            Exit Sub
        End If

        iLoanSetID = SelectedNode.Tag * -1

        fmLoanExtend.iLoanSetID = iLoanSetID
        fmLoanExtend.edExistingRate.Text = s
        fmLoanExtend.Show()
    End Sub

    Private Sub tvLoans_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvLoans.NodeMouseClick
        Dim s As String
        Dim Reader As DataRow
        Dim i, iCounter, iParentID, iLoanSetID As Integer
        Dim dr, dr1 As DataRow
        Dim ds As DataSet

        SelectedNode = e.Node
        s = e.Node.Text

        If s.Length < 5 Then
            iSelectedLoanID = 0
        Else
            s = s.Substring(0, 5)

            Try
                iSelectedLoanID = CInt(s)
            Catch
                iSelectedLoanID = 0
            End Try
        End If

        gridTranches.AutoGenerateColumns = True

        If SelectedNode.Tag < 0 Then
            panelBorower.Visible = False
            panelLoan.Visible = True
            ds = New DataSet
            ds.Tables.Add()
            iLoanSetID = SelectedNode.Tag * -1

            ds.Tables(0).Columns.Add("Id", GetType(Integer))
            ds.Tables(0).Columns.Add("DateCreated", GetType(DateTime))
            ds.Tables(0).Columns.Add("Business_Name", GetType(String))
            ds.Tables(0).Columns.Add("dd_date", GetType(DateTime))
            ds.Tables(0).Clear()
            iCounter = GenDatabase.dsLoans.Tables(0).Rows.Count
            i = 0
            While i < (iCounter)
                dr = GenDatabase.dsLoans.Tables(0).Rows(i)
                iParentID = GenDB.fnDBIntField(dr("LoanSetID"))

                If iParentID = iLoanSetID Then
                    ds.Tables(0).ImportRow(dr)
                End If
                i += 1
            End While

            gridTranches.DataSource = ds.Tables(0)
        Else
            If iSelectedLoanID > 0 Then
                iSelectedLoanIDIdx = GenDatabase.GotoLoanIDIdx(iSelectedLoanID)
                Reader = GenDatabase.dsLoans.Tables(0).Rows(iSelectedLoanIDIdx)

                panelLoan.Visible = False
                panelBorower.Visible = True

                If Trim(GenDB.fnDBStringField(Reader("BUSINESS_NAME"))) = "" Then
                    txtRef.Text = GenDB.fnDBStringField(Reader("TRADING_NAME")) + " LoanID: " +
                                          GenDB.fnDBStringField(Reader("LOANID")) + " AccountID: " +
                                          GenDB.fnDBStringField(Reader("ACCOUNTID"))
                Else
                    txtRef.Text = GenDB.fnDBStringField(Reader("BUSINESS_NAME")) + " LoanID: " +
                                          GenDB.fnDBStringField(Reader("LOANID")) + " AccountID: " +
                                          GenDB.fnDBStringField(Reader("ACCOUNTID"))
                End If

                iLoanStatus = GenDB.fnDBIntField(Reader("LOANSTATUS"))

                edBusinessName.Text = GenDB.fnDBStringField(Reader("BUSINESS_NAME"))
                edRegNumber.Text = GenDB.fnDBStringField(Reader("COMPANY_REG_NO"))
                edTradingName.Text = GenDB.fnDBStringField(Reader("TRADING_NAME"))

                edBusAdd1.Text = GenDB.fnDBStringField(Reader("BUS_HOUSE_NAME"))
                edBusAdd2.Text = GenDB.fnDBStringField(Reader("BUS_ADDRESS1"))
                edBusAdd3.Text = GenDB.fnDBStringField(Reader("BUS_ADDRESS2"))
                edBusAdd4.Text = GenDB.fnDBStringField(Reader("BUS_ADDRESS3"))
                edBusPC.Text = GenDB.fnDBStringField(Reader("BUS_POST_CODE"))

                edContactName.Text = GenDB.fnDBStringField(Reader("CONTACT"))

                edRegAdd1.Text = GenDB.fnDBStringField(Reader("REG_HOUSE_NAME"))
                edRegAdd2.Text = GenDB.fnDBStringField(Reader("REG_ADDRESS1"))
                edRegAdd3.Text = GenDB.fnDBStringField(Reader("REG_ADDRESS2"))
                edRegAdd4.Text = GenDB.fnDBStringField(Reader("REG_ADDRESS3"))
                edRegPC.Text = GenDB.fnDBStringField(Reader("REG_POST_CODE"))

                edBusPhone.Text = GenDB.fnDBStringField(Reader("BUS_PHONE"))
                edMobile.Text = GenDB.fnDBStringField(Reader("MOBILE_PHONE"))
                edWebsite.Text = GenDB.fnDBStringField(Reader("WEBSITE"))
                edEmail.Text = GenDB.fnDBStringField(Reader("EMAIL"))

                If GenDB.fnDBIntField(Reader("VISIBLE")) = 0 Then
                    chVisible.Checked = True
                Else
                    chVisible.Checked = False
                End If

                If GenDB.fnDBIntField(Reader("SECONDARY_TRADING")) = 0 Then
                    chSecTrading.Checked = True
                Else
                    chSecTrading.Checked = False
                End If

                If GenDB.fnDBIntField(Reader("SECONDARY_SELL_OK")) = 0 Then
                    chSecSelling.Checked = True
                Else
                    chSecSelling.Checked = False
                End If

                '            edRegAdd4.Text = GenDB.fnDBStringField(Reader("REG_ADDRESS3"))
                edPurpose.Text = GenDB.fnDBStringField(Reader("PURPOSE_OF_LOAN"))

                edDescription.Text = GenDB.fnDBStringField(Reader("DESCRIPTION"))
                Dim PeopleText As String = GenDB.fnDBStringField(Reader("PEOPLE"))
                'edQA.Text = GenDB.fnDBStringField(Reader("QANDA"))
                '  PeopleText = Regex.Replace(PeopleText, "<.*?>", String.Empty)
                PeopleText = Strings.Replace(PeopleText, """", "'")
                PeopleText = Strings.Replace(PeopleText, vbCrLf, "")
                PeopleText = Strings.Replace(PeopleText, "<table style='width:310px;'>", "")
                PeopleText = Strings.Replace(PeopleText, "<tr style='background-color:#FFFFFF; border-bottom:0px;'>", "")
                PeopleText = Strings.Replace(PeopleText, "<td><strong>", "")
                PeopleText = Strings.Replace(PeopleText, "</td></tr>", vbCrLf)
                PeopleText = Strings.Replace(PeopleText, "</tr>", "")
                PeopleText = Strings.Replace(PeopleText, "</strong>", "")
                PeopleText = Strings.Replace(PeopleText, "</table>", "")
                PeopleText = Strings.Replace(PeopleText, "</strong></td><td style = 'font-weight:inherit;' >", " - ")
                PeopleText = Strings.Replace(PeopleText, "</td><td style = 'font-weight:inherit;' >", " - ")

                lstPeople.Items.Clear()
                Dim PeopleArray() As String = Strings.Split(PeopleText, vbCrLf)

                For Each PeopleItem As String In PeopleArray
                    If Not PeopleItem = "" Then
                        lstPeople.Items.Add(Trim(PeopleItem))
                    End If
                Next

                edDescription.Text = GenDB.fnDBStringField(Reader("ANYTHINGELSE"))
                ' mmmmm

                'txtRef.Text = edBusinessName.Text
                ' txtRef.Text = "Ref: " & GenDB.fnDBStringField(Reader("ACCOUNTID"))
                edFirstname.Text = GenDB.fnDBStringField(Reader("FIRSTNAME"))
                edLastname.Text = GenDB.fnDBStringField(Reader("LASTNAME"))
                edAdd1.Text = GenDB.fnDBStringField(Reader("ADDRESS1"))
                edAdd2.Text = GenDB.fnDBStringField(Reader("ADDRESS2"))
                edAdd3.Text = GenDB.fnDBStringField(Reader("ADDRESS3"))
                edAdd4.Text = GenDB.fnDBStringField(Reader("ADDRESS4"))
                edTown.Text = GenDB.fnDBStringField(Reader("TOWN"))
                edCounty.Text = GenDB.fnDBStringField(Reader("COUNTY"))
                edCountry.Text = GenDB.fnDBStringField(Reader("COUNTRY"))
                edPostCode.Text = GenDB.fnDBStringField(Reader("POSTCODE"))

                Try
                    dtLoanStart.Format = DateTimePickerFormat.Custom
                    dtLoanStart.CustomFormat = "d MMM yyyy H:mm:ss"
                    dtLoanStart.Value = GenDB.fnDBDateField(Reader("DD_DATE"))
                    If dtLoanStart.Value.Year < 2000 Then
                        dtLoanStart.Format = DateTimePickerFormat.Custom
                        dtLoanStart.CustomFormat = " "
                    End If
                Catch ex As Exception
                    dtLoanStart.Format = DateTimePickerFormat.Custom
                    dtLoanStart.CustomFormat = " "
                End Try

                Try
                    dtEndDate.Format = DateTimePickerFormat.Custom
                    dtEndDate.CustomFormat = "d MMM yyyy H:mm:ss"
                    dtEndDate.Value = GenDB.fnDBDateField(Reader("DD_LASTDATE"))
                    If dtEndDate.Value.Year < 2000 Then
                        dtEndDate.Format = DateTimePickerFormat.Custom
                        dtEndDate.CustomFormat = " "
                    End If
                Catch ex As Exception
                    dtEndDate.Format = DateTimePickerFormat.Custom
                    dtEndDate.CustomFormat = " "
                End Try

                Try
                    dtDateOfLastPayment.Format = DateTimePickerFormat.Custom
                    dtDateOfLastPayment.CustomFormat = "d MMM yyyy H:mm:s"
                    dtDateOfLastPayment.Value = GenDB.fnDBDateField(Reader("DATE_OF_LAST_PAYMENT"))
                    If dtDateOfLastPayment.Value.Year < 2000 Then
                        dtDateOfLastPayment.Format = DateTimePickerFormat.Custom
                        dtDateOfLastPayment.CustomFormat = " "
                    End If
                Catch ex As Exception
                    dtDateOfLastPayment.Format = DateTimePickerFormat.Custom
                    dtDateOfLastPayment.CustomFormat = " "
                End Try

                Try
                    dtBidLive.Format = DateTimePickerFormat.Custom
                    dtBidLive.CustomFormat = "d MMM yyyy H:mm:ss"
                    dtBidLive.Value = GenDB.fnDBDateField(Reader("BidLive"))
                    If dtBidLive.Value.Year < 2000 Then
                        dtBidLive.Format = DateTimePickerFormat.Custom
                        dtBidLive.CustomFormat = " "
                    End If
                Catch ex As Exception
                    dtBidLive.Format = DateTimePickerFormat.Custom
                    dtBidLive.CustomFormat = " "
                End Try

                Try
                    dtIPOEnds.Format = DateTimePickerFormat.Custom
                    dtIPOEnds.CustomFormat = "d MMM yyyy H:mm:ss"
                    dtIPOEnds.Value = GenDB.fnDBDateField(Reader("IPO_END"))
                    If dtIPOEnds.Value.Year < 2000 Then
                        dtIPOEnds.Format = DateTimePickerFormat.Custom
                        dtIPOEnds.CustomFormat = " "
                    End If
                Catch ex As Exception
                    dtIPOEnds.Format = DateTimePickerFormat.Custom
                    dtIPOEnds.CustomFormat = " "
                End Try

                i = GenDB.fnDBIntField(Reader("SECONDARY_TRADING"))
                If i = 1 Then
                    chSecTrading.Checked = True
                End If

                edAmount.Text = GenDB.PenceToCurrencyStringPounds(GenDB.fnDBIntField(Reader("MAXLOANAMOUNT")))
                edTerm.Text = GenDB.fnDBStringField(Reader("TERM"))
                edFixedRate.Text = GenDB.fnInterestRateToPercent(GenDB.fnDBIntField(Reader("FIXED_RATE")))
                edLoanStatus.Text = GenDB.GetLoanStatus(GenDB.fnDBIntField(Reader("LOANSTATUS")))
                'Threading.ThreadPool.QueueUserWorkItem(AddressOf AuctionBids)
                'Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileDocs)
                AuctionBids()
                LoadFileDocs()
            End If
        End If
    End Sub

    Private Sub menuUploadDocuments_Click(sender As Object, e As EventArgs) Handles menuUploadDocuments.Click
        fmAddBorrowerDocs.SetUp(iSelectedLoanID)
        fmAddBorrowerDocs.Show()
    End Sub

    Private Sub btnSave1_Click(sender As Object, e As EventArgs) Handles btnSave1.Click
        SaveLoan()
    End Sub

    Private Sub btnSave2_Click(sender As Object, e As EventArgs) Handles btnSave2.Click
        SaveLoan()
    End Sub

    Private Sub btnSave3_Click(sender As Object, e As EventArgs) Handles btnSave3.Click
        SaveLoan()
    End Sub

    Private Sub btnSave4_Click(sender As Object, e As EventArgs) Handles btnSave4.Click
        SaveLoan()
    End Sub

    Private Sub btnSave5_Click(sender As Object, e As EventArgs) Handles btnSave5.Click
        SaveLoan()
    End Sub

    Private Sub menuAddLoan_Click(sender As Object, e As EventArgs) Handles menuAddLoan.Click
        Dim MySQL, strConn, sResult As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim ds As DataSet
        Dim iIndivid As Integer

        If fmAddLoan.ShowDialog = Windows.Forms.DialogResult.OK Then
            Cursor = Cursors.WaitCursor
            Application.DoEvents()

            strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
            MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
            MyConn.Open()
            Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)

            Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand
            Cmd.Connection = MyConn
            Cmd.Parameters.Clear()
            Cmd.CommandType = Data.CommandType.StoredProcedure
            Cmd.CommandText = "INSERT_NEW_LOAN"
            Cmd.Parameters.Add("@I_ACCOUNT_ID", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = AddBorrower.edEmail.Text
            Cmd.Parameters.Add("@I_BUSINESSNAME", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = fmAddLoan.edLoanName.Text

            Cmd.Transaction = Trans

            sResult = ""
            Try
                Cmd.ExecuteNonQuery()
                Trans.Commit()
                sResult = "OK"
            Catch ex As Exception
                Trans.Rollback()
                sResult = ex.Message
            End Try

            Cursor = Cursors.Default
        End If
    End Sub

    Private Sub menuUploadADocument_Click(sender As Object, e As EventArgs) Handles menuUploadADocument.Click

    End Sub


    Private Sub menuUploadDoc_Click(sender As Object, e As EventArgs)
        Dim sDesc, sFile As String
        Dim MyConn As FirebirdSql.Data.FirebirdClient.FbConnection
        Dim strConn, sSQL, s, sDocName, sExt As String
        Dim Reader As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim Trans As FirebirdSql.Data.FirebirdClient.FbTransaction
        Dim Cmd As FirebirdSql.Data.FirebirdClient.FbCommand
        Dim ftpRequest As FtpWebRequest
        Dim ftpResponse As FtpWebResponse
        Dim iMaxID, iFileType As Integer

        If iSelectedLoanID < 1 Then
            MsgBox("You must select a loan before adding a file to upload")
            Exit Sub
        End If

        If fmAddBorrowerDocSingle.ShowDialog = Windows.Forms.DialogResult.OK Then
            Cursor = Cursors.WaitCursor
            Application.DoEvents()

            sSQL = "select max(loan_file_id) as maxid from loan_files"

            strConn = System.Configuration.ConfigurationManager.ConnectionStrings("FBConnectionString").ConnectionString
            MyConn = New FirebirdSql.Data.FirebirdClient.FbConnection(strConn)
            MyConn.Open()

            Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)

            Reader = Cmd.ExecuteReader()

            If Reader.Read Then
                iMaxID = GenDB.fnDBIntField(Reader("maxid")) + 1
            Else
                iMaxID = 1
            End If

            Cmd = Nothing

            ' .................................................................................................................

            sDesc = fmAddBorrowerDocs.edDescription.Text
            sFile = fmAddBorrowerDocs.edFile.Text
            iFileType = fmAddBorrowerDocs.ddFileType.SelectedIndex

            sExt = Path.GetExtension(sFile)

            sDocName = GenDB.GetBorrowerDoc(iMaxID, iSelectedLoanID, sExt)

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

            sSQL = "insert into loan_files (description, filename, loanid, originalfilename, FileType) VALUES (@p1, @p2, @p3, @p4, @p5)"

            Cmd = New FirebirdSql.Data.FirebirdClient.FbCommand(sSQL, MyConn)
            Trans = MyConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Cmd.Transaction = Trans
            Cmd.Parameters.Clear()
            Cmd.Parameters.Add("p1", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sDesc
            Cmd.Parameters.Add("p2", FirebirdSql.Data.FirebirdClient.FbDbType.Char).Value = sDocName
            Cmd.Parameters.Add("p3", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iSelectedLoanID
            Cmd.Parameters.Add("p4", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = sFile
            Cmd.Parameters.Add("p5", FirebirdSql.Data.FirebirdClient.FbDbType.Integer).Value = iFileType

            Cmd.ExecuteReader()

            Try
                Trans.Commit()
            Catch ex As Exception
                Trans.Rollback()
            End Try

            MyConn.Close()
            MyConn = Nothing

            LoadFileDocs()

            Cursor = Cursors.Default
        End If


    End Sub

    Private Sub tvLoans_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs)

    End Sub

End Class
