Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class LinkedBodyPartTree
        Inherits TypeHelpers.LinkedBodyPart

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doStructure As AnimatTools.DataObjects.Physical.PhysicalStructure, _
                       ByVal bpBodyPart As AnimatTools.DataObjects.Physical.BodyPart, _
                       ByVal tpBodyPartType As System.Type)
            MyBase.New(doStructure)
            m_doStructure = doStructure
            m_bpBodyPart = bpBodyPart
            m_tpBodyPartType = tpBodyPartType
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNew As New LinkedBodyPartTree(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            Return oNew
        End Function

        Protected Overrides Sub BuildProperties()

        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)
            If m_doStructure Is Nothing Then Return

            If Not TypeOf (ctrlDropDown) Is TreeView Then
                Throw New System.Exception("The control passed into LinkedSynapse.BuildPropertyDropDown is not a treeview type")
            End If

            Dim tvTree As TreeView = DirectCast(ctrlDropDown, TreeView)

            tvTree.BeginUpdate()
            tvTree.Nodes.Clear()

            If Util.IsTypeOf(m_tpBodyPartType, GetType(AnimatTools.DataObjects.Physical.Joint), False) Then
                m_doStructure.CreateJointTreeView(tvTree, Nothing, Me)
            Else
                m_doStructure.CreateRigidBodyTreeView(tvTree, Nothing, Me)
            End If

            tvTree.Width = 300
            tvTree.ExpandAll()

            MyBase.FormatDropDownTree(tvTree, 8)

            tvTree.EndUpdate()
            tvTree.Invalidate()

        End Sub

#End Region

    End Class

End Namespace
