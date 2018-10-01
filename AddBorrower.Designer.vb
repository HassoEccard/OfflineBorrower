<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddBorrower
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
        Me.edBusinessName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.edLastname = New System.Windows.Forms.TextBox()
        Me.edEmail = New System.Windows.Forms.TextBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.edFirstname = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Snow
        Me.Panel1.Controls.Add(Me.btnGo)
        Me.Panel1.Controls.Add(Me.edBusinessName)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(446, 354)
        Me.Panel1.TabIndex = 0
        '
        'btnGo
        '
        Me.btnGo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGo.AutoSize = True
        Me.btnGo.BackColor = System.Drawing.Color.Coral
        Me.btnGo.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnGo.Location = New System.Drawing.Point(336, 295)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(77, 30)
        Me.btnGo.TabIndex = 70
        Me.btnGo.Text = "OK"
        Me.btnGo.UseVisualStyleBackColor = False
        '
        'edBusinessName
        '
        Me.edBusinessName.BackColor = System.Drawing.SystemColors.Window
        Me.edBusinessName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edBusinessName.Location = New System.Drawing.Point(161, 54)
        Me.edBusinessName.Name = "edBusinessName"
        Me.edBusinessName.Size = New System.Drawing.Size(240, 19)
        Me.edBusinessName.TabIndex = 67
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label2.Location = New System.Drawing.Point(23, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 20)
        Me.Label2.TabIndex = 66
        Me.Label2.Text = "Business Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.edLastname)
        Me.GroupBox1.Controls.Add(Me.edEmail)
        Me.GroupBox1.Controls.Add(Me.Label60)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.edFirstname)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.GroupBox1.Location = New System.Drawing.Point(17, 101)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(405, 163)
        Me.GroupBox1.TabIndex = 71
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Primary Contact"
        '
        'edLastname
        '
        Me.edLastname.BackColor = System.Drawing.SystemColors.Window
        Me.edLastname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edLastname.Location = New System.Drawing.Point(130, 70)
        Me.edLastname.Name = "edLastname"
        Me.edLastname.Size = New System.Drawing.Size(240, 19)
        Me.edLastname.TabIndex = 73
        '
        'edEmail
        '
        Me.edEmail.BackColor = System.Drawing.SystemColors.Window
        Me.edEmail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edEmail.Location = New System.Drawing.Point(130, 108)
        Me.edEmail.Name = "edEmail"
        Me.edEmail.Size = New System.Drawing.Size(240, 19)
        Me.edEmail.TabIndex = 75
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label60.Location = New System.Drawing.Point(35, 35)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(89, 20)
        Me.Label60.TabIndex = 70
        Me.Label60.Text = "First name"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label3.Location = New System.Drawing.Point(69, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 20)
        Me.Label3.TabIndex = 74
        Me.Label3.Text = "EMail"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'edFirstname
        '
        Me.edFirstname.BackColor = System.Drawing.SystemColors.Window
        Me.edFirstname.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.edFirstname.Location = New System.Drawing.Point(130, 36)
        Me.edFirstname.Name = "edFirstname"
        Me.edFirstname.Size = New System.Drawing.Size(240, 19)
        Me.edFirstname.TabIndex = 71
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.Label1.Location = New System.Drawing.Point(41, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 20)
        Me.Label1.TabIndex = 72
        Me.Label1.Text = "Lastname"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'AddBorrower
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Coral
        Me.ClientSize = New System.Drawing.Size(453, 361)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "AddBorrower"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add a Borrower"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents edBusinessName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnGo As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents edLastname As TextBox
    Friend WithEvents edEmail As TextBox
    Friend WithEvents Label60 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents edFirstname As TextBox
    Friend WithEvents Label1 As Label
End Class
