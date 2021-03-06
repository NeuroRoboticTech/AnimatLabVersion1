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

    Public MustInherit Class LinkedBodyPart
        Inherits AnimatTools.Framework.DataObject

#Region " Attributes "

        Protected m_doStructure As AnimatTools.DataObjects.Physical.PhysicalStructure
        Protected m_bpBodyPart As AnimatTools.DataObjects.Physical.BodyPart
        Protected m_tpBodyPartType As System.Type

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Property PhysicalStructure() As AnimatTools.DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.PhysicalStructure)
                m_doStructure = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property BodyPart() As AnimatTools.DataObjects.Physical.BodyPart
            Get
                Return m_bpBodyPart
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.BodyPart)
                m_bpBodyPart = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property BodyPartType() As System.Type
            Get
                Return m_tpBodyPartType
            End Get
            Set(ByVal Value As System.Type)
                m_tpBodyPartType = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

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

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As LinkedBodyPart = DirectCast(doOriginal, LinkedBodyPart)

            Dim thOrig As LinkedBodyPart = DirectCast(OrigNode, LinkedBodyPart)
            m_doStructure = thOrig.m_doStructure
            m_bpBodyPart = thOrig.m_bpBodyPart
            m_tpBodyPartType = thOrig.m_tpBodyPartType
        End Sub

        Protected Overrides Sub BuildProperties()
        End Sub

#End Region

    End Class

End Namespace
