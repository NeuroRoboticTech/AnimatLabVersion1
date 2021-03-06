Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace DataObjects.Behavior

    Public MustInherit Class Link
        Inherits Behavior.Data

#Region " Enums "

        Public Enum enumArrowSize
            Small
            Medium
            Large
        End Enum

        Public Enum enumArrowStyle
            None
            Circle
            Arrow
            Fork
            ClosedFork
            Losange
            Many
            ManyOptional
            One
            OneOptional
            OneOrMany
            OpenedArrow
            HalfArrow
            OpenedHalfArrow
        End Enum

        Public Enum enumArrowAngle
            deg15
            deg30
            deg45
        End Enum

        Public Enum enumJump
            None
            Arc
            Break
        End Enum

        Public Enum enumJumpSize
            Small
            Medium
            Large
        End Enum

        Public Enum enumLineStyle
            Polyline
            Bezier
            Spline
            VH
            HV
            VHV
            HVH
            VHVH
            HVHV
            VHVHV
            HVHVH
            VHVHVH
            HVHVHV
            VHVHVHV
            HVHVHVH
        End Enum

#End Region

#Region " Arrow Class "

        Public Class Arrow
            Inherits Framework.DataObject

#Region " Attributes "

            Protected m_eStyle As enumArrowStyle
            Protected m_eSize As enumArrowSize
            Protected m_eAngle As enumArrowAngle
            Protected m_bFilled As Boolean
            Protected m_ParentLink As Behavior.Link

#End Region

#Region " Properties "

            Public Overridable Property Style() As enumArrowStyle
                Get
                    Return m_eStyle
                End Get
                Set(ByVal Value As enumArrowStyle)
                    m_eStyle = Value
                    UpdateChart()
                End Set
            End Property

            Public Overridable Property Size() As enumArrowSize
                Get
                    Return m_eSize
                End Get
                Set(ByVal Value As enumArrowSize)
                    m_eSize = Value
                    UpdateChart()
                End Set
            End Property

            Public Overridable Property Angle() As enumArrowAngle
                Get
                    Return m_eAngle
                End Get
                Set(ByVal Value As enumArrowAngle)
                    m_eAngle = Value
                    UpdateChart()
                End Set
            End Property

            Public Overridable Property Filled() As Boolean
                Get
                    Return m_bFilled
                End Get
                Set(ByVal Value As Boolean)
                    m_bFilled = Value
                    UpdateChart()
                End Set
            End Property

            Public Property ParentLink() As Behavior.Link
                Get
                    Return m_ParentLink
                End Get
                Set(ByVal Value As Behavior.Link)
                    m_ParentLink = Value
                End Set
            End Property

            'Public Overrides Property IsDirty() As Boolean
            '    Get
            '        Return m_bIsDirty
            '    End Get
            '    Set(ByVal Value As Boolean)
            '        m_bIsDirty = True
            '        If Not m_ParentLink Is Nothing Then m_ParentLink.IsDirty = True
            '    End Set
            'End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_eStyle = enumArrowStyle.Arrow
                m_eSize = enumArrowSize.Small
                m_eAngle = enumArrowAngle.deg15
                m_bFilled = False
                m_bFixedProperties = True
            End Sub

            Public Sub New(ByRef blParent As Behavior.Link, ByVal eStyle As enumArrowStyle, ByVal eSize As enumArrowSize, ByVal eAngle As enumArrowAngle, ByVal bFilled As Boolean)
                MyBase.New(blParent)

                m_ParentLink = blParent
                m_eStyle = eStyle
                m_eSize = eSize
                m_eAngle = eAngle
                m_bFilled = bFilled
            End Sub

            'Public Sub New(ByVal eStyle As enumArrowStyle, ByVal eSize As enumArrowSize, ByVal eAngle As enumArrowAngle, ByVal bFilled As Boolean)
            '    m_eStyle = eStyle
            '    m_eSize = eSize
            '    m_eAngle = eAngle
            '    m_bFilled = bFilled
            'End Sub

            Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
                Dim newArrow As New Arrow(m_ParentLink, m_eStyle, m_eSize, m_eAngle, m_bFilled)
                Return newArrow
            End Function

            Protected Overridable Sub UpdateChart()
                If Not m_ParentLink Is Nothing AndAlso Not m_ParentLink.ParentDiagram Is Nothing Then
                    m_ParentLink.ParentDiagram.UpdateChart(DirectCast(m_ParentLink, Behavior.Data))
                End If
            End Sub

            Protected Overrides Sub BuildProperties()

                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Angle", m_eAngle.GetType(), "Angle", _
                                            "Node Display", "Sets the angle of the arrow.", m_eAngle))

                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Filled", m_bFilled.GetType(), "Filled", _
                                            "Node Display", "Determines if the arrow is filled with the drawing color or empty.", m_bFilled))

                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Size", m_eSize.GetType(), "Size", _
                                            "Node Display", "Sets the size of the arrow.", m_eSize))

                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Style", m_eStyle.GetType(), "Style", _
                                            "Node Display", "Sets the shape of the arrow head.", m_eStyle))

            End Sub

            Public Overloads Sub LoadData(ByVal strName As String, ByRef oXml As AnimatTools.Interfaces.StdXml)

                oXml.IntoChildElement(strName)

                m_eStyle = DirectCast([Enum].Parse(GetType(enumArrowStyle), oXml.GetChildString("Style"), True), enumArrowStyle)
                m_eSize = DirectCast([Enum].Parse(GetType(enumArrowSize), oXml.GetChildString("Size"), True), enumArrowSize)
                m_eAngle = DirectCast([Enum].Parse(GetType(enumArrowAngle), oXml.GetChildString("Angle"), True), enumArrowAngle)
                m_bFilled = oXml.GetChildBool("Filled")

                oXml.OutOfElem()
            End Sub

            Public Overloads Sub SaveData(ByVal strName As String, ByRef oXml As AnimatTools.Interfaces.StdXml)
                oXml.AddChildElement(strName)
                oXml.IntoElem()

                oXml.AddChildElement("Style", m_eStyle.ToString)
                oXml.AddChildElement("Size", m_eSize.ToString)
                oXml.AddChildElement("Angle", m_eAngle.ToString)
                oXml.AddChildElement("Filled", m_bFilled)

                oXml.OutOfElem()
            End Sub

