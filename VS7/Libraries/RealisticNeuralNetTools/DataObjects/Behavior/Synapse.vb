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

    Public Class Synapse
        Inherits AnimatTools.DataObjects.Behavior.Links.Synapse

#Region " Attributes "

        Protected m_stSynapseType As SynapseType

        Protected m_strUserText As String = ""
        Protected m_snSynapticConductance As AnimatTools.Framework.ScaledNumber
        Protected m_snConductionDelay As AnimatTools.Framework.ScaledNumber

        'Only used during loading
        Protected m_strSynapticTypeID As String = ""

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.TypeName
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(RealisticNeuralNetTools.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable Property SynapseType() As SynapseType
            Get
                Return m_stSynapseType
            End Get
            Set(ByVal Value As SynapseType)
                Dim bReselect As Boolean = False

                If Not m_stSynapseType Is Nothing Then
                    RemoveHandler m_stSynapseType.SynapseTypeChanged, AddressOf Me.OnSynapseTypeChanged

                    'If we change the class of synapse types. (electical to spiking) then we need to reselect the object
                    'because different types display slightly different data for the synapse
                    If Not Value Is Nothing AndAlso Not m_stSynapseType.GetType() Is Value.GetType() Then
                        bReselect = True
                    End If
                End If

                m_stSynapseType = Value

                If Not m_stSynapseType Is Nothing AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                    Dim scType As SynapseTypes.SpikingChemical = DirectCast(m_stSynapseType, SynapseTypes.SpikingChemical)
                    Me.SynapticConductance = DirectCast(scType.SynapticConductance.Clone(Me, False, Nothing), ScaledNumber)
                End If

                If Not m_stSynapseType Is Nothing Then
                    AddHandler m_stSynapseType.SynapseTypeChanged, AddressOf Me.OnSynapseTypeChanged
                End If

                If bReselect AndAlso Not m_ParentDiagram Is Nothing Then
                    m_ParentDiagram.SelectDataItem(Me)
                End If

                UpdateChart(True)
            End Set
        End Property

        Public Overridable ReadOnly Property SynapseTypeName() As String
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.Name
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overridable Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                If Not m_snSynapticConductance Is Nothing AndAlso Not m_stSynapseType Is Nothing _
                   AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                    Me.Text = m_snSynapticConductance.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_strUserText
                End If
            End Set
        End Property

        Public Overridable Property SynapticConductance() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snSynapticConductance
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.0001 Then
                    Throw New System.Exception("The synaptic conductance must be between the range 0 to 100 uS/size.")
                End If

                m_snSynapticConductance.CopyData(Value)

                If Not m_snSynapticConductance Is Nothing AndAlso Not m_stSynapseType Is Nothing _
                   AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                    Me.Text = m_snSynapticConductance.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_strUserText
                End If
            End Set
        End Property

        Public Overridable Property ConductionDelay() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snConductionDelay
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.1 Then
                    Throw New System.Exception("The conduction delay must be between the range 0 to 100 ms.")
                End If

                m_snConductionDelay.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ImageName() As String
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.ImageName
                Else
                    Return ""
                End If
            End Get
        End Property

#Region " Overridden Link Properties "

        Public Overrides Property ArrowDestination() As Arrow
            Get
                Return m_stSynapseType.ArrowDestination
            End Get
            Set(ByVal Value As Arrow)
                m_stSynapseType.ArrowDestination = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property ArrowMiddle() As Arrow
            Get
                Return m_stSynapseType.ArrowMiddle
            End Get
            Set(ByVal Value As Arrow)
                m_stSynapseType.ArrowMiddle = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property ArrowOrigin() As Arrow
            Get
                Return m_stSynapseType.ArrowOrigin
            End Get
            Set(ByVal Value As Arrow)
                m_stSynapseType.ArrowOrigin = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property DashStyle() As System.Drawing.Drawing2D.DashStyle
            Get
                Return m_stSynapseType.DashStyle
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.DashStyle)
                m_stSynapseType.DashStyle = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property DrawColor() As System.Drawing.Color
            Get
                Return m_stSynapseType.DrawColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_stSynapseType.DrawColor = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property DrawWidth() As Integer
            Get
                Return m_stSynapseType.DrawWidth
            End Get
            Set(ByVal Value As Integer)
                m_stSynapseType.DrawWidth = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property Font() As System.Drawing.Font
            Get
                Return m_stSynapseType.Font
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_stSynapseType.Font = Value
                UpdateChart()
            End Set
        End Property

