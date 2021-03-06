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

Namespace DataObjects.Charting

    Public Class DataColumn
        Inherits Framework.DataObject

#Region " Attributes "

        'The dataitem module and class names are for the class that was dragged. 
        'For instance it is a FastNeuralNet.DataObjects.Neurons.Neuron class. But the
        'the ColumnModuleName and class type is the data needed to create the appropriate
        'class inside the c++ simulation.
        Protected m_strDataItemAssemblyFile As String = ""
        Protected m_strDataItemClassName As String = ""
        Protected m_strStructureID As String = ""
        Protected m_strDataItemID As String = ""
        Protected m_strDataItemName As String = ""
        Protected m_strDataItemImageName As String = ""
        Protected m_imgDataItemImage As Image
        Protected m_imgDataItemDragImage As Image
        Protected m_thDataType As AnimatTools.TypeHelpers.DataTypeID
        Protected m_iDataSubSet As Integer = -1
        Protected m_iPrevDataSubset As Integer = -1
        Protected m_strColumnModuleName As String
        Protected m_strColumnClassType As String
        Protected m_bUseIncomingDataType As Boolean = False
        Protected m_strSelectedDataTypeID As String = ""

        Protected m_tnTreeNode As TreeNode
        Protected m_frmParentAxis As AnimatTools.DataObjects.Charting.Axis

        Protected m_bAutoAddToAxis As Boolean = True

        Protected m_bSelectionInProgress As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property DataItem() As AnimatTools.DataObjects.DragObject
            Get
                Dim doBase As DataObjects.DragObject = DirectCast(Util.LoadClass(m_strDataItemAssemblyFile, m_strDataItemClassName, Me), DataObjects.DragObject)
                Dim doItem As DataObjects.DragObject = doBase.FindDragObject(m_strStructureID, m_strDataItemID, False)
                Return doItem
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.DragObject)
                If Not Value Is Nothing Then
                    Me.DataItemAssemblyFile = Value.AssemblyFile
                    Me.DataItemClassName = Value.ClassName
                    Me.StructureID = Value.StructureID
                    Me.DataItemID = Value.ID
                    Me.DataItemName = Value.ItemName
                    Me.DataItemImageName = Value.ImageName
                    Me.DataItemImage = DirectCast(Value.Image.Clone(), System.Drawing.Image)
                    Me.DataItemDragImage = DirectCast(Value.DragImage.Clone(), System.Drawing.Image)
                    m_strColumnModuleName = Value.DataColumnModuleName
                    m_strColumnClassType = Value.DataColumnClassType

                    'By default we should use the normal Datatypes property from the selected object, but in 
                    'some instances we might want to limit this choice to the incoming data type for the selected object
                    If Not m_bUseIncomingDataType Then
                        m_thDataType = DirectCast(Value.DataTypes.Clone(Me, False, Nothing), AnimatTools.TypeHelpers.DataTypeID)

                        If m_thDataType.DataTypes.Contains(m_strSelectedDataTypeID) Then
                            m_thDataType.ID = m_strSelectedDataTypeID
                        End If
                    Else
                        m_thDataType = New TypeHelpers.DataTypeID(Me)
                        If Not Value.IncomingDataType Is Nothing Then
                            m_thDataType.DataTypes.Add(DirectCast(Value.IncomingDataType.Clone(Value, False, Nothing), DataObjects.DataType))
                            m_thDataType.ID = Value.IncomingDataType.ID
                        End If
                    End If
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length <= 0 Then
                    Throw New System.Exception("The name of the data column can not be blank.")
                End If

                If m_bAutoAddToAxis Then
                    m_strName = Value

                    If Not m_tnTreeNode Is Nothing Then
                        m_tnTreeNode.Remove()
                    End If
                    Me.CreateTreeView()
                Else
                    m_strName = Value
                End If

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StructureID() As String
            Get
                Return m_strStructureID
            End Get
            Set(ByVal Value As String)
                m_strStructureID = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemAssemblyFile() As String
            Get
                Return m_strDataItemAssemblyFile
            End Get
            Set(ByVal Value As String)
                m_strDataItemAssemblyFile = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemClassName() As String
            Get
                Return m_strDataItemClassName
            End Get
            Set(ByVal Value As String)
                m_strDataItemClassName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemID() As String
            Get
                Return m_strDataItemID
            End Get
            Set(ByVal Value As String)
                m_strDataItemID = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemName() As String
            Get
                Return m_strDataItemName
            End Get
            Set(ByVal Value As String)
                m_strDataItemName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemImageName() As String
            Get
                Return m_strDataItemImageName
            End Get
            Set(ByVal Value As String)
                m_strDataItemImageName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemImage() As Image
            Get
                Return m_imgDataItemImage
            End Get
            Set(ByVal Value As Image)
                m_imgDataItemImage = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataItemDragImage() As Image
            Get
                Return m_imgDataItemDragImage
            End Get
            Set(ByVal Value As Image)
                m_imgDataItemDragImage = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TreeNode() As TreeNode
            Get
                Return m_tnTreeNode
            End Get
            Set(ByVal Value As TreeNode)
                m_tnTreeNode = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataType() As AnimatTools.TypeHelpers.DataTypeID
            Get
                Return m_thDataType
            End Get
            Set(ByVal Value As AnimatTools.TypeHelpers.DataTypeID)
                If Not Value Is Nothing Then
                    m_thDataType.ID = Value.ID

                    If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
                       AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then
                        If Util.Application.SimulationView.Simulator.FindDataColumn(Me.ParentAxis.ParentChart.ID, Me.ID, False) Then
                            Util.Application.SimulationView.Simulator.ModifyDataColumn(Me.ParentAxis.ParentChart.ID, Me.ID, Value.ID.ToString())
                        End If
                    End If

                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ParentAxis() As AnimatTools.DataObjects.Charting.Axis
            Get
                Return m_frmParentAxis
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Charting.Axis)
                m_frmParentAxis = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DataSubSet() As Integer
            Get
                Return m_iDataSubSet
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Throw New System.Exception("The data subset value must not be less than zero.")
                End If

                m_iPrevDataSubset = m_iDataSubSet
                m_iDataSubSet = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PrevDataSubSet() As Integer
            Get
                Return m_iPrevDataSubset
            End Get
            Set(ByVal Value As Integer)
                m_iPrevDataSubset = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ColumnModuleName() As String
            Get
                Return m_strColumnModuleName
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ColumnClassType() As String
            Get
                Return m_strColumnClassType
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property AutoAddToAxis() As Boolean
            Get
                Return m_bAutoAddToAxis
            End Get
            Set(ByVal Value As Boolean)
                m_bAutoAddToAxis = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LineColor() As System.Drawing.Color
            Get
                Return System.Drawing.Color.Red
            End Get
            Set(ByVal Value As System.Drawing.Color)

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseIncomingDataType() As Boolean
            Get
                Return m_bUseIncomingDataType
            End Get
            Set(ByVal Value As Boolean)
                m_bUseIncomingDataType = Value
            End Set
        End Property

        'This property is set when the select data item dialog is shown.
        'It keeps datacolumn properties from trying to update the chart
        'while users are simply trying to configure the item they are selecting.
        <Browsable(False)> _
        Public Overridable Property SelectionInProgress() As Boolean
            Get
                Return m_bSelectionInProgress
            End Get
            Set(ByVal Value As Boolean)
                m_bSelectionInProgress = Value
            End Set
        End Property

        Public Overridable ReadOnly Property RequiresAutoDataCollectInterval() As Boolean
            Get
                If m_strDataItemAssemblyFile.Trim.Length = 0 OrElse m_strDataItemClassName.Trim.Length = 0 Then
                    Return False
                End If

                Dim doBase As DataObjects.DragObject = DirectCast(Util.LoadClass(m_strDataItemAssemblyFile, m_strDataItemClassName, Me), DataObjects.DragObject)
                Dim oItem As DataObjects.DragObject = DirectCast(doBase.FindDragObject(m_strStructureID, m_strDataItemID, False), DataObjects.DragObject)

                If Not oItem Is Nothing Then
                    Return oItem.RequiresAutoDataCollectInterval
                Else
                    Return False
                End If
            End Get
        End Property


