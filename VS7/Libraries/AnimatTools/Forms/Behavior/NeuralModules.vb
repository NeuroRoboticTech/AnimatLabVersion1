Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace Forms.Behavior

    Public Class NeuralModules
        Inherits AnimatForm 'System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents lvNeuralModules As System.Windows.Forms.ListView
        Friend WithEvents pgModuleProperties As System.Windows.Forms.PropertyGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvNeuralModules = New System.Windows.Forms.ListView
            Me.pgModuleProperties = New System.Windows.Forms.PropertyGrid
            Me.SuspendLayout()
            '
            'lvNeuralModules
            '
            Me.lvNeuralModules.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvNeuralModules.Location = New System.Drawing.Point(8, 8)
            Me.lvNeuralModules.Name = "lvNeuralModules"
            Me.lvNeuralModules.Size = New System.Drawing.Size(224, 88)
            Me.lvNeuralModules.TabIndex = 0
            '
            'pgModuleProperties
            '
            Me.pgModuleProperties.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pgModuleProperties.CommandsVisibleIfAvailable = True
            Me.pgModuleProperties.LargeButtons = False
            Me.pgModuleProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgModuleProperties.Location = New System.Drawing.Point(8, 104)
            Me.pgModuleProperties.Name = "pgModuleProperties"
            Me.pgModuleProperties.Size = New System.Drawing.Size(224, 352)
            Me.pgModuleProperties.TabIndex = 2
            Me.pgModuleProperties.Text = "PropertyGrid1"
            Me.pgModuleProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.pgModuleProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'NeuralModules
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(240, 462)
            Me.Controls.Add(Me.pgModuleProperties)
            Me.Controls.Add(Me.lvNeuralModules)
            Me.Name = "NeuralModules"
            Me.Text = "NeuralModules"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatTools.Forms.Behavior.Editor

#End Region

#Region " Properties "

        Public Overridable Property Editor() As Forms.Behavior.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_beEditor = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByRef frmApplication As AnimatApplication, _
                                        Optional ByVal frmMdiParent As MdiChild = Nothing, _
                                        Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmApplication, frmMdiParent, frmParent)

                m_beEditor = DirectCast(frmMdiParent, AnimatTools.Forms.Behavior.Editor)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                If Not m_beEditor.Organism Is Nothing Then
                    PopulateNeuralModules()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub PopulateNeuralModules()

            lvNeuralModules.Clear()

            lvNeuralModules.HideSelection = False
            lvNeuralModules.MultiSelect = False
            lvNeuralModules.FullRowSelect = True
            lvNeuralModules.GridLines = True
            lvNeuralModules.View = View.Details
            lvNeuralModules.LabelEdit = False
            lvNeuralModules.Sorting = SortOrder.Ascending

            lvNeuralModules.Columns.Add("Neural Modules", (lvNeuralModules.Width - 1), HorizontalAlignment.Left)

            Dim nmModule As DataObjects.Behavior.NeuralModule
            Dim liItem As ListViewItem
            For Each deEntry As DictionaryEntry In m_beEditor.Organism.NeuralModules
                nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                If Not Util.IsTypeOf(nmModule.GetType(), GetType(DataObjects.Behavior.PhysicsModule), False) Then
                    liItem = New ListViewItem(nmModule.ModuleName)
                    liItem.Tag = nmModule

                    lvNeuralModules.Items.Add(liItem)
                End If
            Next
        End Sub

        Public Sub RefreshProperties()
            pgModuleProperties.Refresh()
        End Sub

#End Region

#Region " Events "

        Private Sub lvNeuralModules_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvNeuralModules.Click

            Try

                If lvNeuralModules.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = lvNeuralModules.SelectedItems(0)

                    Dim nmModule As DataObjects.Behavior.NeuralModule = DirectCast(liItem.Tag, DataObjects.Behavior.NeuralModule)

                    pgModuleProperties.SelectedObject = nmModule.Properties
                Else
                    pgModuleProperties.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region
 
    End Class

End Namespace
