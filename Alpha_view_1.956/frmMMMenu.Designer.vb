<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMMMenu
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMMMenu))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.UcOnAirRecognizer1 = New TestHelpers.ucOnAirRecognizer()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 10
        '
        'UcOnAirRecognizer1
        '
        Me.UcOnAirRecognizer1.BackColor = System.Drawing.Color.White
        Me.UcOnAirRecognizer1.BackgroundImage = CType(resources.GetObject("UcOnAirRecognizer1.BackgroundImage"), System.Drawing.Image)
        Me.UcOnAirRecognizer1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.UcOnAirRecognizer1.Location = New System.Drawing.Point(3, 3)
        Me.UcOnAirRecognizer1.Name = "UcOnAirRecognizer1"
        Me.UcOnAirRecognizer1.Size = New System.Drawing.Size(37, 15)
        Me.UcOnAirRecognizer1.TabIndex = 0
        Me.UcOnAirRecognizer1.UseWaitCursor = True
        Me.UcOnAirRecognizer1.Visible = False
        '
        'frmMMMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Magenta
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.UcOnAirRecognizer1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmMMMenu"
        Me.Opacity = 0.0R
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.UseWaitCursor = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents UcOnAirRecognizer1 As TestHelpers.ucOnAirRecognizer
End Class
