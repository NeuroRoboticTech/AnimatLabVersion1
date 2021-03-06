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

    Public MustInherit Class NodeStimulus
        Inherits AnimatTools.DataObjects.ExternalStimuli.Stimulus

#Region " Attributes "

        Protected m_doOrganism As DataObjects.Physical.Organism
        Protected m_doNode As DataObjects.Behavior.Node

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Organism() As DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As DataObjects.Physical.Organism)
                m_doOrganism = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property OrganismName() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Node() As DataObjects.Behavior.Node
            Get
                Return m_doNode
            End Get
            Set(ByVal Value As DataObjects.Behavior.Node)
                m_doNode = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property NodeName() As String
            Get
                If Not m_doNode Is Nothing Then
                    Return m_doNode.Text
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusModuleName() As String
            Get
                If Not m_doNode Is Nothing Then
                    Return m_doNode.DataColumnModuleName
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusNoLongerValid() As Boolean
            Get
                Dim doOrganism As DataObjects.Physical.Organism
                Dim doNode As DataObjects.Behavior.Node

                Try
                    doOrganism = DirectCast(Util.Environment.Organisms(m_doOrganism.ID), DataObjects.Physical.Organism)
                    doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
                Catch ex As System.Exception
                End Try

                If Not doOrganism Is Nothing AndAlso Not doNode Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doNode As DataObjects.ExternalStimuli.NodeStimulus = DirectCast(doOriginal, DataObjects.ExternalStimuli.NodeStimulus)

            m_doOrganism = doNode.m_doOrganism
            m_doNode = doNode.m_doNode
        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            If Not m_doOrganism Is Nothing Then
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Organism", GetType(String), "OrganismName", _
                                            "Stimulus Properties", "The name of the organism to which this stimulus is applied.", Me.OrganismName, True))
            End If

            If Not m_doNode Is Nothing Then
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Node", GetType(String), "NodeName", _
                                            "Stimulus Properties", "The name of the node to which this stimulus is applied.", Me.NodeName, True))
            End If

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            Dim strOrganismID As String = oXml.GetChildString("OrganismID")
            If strOrganismID.Trim.Length > 0 Then
                m_doOrganism = DirectCast(Util.Environment.Organisms(strOrganismID), DataObjects.Physical.Organism)
            End If

            Dim strNodeID As String = oXml.GetChildString("NodeID")
            If Not m_doOrganism Is Nothing AndAlso strNodeID.Trim.Length > 0 Then
                m_doNode = m_doOrganism.FindBehavioralNode(strNodeID, False)
            End If

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            If Not m_doOrganism Is Nothing AndAlso Not m_doNode Is Nothing Then
                oXml.AddChildElement("OrganismID", m_doOrganism.ID)
                oXml.AddChildElement("NodeID", m_doNode.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace
