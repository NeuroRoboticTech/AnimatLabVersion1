Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls

Namespace DataObjects.Behavior.Nodes

    Public Class Graphical
        Inherits Behavior.Node

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Graphical Item"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "AnimatTools.GraphicalNode.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = Behavior.Node.enumShape.Rectangle
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.BurlyWood

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                Me.Image = AnimatTools.Framework.ImageManager.LoadImage(myAssembly, "AnimatTools.GraphicalNode.gif")
                Me.Name = "Graphical Item"

                Me.Font = New Font("Arial", 12, FontStyle.Bold)
                Me.Description = "Allows the user to place a purely graphical item onto the diagram."

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.Graphical(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub CheckForErrors()
            'There can be no "Errors" for graphical nodes
        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode
        End Function

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace

