Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace DataObjects.ExternalStimuli

    Public MustInherit Class Stimulus
        Inherits AnimatTools.DataObjects.DragObject


#Region " Enums "

        Public Enum enumValueType
            Constant
            Equation
        End Enum

#End Region

#Region " Attributes "

        Protected m_snEndTime As AnimatTools.Framework.ScaledNumber
        Protected m_snStartTime As AnimatTools.Framework.ScaledNumber
        Protected m_snStepInterval As AnimatTools.Framework.ScaledNumber
        Protected m_bAlwaysActive As Boolean = False
        Protected m_eValueType As enumValueType = enumValueType.Constant
        Protected m_strEquation As String = ""

        Protected m_nodeStimulus As TreeNode

#End Region

#Region " Properties "

        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("You can not set the name for the stimulus to blank.")
                End If

                'Now add it back in the list with the new name
                m_strName = Value.Trim

                Me.CreateWorkspaceTreeView(Util.Simulation, Util.Application.ProjectWorkspace)
                If Not m_nodeStimulus Is Nothing Then
                    Util.Application.ProjectWorkspace.TreeView.SelectedNode = m_nodeStimulus
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Enabled() As Boolean
            Get
                Return MyBase.Enabled
            End Get
            Set(ByVal Value As Boolean)
                MyBase.Enabled = Value

                If Not Me.WorkspaceTreeNode Is Nothing Then
                    If m_bEnabled Then
                        Me.WorkspaceTreeNode.BackColor = Color.White
                    Else
                        Me.WorkspaceTreeNode.BackColor = Color.Gray
                    End If
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StartTime() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snStartTime
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The start time must be greater than 0.")
                End If

                If Value.ActualValue >= m_snEndTime.ActualValue Then
                    Throw New System.Exception("The start time must be less than the end time.")
                End If

                m_snStartTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EndTime() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snEndTime
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The end time must be greater than 0.")
                End If

                If Value.ActualValue <= m_snStartTime.ActualValue Then
                    Throw New System.Exception("The end time must be greater than the end time.")
                End If

                m_snEndTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StepInterval() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snStepInterval
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The step interval must be greater than 0.")
                End If

                If Value.ActualValue > (m_snEndTime.ActualValue - m_snStartTime.ActualValue) AndAlso Not m_bAlwaysActive Then
                    Throw New System.Exception("The step interval must not be greater than the duration of the stimulus.")
                End If

                m_snStepInterval.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AlwaysActive() As Boolean
            Get
                Return m_bAlwaysActive
            End Get
            Set(ByVal Value As Boolean)
                m_bAlwaysActive = Value

                'If we change this property it could change the readonly settings of other properties so lets refresh the property grid.
                If Not Util.Application.ProjectWorkspace Is Nothing AndAlso Not Util.Application.ProjectWorkspace.TreeView.SelectedNode Is Nothing _
                   AndAlso Not Util.Application.ProjectWorkspace.TreeView.SelectedNode.Tag Is Nothing AndAlso _
                   TypeOf Util.Application.ProjectWorkspace.TreeView.SelectedNode.Tag Is AnimatTools.Framework.DataObject Then

                    Dim doNode As AnimatTools.Framework.DataObject = DirectCast(Util.Application.ProjectWorkspace.TreeView.SelectedNode.Tag, AnimatTools.Framework.DataObject)
                    Util.Application.ProjectWorkspace.PropertyData = doNode.Properties
                End If
            End Set
        End Property

        Public Overridable Property ValueType() As enumValueType
            Get
                Return m_eValueType
            End Get
            Set(ByVal Value As enumValueType)
                m_eValueType = Value

                If Not Util.Application.ProjectWorkspace Is Nothing Then
                    Util.Application.ProjectWorkspace.ctrlProperties.SelectedObject = Me.Properties
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Equation() As String
            Get
                Return m_strEquation
            End Get
            Set(ByVal Value As String)
                'Lets verify the equation before we use it.
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.Equation = Value
                oMathEval.Parse()

                m_strEquation = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property StimulusNoLongerValid() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property WorkspaceTreeNode() As TreeNode
            Get
                Return m_nodeStimulus
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property WorkspaceNodeAssemblyName() As String
            Get
                Return "AnimatTools"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property WorkspaceNodeImageName() As String
            Get
                Return "AnimatTools.ExternalStimulus.gif"
            End Get
        End Property

        Public MustOverride ReadOnly Property TypeName() As String
        Public MustOverride ReadOnly Property Description() As String

        Public Overridable ReadOnly Property StimulusModuleName() As String
            Get
                Return Me.ModuleName
            End Get
        End Property

        Public MustOverride ReadOnly Property StimulusClassType() As String

        Public Overrides Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty
            End Get
            Set(ByVal Value As Boolean)
                Dim bOldVal As Boolean = MyBase.IsDirty

                MyBase.IsDirty = Value

                'Only modify stimuli if we were changed.
                If bOldVal Then
                    ModifyStimulus()
                End If
            End Set
        End Property

