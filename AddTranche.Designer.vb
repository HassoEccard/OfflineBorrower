<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fmAddTranche
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
        Me.edLoan = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.edAmount = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.edRate = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.edTerm = New System.Windows.Forms.TextBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.txtError = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Snow
        Me.Panel1.Controls.Add(Me.txtError)
        Me.Panel1.Controls.Add(Me.edLoan)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnGo)
        Me.Panel1.Controls.Add(Me.edAmount)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.edRate)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.edTerm)
        Me.Panel1.Controls.Add(Me.Label60)
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(471, 262)
        Me.Panel1.TabIndex = 1
        '
        'edLoan
        '
        Me.edLoan.BackColor = System.Drawing.Color.LavenderBlush
        Me.edLoan.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edLoan.Location = New System.Drawing.Point(159, 37)
        Me.edLoan.Multiline = True
        Me.edLoan.Name = "edLoan"
        Me.edLoan.ReadOnly = True
        Me.edLoan.Size = New System.Drawing.Size(240, 45)
        Me.edLoan.TabIndex = 72
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label3.Location = New System.Drawing.Point(84, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 20)
        Me.Label3.TabIndex = 71
        Me.Label3.Text = "Loan"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnGo
        '
        Me.btnGo.AutoSize = True
        Me.btnGo.BackColor = System.Drawing.Color.Coral
        Me.btnGo.Location = New System.Drawing.Point(322, 209)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(77, 30)
        Me.btnGo.TabIndex = 70
        Me.btnGo.Text = "Create"
        Me.btnGo.UseVisualStyleBackColor = False
        '
        'edAmount
        '
        Me.edAmount.BackColor = System.Drawing.SystemColors.Window
        Me.edAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edAmount.Location = New System.Drawing.Point(159, 100)
        Me.edAmount.Name = "edAmount"
        Me.edAmount.Size = New System.Drawing.Size(240, 19)
        Me.edAmount.TabIndex = 67
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label2.Location = New System.Drawing.Point(64, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 20)
        Me.Label2.TabIndex = 66
        Me.Label2.Text = "Amount"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'edRate
        '
        Me.edRate.BackColor = System.Drawing.SystemColors.Window
        Me.edRate.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edRate.Location = New System.Drawing.Point(159, 168)
        Me.edRate.Name = "edRate"
        Me.edRate.Size = New System.Drawing.Size(240, 19)
        Me.edRate.TabIndex = 65
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label1.Location = New System.Drawing.Point(86, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 20)
        Me.Label1.TabIndex = 64
        Me.Label1.Text = "Rate"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'edTerm
        '
        Me.edTerm.BackColor = System.Drawing.SystemColors.Window
        Me.edTerm.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edTerm.Location = New System.Drawing.Point(159, 134)
        Me.edTerm.Name = "edTerm"
        Me.edTerm.Size = New System.Drawing.Size(240, 19)
        Me.edTerm.TabIndex = 63
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label60.Location = New System.Drawing.Point(82, 134)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(48, 20)
        Me.Label60.TabIndex = 62
        Me.Label60.Text = "Term"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtError
        '
        Me.txtError.AutoSize = True
        Me.txtError.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtError.ForeColor = System.Drawing.Color.Red
        Me.txtError.Location = New System.Drawing.Point(10, 214)
        Me.txtError.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.txtError.Name = "txtError"
        Me.txtError.Size = New System.Drawing.Size(109, 20)
        Me.txtError.TabIndex = 73
        Me.txtError.Text = "File to upload"
        '
        'fmAddTranche
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 269)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "fmAddTranche"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Tranche"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnGo As Button
    Friend WithEvents edAmount As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents edRate As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents edTerm As TextBox
    Friend WithEvents Label60 As Label
    Friend WithEvents edLoan As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtError As Label
End Class