#End Region

        End Class

#End Region

#Region " Attributes "

        Protected m_iLinkIndex As Integer

        Protected m_bAdjustDst As Boolean
        Protected m_bAdjustOrg As Boolean
        Protected m_ArrowDst As Arrow
        Protected m_ArrowMid As Arrow
        Protected m_ArrowOrg As Arrow
        Protected m_eBackMode As enumBackmode
        Protected m_CustomEndCap As System.Drawing.Drawing2D.CustomLineCap
        Protected m_CustomStartCap As System.Drawing.Drawing2D.CustomLineCap
        Protected m_DashStyle As System.Drawing.Drawing2D.DashStyle
        Protected m_clDrawColor As System.Drawing.Color
        Protected m_iDrawWidth As Integer
        Protected m_bnDestination As Behavior.Node
        Protected m_eEndCap As System.Drawing.Drawing2D.LineCap
        Protected m_Font As System.Drawing.Font
        Protected m_bHidden As Boolean
        Protected m_eJump As enumJump
        Protected m_eLineStyle As enumLineStyle
        Protected m_bOrthogonalDynamic As Boolean
        Protected m_bnOrigin As Behavior.Node
        Protected m_bOrientedText As Boolean
        Protected m_bSelectable As Boolean
        Protected m_bStretchable As Boolean
        Protected m_eStartCap As System.Drawing.Drawing2D.LineCap
        Protected m_strText As String
        Protected m_clTextColor As System.Drawing.Color
        Protected m_strToolTip As String
        Protected m_strUrl As String
        Protected m_iZOrder As Integer

        Protected m_strOriginID As String
        Protected m_strDestinationID As String

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Enabled() As Boolean
            Get
                Return MyBase.Enabled
            End Get
            Set(ByVal Value As Boolean)
                MyBase.Enabled = Value

                If Not Me.TreeNode Is Nothing Then
                    If m_bEnabled Then
                        Me.TreeNode.BackColor = Color.White
                    Else
                        Me.TreeNode.BackColor = Color.Gray
                    End If
                    UpdateChart()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SelectedID() As String
            Get
                'If a link does not have a temp id then it is not being copied and thus
                'should not be associated with anything in the pasted version.
                If m_strTempSelectedID.Trim.Length = 0 Then
                    If Util.CopyInProgress Then
                        Return ""
                    Else
                        Return m_strID
                    End If
                Else
                    Return m_strTempSelectedID
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkIndex() As Integer
            Get
                Return m_iLinkIndex
            End Get
            Set(ByVal Value As Integer)
                m_iLinkIndex = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AdjustDestination() As Boolean
            Get
                Return m_bAdjustDst
            End Get
            Set(ByVal Value As Boolean)
                m_bAdjustDst = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AdjustOrigin() As Boolean
            Get
                Return m_bAdjustOrg
            End Get
            Set(ByVal Value As Boolean)
                m_bAdjustOrg = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ArrowDestination() As Arrow
            Get
                Return m_ArrowDst
            End Get
            Set(ByVal Value As Arrow)
                m_ArrowDst = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ArrowMiddle() As Arrow
            Get
                Return m_ArrowMid
            End Get
            Set(ByVal Value As Arrow)
                m_ArrowMid = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ArrowOrigin() As Arrow
            Get
                Return m_ArrowOrg
            End Get
            Set(ByVal Value As Arrow)
                m_ArrowOrg = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property BackMode() As enumBackmode
            Get
                Return m_eBackMode
            End Get
            Set(ByVal Value As enumBackmode)
                m_eBackMode = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CustomEndCap() As System.Drawing.Drawing2D.CustomLineCap
            Get
                Return m_CustomEndCap
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.CustomLineCap)
                m_CustomEndCap = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CustomStartCap() As System.Drawing.Drawing2D.CustomLineCap
            Get
                Return m_CustomStartCap
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.CustomLineCap)
                m_CustomStartCap = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DashStyle() As System.Drawing.Drawing2D.DashStyle
            Get
                Return m_DashStyle
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.DashStyle)
                m_DashStyle = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DrawColor() As System.Drawing.Color
            Get
                Return m_clDrawColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clDrawColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DrawWidth() As Integer
            Get
                Return m_iDrawWidth
            End Get
            Set(ByVal Value As Integer)
                m_iDrawWidth = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ActualDestination() As Behavior.Node
            Get
                Return m_bnDestination
            End Get
            Set(ByVal Value As Behavior.Node)
                m_bnDestination = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Destination() As Behavior.Node
            Get
                If TypeOf (m_bnDestination) Is AnimatTools.DataObjects.Behavior.Nodes.OffPage Then
                    Dim opDestination As AnimatTools.DataObjects.Behavior.Nodes.OffPage = DirectCast(m_bnDestination, AnimatTools.DataObjects.Behavior.Nodes.OffPage)
                    If Not opDestination.LinkedNode Is Nothing AndAlso Not opDestination.LinkedNode.Node Is Nothing Then
                        Return opDestination.LinkedNode.Node
                    Else
                        Return m_bnDestination
                    End If
                Else
                    Return m_bnDestination
                End If
                Return m_bnDestination
            End Get
            Set(ByVal Value As Behavior.Node)
                m_bnDestination = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EndCap() As System.Drawing.Drawing2D.LineCap
            Get
                Return m_eEndCap
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.LineCap)
                m_eEndCap = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Font() As System.Drawing.Font
            Get
                Return m_Font
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_Font = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Hidden() As Boolean
            Get
                Return m_bHidden
            End Get
            Set(ByVal Value As Boolean)
                m_bHidden = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Jump() As enumJump
            Get
                Return m_eJump
            End Get
            Set(ByVal Value As enumJump)
                m_eJump = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LineStyle() As enumLineStyle
            Get
                Return m_eLineStyle
            End Get
            Set(ByVal Value As enumLineStyle)
                m_eLineStyle = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OrthogonalDynamic() As Boolean
            Get
                Return m_bOrthogonalDynamic
            End Get
            Set(ByVal Value As Boolean)
                m_bOrthogonalDynamic = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ActualOrigin() As Behavior.Node
            Get
                Return m_bnOrigin
            End Get
            Set(ByVal Value As Behavior.Node)
                m_bnOrigin = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Origin() As Behavior.Node
            Get
                If TypeOf (m_bnOrigin) Is AnimatTools.DataObjects.Behavior.Nodes.OffPage Then
                    Dim opOrigin As AnimatTools.DataObjects.Behavior.Nodes.OffPage = DirectCast(m_bnOrigin, AnimatTools.DataObjects.Behavior.Nodes.OffPage)
                    If Not opOrigin.LinkedNode Is Nothing AndAlso Not opOrigin.LinkedNode.Node Is Nothing Then
                        Return opOrigin.LinkedNode.Node
                    Else
                        Return m_bnOrigin
                    End If
                Else
                    Return m_bnOrigin
                End If
                Return m_bnOrigin
            End Get
            Set(ByVal Value As Behavior.Node)
                m_bnOrigin = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OrientedText() As Boolean
            Get
                Return m_bOrientedText
            End Get
            Set(ByVal Value As Boolean)
                m_bOrientedText = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Selectable() As Boolean
            Get
                Return m_bSelectable
            End Get
            Set(ByVal Value As Boolean)
                m_bSelectable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Stretchable() As Boolean
            Get
                Return m_bStretchable
            End Get
            Set(ByVal Value As Boolean)
                m_bStretchable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StartCap() As System.Drawing.Drawing2D.LineCap
            Get
                Return m_eStartCap
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.LineCap)
                m_eStartCap = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Text() As String
            Get
                If m_bEnabled Then
                    Return m_strText
                Else
                    Return "Disabled"
                End If
            End Get
            Set(ByVal Value As String)
                m_strText = Value
                UpdateChart()
                UpdateTreeNode()
                CheckForErrors()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Text
            End Get
            Set(ByVal Value As String)
                Me.Text = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TextColor() As System.Drawing.Color
            Get
                Return m_clTextColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clTextColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ToolTip() As String
            Get
                Return m_strToolTip
            End Get
            Set(ByVal Value As String)
                m_strToolTip = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Url() As String
            Get
                Return m_strUrl
            End Get
            Set(ByVal Value As String)
                m_strUrl = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ZOrder() As Integer
            Get
                Return m_iZOrder
            End Get
            Set(ByVal Value As Integer)
                m_iZOrder = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property TreeViewName() As String
            Get
                Dim strName As String = ""
                If Not Me.Origin Is Nothing AndAlso Not Me.Destination Is Nothing Then
                    If Me.Text.Trim.Length = 0 Then
                        strName = Me.Origin.Text & " --> " & Me.Destination.Text
                    Else
                        strName = "( " & Me.Text & " ) " & Me.Origin.Text & " --> " & Me.Destination.Text
                    End If
                Else
                    strName = Me.Text
                End If

                Return strName
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_bAdjustDst = False
            m_bAdjustOrg = False
            m_ArrowDst = New Arrow(Me, enumArrowStyle.Arrow, enumArrowSize.Medium, enumArrowAngle.deg30, False)
            m_ArrowMid = New Arrow(Me, enumArrowStyle.None, enumArrowSize.Small, enumArrowAngle.deg30, False)
            m_ArrowOrg = New Arrow(Me, enumArrowStyle.None, enumArrowSize.Small, enumArrowAngle.deg30, False)
            m_eBackMode = Data.enumBackmode.Transparent
            m_CustomEndCap = Nothing
            m_CustomStartCap = Nothing
            m_DashStyle = Drawing2D.DashStyle.Solid
            m_clDrawColor = System.Drawing.Color.Black
            m_iDrawWidth = 1
            m_eEndCap = Drawing2D.LineCap.Flat
            m_Font = New System.Drawing.Font("Arial", 10)
            m_bHidden = False
            m_eJump = enumJump.Arc
            m_eLineStyle = enumLineStyle.Polyline
            m_bOrthogonalDynamic = True
            m_bOrientedText = True
            m_bSelectable = True
            m_bStretchable = True
            m_eStartCap = Drawing2D.LineCap.Flat
            m_clTextColor = System.Drawing.Color.Black
            m_strText = ""
            m_strUrl = ""
            m_strToolTip = ""
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim blOrig As Behavior.Link = DirectCast(doOriginal, Behavior.Link)

            m_bAdjustDst = blOrig.m_bAdjustDst
            m_bAdjustOrg = blOrig.m_bAdjustOrg
            m_ArrowDst = DirectCast(blOrig.m_ArrowDst.Clone(Me, bCutData, doRoot), Arrow)
            m_ArrowDst.ParentLink = Me
            m_ArrowMid = DirectCast(blOrig.m_ArrowMid.Clone(Me, bCutData, doRoot), Arrow)
            m_ArrowMid.ParentLink = Me
            m_ArrowOrg = DirectCast(blOrig.m_ArrowOrg.Clone(Me, bCutData, doRoot), Arrow)
            m_ArrowOrg.ParentLink = Me
            m_eBackMode = blOrig.m_eBackMode
            m_CustomEndCap = blOrig.m_CustomEndCap
            m_CustomStartCap = blOrig.m_CustomStartCap
            m_DashStyle = blOrig.m_DashStyle
            m_clDrawColor = blOrig.m_clDrawColor
            m_iDrawWidth = blOrig.m_iDrawWidth
            m_eEndCap = blOrig.m_eEndCap
            m_Font = blOrig.m_Font
            m_bHidden = blOrig.m_bHidden
            m_eJump = blOrig.m_eJump
            m_eLineStyle = blOrig.m_eLineStyle
            m_bOrthogonalDynamic = blOrig.m_bOrthogonalDynamic
            m_bOrientedText = blOrig.m_bOrientedText
            m_bSelectable = blOrig.m_bSelectable
            m_bStretchable = blOrig.m_bStretchable
            m_eStartCap = blOrig.m_eStartCap
            m_clTextColor = blOrig.m_clTextColor
        End Sub

        Public Overrides Sub AddToHierarchyBar()
            'Add this icon to the heirarchy bar
            If Not m_ParentDiagram Is Nothing Then
                Dim strName As String = Me.TreeViewName

                m_tnTreeNode = m_ParentDiagram.LinksTreeNode.Nodes.Add(strName)
                If Me.ImageName.Trim.Length > 0 Then
                    m_tnTreeNode.ImageIndex = m_ParentEditor.HierarchyBar.ImageManager.GetImageIndex(Me.ImageName)
                    m_tnTreeNode.SelectedImageIndex = m_ParentEditor.HierarchyBar.ImageManager.GetImageIndex(Me.ImageName)
                End If

                If Me.Enabled Then
                    m_tnTreeNode.BackColor = Color.White
                Else
                    m_tnTreeNode.BackColor = Color.Gray
                End If

                m_tnTreeNode.Tag = Me
            End If
        End Sub

        Public Overrides Sub UpdateTreeNode()
            If Not m_ParentDiagram Is Nothing AndAlso Not m_tnTreeNode Is Nothing Then
                m_tnTreeNode.Text = Me.TreeViewName
            End If
        End Sub

        Public Overridable Sub BeforeAddLink()
        End Sub

        Public Overridable Sub AfterAddLink()
            AddToHierarchyBar()
            CheckForErrors()
        End Sub

        Public Overridable Sub BeforeRemoveLink()
        End Sub

        Public Overridable Sub AfterRemoveLink()
            RemoveFromHierarchyBar()
            CheckForErrors()
        End Sub

        Public Overrides Sub AfterUndoRemove()
            AddToHierarchyBar()
            ClearErrors()
        End Sub

        Public Overrides Function CanCopy() As Boolean

            'We should only attempt to copy a synapse if both its origin and destination nodes are also being copied.
            If Me.ParentEditor Is Nothing OrElse m_bnOrigin Is Nothing OrElse m_bnDestination Is Nothing Then
                Return False
            End If

            If (Me.ParentEditor.SelectedObjects.Contains(Me.ActualOrigin) OrElse Me.ParentEditor.SelectedObjects.Contains(Me.Origin)) _
               AndAlso (Me.ParentEditor.SelectedObjects.Contains(Me.ActualDestination) OrElse Me.ParentEditor.SelectedObjects.Contains(Me.Destination)) Then
                Return True
            End If

            Return False
        End Function

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

            Dim oOrg As Object = Util.Environment.FindOrganism(strStructureName, bThrowError)
            If oOrg Is Nothing Then Return Nothing

            Dim doOrganism As AnimatTools.DataObjects.Physical.Organism = DirectCast(oOrg, AnimatTools.DataObjects.Physical.Organism)
            Dim doLink As AnimatTools.DataObjects.Behavior.Link

            If Not doOrganism Is Nothing Then
                If doOrganism.BehavioralLinks.Contains(strDataItemID) Then
                    doLink = doOrganism.FindBehavioralLink(strDataItemID, False)
                Else
                    If bThrowError Then
                        Throw New System.Exception("The drag object with id '" & strDataItemID & "' was not found.")
                    End If
                End If
            End If

            Return doLink

        End Function

        Public Overrides Sub DoubleClicked()

            Dim frmProperties As New Forms.Properties

            frmProperties.PropertyData = Me.Properties
            frmProperties.StartPosition = FormStartPosition.CenterScreen
            frmProperties.Title = Me.TypeName & " Properties"
            frmProperties.MinimizeBox = False
            frmProperties.MaximizeBox = False
            frmProperties.Width = 400
            frmProperties.Height = 800
            frmProperties.ShowDialog(Me.m_ParentEditor)

        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
            frmDataItem.ImageManager.AddImage(myAssembly, Me.ImageName)

            Dim tnNode As TreeNode = tnParent.Nodes.Add(Me.TreeViewName)
            tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.ImageName)
            tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.ImageName)
            tnNode.Tag = Me

            Return tnNode
        End Function

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Link Properties", "ID", Me.ID, True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Link Type", GetType(String), "TypeName", _
                                        "Link Properties", "Returns the type of this link.", TypeName(), True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Adjust Destination", m_bAdjustDst.GetType(), "AdjustDestination", _
                                        "Graphical Properties", "Determines whether it is possible to adjust the position of the last point of the link.", m_bAdjustDst))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Adjust Origin", m_bAdjustOrg.GetType(), "AdjustOrigin", _
                                        "Graphical Properties", "Determines whether it is possible to adjust the position of the first point of the link.", m_bAdjustOrg))

            Dim pbArrowBag As Crownwood.Magic.Controls.PropertyBag = m_ArrowDst.Properties
            m_Properties.Properties.Add(New PropertySpec("Arrow Destination", pbArrowBag.GetType(), _
                             "ArrowDestination", "Graphical Properties", "Sets the destination arrow shape of the link.", pbArrowBag, _
                              "", GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

            pbArrowBag = m_ArrowMid.Properties
            m_Properties.Properties.Add(New PropertySpec("Arrow Middle", pbArrowBag.GetType(), _
                             "ArrowMiddle", "Graphical Properties", "Sets the middle arrow shape of the link.", pbArrowBag, _
                              "", GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

            pbArrowBag = m_ArrowOrg.Properties
            m_Properties.Properties.Add(New PropertySpec("Arrow Origin", pbArrowBag.GetType(), _
                             "ArrowOrigin", "Graphical Properties", "Sets the origin arrow shape of the link.", pbArrowBag, _
                              "", GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Back Mode", m_eBackMode.GetType(), "BackMode", _
                                        "Graphical Properties", "Determines whether the background remains untouched or if the background " & _
                                        "is filled with the current background color before the text of the item.", m_eBackMode))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Dash Style", m_DashStyle.GetType(), "DashStyle", _
                                        "Graphical Properties", "Sets the DashStyle of the pen used to draw the item.", m_DashStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Draw Color", m_clDrawColor.GetType(), "DrawColor", _
                                        "Graphical Properties", "Sets the pen color used to draw the item.", m_clDrawColor))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Draw Width", m_iDrawWidth.GetType(), "DrawWidth", _
                                        "Graphical Properties", "Sets the pen width used to draw the item.", m_iDrawWidth))

            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("End Cap", m_eEndCap.GetType(), "EndCap", _
            '                            "Graphical Properties", "Sets the cap used at the end of the link.", m_eEndCap))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Font", m_Font.GetType(), "Font", _
                                        "Graphical Properties", "Sets or returns the font used to display the text associated to the font.", m_Font))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Jump", m_eJump.GetType(), "Jump", _
                                        "Graphical Properties", "Sets the jump style of the link. A jump is displayed at the intersection of 2 links.", m_eJump))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Line Style", m_eLineStyle.GetType(), "LineStyle", _
                                        "Graphical Properties", "Sets the style of the line.", m_eLineStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Orthogonal Dynamic", m_bOrthogonalDynamic.GetType(), "OrthogonalDynamic", _
                                        "Graphical Properties", "Determines whether the line is composed of several orthogonal segments (vertical or horizontal) " & _
                                        "so that the first and last segments of the line are orthogonal to the origin and destination nodes.", m_bOrthogonalDynamic))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Oriented Text", m_bOrientedText.GetType(), "OrientedText", _
                                        "Graphical Properties", "Determines whether whether its text can be drawn in the same direction as the link itself.", m_bOrientedText))

            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Start Cap", m_eStartCap.GetType(), "StartCap", _
            '                            "Graphical Properties", "Sets the cap used at the start of the link.", m_eStartCap))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Text", m_strText.GetType(), "Text", _
                                        "Graphical Properties", "Sets or returns the text associated with the link.", _
                                        m_strText, GetType(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Text Color", m_clTextColor.GetType(), "TextColor", _
                                        "Graphical Properties", "Sets the color used to display the item text.", m_clTextColor))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Graphical Properties", "Sets the description for this link.", m_strToolTip, _
                                        GetType(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_ArrowDst Is Nothing Then m_ArrowDst.ClearIsDirty()
            If Not m_ArrowMid Is Nothing Then m_ArrowMid.ClearIsDirty()
            If Not m_ArrowOrg Is Nothing Then m_ArrowOrg.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            MyBase.LoadData(oXml)

            oXml.IntoElem()  'Into Link Element

            m_strID = Util.LoadID(oXml, "")

            m_strDestinationID = Util.LoadID(oXml, "Destination")
            m_strOriginID = Util.LoadID(oXml, "Origin")
            m_bAdjustDst = oXml.GetChildBool("AdjustDst")
            m_bAdjustOrg = oXml.GetChildBool("AdjustOrg")
            m_ArrowDst.LoadData("ArrowDestination", oXml)
            m_ArrowMid.LoadData("ArrowMiddle", oXml)
            m_ArrowOrg.LoadData("ArrowOrigin", oXml)
            m_eBackMode = DirectCast([Enum].Parse(GetType(enumBackmode), oXml.GetChildString("BackMode"), True), enumBackmode)
            m_DashStyle = DirectCast([Enum].Parse(GetType(System.Drawing.Drawing2D.DashStyle), oXml.GetChildString("DashStyle"), True), System.Drawing.Drawing2D.DashStyle)
            m_clDrawColor.FromArgb(oXml.GetChildInt("DrawColor"))
            m_iDrawWidth = oXml.GetChildInt("DrawWidth")
            m_Font = Util.LoadFont(oXml, "Font")
            m_bHidden = oXml.GetChildBool("Hidden")
            m_eJump = DirectCast([Enum].Parse(GetType(enumJump), oXml.GetChildString("Jump"), True), enumJump)
            m_eLineStyle = DirectCast([Enum].Parse(GetType(enumLineStyle), oXml.GetChildString("LineStyle"), True), enumLineStyle)
            m_bOrthogonalDynamic = oXml.GetChildBool("OrthogonalDynamic")
            m_bOrientedText = oXml.GetChildBool("OrientedText")
            m_bSelectable = oXml.GetChildBool("Selectable")
            m_bStretchable = oXml.GetChildBool("Stretchable")
            m_strText = oXml.GetChildString("Text")
            m_strToolTip = oXml.GetChildString("ToolTip")
            m_strUrl = oXml.GetChildString("Url")
            m_iZOrder = oXml.GetChildInt("ZOrder")

            oXml.OutOfElem()  'Outof Link Element


        End Sub

        Public Overrides Sub InitializeAfterLoad(ByVal iAttempt As Integer)

            Try
                If m_strOriginID.Trim.Length > 0 AndAlso m_strDestinationID.Trim.Length > 0 Then
                    m_bnOrigin = Me.Organism.FindBehavioralNode(m_strOriginID)
                    m_bnDestination = Me.Organism.FindBehavioralNode(m_strDestinationID)
                    UpdateTreeNode()
                Else
                    Throw New System.Exception("Either a destination or origin was missing for this link. '" & Me.ID & "'")
                End If

                m_bInitialized = True

            Catch ex As System.Exception
                m_bInitialized = False
                If iAttempt = 1 Then
                    AnimatTools.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Sub

        Public Overrides Sub FailedToInitialize()

            Try
                If Not Me.ParentDiagram Is Nothing Then
                    Me.ParentDiagram.RemoveLink(Me)
                ElseIf Not Me.Organism Is Nothing Then
                    Me.Organism.BehavioralLinks.Remove(Me.ID)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            If Not m_bnDestination Is Nothing AndAlso Not m_bnOrigin Is Nothing Then
                oXml.AddChildElement("Link")
                oXml.IntoElem()  'Into Link Element

                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                oXml.AddChildElement("ID", Me.SelectedID)
                oXml.AddChildElement("AdjustDst", m_bAdjustDst)
                oXml.AddChildElement("AdjustOrg", m_bAdjustOrg)
                m_ArrowDst.SaveData("ArrowDestination", oXml)
                m_ArrowMid.SaveData("ArrowMiddle", oXml)
                m_ArrowOrg.SaveData("ArrowOrigin", oXml)
                oXml.AddChildElement("BackMode", m_eBackMode.ToString)
                oXml.AddChildElement("DashStyle", m_DashStyle.ToString)
                oXml.AddChildElement("DrawColor", m_clDrawColor.ToArgb)
                oXml.AddChildElement("DrawWidth", m_iDrawWidth)
                oXml.AddChildElement("DestinationID", m_bnDestination.SelectedID)
                Util.SaveFont(oXml, "Font", m_Font)
                oXml.AddChildElement("Hidden", m_bHidden)
                oXml.AddChildElement("Jump", m_eJump.ToString)
                oXml.AddChildElement("LineStyle", m_eLineStyle.ToString)
                oXml.AddChildElement("OrthogonalDynamic", m_bOrthogonalDynamic)
                oXml.AddChildElement("OriginID", m_bnOrigin.SelectedID)
                oXml.AddChildElement("OrientedText", m_bOrientedText)
                oXml.AddChildElement("Selectable", m_bSelectable)
                oXml.AddChildElement("Stretchable", m_bStretchable)
                oXml.AddChildElement("Text", m_strText)
                oXml.AddChildElement("ToolTip", m_strToolTip)
                oXml.AddChildElement("Url", m_strUrl)
                oXml.AddChildElement("ZOrder", m_iZOrder)

                oXml.OutOfElem()  'Outof Link Element
            Else
                Throw New System.Exception("Link '" & Me.ID & "' was missing either an origin or destination node.")
            End If

        End Sub

#End Region

#End Region

    End Class

End Namespace