#Region " DragObject Properties "

        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return Me.WorkspaceNodeImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property Image() As System.Drawing.Image
            Get
                If m_Image Is Nothing Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                    m_Image = ImageManager.LoadImage(myAssembly, Me.ImageName)
                End If

                Return m_Image
            End Get
            Set(ByVal Value As System.Drawing.Image)
                m_Image = Value
            End Set
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get

            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property DragImage() As System.Drawing.Image
            Get
                If m_Image Is Nothing Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                    m_Image = ImageManager.LoadImage(myAssembly, Me.ImageName)
                End If

                Return m_Image
            End Get
            Set(ByVal Value As System.Drawing.Image)
                m_Image = Value
            End Set
        End Property

        Public Overrides ReadOnly Property DataColumnModuleName() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property DataColumnClassType() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_strID = System.Guid.NewGuid().ToString()
            m_snStartTime = New AnimatTools.Framework.ScaledNumber(Me, "StartTime", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snEndTime = New AnimatTools.Framework.ScaledNumber(Me, "EndTime", 5, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snStepInterval = New AnimatTools.Framework.ScaledNumber(Me, "StepInterval", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigStim As DataObjects.ExternalStimuli.Stimulus = DirectCast(doOriginal, Stimulus)

            m_strName = OrigStim.m_strName
            m_strID = OrigStim.m_strID
            m_bAlwaysActive = OrigStim.m_bAlwaysActive
            m_bEnabled = OrigStim.m_bEnabled
            If Not OrigStim.m_Image Is Nothing Then m_Image = DirectCast(OrigStim.m_Image.Clone, System.Drawing.Image)
            m_snEndTime = DirectCast(OrigStim.m_snEndTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStartTime = DirectCast(OrigStim.m_snStartTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snStepInterval = DirectCast(OrigStim.m_snStepInterval.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_eValueType = OrigStim.m_eValueType
            m_strEquation = OrigStim.m_strEquation
        End Sub

        Public Overridable Sub PrepareForSimulation()

            If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
               AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then

                If m_bEnabled AndAlso Not Util.Application.SimulationView.Simulator.FindStimulus(m_strID, False) Then
                    Util.DisableDirtyFlags = True
                    Dim strXml As String = SaveStimulusToXml()

                    If strXml.Trim().Length > 0 Then
                        Util.Application.SimulationView.Simulator.AddStimulus(m_strID, m_strName, Me.StimulusModuleName, Me.StimulusClassType, strXml)
                    End If
                    Util.DisableDirtyFlags = False
                End If

            End If

        End Sub

        Protected Overridable Sub RemoveFromSimulation()

            If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
               AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then

                If Util.Application.SimulationView.Simulator.FindStimulus(m_strID, False) Then
                    Util.DisableDirtyFlags = True
                    Util.Application.SimulationView.Simulator.RemoveStimulus(m_strID)
                    Util.DisableDirtyFlags = False
                End If

            End If

        End Sub

        Protected Overridable Sub ModifyStimulus()

            If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
               AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then

                Util.DisableDirtyFlags = True
                Dim strXml As String = SaveStimulusToXml()
                Util.Application.SimulationView.Simulator.ModifyStimulus(m_strID, m_strName, Me.StimulusModuleName, Me.StimulusClassType, m_bEnabled, strXml)
                Util.DisableDirtyFlags = False

            End If

        End Sub

        Public Overridable Sub Delete(Optional ByVal bAskToDelete As Boolean = True)

            If Not bAskToDelete OrElse (bAskToDelete AndAlso MessageBox.Show("Are you certain that you want to permanently delete this " & _
                                "stimulus?", "Delete Stimulus", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                If Not m_nodeStimulus Is Nothing Then m_nodeStimulus.Remove()
                Util.Application.Stimuli.Remove(Me.ID)
                RemoveFromSimulation()
            End If

        End Sub

        Public MustOverride Function SaveStimulusToXml() As String

#Region " DragObject Methods "

        Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatTools.Interfaces.StdXml)

        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

            If Util.Application.Stimuli.Contains(strDataItemID) Then
                Dim doStimulus As DataObjects.DragObject = DirectCast(Util.Application.Stimuli(strDataItemID), DataObjects.DragObject)
                Return doStimulus
            Else
                Throw New System.Exception("No stimulus with the ID " & strDataItemID & " was found.")
            End If

            Return Nothing
        End Function

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode

            'If a template part type is supplied and this part is not one of those template types then do not add it to the tree view
            If tpTemplatePartType Is Nothing OrElse (Not tpTemplatePartType Is Nothing AndAlso Util.IsTypeOf(Me.GetType(), tpTemplatePartType, False)) Then
                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                frmDataItem.ImageManager.AddImage(myAssembly, Me.ImageName)

                Dim tnNode As TreeNode
                Dim tnStimuli As TreeNode
                For Each tnNode In frmDataItem.TreeView.Nodes
                    If tnNode.Text = "Stimuli" Then
                        tnStimuli = tnNode
                    End If
                Next

                If tnStimuli Is Nothing Then
                    tnStimuli = frmDataItem.TreeView.Nodes.Add("Stimuli")
                End If

                tnNode = tnStimuli.Nodes.Add(Me.ItemName)
                tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.ImageName)
                tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.ImageName)
                tnNode.Tag = Me

                Return tnNode
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region " Workspace Methods "

        Public Overridable Sub CreateWorkspaceTreeView(ByVal dsSim As AnimatTools.DataObjects.Simulation, _
                                                       ByVal frmWorkspace As Forms.ProjectWorkspace)
            If Not m_nodeStimulus Is Nothing Then
                m_nodeStimulus.Remove()
            End If

            m_nodeStimulus = frmWorkspace.StimuliTreeNode.Nodes.Add(Me.Name)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load(Me.WorkspaceNodeAssemblyName)
            frmWorkspace.ImageManager.AddImage(myAssembly, Me.WorkspaceNodeImageName)

            m_nodeStimulus.ImageIndex = frmWorkspace.ImageManager.GetImageIndex(Me.WorkspaceNodeImageName)
            m_nodeStimulus.SelectedImageIndex = frmWorkspace.ImageManager.GetImageIndex(Me.WorkspaceNodeImageName)
            m_nodeStimulus.Tag = Me

            If Me.Enabled Then
                m_nodeStimulus.BackColor = Color.White
            Else
                m_nodeStimulus.BackColor = Color.Gray
            End If

            frmWorkspace.ctrlTreeView.SelectedNode = m_nodeStimulus

        End Sub

        Public Overridable Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As TreeNode, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_nodeStimulus Then
                ' Create the menu items
                Dim mcDelete As New MenuCommand("Delete Stimulus ", "DeleteStimulus", Util.Application.SmallImages.ImageList, _
                                                  Util.Application.SmallImages.GetImageIndex("AnimatTools.Delete.gif"), _
                                                  New EventHandler(AddressOf Me.OnDeleteStimulus))

                ' Create the popup menu object
                Dim popup As New PopupMenu
                popup.MenuCommands.Add(mcDelete)

                ' Show it!
                Dim selected As MenuCommand = popup.TrackPopup(ptPoint)

                Return True
            End If

            Return False
        End Function

        Public Overridable Function WorkspaceTreeviewDoubleClick(ByRef tnSelectedNode As TreeNode) As Boolean

            If tnSelectedNode Is m_nodeStimulus Then
                Return True
            End If

            Return False
        End Function

#End Region

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Stimulus Properties", "The name of this stimulus.", m_strName))

            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snStartTime.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Start Time", pbNumberBag.GetType(), "StartTime", _
                                        "Stimulus Properties", "This is the time at which the stimulus will be applied.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bAlwaysActive))

            pbNumberBag = m_snEndTime.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("End Time", pbNumberBag.GetType(), "EndTime", _
                                        "Stimulus Properties", "This is the time at which the stimulus will stop being applied.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bAlwaysActive))

            'pbNumberBag = m_snStepInterval.Properties
            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Step Interval", pbNumberBag.GetType(), "StepInterval", _
            '                            "Stimulus Properties", "If you only want the stimulus to be applied at certain, evenly spaced time steps " & _
            '                            "while the stimulus is active you can use this paramater. It must be some time value less than the total " & _
            '                            "durationd of the stimulus. If it is 0 then the stimulus will be applied at every step.", pbNumberBag, _
            '                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Always Active", m_bAlwaysActive.GetType(), "AlwaysActive", _
                                        "Stimulus Properties", "If this is true then this stimulus is active constantly during the simulation.", m_bAlwaysActive))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Enabled", m_bAlwaysActive.GetType(), "Enabled", _
                                        "Stimulus Properties", "If this is false then this stimulus is not applied.", m_bEnabled))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snStartTime Is Nothing Then m_snStartTime.ClearIsDirty()
            If Not m_snEndTime Is Nothing Then m_snEndTime.ClearIsDirty()
            If Not m_snStepInterval Is Nothing Then m_snStepInterval.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.IntoElem()

            m_strName = oXml.GetChildString("Name")
            m_strID = oXml.GetChildString("ID")

            m_snStartTime.LoadData(oXml, "StartTime")
            m_snEndTime.LoadData(oXml, "EndTime")
            m_snStepInterval.LoadData(oXml, "StepInterval")

            m_bAlwaysActive = oXml.GetChildBool("AlwaysActive", m_bAlwaysActive)
            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

            m_eValueType = DirectCast([Enum].Parse(GetType(enumValueType), oXml.GetChildString("ValueType", "Constant"), True), enumValueType)
            m_strEquation = oXml.GetChildString("Equation", m_strEquation)

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.AddChildElement("Stimulus")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            m_snStartTime.SaveData(oXml, "StartTime")
            m_snEndTime.SaveData(oXml, "EndTime")
            m_snStepInterval.SaveData(oXml, "StepInterval")

            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("ValueType", m_eValueType.ToString)
            oXml.AddChildElement("Equation", m_strEquation)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

#Region " Events "

        Protected Sub OnDeleteStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.Delete(True)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace
