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

Namespace DataObjects.Physical

    Public Class Odor
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_doOdorType As OdorType
        Protected m_fltQuantity As Single = 100
        Protected m_bUseFoodQuantity As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                If Not m_doOdorType Is Nothing Then
                    Return m_doOdorType.Name
                Else
                    Return ""
                End If
            End Get
            Set(ByVal Value As String)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OdorType() As OdorType
            Get
                Return m_doOdorType
            End Get
            Set(ByVal Value As OdorType)
                If Not Value Is Nothing Then
                    m_doOdorType = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DiffusionConstant() As ScaledNumber
            Get
                If Not m_doOdorType Is Nothing Then
                    Return m_doOdorType.DiffusionConstant
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Quantity() As Single
            Get
                If m_bUseFoodQuantity Then
                    If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is AnimatTools.DataObjects.Physical.RigidBody Then
                        Dim doPart As AnimatTools.DataObjects.Physical.RigidBody = DirectCast(Me.Parent, AnimatTools.DataObjects.Physical.RigidBody)
                        Return CType(doPart.FoodQuantity.ActualValue, Single)
                    Else
                        Return m_fltQuantity
                    End If
                Else
                    Return m_fltQuantity
                End If
            End Get
            Set(ByVal Value As Single)
                If Value < 0 Then
                    Throw New System.Exception("Quantity must be greater than or eqaul to zero.")
                End If

                m_fltQuantity = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseFoodQuantity() As Boolean
            Get
                Return m_bUseFoodQuantity
            End Get
            Set(ByVal Value As Boolean)
                m_bUseFoodQuantity = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Protected Overrides Sub BuildProperties()

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Odor Properties", "The name of this odor.", m_strName, True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Odor Properties", "ID", Me.ID, True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Use Food Quantity", m_bUseFoodQuantity.GetType(), "UseFoodQuantity", _
                                        "Odor Properties", "If this is true then the odor quantity is determined by the food quantity value of this part.", m_bUseFoodQuantity))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Quantity", m_fltQuantity.GetType(), "Quantity", _
                                        "Odor Properties", "The quantity of substance producing this odor.", m_fltQuantity, m_bUseFoodQuantity))

            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = Me.DiffusionConstant.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Diffusion Constant", pbNumberBag.GetType(), "DiffusionConstant", _
                                        "Odor Type Properties", "Sets the rate of diffusion of this odor.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doItem As New Odor(doParent)

            doItem.m_doOdorType = m_doOdorType
            doItem.m_fltQuantity = m_fltQuantity
            doItem.m_bUseFoodQuantity = m_bUseFoodQuantity

            Return doItem
        End Function

        Public Overridable Overloads Sub SaveData(ByRef dsSim As Simulation, ByRef oXml As Interfaces.StdXml)

            If Not m_doOdorType Is Nothing Then
                oXml.AddChildElement("Odor")
                oXml.IntoElem()

                oXml.AddChildElement("Name", m_doOdorType.Name)
                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("OdorTypeID", m_doOdorType.ID)
                oXml.AddChildElement("Quantity", m_fltQuantity)
                oXml.AddChildElement("UseFoodQuantity", m_bUseFoodQuantity)

                oXml.OutOfElem()
            End If

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef dsSim As Simulation, ByRef oXml As Interfaces.StdXml)

            Try
                oXml.FindChildElement("Odor")
                oXml.IntoElem() 'Into OdorType Element

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID")
                Dim strOdorTypeID As String = oXml.GetChildString("OdorTypeID")
                m_fltQuantity = oXml.GetChildFloat("Quantity")
                m_bUseFoodQuantity = oXml.GetChildBool("UseFoodQuantity", False)

                oXml.OutOfElem() 'Outof OdorType Element

                If Util.Environment.OdorTypes.Contains(strOdorTypeID) Then
                    m_doOdorType = Util.Environment.OdorTypes(strOdorTypeID)

                    If Not m_doOdorType.OdorSources.Contains(Me.ID) Then
                        m_doOdorType.OdorSources.Add(Me.ID, Me)
                    End If
                Else
                    Throw New System.Exception("No odor type with ID '" & strOdorTypeID & "' was found.")
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
