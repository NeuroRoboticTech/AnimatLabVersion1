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
Imports AnimatTools.Framework

Namespace Collections

    Public MustInherit Class AnimatDictionaryBase
        Inherits DictionaryBase

        Protected m_doParent As Framework.DataObject
        Protected m_bIsDirty As Boolean = False

        <Browsable(False)> _
        Public Property Parent() As Framework.DataObject
            Get
                Return m_doParent
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doParent = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property IsDirty() As Boolean
            Get
                Return m_bIsDirty
            End Get
            Set(ByVal Value As Boolean)
                If Not Util.DisableDirtyFlags Then
                    m_bIsDirty = Value

                    If m_bIsDirty AndAlso Not m_doParent Is Nothing Then
                        m_doParent.IsDirty = True
                    End If
                End If
            End Set
        End Property

        Public Sub New(ByVal doParent As Framework.DataObject)
            m_doParent = doParent
        End Sub

        Public Sub ClearIsDirty()
            Me.IsDirty = False

            Dim doObject As Framework.DataObject
            For Each deObject As DictionaryEntry In Me
                If TypeOf deObject.Value Is Framework.DataObject Then
                    doObject = DirectCast(deObject.Value, Framework.DataObject)
                    doObject.ClearIsDirty()
                End If
            Next

        End Sub

        Public Overridable Overloads Function GetItem(ByVal iSelIndex As Integer) As Object

            Dim iIndex As Integer = 0
            For Each deEntry As DictionaryEntry In Me
                If iSelIndex = iIndex Then
                    Return deEntry.Value
                End If
            Next

            Throw New System.Exception("No entry with index '" & iSelIndex & "' was found.")
        End Function

        Protected Overridable Sub CopyInternal(ByVal aryOrig As AnimatDictionaryBase)

            Me.Clear()

            Dim doOrig As AnimatTools.Framework.DataObject
            Dim doItem As AnimatTools.Framework.DataObject
            For Each deItem As DictionaryEntry In aryOrig
                If TypeOf deItem.Value Is AnimatTools.Framework.DataObject Then
                    doOrig = DirectCast(deItem.Value, AnimatTools.Framework.DataObject)
                    Dictionary.Add(doOrig.ID, doOrig)
                End If
            Next

        End Sub

        Protected Overridable Sub CloneInternal(ByVal aryOrig As AnimatDictionaryBase, ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatTools.Framework.DataObject)

            Me.Clear()

            Dim doOrig As AnimatTools.Framework.DataObject
            Dim doItem As AnimatTools.Framework.DataObject
            For Each deItem As DictionaryEntry In aryOrig
                If TypeOf deItem.Value Is AnimatTools.Framework.DataObject Then
                    doOrig = DirectCast(deItem.Value, AnimatTools.Framework.DataObject)
                    doItem = doOrig.Clone(doParent, bCutData, doRoot)
                    Dictionary.Add(doItem.ID, doItem)
                End If
            Next

        End Sub

        Public Overridable Function Copy() As AnimatDictionaryBase
        End Function

        Public Overridable Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatDictionaryBase
        End Function

        Public Overridable Function FindDataObjectByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As AnimatTools.Framework.DataObject

            Dim doVal As AnimatTools.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatTools.Framework.DataObject Then
                    doVal = DirectCast(deEntry.Value, AnimatTools.Framework.DataObject)
                    If doVal.Name = strName Then
                        Return doVal
                    End If
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No data object with the name '" & strName & "' was found.")
            End If

        End Function

        Public Overridable Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatTools.Framework.DataObject
            Dim doFound As Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatTools.Framework.DataObject Then
                    doObject = DirectCast(deEntry.Value, AnimatTools.Framework.DataObject)
                    doFound = doObject.FindObjectByID(strID)
                    If Not doFound Is Nothing Then
                        Return doFound
                    End If
                End If
            Next

            Return Nothing
        End Function

        Public Overridable Sub DumpListInfo()

            'Debug.WriteLine("")

            Dim iNum As Integer = 0
            Dim doVal As AnimatTools.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatTools.Framework.DataObject Then
                    doVal = DirectCast(deEntry.Value, AnimatTools.Framework.DataObject)
                    Debug.WriteLine(iNum.ToString() & ": " & doVal.Name & "  " & doVal.ID)
                End If
            Next

            'Debug.WriteLine("")
        End Sub

    End Class

End Namespace

