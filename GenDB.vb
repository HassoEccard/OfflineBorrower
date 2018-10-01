Imports System.IO
Imports Microsoft.VisualBasic
Imports System.Net.Mail

Public Class GenDB

    Public Class UserAccount
        Public Description As String
        Public AccountID As Integer
        Public AccountType As Integer
        Public Rate As Integer
        Public Period As Integer
        Public Risk As Integer
    End Class

    ' ....................................................................................................
    ' Company name - Amount required - Term -  Star rating - Auction ends date - Raised so far
    Public Class PrimaryMktItem
        Public CompanyName As String
        Public AmountRequired As String
        Public Term As String
        Public StarRating As String
        Public AuctionEdsDate As String
        Public RaisedSoFar As String
    End Class

    Public Shared TheIniFile As New SortedList(Of String, String)

    Public Shared Accounts() As UserAccount

    Public Shared Current_Domain As String = "http://www.investandfund.com/"
    Public Shared Current_Secure_Domain As String = "http://www.investandfund.com/"
    Public Shared Email_Template_Path As String = "z:\iandf\templates\"

    Public Shared BankTable As DataTable



    ' ***********************************************************************************************************
    ' ***********************************************************************************************************
    ' ** Testing & development
    Public Shared TESTING_Unit As Integer = 0

    Public Shared FacilityFeeRate As Double = 0.0025   ' 0.25%

    Public Shared SESS_AccountArray As String = "AC_ARRAY"
    Public Shared SESS_CurrentAccountIdx As String = "AC_ACIDX"

    Public Shared SESS_Firstname As String = "USR_FN"
    Public Shared SESS_Lastname As String = "USR_LN"
    Public Shared SESS_UserID As String = "USR_ID"
    Public Shared SESS_UserType As String = "USR_TYPE"
    Public Shared SESS_AccountID As String = "ACC_ID"
    Public Shared SESS_LoanID As String = "LOAN_ID"
    Public Shared SESS_LoggedInStr As String = "LOGGEDIN"
    Public Shared GetStars As String = "TEMP_LOAN_ID"
    Public Shared SESS_TempOrderID As String = "TEMP_ORDER_ID"
    Public Shared SESS_TempLoanID As String = "TEMP_LOAN_ID"
    Public Shared SESS_TempOne As String = "TEMP_ONE"
    Public Shared SESS_TempTwo As String = "TEMP_TWO"
    Public Shared SESS_TempThree As String = "TEMP_THREE"
    Public Shared SESS_TempFour As String = "TEMP_FOUR"
    Public Shared SESS_TempFive As String = "TEMP_FIVE"
    Public Shared SESS_TempSix As String = "TEMP_SIX"
    Public Shared SESS_TempSeven As String = "TEMP_SEVEN"
    Public Shared SESS_352_1 As String = "352_1"

    Public Shared SESS_Bid_Amount As String = "BID_AMT"
    Public Shared SESS_Bid_Rate_High As String = "BID_HIGH"
    Public Shared SESS_Bid_Rate_Low As String = "BID_LOW"
    Public Shared SESS_Bid_Company As String = "BID_CO"
    Public Shared SESS_Bid_Ref As String = "BID_REF"

    Public Shared SESS_Login_Return As String = "LOGIN_RETURN"

    ' Current tax year
    Public Shared TaxYearStart As Date = "4/6/2013"
    Public Shared TaxYearEnd As Date = "4/5/2014"

    ' Logged in details
    Public Shared bLoggedInFlag As Boolean = False

    ' Loan statuses
    Public Shared lnPreIPO As Integer = 0
    Public Shared lnDuringIPO As Integer = 1
    Public Shared lnListed As Integer = 2
    Public Shared lnIPOFailed As Integer = 3
    Public Shared lnComplete As Integer = 4

    ' Loan types
    Public Shared lntypeNormal As Integer = 0
    Public Shared lntypeInterestOnlyFixedRate As Integer = 1
    Public Shared lntypeInterestOnlyVarRate As Integer = 2

    ' Order Types
    Public Shared orLoanOffer As Integer = 0
    Public Shared orSubscription As Integer = 1
    Public Shared orBuy As Integer = 2
    Public Shared orSell As Integer = 3
    Public Shared orAutoBuy As Integer = 4
    Public Shared orAutoSell As Integer = 5
    Public Shared orGrossLoan As Integer = 6   'The total amount that a borrower will repay

    ' General
    Public Shared Bid_Expiry_24_Clock As String = "21.00 hr"

    ' Equifax
    Public Shared eqBusinessCharge As Integer = 5000    ' 5000 pence = #50

    ' Timeout pages
    Public Shared TimeOutFull As String = "500.aspx"
    Public Shared TimeOutMin As String = "510.aspx"

    ' DBE -  ' 0-No Change  1-Updated  2-New  3-Delete
    Public Shared RateDecrementAmount As Integer = 10
    Public Shared biditemChangeStatus_None As Integer = 0
    Public Shared biditemChangeStatus_Updated As Integer = 1
    Public Shared biditemChangeStatus_Inserted As Integer = 2
    Public Shared biditemChangeStatus_Deactivated As Integer = 3

    Public Shared userUndefined As Integer = -1
    Public Shared userInvestor As Integer = 0
    Public Shared userBorrower As Integer = 1

    ' Pages
    Public Shared Main_Login_Page As String = "01-01.aspx"

    ' System account numbers
    Public Shared acSuspense As Integer = 10
    Public Shared acBank As Integer = 20
    Public Shared acIandF As Integer = 30

    ' System alerts
    Public Shared alertBorrowerEquifaxWaiting As Integer = 0
    Public Shared alertInvestorFailedEquifaxAMLTwice As Integer = 1
    Public Shared alertRequestAlternateStartDate As Integer = 2
    Public Shared alertWithdrawalRequest As Integer = 3
    Public Shared alertInvestorEquifaxFailedToConnect As Integer = 4
    Public Shared alertBorrowerEquifaxFailedToConnect As Integer = 5
    Public Shared alertBorrowerEquifaxAssessmentWaiting As Integer = 6

    ' Financial transactions
    Public Shared ft_Secondary_Buy As Integer = 0
    Public Shared ft_Secondary_Sell As Integer = 1
    Public Shared ft_Deposit_Received As Integer = 2
    Public Shared ft_User_Account_Credit As Integer = 3
    Public Shared ft_Loan_Repayment_Capital As Integer = 4
    Public Shared ft_Subscribe_Investor As Integer = 6
    Public Shared ft_Subscribe_Complete_Investor As Integer = 7
    Public Shared ft_Loan_Offer_Borrower As Integer = 8
    Public Shared ft_Loan_Complete_Borrower As Integer = 9
    Public Shared ft_Withdrawal As Integer = 10
    Public Shared ft_Withdrawal_Request As Integer = 11
    Public Shared ft_Loan_Repayemt_Interest As Integer = 12   ' Investor receives
    Public Shared ft_Borrower_Draws_Down_Funds As Integer = 13
    Public Shared ft_Borrower_Repayment As Integer = 14

    Public Shared hv_030_01 As String = "onmouseover=""doHint(event, 'Deposit Funds. This tooltip can be modified on line 75.');"" onmouseout=""hideHint();"" "

    Public Shared Sub GetReverseAccruedCalc(ByVal fTotal As Double, dStartDate1 As Date, dEndDate1 As Date, iRate As Integer, ByRef RtnCapital As Double, ByRef RtnInterest As Double)
        Dim OrigCap, OrigAcc, OrigTotal As Double
        Dim CapRatio, AccRatio As Double
        Dim NewTotal, NewCapital, NewAcc As Double


        OrigCap = 10000000
        OrigAcc = GetAccruedInterest(OrigCap, dStartDate1, dEndDate1, iRate)
        OrigTotal = OrigAcc + OrigCap

        CapRatio = OrigCap / OrigTotal
        AccRatio = OrigAcc / OrigTotal

        '.....................................
        ' .. Now do the new calc
        NewTotal = fTotal

        NewCapital = NewTotal * CapRatio
        NewAcc = NewTotal * AccRatio

        RtnCapital = NewCapital
        RtnInterest = NewAcc
    End Sub


    ' ****************************************************************************************
    ' ** iRate is in the IandF format i.e. the percentage multiplied by 100
    Public Shared Function GetAccruedInterest(iCapital As Integer, dStartDate1 As Date, dEndDate1 As Date, iRate As Integer) As Integer
        Dim iNumDays As Integer
        Dim iFirstDayCounter, iLastDayCounter, iDaysInPeriod, iTotal As Integer
        Dim iAmt, rCapital, rRate As Double
        Dim TempDateFirst, TempDateLast, tempDate As Date

        Dim dStartDate = New Date(dStartDate1.Year, dStartDate1.Month, dStartDate1.Day, 0, 0, 1)
        Dim dEndDate = New Date(dEndDate1.Year, dEndDate1.Month, dEndDate1.Day, 23, 59, 59)

        TempDateLast = dStartDate.AddYears(1)
        iFirstDayCounter = 1
        iLastDayCounter = (TempDateLast - dStartDate).Days

        rCapital = iCapital
        rRate = iRate / 10000

        iNumDays = (dEndDate - dStartDate).Days

        If iNumDays < iLastDayCounter Then
            iAmt = ((iCapital * (iRate / 10000)) / 365) * iNumDays
            GetAccruedInterest = iAmt
            Exit Function
        End If

        iDaysInPeriod = (TempDateLast - dStartDate).Days

        iTotal = iCapital
        iAmt = 0
        While iFirstDayCounter <= iLastDayCounter
            iAmt = (iTotal * rRate) * (iDaysInPeriod / 365)
            iTotal += Math.Ceiling(iAmt)

            'tempDate = TempDateLast.AddDays(1)
            tempDate = TempDateLast
            TempDateLast = tempDate.AddYears(1)
            TempDateFirst = tempDate
            If TempDateLast > dEndDate Then
                TempDateLast = dEndDate
            End If

            iDaysInPeriod = (TempDateLast - TempDateFirst).Days ' the dates are inclusive
            iFirstDayCounter = iLastDayCounter + 1
            iLastDayCounter = (TempDateLast - dStartDate).Days
        End While

        GetAccruedInterest = Math.Round(iTotal - iCapital)

    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetSecondaryBuyTotal(iPrincipal As Double, iPremiumPercent As Double) As Integer
        Dim i As Double

        i = iPrincipal + ((iPrincipal) * (iPremiumPercent / 10000))
        'i = i + (i * FacilityFeeRate)

        GetSecondaryBuyTotal = Math.Round(i)
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetSecondaryBuyFacilityFee(iPrincipal As Integer, iPremiumAmount As Integer) As Integer
        'Dim i As Integer
        Dim j As Double

        j = iPrincipal + iPremiumAmount
        j = j * FacilityFeeRate


        GetSecondaryBuyFacilityFee = Math.Ceiling(j)
    End Function

    ' ****************************************************************************************
    ' ** iRate is in the IandF format i.e. the percentage multiplied by 100
    Public Shared Function GetAmountFromInterestRate365(iCapital As Long, dStartDate As Date, dEndDate As Date, iRate As Integer) As Integer
        Dim iNumDays As Integer
        Dim iAmt As Double

        iNumDays = dEndDate.Subtract(dStartDate).Days

        iAmt = (iCapital * (iRate / 10000)) / 365.25

        iAmt = iAmt * iNumDays

        GetAmountFromInterestRate365 = iAmt
    End Function

    Public Shared Function GetStr(ByVal sField) As String
        Dim s As String
        If IsDBNull(sField) Then
            s = "Not logged in"
        Else
            s = Trim(CStr(sField))
        End If
        If Trim(s) = "" Then
            s = "Not logged in"
        End If
        GetStr = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnGetFileNameFromPath(ByVal strPath As String) As String
        Do Until InStr(strPath, "\") = 0
            strPath = Mid(strPath, InStr(strPath, "\") + 1)
        Loop
        fnGetFileNameFromPath = strPath
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnGetFileNameFromURL(ByVal strURL As String) As String
        Dim filename As String()
        filename = strURL.Split("/")
        fnGetFileNameFromURL = filename(UBound(filename))
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnAddAddressSingleLine(ByVal sAdd1 As String, ByVal sAdd2 As String, _
                                            ByVal sAdd3 As String, ByVal sAdd4 As String, _
                                            ByVal sTown As String, ByVal sCounty As String, _
                                            ByVal sCountry As String, ByVal sPostCode As String) As String
        Dim s As String
        Dim l, j As Integer

        s = ""
        If Trim(sAdd1) <> "" Then
            s = s & sAdd1 & ", <br>"
        End If
        If Trim(sAdd2) <> "" Then
            s = s & sAdd2 & ", <br>"
        End If
        If Trim(sAdd3) <> "" Then
            s = s & sAdd3 & ", <br>"
        End If
        If Trim(sAdd4) <> "" Then
            s = s & sAdd4 & ", <br>"
        End If
        If Trim(sTown) <> "" Then
            s = s & sTown & ", <br>"
        End If
        If Trim(sCounty) <> "" Then
            s = s & sCounty & ", <br>"
        End If
        If Trim(sCountry) <> "" Then
            s = s & sCountry & ", <br>"
        End If
        If Trim(sPostCode) <> "" Then
            s = s & sPostCode & ", <br>"
        End If

        l = s.Length

        If l > 6 Then
            j = InStr(l - 3, s, ", ")
            If j > 1 Then
                s.Remove(j - 1, 2)
            End If
        End If

        l = s.Length
        If l > 0 Then
            j = InStr(l - 1, s, ",")
        End If

        If j > 0 Then
            s.Remove(j - 1, 1)
        End If

        fnAddAddressSingleLine = s
    End Function

    ' ****************************************************************************************
    ' **
    Public Shared Function Log_File() As String
        'Dim s As String = System.Configuration.ConfigurationManager.AppSettings("LogDir")

        'Return s & Now.ToString("yyyyMMdd") & ".log"
    End Function

    ' ****************************************************************************************
    ' **
    Public Shared Function Log_Equifax_Investor_File(ByVal iAccID As Integer) As String
        ' Dim s As String = System.Configuration.ConfigurationManager.AppSettings("LogEqInv")

        ' Return s & Now.ToString("yyyyMMdd-HH-mm-ss-fff" & iAccID.ToString) & ".log"
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function reader(ByVal sql As String, fb As FirebirdSql.Data.FirebirdClient.FbConnection) As FirebirdSql.Data.FirebirdClient.FbDataReader
        Dim cm As New FirebirdSql.Data.FirebirdClient.FbCommand

        cm.Connection = fb
        cm.CommandText = sql
        reader = cm.ExecuteReader
    End Function

    ' ****************************************************************************************
    ' iType - 1 = 10th Mar 2005
    '       - 2 = Mar 2005
    Public Shared Function fnDBDateFieldString(ByVal sField, ByVal iType) As String

        fnDBDateFieldString = " "
        If IsDBNull(sField) Then
            fnDBDateFieldString = " "
        Else
            Select Case iType
                Case 1 : fnDBDateFieldString = Format(sField, "d MMM yyyy")
                Case 2 : fnDBDateFieldString = Format(sField, "MMM yyyy")
            End Select
        End If
    End Function

    ' ****************************************************************************************
    ' 
    ' Star    Scorecheck      Equifax Ratings   Bad Debt % (derived weighted av)
    ' 5*        100 - 87          A+   A                              0.031%
    ' 4*         86 - 79          A     A-    B+                     0.038%
    ' 3*         78 - 71          B+   B                               0.086%
    ' 2*         70 - 63          B     B-     C+                     0.097%
    ' 1*         62 - 55          C+   C                               0.128%
    ' 0          <55              C- to Z                  Not accepted on exchange

    Public Shared Function GetStarsFromRisk(ByVal sField As String, ByRef StarInt As Integer) As String
        Dim sStar As String
        Dim iRisk As Double

        If IsDBNull(sField) Then
            iRisk = 0
        Else
            Try
                iRisk = CInt(sField)
            Catch ex As Exception
                iRisk = 0
            End Try
        End If

        sStar = ""
        StarInt = 0
        Select Case iRisk
            Case 55 To 62
                sStar = "*"
                StarInt = 1
            Case 63 To 70
                sStar = "**"
                StarInt = 2
            Case 71 To 78
                sStar = "***"
                StarInt = 3
            Case 79 To 86
                sStar = "****"
                StarInt = 4
            Case 87 To 100
                sStar = "*****"
                StarInt = 5
        End Select

        GetStarsFromRisk = sStar
    End Function

    ' ****************************************************************************************
    ' *
    Public Shared Function GetStarsFromStarInt(ByVal StarInt As Integer) As String
        Dim sRes As String

        sRes = ""
        Select Case StarInt
            Case 1
                sRes = "*"
            Case 2
                sRes = "**"
            Case 3
                sRes = "***"
            Case 4
                sRes = "****"
            Case 5
                sRes = "*****"
        End Select

        GetStarsFromStarInt = sRes
    End Function

    ' ****************************************************************************************
    '  Remember that it must be multiplied by 100
    Public Shared Function GetFloorFromRisk(ByVal iRisk As Integer) As Integer
        Dim iRes As Integer

        Select Case iRisk
            Case 0 To 20
                iRes = 1100
            Case 21 To 40
                iRes = 900
            Case 41 To 60
                iRes = 700
            Case 61 To 80
                iRes = 500
            Case Else
                iRes = 300
        End Select

        GetFloorFromRisk = iRes
    End Function

    ' ****************************************************************************************
    '  Remember that it must be multiplied by 100
    ' 5* 3%, 4* 5%, 3* 7%, 2* 9%, 1* 11%).
    Public Shared Function GetFloorFromStarRating(ByVal iNumStars As Integer) As Integer
        Dim iRes As Integer

        Select Case iNumStars
            Case 5
                iRes = 300
            Case 4
                iRes = 500
            Case 3
                iRes = 700
            Case 2
                iRes = 900
            Case 1
                iRes = 1100
        End Select

        GetFloorFromStarRating = iRes
    End Function


    Public Shared Function GetLoanStatus(iStatus As Integer) As String
        Dim s As String

        '0-Pre-IPO   1-During IPO   2-Listed   3-IPO Failed  4-Complete  5-Cancelled before IPO 6-Suspended (only during auction)  7-Default  8-Cancelled during IPO  9-Extended  10-Holding
        s = ""
        Select Case iStatus
            Case 0
                s = "Pre Auction"
            Case 1
                s = "During Auction"
            Case 2
                s = "Listed"
            Case 3
                s = "Auction Failed"
            Case 4
                s = "Loan Complete"
            Case 5
                s = "Cancelled before Auction"
            Case 6
                s = "Suspended during Auction"
            Case 7
                s = "Default"
            Case 8
                s = "Cancelled during Auction"
            Case 9
                s = "Extended"
            Case 10
                s = "Holding"
        End Select

        GetLoanStatus = s
    End Function



    ' ****************************************************************************************
    ' 
    Public Shared Function CurrencyStringPoundsToPence(ByVal sField) As Integer
        Dim d As Double
        Dim i, rVal As Integer
        Dim s As String

        If IsDBNull(sField) Then
            rVal = 0
        Else
            Try
                s = sField.ToString
                i = InStr(s, "£")
                If i > 0 Then
                    s = s.Remove(i - 1, 1)
                    Trim(s)
                End If

                If Double.TryParse(s, d) Then
                    rVal = CInt(d * 100)
                Else
                    rVal = 0
                End If
            Catch ex As Exception
                rVal = 0
            End Try
        End If
        CurrencyStringPoundsToPence = rVal
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function DisplayToCurrency(ByVal sField) As Double
        Dim i As Integer
        Dim rVal As Double

        If IsDBNull(sField) Then
            rVal = 0
        Else
            Try
                i = CurrencyStringPoundsToPence(sField)
                rVal = i / 100
            Catch ex As Exception
                rVal = 0
            End Try
        End If
        DisplayToCurrency = rVal
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetRateOfInterestAsString(ByVal sField) As String

        Dim iRate, iPence As Double

        If IsDBNull(sField) Then
            iRate = 0.0
        Else
            Try
                iPence = CInt(sField)
                iRate = iPence / 100
            Catch ex As Exception
                iRate = 0.0
            End Try
        End If

        GetRateOfInterestAsString = Format(iRate, "#0.00") & "%"
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function DisplayToInterestRate(ByVal sField) As Double
        Dim d As Double
        Dim i As Integer
        Dim rVal As Double
        Dim s As String

        If IsDBNull(sField) Then
            rVal = 0
        Else
            Try
                s = sField.ToString
                i = InStr(s, "%")
                If i > 0 Then
                    s = s.Remove(i - 1)
                    Trim(s)
                End If

                If Double.TryParse(s, d) Then
                    rVal = CInt(d * 100)
                Else
                    rVal = 0
                End If
            Catch ex As Exception
                rVal = 0
            End Try
        End If
        DisplayToInterestRate = rVal
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function PenceToCurrencyStringPounds(ByVal sField) As String
        Dim rVal As Double
        Dim iPence As Integer

        If IsDBNull(sField) Then
            rVal = 0.0
        Else
            Try
                iPence = CInt(sField)
                rVal = iPence / 100
            Catch ex As Exception
                rVal = 0.0
            End Try
        End If
        PenceToCurrencyStringPounds = "£" & Format(rVal, "###,###,##0.00")
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnDBStringFieldCommas(ByVal sField) As String
        If IsDBNull(sField) Then
            fnDBStringFieldCommas = " "
        Else
            fnDBStringFieldCommas = Format(sField, "###,###,###,##0")
        End If
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnDBStringField(ByVal sField) As String
        If IsDBNull(sField) Then
            fnDBStringField = " "
        Else
            fnDBStringField = Trim(CStr(sField))
        End If
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnDBIntField(ByVal sField) As Integer
        Try
            If IsDBNull(sField) Then
                fnDBIntField = 0
            Else
                fnDBIntField = CInt(sField)
            End If
        Catch ex As Exception
            fnDBIntField = 0
        End Try
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnDBIntField(ByVal sField, ByVal iDef) As Integer
        Dim iDefault As Integer

        If Not isNumeric(iDef) Then
            iDefault = 1
        Else
            iDefault = iDef
        End If

        Try
            If IsDBNull(sField) Then
                fnDBIntField = iDefault
            Else
                fnDBIntField = CInt(sField)
            End If
        Catch ex As Exception
            fnDBIntField = iDefault
        End Try
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnDBDoubleField(ByVal sField) As Double
        Try
            If IsDBNull(sField) Then
                fnDBDoubleField = 0.0
            Else
                fnDBDoubleField = CDbl(sField)
            End If
        Catch ex As Exception
            fnDBDoubleField = 0.0
        End Try
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnDBDateField(ByVal sField) As DateTime
        If IsDBNull(sField) Then
            fnDBDateField = DateTime.MinValue
        Else
            fnDBDateField = CDate(sField)
        End If
    End Function

    ' ****************************************************************************************
    ' New SmtpClient("mail.investandfund.com")
    ' System.Net.NetworkCredential("admin@investandfund.com", "hastings001")
    '
    Public Shared Function SendSimpleMail(sEmail As String, sSubject As String, sBody As String) As String
        Dim s As String
        Dim MyMailMessage As MailMessage = New MailMessage()
        MyMailMessage.From = New MailAddress("registrations@iandfmailer.com")
        MyMailMessage.To.Add(sEmail)
        MyMailMessage.Subject = sSubject
        MyMailMessage.IsBodyHtml = True

        MyMailMessage.Body = "<table><tr><td>" + sBody + "</table></td></tr>"

        'Dim SMTPServer As SmtpClient = New SmtpClient("rsj30.rhostjh.com")
        Dim SMTPServer As SmtpClient = New SmtpClient("mail.iandfmailer.com")
        'SMTPServer.Port = 465
        SMTPServer.Port = 25
        SMTPServer.Credentials = New System.Net.NetworkCredential("registrations@iandfmailer.com", "register,1980")
        SMTPServer.EnableSsl = False

        s = ""
        Try
            SMTPServer.Send(MyMailMessage)
            'Response.Redirect("Thankyou.aspx")
        Catch ex As Exception
            s = ex.Message
        End Try

        SMTPServer = Nothing
        MyMailMessage = Nothing

        SendSimpleMail = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnInterestRateToPercent(ByVal sField) As String
        Dim iVal As Integer
        Dim fVal As Double
        Dim sRes As String

        If IsDBNull(sField) Then
            fnInterestRateToPercent = "0.00%"
        Else
            iVal = CInt(sField)
            fVal = iVal / 100
            sRes = String.Format("{0:f2}", fVal)
            fnInterestRateToPercent = sRes & "%"
        End If
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function fnInterestRateToPercentOne(ByVal sField) As String
        Dim iVal As Integer
        Dim fVal As Double
        Dim sRes As String

        If IsDBNull(sField) Then
            fnInterestRateToPercentOne = "0.00%"
        Else
            iVal = CInt(sField)
            fVal = iVal / 100
            sRes = String.Format("{0:f1}", fVal)
            fnInterestRateToPercentOne = sRes & "%"
        End If
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetTaxYearStr(ByVal TheDate As Date) As String
        Dim iMon, iDay, iStartYear, iEndYear As Integer

        iMon = TheDate.Month
        iDay = TheDate.Day

        iStartYear = TheDate.Year
        If iMon < 4 Then
            iStartYear = TheDate.Year - 1
        End If

        If iMon = 4 And iDay < 6 Then
            iStartYear = TheDate.Year - 1
        End If

        iEndYear = iStartYear + 1

        GetTaxYearStr = iStartYear.ToString & " / " & iEndYear.ToString
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetSeqAlpha(ByVal sLastname As String) As String
        Dim s As String
        Dim i As Integer

        s = Replace(sLastname, " ", "")
        s = Replace(s, "&", "")
        s = Replace(s, ".", "")
        s = Replace(s, ",", "")
        s = Replace(s, ":", "")
        s = Replace(s, ";", "")
        s = Replace(s, "?", "")
        s = Replace(s, "'", "")
        s = Replace(s, """", "")
        s = Replace(s, "%", "")
        s = Replace(s, "#", "")
        s = Replace(s, "|", "")
        s = Replace(s, "\", "")
        s = Replace(s, "/", "")
        s = Replace(s, "!", "")
        s = Replace(s, "$", "")
        s = Replace(s, "*", "")
        s = Replace(s, "^", "")
        s = Replace(s, "@", "")
        s = Replace(s, "+", "")
        s = Replace(s, "-", "")
        s = Replace(s, "=", "")
        s = Replace(s, "_", "")
        s = Replace(s, ")", "")
        s = Replace(s, "(", "")
        s = Replace(s, "]", "")
        s = Replace(s, "[", "")
        s = Replace(s, "{", "")
        s = Replace(s, "}", "")
        s = Replace(s, ">", "")
        s = Replace(s, "<", "")

        s = Trim(s)
        If s.Length > 3 Then
            s = s.Remove(3, s.Length - 3)
        End If

        If s.Length < 3 Then
            For i = 1 To 3 - s.Length
                s = s & "X"
            Next
        End If
        GetSeqAlpha = s.ToUpper
    End Function ' GetSeqAlpha

    ' ****************************************************************************************
    ' 
    Public Shared Function GetTagValue(ByVal sTag As String, ByVal MainString As String, ByVal sDelimiter As String) As String
        Dim i, j, TagLen As Integer
        Dim s As String
        Dim s1(120) As Char

        s = ""
        i = InStr(MainString, sTag)
        TagLen = sTag.Length

        If i > 0 Then
            i = i + TagLen + 1
            j = InStr(i + 1, MainString, sDelimiter) - 1

            s = MainString.Substring(i, j - i)
        End If
        s = Trim(s)
        Return s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetCSV(ByVal dg As DataGridView) As String
        Dim table = TryCast(dg.DataSource, DataTable)
        Dim s As String

        s = ""
        For Each row As DataRow In table.Rows
            For Each column As DataColumn In table.Columns
                s = s & fnDBStringField(row(column)) & ", "
            Next
            s = s & vbNewLine
        Next

        GetCSV = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetCSVPipe(ByVal dg As DataGridView) As String
        Dim table = TryCast(dg.DataSource, DataTable)
        Dim s, s1 As String

        s = ""
        For Each row As DataRow In table.Rows
            For Each column As DataColumn In table.Columns
                s1 = fnDBStringField(row(column))
                s = s & Trim(s1) & "|"
            Next
            s = s & vbNewLine
        Next

        GetCSVPipe = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetCSVFieldNumber(ByVal i As Integer, ByVal sLine As String) As String
        Dim iCount, j As Integer
        Dim iCharCount, iFirst, iLast As Integer
        Dim InQuotes As Boolean
        Dim bfound As Boolean
        Dim sQuo As String

        sQuo = Chr(34)
        iCount = 0
        iCharCount = 0
        bfound = False
        While Not bfound
            iFirst = 0
            iLast = 0
            InQuotes = False

            If i = 1 Then
                If sLine.Chars(iCharCount) = sQuo Then
                    iFirst = 1
                    iCharCount = 1
                Else
                    iFirst = 0
                End If
                iCharCount = iCharCount + 1
            Else ' find first comma
                While iCharCount < sLine.Length And iCount < i - 1
                    If sLine.Chars(iCharCount) = sQuo Then
                        If InQuotes Then
                            InQuotes = False
                        Else
                            InQuotes = True
                        End If
                    End If
                    If sLine.Chars(iCharCount) = "," Then
                        If Not InQuotes Then
                            iCount = iCount + 1
                        End If
                    End If
                    iCharCount = iCharCount + 1
                End While
                iFirst = iCharCount
                bfound = True
            End If
            ' Now find second

            'iCount = 0
            While iCharCount < sLine.Length And iCount < i
                If sLine.Chars(iCharCount) = "," Then
                    If Not InQuotes Then
                        iCount = iCount + 1
                    End If
                Else
                    If sLine.Chars(iCharCount) = sQuo Then
                        If InQuotes Then
                            InQuotes = False
                        Else
                            InQuotes = True
                        End If
                    End If
                End If
                iCharCount = iCharCount + 1
            End While
            iLast = iCharCount - 1
            bfound = True
        End While

        Dim s, s1 As String
        s = ""
        For i = iFirst To iLast
            If i = iLast Then
                If sLine.Chars(i) <> "," Then
                    s = s & sLine.Chars(i)
                End If
            Else
                s = s & sLine.Chars(i)
            End If
        Next

        s = Trim(s)
        s1 = ""
        j = s.Length
        For i = 0 To j - 1
            If (i = 0) Or (i = j - 1) Then
                If s.Chars(i) <> sQuo Then
                    s1 = s1 & s.Chars(i)
                End If
            Else
                s1 = s1 & s.Chars(i)
            End If
        Next

        s = ""
        j = s1.Length
        For i = 0 To j - 1
            If (i = 0) Then
                If s1.Chars(i) <> "'" Then
                    s = s & s1.Chars(i)
                End If
            Else
                s = s & s1.Chars(i)
            End If
        Next

        GetCSVFieldNumber = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function isNumeric(ByVal cChr As String) As Boolean
        Dim i As Integer
        Dim isNm As Boolean

        If cChr.Length = 0 Then
            isNumeric = False
            Exit Function
        End If

        isNm = True
        For i = 0 To cChr.Length - 1
            If Not Char.IsNumber(cChr.Chars(i)) Then
                isNm = False
            End If
        Next

        isNumeric = isNm
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function isAlpha(ByVal cChr As String) As Boolean
        Dim i As Integer
        Dim isAl As Boolean

        If cChr.Length = 0 Then
            isAlpha = False
            Exit Function
        End If

        isAl = True
        For i = 0 To cChr.Length - 1
            If Not Char.IsLetter(cChr.Chars(i)) Then
                isAl = False
            End If
        Next

        isAlpha = isAl
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function CopyStr(ByVal sStr As String, ByVal iStart As Integer, ByVal iLength As Integer) As String
        Dim s As String
        Dim i, j As Integer

        s = ""
        For i = iStart To iStart + iLength - 1
            If i <= sStr.Length Then
                s = s & sStr.Chars(i - 1)
            End If
        Next

        CopyStr = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetIandFRef(ByVal sRef As String, ByRef AccType As Integer) As Integer
        Dim sStr() As String
        Dim iCount, i, iIdx, iTemp As Integer
        Dim s1, s2 As String

        sRef = sRef.Replace(".", " ")
        sStr = Split(sRef, " ", )
        iIdx = -1
        AccType = -1

        For iCount = 0 To sStr.Count - 1
            If sStr(iCount).Length = 10 Then ' could be one
                s1 = CopyStr(sStr(iCount), 1, 3)
                If isAlpha(s1) Then
                    s2 = CopyStr(sStr(iCount), 4, 7)
                    If isNumeric(s2) Then ' FoundIt!
                        iIdx = CInt(s2) - 100000
                        AccType = GenDB.userInvestor
                    End If
                Else
                    s1 = CopyStr(sStr(iCount), 1, 7)
                    If isNumeric(s1) Then
                        s2 = CopyStr(sStr(iCount), 8, 3)
                        If isAlpha(s2) Then
                            AccType = GenDB.userBorrower
                            iTemp = CInt(s1)
                            If iTemp > 9000000 Then
                                iIdx = iTemp - 9000000
                            Else
                                iIdx = iTemp - 900000
                            End If
                        End If
                    End If

                End If
            Else
                If sStr(i).Length = 7 Then ' could be one - excluding the three letters
                    If isNumeric(sStr(iCount)) Then ' Found !
                        i = CInt(sStr(iCount))
                        If i > 900000 Then
                            iIdx = i - 900000
                        Else
                            iIdx = i - 100000
                        End If
                    End If
                End If
            End If
        Next

        GetIandFRef = iIdx

    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetUserTypeFromTypeID(ByVal iUserType As Integer) As String
        Dim s As String

        s = ""

        Select Case iUserType
            Case 0
                s = "Investor"
            Case 1
                s = "Borrower"
        End Select

        GetUserTypeFromTypeID = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetFinTrans(ByVal iFinTransID As Integer) As String
        Dim s As String

        s = ""
        Select Case iFinTransID
            Case 1000
                s = " IandF Account money in from outside"
            Case 1001
                s = " IandF Account money out to real world user "
            Case 1002
                s = "IandF Account money to a user's account"
            Case 1003
                s = "IandF Account money transfer from user for withdrawal"
            Case 1100
                s = "Account deposit" ' -  Investor from outside world i.e. from IandF Account (Cr) - 1002"
            Case 1101
                s = "Account repayment" '  - (deposit) borrower from outside world from IandF Account (Cr)"
            Case 1102
                s = "Withdrawal" '  - M Investor account withdrawal request (to IandF account, for payment out) (Dr)"
            Case 1103
                s = "Withdrawal" '  - M Borrower account withdrawal request (after successful auction) (to IandF account, for payment out)  (Dr)"
            Case 1200
                s = "Bid" ' - ' M Investor bids account debit (Dr)"
            Case 1201
                s = "Suspense adjustment" ' - ' S Investor bids suspense credit (Cr)"
            Case 1203
                s = "Cancelled auction" ' - M Investor main account credit, reversing out the bid, after failed auction (Cr)"
            Case 1204
                s = "Cancelled auction" ' - M Investor suspense account debit bid after failed auction (Dr)"
            Case 1205
                s = "Loan transfer " 'M Borrower account receives whole loan amount from suspense on selection to DrawDown (after successful auction) (Cr) - see 1208"
            Case 1206
                s = "Auction complete" ' - S Investor suspense pays to IandF on successful auction (DR)    - see 1207"
            Case 1207
                s = "Receipt from investor on successful auction" ' - M IandF receives from Investor Suspense on successful auction (CR)  - see 1206 "
            Case 1208
                s = "Transfer to borrower in successful auction" ' - M IandF pays to Borrower on successful auction (DR)  - see 1205"
            Case 1209
                s = "Arrangement Fee" ' to I and F - M Borrower pays arrangement fee to IandF on drawdown (DR)"
            Case 1210
                s = "Arrangement fee from borrower" ' - M IandF receives arrangement fee from borrower on successful drawdown (CR)"
            Case 1300
                s = "Capital paid" ' - M Borrower pays each investor from repayment - capital (Dr)"
            Case 1301
                s = "Capital on loan " ' xxxxx - M Investor receives repayment - capital (Cr)"
            Case 1302
                s = "Interest paid" ' - M Borrower pays each investor from repayment - interest (Dr)"
            Case 1303
                s = "Interest on loan " 'xxxxx - M Investor receives repayment - interest(Cr)"
            Case 1304
                s = "Facility Fee" ' - M Investor pays facility fee to IandF (Dr)"
            Case 1305
                s = "M IandF receives investor facility Fee (Cr)"
            Case 1308
                s = "Interest Purchased" ' - M Investor purchased (Dr)"
            Case 1400
                s = "Sell of " 'xxxxx - M Secondary sell seller (Cr)"
            Case 1401
                s = "Buy of " 'xxxxx - M Secondary buy buyer (Dr)"
            Case 1402
                s = "Premium paid" ' - M Secondary buyer pays premium (Dr)"
            Case 1403
                s = "Premium received" ' - M Secondary seller receives premium (Cr)"
            Case 1404
                s = "Premium received" ' - M Secondary buyer receives premium (Cr)"
            Case 1405
                s = "Premium paid" ' - M Secondary seller pays premium (Dr)"
            Case 1406
                s = "Transaction fee" ' - M Secondary buyer pays transaction fee to IandF (Dr)"
            Case 1407
                s = "Transaction Fee" ' from  xxxxx - M Secondary IandF receives transaction fee from buyer (Cr)"
            Case 1408
                s = "Accrued interest" ' - M Secondary buyer pays accrued interest to seller (Dr)"
            Case 1409
                s = "Accrued interest" ' - M Secondary seller receives accrued interest from buyer (Cr)"
            Case 1410
                s = "Interest" ' - M Secondary seller receives accrued interest from buyer (Cr)"
            Case 1411
                s = "Interest" ' - M Secondary seller receives accrued interest from buyer (Cr)"
            Case 2304
                s = "Facility fee adjustment"
            Case 2305
                s = "Facility fee adjustment"
                'Case 0
                '    s = "Secondary Buy" '(invs = "inv)                           
                'Case 1
                '    s = "Secondary Sell" '(invs = "inv)
                'Case 2
                '    s = "IandF Client Account Credit"
                'Case 3
                '    s = "User account credit"
                'Case 4
                '    s = "Loan repayment - capital"
                'Case 5
                '    s = "Loan repayemt - interest"
                'Case 6
                '    s = "Bid"
                'Case 7
                '    s = "Completion"
                'Case 8
                '    s = "Loan offer" ' (Borrower IPO start)              
                'Case 9
                '    s = "Loan repayment" ' (IandF repayment account (transtype 2)s = "> Borrower account )
                'Case 10
                '    s = "IandF Payaway" ' (IandF holding s = "> outside)           
                'Case 11
                '    s = "Withdrawal request" ' (user/ac s = "> IandF holding)
                'Case 12
                '    s = "Facility Fee"
                'Case 13
                '    s = "Borrower draws down funds"
                'Case 14
                '    s = "Subscribe Cancellation" ' (suspenses = ">investor)     
                'Case 15
                '    s = "Premium"
        End Select

        GetFinTrans = s
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetInvestorRef(ByVal sAlpha As String, ByVal iInt As Integer) As String

        GetInvestorRef = sAlpha & Format(iInt + 100000, "0000000")
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetBorrowerRef(ByVal sAlpha As String, ByVal iInt As Integer) As String
        GetBorrowerRef = Format(iInt + 9000000, "0000000") & sAlpha
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Sub SaveFormToIni(sForm As Form)
        Dim sFormName, sKey, sValue As String

        sFormName = sForm.Name

        sKey = sFormName & ":x"
        sValue = sForm.Left.ToString
        SaveValuePair(sKey, sValue)

        sKey = sFormName & ":y"
        sValue = sForm.Top.ToString
        SaveValuePair(sKey, sValue)

        sKey = sFormName & ":width"
        sValue = sForm.Width.ToString
        SaveValuePair(sKey, sValue)

        sKey = sFormName & ":height"
        sValue = sForm.Height.ToString
        SaveValuePair(sKey, sValue)
    End Sub

    ' ****************************************************************************************
    ' 
    Public Shared Sub LoadFormIni(ByRef sForm As Form)
        Dim sFormName, sKey, sValue As String
        Dim i As Integer

        Exit Sub

        sFormName = sForm.Name

        sKey = sFormName & ":x"
        If ReadValuePair(sKey, sValue) Then
            i = GenDB.fnDBIntField(sValue)
            sForm.Left = i
        End If

        sKey = sFormName & ":y"
        If ReadValuePair(sKey, sValue) Then
            i = GenDB.fnDBIntField(sValue)
            sForm.Top = i
        End If

        sKey = sFormName & ":width"
        If ReadValuePair(sKey, sValue) Then
            i = GenDB.fnDBIntField(sValue)
            sForm.Width = i
        End If

        sKey = sFormName & ":height"
        If ReadValuePair(sKey, sValue) Then
            i = GenDB.fnDBIntField(sValue)
            sForm.Height = i
        End If

        If sForm.Left < 0 Then
            sForm.Left = 0
        End If

        If sForm.Left > 200 Then
            sForm.Left = 0
        End If

        If sForm.Top > 200 Then
            sForm.Top = 0
        End If

        If sForm.Top < 0 Then
            sForm.Top = 0
        End If
    End Sub

    ' ****************************************************************************************
    ' 
    Public Shared Sub LoanIniFile()
        Dim rdr As StreamReader = Nothing
        Dim txt As String
        Dim s As String()
        Dim sFilePath As String

        sFilePath = Application.LocalUserAppDataPath & "Finances.ini"

        Try
            rdr = File.OpenText(sFilePath)
            While rdr.EndOfStream = False
                txt = rdr.ReadLine
                s = txt.Split("=")
                TheIniFile.Add(s(0), s(1))
            End While
        Catch ex As Exception
            Return
        Finally
            rdr.Close()
        End Try
    End Sub

    ' ****************************************************************************************
    ' 
    Public Shared Sub SaveIniFile()
        Dim wrtr As StreamWriter = Nothing
        Dim i As Integer
        Dim sFilePath As String

        sFilePath = Application.LocalUserAppDataPath & "Finances.ini"

        Try
            wrtr = File.CreateText(sFilePath)
            For i = 0 To TheIniFile.Count - 1
                wrtr.WriteLine(TheIniFile.Keys(i) & "=" & TheIniFile.Values(i))
            Next
        Catch ex As Exception
            Console.WriteLine("Cannot write state list " & ex.Message)
            Return
        Finally
            wrtr.Close()
        End Try
    End Sub

    ' ****************************************************************************************
    ' 
    Public Shared Function ReadValuePair(ByVal sKey As String, ByRef sValue As String) As Boolean
        Dim i As Integer
        Dim b As Boolean

        i = TheIniFile.IndexOfKey(sKey)
        If i >= 0 Then
            sValue = TheIniFile.Values(i)
            b = True
        Else
            sValue = ""
            b = False
        End If

        ReadValuePair = b
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Sub SaveValuePair(ByVal sKey As String, sValue As String)
        Dim i As Integer

        i = TheIniFile.IndexOfKey(sKey)

        If i >= 0 Then
            TheIniFile.Item(sKey) = sValue
        Else
            TheIniFile.Add(sKey, sValue)
        End If

    End Sub

    ' ****************************************************************************************
    ' 
    Public Shared Function GetBorrowerDoc(ByVal iSeq As Integer, ByVal iLoanID As Integer, ByVal sExt As String) As String
        GetBorrowerDoc = Format(iLoanID, "0000000") & "-" & Format(iSeq, "000") & sExt
    End Function

    ' ****************************************************************************************
    ' 
    Public Shared Function GetLoanImageURL(iLoanID As Integer) As String
        GetLoanImageURL = iLoanID.ToString("0000000") & "-001.jpg"
    End Function

End Class