#End Region

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_stSynapseType = New SynapseTypes.SpikingChemical(Me)

            m_snSynapticConductance = New AnimatTools.Framework.ScaledNumber(Me, "SynapticConductance", 0.5, AnimatTools.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
            m_snConductionDelay = New AnimatTools.Framework.ScaledNumber(Me, "ConductionDelay", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewLink As New DataObjects.Behavior.Synapse(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim blOrig As DataObjects.Behavior.Synapse = DirectCast(doOriginal, DataObjects.Behavior.Synapse)

            m_stSynapseType = blOrig.m_stSynapseType
            m_snSynapticConductance = DirectCast(blOrig.m_snSynapticConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snConductionDelay = DirectCast(blOrig.m_snConductionDelay.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Protected Sub OnSynapseTypeChanged()
            UpdateChart(True)
        End Sub

        Public Overrides Sub SaveNetwork(ByRef oXml As AnimatTools.Interfaces.StdXml, ByRef nmModule As AnimatTools.DataObjects.Behavior.NeuralModule)

            'If the synapse type is not set then exit.
            If m_stSynapseType Is Nothing Then
                Return
            End If

            'If either the origin or destinationi is missing then jump out
            If Me.Origin Is Nothing OrElse Me.Destination Is Nothing Then
                Return
            End If

            'Both the origin and destination must be based on a spiking neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(DataObjects.Behavior.Neurons.Spiking), False) OrElse _
               Not Util.IsTypeOf(Me.Destination.GetType(), GetType(DataObjects.Behavior.Neurons.Spiking), False) Then
                Return
            End If

            oXml.AddChildElement("Connexion")
            oXml.IntoElem()

            oXml.AddChildElement("Source", Me.Origin.NodeIndex)
            oXml.AddChildElement("Target", Me.Destination.NodeIndex)
            oXml.AddChildElement("Type", m_stSynapseType.SynapseType)
            oXml.AddChildElement("ID", m_stSynapseType.LinkIndex)
            oXml.AddChildElement("Delay", m_snConductionDelay.ValueFromDefaultScale)
            oXml.AddChildElement("GFacilCx", m_snSynapticConductance.ValueFromDefaultScale)
            oXml.AddChildElement("G", m_snSynapticConductance.ValueFromDefaultScale)

            oXml.OutOfElem() 'Outof Connexion

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            'First lets remove the link properties that are not used or can not be set by the synapse.
            m_Properties.Properties.Remove("Text")
            m_Properties.Properties.Remove("Arrow Destination")
            m_Properties.Properties.Remove("Arrow Middle")
            m_Properties.Properties.Remove("Arrow Origin")
            m_Properties.Properties.Remove("Dash Style")
            m_Properties.Properties.Remove("Draw Color")
            m_Properties.Properties.Remove("Draw Width")
            m_Properties.Properties.Remove("Font")
            m_Properties.Properties.Remove("Link Type")

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Synapse Class", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the classification for the type of this synapse.", TypeName(), True))

            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Synapse Type", GetType(String), "SynapseTypeName", _
            '                            "Synapse Properties", "Returns the name of this synapse type.", TypeName(), True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Synapse Type", GetType(DataObjects.Behavior.SynapseType), "SynapseType", _
                                        "Synapse Properties", "Sets the type of synapse.", m_stSynapseType, _
                                        GetType(TypeHelpers.SynapseTypeEditor), _
                                        GetType(TypeHelpers.SynapseTypeConverter)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strText.GetType(), "UserText", _
                                        "Synapse Properties", "Sets the name of this synapse.", m_strText, _
                                        GetType(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)))

            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag

            If Not m_stSynapseType Is Nothing AndAlso Not m_stSynapseType.GetType() Is GetType(SynapseTypes.NonSpikingChemical) Then
                pbNumberBag = m_snConductionDelay.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Conduction Delay", pbNumberBag.GetType(), "ConductionDelay", _
                                            "Synapse Properties", "Sets the delay between the spike in the source neuron and the start of the " & _
                                            "conductance change in the target neuron. Acceptable values are in the range 0 to 100 ms", pbNumberBag, _
                                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            If Not m_stSynapseType Is Nothing AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                pbNumberBag = m_snSynapticConductance.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Synaptic Conductance", pbNumberBag.GetType(), "SynapticConductance", _
                                            "Synapse Properties", "Sets the amplitude of the post-synaptic conductance change which this synapse mediates. " & _
                                            "Acceptable values are in the range 0 to 100 uS/size.", _
                                            pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snSynapticConductance Is Nothing Then m_snSynapticConductance.ClearIsDirty()
            If Not m_snConductionDelay Is Nothing Then m_snConductionDelay.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_strSynapticTypeID = Util.LoadID(oXml, "SynapticType", True, "")
                m_strUserText = oXml.GetChildString("UserText")
                m_snSynapticConductance.LoadData(oXml, "SynapticConductance")
                m_snConductionDelay.LoadData(oXml, "ConductionDelay")

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitializeAfterLoad(ByVal iAttempt As Integer)

            Try
                MyBase.InitializeAfterLoad(iAttempt)

                If m_bInitialized Then
                    Dim nmModule As DataObjects.Behavior.NeuralModule = DirectCast(Me.Organism.NeuralModules(Me.NeuralModuleType.FullName()), DataObjects.Behavior.NeuralModule)

                    If m_strSynapticTypeID.Trim.Length > 0 Then
                        m_stSynapseType = nmModule.SynapseTypes(m_strSynapticTypeID)
                    Else
                        Debug.WriteLine("I failed to load the specified synaptic type: " & m_strSynapticTypeID)
                    End If

                    If m_stSynapseType Is Nothing Then
                        m_bInitialized = False
                    End If

                    If Not m_stSynapseType Is Nothing Then
                        AddHandler m_stSynapseType.SynapseTypeChanged, AddressOf Me.OnSynapseTypeChanged
                    End If
                End If

            Catch ex As System.Exception
                m_bInitialized = False
                If iAttempt = 1 Then
                    AnimatTools.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                If Not m_stSynapseType Is Nothing Then
                    oXml.AddChildElement("SynapticTypeID", m_stSynapseType.ID)
                End If

                oXml.AddChildElement("UserText", m_strUserText)
                m_snSynapticConductance.SaveData(oXml, "SynapticConductance")
                m_snConductionDelay.SaveData(oXml, "ConductionDelay")

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
