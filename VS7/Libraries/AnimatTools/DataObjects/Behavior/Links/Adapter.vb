Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls

Namespace DataObjects.Behavior.Links

    Public Class Adapter
        Inherits Behavior.Link

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Adapter Link"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "AnimatTools.AdapterLink.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_ArrowDst = New Arrow(Me, Link.enumArrowStyle.None, Link.enumArrowSize.Small, Link.enumArrowAngle.deg15, False)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatTools")

            Me.Image = AnimatTools.Framework.ImageManager.LoadImage(myAssembly, "AnimatTools.AdapterLink.gif")
            Me.Name = "Adapter Link"

            Me.Font = New Font("Arial", 12)
            Me.Description = "Adds a link between a node and an adapter."

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewLink As New Behavior.Links.Adapter(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Public Overrides Sub AfterRemoveLink()

            Dim bnAdapter As Behavior.Node

            If TypeOf (Me.Origin) Is Behavior.Nodes.Adapter Then
                bnAdapter = Me.Origin
            ElseIf TypeOf (Me.Destination) Is Behavior.Nodes.Adapter Then
                bnAdapter = Me.Destination
            Else
                Return
            End If

            'If we still see a link to this adapter then we need to remove the adapter as well.
            'I have no idea why, but if I make this call to RemoveNode while I am deleting the entire
            'diagram then it kills the ability of the parent diagram to be able to update itself.
            'If this adpater is in a subsystem that is deleted, then I can no longer click on items
            'in that parent diagram and select them???? No idea why. But if I do not make this call
            'then everything appears to work fine???
            If bnAdapter.Links.Count > 0 AndAlso Not Me.ParentDiagram.DeletingDiagram Then
                m_ParentDiagram.RemoveNode(bnAdapter)
            End If

            RemoveFromHierarchyBar()
            CheckForErrors()
        End Sub

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace

