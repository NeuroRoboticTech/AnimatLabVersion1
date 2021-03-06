Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.DataObjects

Namespace Collections

    Public Class DiagramImages
        Inherits AnimatDictionaryBase

        Protected m_beEditor As Forms.Behavior.Editor

        Public Property Editor() As Forms.Behavior.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_beEditor = Value
            End Set
        End Property

        Default Public Property Item(ByVal key As [String]) As AnimatTools.DataObjects.Behavior.DiagramImage
            Get
                Return CType(Dictionary(key), AnimatTools.DataObjects.Behavior.DiagramImage)
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Behavior.DiagramImage)
                Dictionary(key) = Value
            End Set
        End Property

        Public ReadOnly Property Keys() As ICollection
            Get
                Return Dictionary.Keys
            End Get
        End Property

        Public ReadOnly Property Values() As ICollection
            Get
                Return Dictionary.Values
            End Get
        End Property

        Public Sub New(ByVal doParent As Framework.DataObject, ByRef beEditor As Forms.Behavior.Editor)
            MyBase.New(doParent)

            m_beEditor = beEditor
        End Sub

        Public Sub Add(ByVal key As [String], ByVal value As AnimatTools.DataObjects.Behavior.DiagramImage)
            Dictionary.Add(key, value)
            Me.IsDirty = True
        End Sub 'Add

        Public Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Public Sub Remove(ByVal key As [String])
            Dictionary.Remove(key)
            Me.IsDirty = True
        End Sub 'Remove

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatTools.DataObjects.Behavior.DiagramImage Then
                Throw New ArgumentException("value must be of type DiagramImage.", "value")
            End If

            Dim diImage As AnimatTools.DataObjects.Behavior.DiagramImage = DirectCast(value, AnimatTools.DataObjects.Behavior.DiagramImage)

            Dim bdDiagram As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_beEditor.Diagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                bdDiagram.AddImage(diImage)
            Next

        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            Dim diImage As AnimatTools.DataObjects.Behavior.DiagramImage = DirectCast(value, AnimatTools.DataObjects.Behavior.DiagramImage)
            RemoveImage(diImage)

        End Sub 'OnRemove

        Protected Sub RemoveImage(ByVal diImage As AnimatTools.DataObjects.Behavior.DiagramImage)

            Dim bdDiagram As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_beEditor.Diagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                bdDiagram.RemoveImage(diImage)
            Next

        End Sub

        Protected Overrides Sub OnClear()

            Dim diImage As AnimatTools.DataObjects.Behavior.DiagramImage
            For Each deEntry As DictionaryEntry In Me
                diImage = DirectCast(deEntry.Value, AnimatTools.DataObjects.Behavior.DiagramImage)
                RemoveImage(diImage)
            Next

        End Sub

        Protected Overrides Sub OnSet(ByVal key As [Object], ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (newValue) Is AnimatTools.DataObjects.Behavior.DiagramImage Then
                Throw New ArgumentException("newValue must be of type DiagramImage.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatTools.DataObjects.Behavior.DiagramImage Then
                Throw New ArgumentException("value must be of type DiagramImage.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New DiagramImages(m_doParent, m_beEditor)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New DiagramImages(doParent, m_beEditor)
            aryList.CloneInternal(Me, doParent, bCutData, doParent)
            Return aryList
        End Function

        Public Overridable Function FindIndexByID(ByVal strID As String) As Integer

            Dim bdDiagram As AnimatTools.DataObjects.Behavior.DiagramImage
            Dim iIndex As Integer = 0
            For Each deEntry As DictionaryEntry In Me
                bdDiagram = DirectCast(deEntry.Value, AnimatTools.DataObjects.Behavior.DiagramImage)
                If bdDiagram.ID = strID Then
                    Return iIndex
                End If
                iIndex = iIndex + 1
            Next

        End Function

        Public Overridable Function FindDiagramImageByID(ByVal strID As String) As AnimatTools.DataObjects.Behavior.DiagramImage

            Dim bdDiagram As AnimatTools.DataObjects.Behavior.DiagramImage
            For Each deEntry As DictionaryEntry In Me
                bdDiagram = DirectCast(deEntry.Value, AnimatTools.DataObjects.Behavior.DiagramImage)
                If bdDiagram.ID = strID Then
                    Return bdDiagram
                End If
            Next

        End Function

        Public Overridable Function FindImageByID(ByVal strID As String) As System.Drawing.Image

            Dim bdDiagram As AnimatTools.DataObjects.Behavior.DiagramImage
            For Each deEntry As DictionaryEntry In Me
                bdDiagram = DirectCast(deEntry.Value, AnimatTools.DataObjects.Behavior.DiagramImage)
                If bdDiagram.ID = strID Then
                    Return bdDiagram.Image
                End If
            Next

        End Function

    End Class 'DiagramImageDictionary

End Namespace
