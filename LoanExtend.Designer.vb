<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fmLoanExtend
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtDateStart = New System.Windows.Forms.DateTimePicker()
        Me.txtError = New System.Windows.Forms.Label()
        Me.edExistingRate = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.edNewRate = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtDateEnd = New System.Windows.Forms.DateTimePicker()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Snow
        Me.Panel1.Controls.Add(Me.dtDateEnd)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.dtDateStart)
        Me.Panel1.Controls.Add(Me.txtError)
        Me.Panel1.Controls.Add(Me.edExistingRate)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnGo)
        Me.Panel1.Controls.Add(Me.edNewRate)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label60)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(443, 412)
        Me.Panel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label1.Location = New System.Drawing.Point(87, 269)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 20)
        Me.Label1.TabIndex = 75
        Me.Label1.Text = "End Date"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dtDateStart
        '
        Me.dtDateStart.CalendarForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.dtDateStart.CalendarMonthBackground = System.Drawing.Color.Snow
        Me.dtDateStart.CalendarTitleForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.dtDateStart.CustomFormat = "d MMMM yyyy"
        Me.dtDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDateStart.Location = New System.Drawing.Point(172, 220)
        Me.dtDateStart.Name = "dtDateStart"
        Me.dtDateStart.Size = New System.Drawing.Size(178, 26)
        Me.dtDateStart.TabIndex = 74
        '
        'txtError
        '
        Me.txtError.AutoSize = True
        Me.txtError.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtError.ForeColor = System.Drawing.Color.Red
        Me.txtError.Location = New System.Drawing.Point(23, 349)
        Me.txtError.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtError.Name = "txtError"
        Me.txtError.Size = New System.Drawing.Size(109, 20)
        Me.txtError.TabIndex = 73
        Me.txtError.Text = "File to upload"
        '
        'edExistingRate
        '
        Me.edExistingRate.BackColor = System.Drawing.Color.LavenderBlush
        Me.edExistingRate.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edExistingRate.Location = New System.Drawing.Point(172, 125)
        Me.edExistingRate.Name = "edExistingRate"
        Me.edExistingRate.ReadOnly = True
        Me.edExistingRate.Size = New System.Drawing.Size(240, 19)
        Me.edExistingRate.TabIndex = 72
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label3.Location = New System.Drawing.Point(58, 125)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(108, 20)
        Me.Label3.TabIndex = 71
        Me.Label3.Text = "Existing Rate"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnGo
        '
        Me.btnGo.AutoSize = True
        Me.btnGo.BackColor = System.Drawing.Color.Coral
        Me.btnGo.Location = New System.Drawing.Point(335, 344)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(77, 30)
        Me.btnGo.TabIndex = 70
        Me.btnGo.Text = "Create"
        Me.btnGo.UseVisualStyleBackColor = False
        '
        'edNewRate
        '
        Me.edNewRate.BackColor = System.Drawing.SystemColors.Window
        Me.edNewRate.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edNewRate.Location = New System.Drawing.Point(172, 176)
        Me.edNewRate.Name = "edNewRate"
        Me.edNewRate.Size = New System.Drawing.Size(240, 19)
        Me.edNewRate.TabIndex = 67
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label2.Location = New System.Drawing.Point(84, 175)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 20)
        Me.Label2.TabIndex = 66
        Me.Label2.Text = "New Rate"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label60.Location = New System.Drawing.Point(80, 225)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(86, 20)
        Me.Label60.TabIndex = 62
        Me.Label60.Text = "Start Date"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.GroupBox1.Location = New System.Drawing.Point(43, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(351, 81)
        Me.GroupBox1.TabIndex = 77
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Note"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.Highlight
        Me.Label4.Location = New System.Drawing.Point(32, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(290, 40)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "This will cause visible and calculated " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "rates to change on the website"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtDateEnd
        '
        Me.dtDateEnd.CalendarForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.dtDateEnd.CalendarMonthBackground = System.Drawing.Color.Snow
        Me.dtDateEnd.CalendarTitleForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.dtDateEnd.CustomFormat = "d MMMM yyyy"
        Me.dtDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDateEnd.Location = New System.Drawing.Point(172, 264)
        Me.dtDateEnd.Name = "dtDateEnd"
        Me.dtDateEnd.Size = New System.Drawing.Size(178, 26)
        Me.dtDateEnd.TabIndex = 78
        '
        'fmLoanExtend
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(443, 412)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "fmLoanExtend"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Extend the Loan"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents dtDateStart As DateTimePicker
    Friend WithEvents txtError As Label
    Friend WithEvents edExistingRate As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnGo As Button
    Friend WithEvents edNewRate As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label60 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dtDateEnd As DateTimePicker
End Class
