<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmAddLoan
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.edLoanName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Snow
        Me.Panel1.Controls.Add(Me.edLoanName)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnGo)
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(3, 1)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(522, 132)
        Me.Panel1.TabIndex = 2
        '
        'btnGo
        '
        Me.btnGo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGo.AutoSize = True
        Me.btnGo.BackColor = System.Drawing.Color.Coral
        Me.btnGo.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnGo.Location = New System.Drawing.Point(398, 83)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(77, 30)
        Me.btnGo.TabIndex = 70
        Me.btnGo.Text = "OK"
        Me.btnGo.UseVisualStyleBackColor = False
        '
        'edLoanName
        '
        Me.edLoanName.BackColor = System.Drawing.SystemColors.Window
        Me.edLoanName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edLoanName.Location = New System.Drawing.Point(47, 42)
        Me.edLoanName.Name = "edLoanName"
        Me.edLoanName.Size = New System.Drawing.Size(428, 19)
        Me.edLoanName.TabIndex = 84
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label3.Location = New System.Drawing.Point(43, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 20)
        Me.Label3.TabIndex = 83
        Me.Label3.Text = "Loan Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fmAddLoan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 138)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "fmAddLoan"
        Me.Text = "New Loan"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnGo As Button
    Friend WithEvents edLoanName As TextBox
    Friend WithEvents Label3 As Label
End Class