#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            If TypeOf doParent Is Axis Then
                m_frmParentAxis = DirectCast(doParent, AnimatTools.DataObjects.Charting.Axis)
            End If
        End Sub

        Public Sub AddtoAxis(ByVal doAxis As DataObjects.Charting.Axis)
            m_frmParentAxis = doAxis
            Dim strName As String = Me.Name
            Me.Name = strName

            m_frmParentAxis.AddColumnToSimulation(Me, False)

            'Setting the name automatically adds it into the list of data columns.
            m_frmParentAxis.ParentChart.Viewer.HierarchyBar.TreeView.SelectedNode = Me.TreeNode
        End Sub

        Public Sub RemoveFromAxis()
            m_frmParentAxis.DataColumns.Remove(Me.ID)

            If Not m_tnTreeNode Is Nothing Then
                m_tnTreeNode.Remove()
                m_tnTreeNode = Nothing
            End If

            'Remove data columns if it exists.
            m_frmParentAxis.RemoveColumnFromSimulation(Me)

            'If we are automatically setting the collect data interval then lets recalculate the value
            If Not m_frmParentAxis Is Nothing AndAlso Not m_frmParentAxis.ParentChart Is Nothing AndAlso m_frmParentAxis.ParentChart.AutoCollectDataInterval Then
                m_frmParentAxis.ParentChart.ResetCollectDataInterval()
            End If

        End Sub

        Public Overridable Sub UpdateChartConfiguration(ByRef iSubSet As Integer)
        End Sub

        Public Overridable Sub ReconfigureChartData(ByRef aryOldX(,) As Single, ByRef aryOldY(,) As Single, ByRef aryNewX(,) As Single, ByRef aryNewY(,) As Single)
        End Sub

        Public Overridable Sub PrepareForCharting()
            Me.ParentAxis.AddColumnToSimulation(Me, True)
        End Sub

        Public Overridable Function IsValidColumn() As Boolean

            If m_strDataItemAssemblyFile.Trim.Length = 0 OrElse m_strDataItemClassName.Trim.Length = 0 Then
                Return False
            End If

            Dim doBase As DataObjects.DragObject = DirectCast(Util.LoadClass(m_strDataItemAssemblyFile, m_strDataItemClassName, Me), DataObjects.DragObject)
            Dim oItem As Object = doBase.FindDragObject(m_strStructureID, m_strDataItemID, False)

            If Not oItem Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overridable Function SaveDataColumnToXml() As String
            Dim oXml As New AnimatTools.Interfaces.StdXml

            oXml.AddElement("Data")
            oXml.AddChildElement("DataColumn")
            SaveDataColumnXml(oXml)

            Return oXml.Serialize()
        End Function

        Public Overridable Sub SaveXml(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.AddChildElement("DataColumn")
            SaveDataColumnXml(oXml)

        End Sub

        Public Overridable Sub SaveDataColumnXml(ByRef oXml As AnimatTools.Interfaces.StdXml)
            Dim doItem As AnimatTools.DataObjects.DragObject = Me.DataItem

            If doItem Is Nothing Then
                Throw New System.Exception("No data item was defined that could be used to create a data column in the simulation.")
            End If

            oXml.IntoElem()
            oXml.AddChildElement("ModuleName", m_strColumnModuleName)
            oXml.AddChildElement("Type", m_strColumnClassType)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("ColumnName", m_strName)
            oXml.AddChildElement("DataType", m_thDataType.ID.ToString())
            oXml.OutOfElem()

            doItem.SaveDataColumnToXml(oXml)
        End Sub

        Public Overridable Function CreateDataColumn(ByVal doItem As AnimatTools.DataObjects.DragObject, Optional ByVal bAutoAddToAxis As Boolean = True) As AnimatTools.DataObjects.Charting.DataColumn
            Dim doColumn As New DataObjects.Charting.DataColumn(Me.ParentAxis)

            Dim strName As String = doItem.ItemName

            doColumn.UseIncomingDataType = Me.UseIncomingDataType
            doColumn.DataItem = doItem.DataColumnItem
            doColumn.AutoAddToAxis = bAutoAddToAxis
            doColumn.Name = strName
            Return doColumn
        End Function

#Region " TreeView Methods "

        Public Overridable Sub CreateTreeView()

            If Not m_frmParentAxis Is Nothing AndAlso Not m_frmParentAxis.ParentChart Is Nothing AndAlso Not m_frmParentAxis.TreeNode Is Nothing AndAlso Not m_frmParentAxis.ParentChart.Viewer Is Nothing Then
                m_frmParentAxis.ParentChart.Viewer.HierarchyBar.ImageManager.AddImage(Me.DataItemImageName, Me.DataItemImage)
                m_frmParentAxis.ParentChart.Viewer.HierarchyBar.DragImageManager.AddImage(Me.DataItemImageName, Me.DataItemDragImage)

                m_tnTreeNode = m_frmParentAxis.TreeNode.Nodes.Add(Me.Name)
                m_tnTreeNode.ImageIndex = m_frmParentAxis.ParentChart.Viewer.HierarchyBar.ImageManager.GetImageIndex(Me.DataItemImageName)
                m_tnTreeNode.SelectedImageIndex = m_frmParentAxis.ParentChart.Viewer.HierarchyBar.ImageManager.GetImageIndex(Me.DataItemImageName)
                m_tnTreeNode.Tag = Me

                m_frmParentAxis.ParentChart.Viewer.HierarchyBar.TreeView.SelectedNode = m_tnTreeNode
            End If

        End Sub

        Public Overridable Function TreeviewPopupMenu(ByRef tnSelectedNode As TreeNode, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnTreeNode Then
                ' Create the menu items
                Dim mcDelete As New MenuCommand("Delete Column ", "DeleteColumn", Util.Application.SmallImages.ImageList, _
                                                  Util.Application.SmallImages.GetImageIndex("AnimatTools.Delete.gif"), _
                                                  New EventHandler(AddressOf Me.OnDeleteColumn))

                ' Create the popup menu object
                Dim popup As New PopupMenu
                popup.MenuCommands.Add(mcDelete)

                ' Show it!
                Dim selected As MenuCommand = popup.TrackPopup(ptPoint)

                Return True
            End If

            Return False
        End Function

#End Region

        Protected Overrides Sub BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Column Properties", "ID", Me.ID, True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", GetType(String), "Name", _
                                        "Column Properties", "The name of this column.", Name()))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Item Name", GetType(String), "DataItemName", _
                                        "Column Properties", "The data item of this column.", DataItemName(), True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Data Type", GetType(AnimatTools.TypeHelpers.DataTypeID), "DataType", _
                                            "Column Properties", "Sets the type of data to chart.", Me.DataType, _
                                            GetType(AnimatTools.TypeHelpers.DropDownListEditor), _
                                            GetType(AnimatTools.TypeHelpers.DataTypeIDTypeConverter)))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doItem As New DataColumn(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigColumn As DataColumn = DirectCast(doOriginal, DataColumn)

            m_strName = doOrigColumn.m_strName
            m_strDataItemAssemblyFile = doOrigColumn.m_strDataItemAssemblyFile
            m_strDataItemClassName = doOrigColumn.m_strDataItemClassName
            m_strStructureID = doOrigColumn.m_strStructureID
            m_strDataItemID = doOrigColumn.m_strDataItemID
            m_strDataItemName = doOrigColumn.m_strDataItemName
            m_strDataItemImageName = doOrigColumn.m_strDataItemImageName
            m_bUseIncomingDataType = doOrigColumn.m_bUseIncomingDataType

            If Not doOrigColumn.m_imgDataItemImage Is Nothing Then
                m_imgDataItemImage = DirectCast(doOrigColumn.m_imgDataItemImage.Clone, Image)
            End If

            If Not doOrigColumn.m_imgDataItemDragImage Is Nothing Then
                m_imgDataItemDragImage = DirectCast(doOrigColumn.m_imgDataItemDragImage.Clone, Image)
            End If

            If Not doOrigColumn.m_thDataType Is Nothing Then
                m_thDataType = DirectCast(doOrigColumn.m_thDataType.Clone(Me, bCutData, doRoot), TypeHelpers.DataTypeID)
            End If

            m_iDataSubSet = doOrigColumn.m_iDataSubSet
            m_iPrevDataSubset = doOrigColumn.m_iPrevDataSubset
            m_strColumnModuleName = doOrigColumn.m_strColumnModuleName
            m_strColumnClassType = doOrigColumn.m_strColumnClassType
            m_frmParentAxis = doOrigColumn.m_frmParentAxis
            m_bAutoAddToAxis = doOrigColumn.m_bAutoAddToAxis

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.IntoElem()

            m_strName = oXml.GetChildString("Name")
            m_strID = Util.LoadID(oXml, "")
            m_strDataItemAssemblyFile = oXml.GetChildString("DataItemAssemblyFile")
            m_strDataItemClassName = oXml.GetChildString("DataItemClassName")
            m_strDataItemID = Util.LoadID(oXml, "DataItem")
            m_iDataSubSet = oXml.GetChildInt("DataSubSet", m_iDataSubSet)
            m_iPrevDataSubset = oXml.GetChildInt("PrevDataSubset", m_iPrevDataSubset)
            m_bUseIncomingDataType = oXml.GetChildBool("UseIncomingDataType", m_bUseIncomingDataType)

            If oXml.FindChildElement("StructureID", False) Then
                m_strStructureID = oXml.GetChildString("StructureID")
            Else
                m_strStructureID = oXml.GetChildString("StructureName")
            End If

            m_strSelectedDataTypeID = oXml.GetChildString("SelectedDataType")

            'This statement looks a little weird. We do not keep a reference to the actual data item we are charting.
            'There are several reasons for this, but the main one is that this object can be deleted in its editor and
            'then we will be trying to chart an item that no longer exists. Instead we have a way to lookup that item
            'when it is needed to check each time that it is still valid (during load/save for example.) So in this statement
            'we first look up the item with the get accessor and then set id data with the set mutator.
            Me.DataItem = Me.DataItem

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            SaveDataWithName(oXml, "DataColumn")
        End Sub

        Public Overridable Sub SaveDataWithName(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strDataName As String)

            oXml.AddChildElement(strDataName)
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("DataItemAssemblyFile", m_strDataItemAssemblyFile)
            oXml.AddChildElement("DataItemClassName", m_strDataItemClassName)
            oXml.AddChildElement("StructureID", m_strStructureID)
            oXml.AddChildElement("DataItemID", m_strDataItemID)
            oXml.AddChildElement("SelectedDataType", Me.DataType().ID)
            oXml.AddChildElement("DataSubSet", m_iDataSubSet)
            oXml.AddChildElement("PrevDataSubset", m_iPrevDataSubset)
            oXml.AddChildElement("UseIncomingDataType", m_bUseIncomingDataType)

            oXml.OutOfElem()

        End Sub

#End Region

#Region " Events "

        Protected Sub OnDeleteColumn(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If MessageBox.Show("Are you certain that you want to delete this " & _
                                    "data column?", "Delete Axis", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    If Not m_frmParentAxis Is Nothing AndAlso m_frmParentAxis.DataColumns.Contains(Me.ID) Then
                        Me.RemoveFromAxis()
                    End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace

