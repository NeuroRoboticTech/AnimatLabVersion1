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

Namespace DataObjects.Behavior.SynapseTypes

    Public Class NonSpikingChemical
        Inherits DataObjects.Behavior.SynapseType

#Region " Attributes "

        Protected m_snEquilibriumPotential As AnimatTools.Framework.ScaledNumber
        Protected m_snMaxSynapticConductance As AnimatTools.Framework.ScaledNumber
        Protected m_snPreSynapticThreshold As AnimatTools.Framework.ScaledNumber
        Protected m_snPreSynapticSaturationLevel As AnimatTools.Framework.ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Non-Spiking Chemical Synapse"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property EquilibriumPotential() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.3 Then
                    Throw New System.Exception("The equilibrium potential must be between the range -100 to 300 mV.")
                End If

                m_snEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxSynapticConductance() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snMaxSynapticConductance
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.0001 Then
                    Throw New System.Exception("The maximum synaptic conductance must be between the range 0 to 100 uS/size.")
                End If

                m_snMaxSynapticConductance.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PreSynapticThreshold() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snPreSynapticThreshold
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.3 Then
                    Throw New System.Exception("The pre-synaptic threshold must be between the range -100 to 100 mV.")
                End If

                m_snPreSynapticThreshold.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PreSynapticSaturationLevel() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snPreSynapticSaturationLevel
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.3 Then
                    Throw New System.Exception("The pre-synaptic saturation level must be between the range -100 to 100 mV.")
                End If

                m_snPreSynapticSaturationLevel.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SynapseType() As Integer
            Get
                Return 1 'For non-Spiking Chemical synapse
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "RealisticNeuralNetTools.NonSpikingSynapse.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_snEquilibriumPotential = New AnimatTools.Framework.ScaledNumber(Me, "EquilibriumPotential", -10, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snMaxSynapticConductance = New AnimatTools.Framework.ScaledNumber(Me, "MaxSynapticConductance", 0.5, AnimatTools.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
            m_snPreSynapticThreshold = New AnimatTools.Framework.ScaledNumber(Me, "PreSynapticThreshold", -65, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snPreSynapticSaturationLevel = New AnimatTools.Framework.ScaledNumber(Me, "PreSynapticSaturationLevel", -20, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("RealisticNeuralNetTools")

            Me.Image = AnimatTools.Framework.ImageManager.LoadImage(myAssembly, "RealisticNeuralNetTools.NonSpikingSynapse.gif", False)
            Me.Name = "Non-Spiking Chemical Synapse"

            Me.Font = New Font("Arial", 12)
            Me.Description = "Adds a non-spiking chemical synapse between two neurons. A non-spiking chemical synapse is " & _
                            "modelled as a variable increase in post-synaptic conductance whose level depends upon the pre-synaptic membrane potential."

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewLink As New NonSpikingChemical(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim blOrig As SynapseTypes.NonSpikingChemical = DirectCast(doOriginal, SynapseTypes.NonSpikingChemical)

            'Basic Synaptic Properties
            m_snEquilibriumPotential = DirectCast(blOrig.m_snEquilibriumPotential.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMaxSynapticConductance = DirectCast(blOrig.m_snMaxSynapticConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPreSynapticThreshold = DirectCast(blOrig.m_snPreSynapticThreshold.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPreSynapticSaturationLevel = DirectCast(blOrig.m_snPreSynapticSaturationLevel.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Sub SaveNetwork(ByRef oXml As AnimatTools.Interfaces.StdXml, ByRef nmModule As AnimatTools.DataObjects.Behavior.NeuralModule)

            oXml.AddChildElement("NonSpikingSynapse")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", Me.LinkIndex)
            oXml.AddChildElement("Equil", m_snEquilibriumPotential.ValueFromDefaultScale)
            oXml.AddChildElement("SynAmp", m_snMaxSynapticConductance.ValueFromDefaultScale)
            oXml.AddChildElement("ThreshV", m_snPreSynapticThreshold.ValueFromDefaultScale)
            oXml.AddChildElement("SaturateV", m_snPreSynapticSaturationLevel.ValueFromDefaultScale)

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            ''Now lets add the properties for this neuron
            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snEquilibriumPotential.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Equilibrium Potential", pbNumberBag.GetType(), "EquilibriumPotential", _
                                        "Synapse Properties", "Sets the equilibrium (reversal) potential for this synaptic type. " & _
                                        "Acceptable values are in the range -100 to 300 mV", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxSynapticConductance.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Max Synaptic Conductance", pbNumberBag.GetType(), "MaxSynapticConductance", _
                                        "Synapse Properties", "Sets the maximum amplitude of the post-synaptic conductance change which this synapse mediates. " & _
                                        "Acceptable values are in the range 0 to 100 uS/size.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snPreSynapticThreshold.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Pre-Synaptic Threshold", pbNumberBag.GetType(), "PreSynapticThreshold", _
                                        "Synapse Properties", "Sets the threshold membrane potential of the pre-synaptic neuron at which transmitter " & _
                                        "release, and hence the post-synaptic conductance defined above, starts to increase above the 0 level. " & _
                                        "Acceptable values are in the range -100 to 300 mV.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snPreSynapticSaturationLevel.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Pre-Synaptic Saturation Level", pbNumberBag.GetType(), "PreSynapticSaturationLevel", _
                                        "Synapse Properties", "Sets the membrane potential of the pre-synaptic neuron at which transmitter release, and hence the " & _
                                        "post-synaptic conductancem reaches its maximum value. Acceptable values are in the range -100 to 300 mV.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snEquilibriumPotential Is Nothing Then m_snEquilibriumPotential.ClearIsDirty()
            If Not m_snMaxSynapticConductance Is Nothing Then m_snMaxSynapticConductance.ClearIsDirty()
            If Not m_snPreSynapticThreshold Is Nothing Then m_snPreSynapticThreshold.ClearIsDirty()
            If Not m_snPreSynapticSaturationLevel Is Nothing Then m_snPreSynapticSaturationLevel.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_snEquilibriumPotential.LoadData(oXml, "EquilibriumPotential")
                m_snMaxSynapticConductance.LoadData(oXml, "MaxSynapticConductance")
                m_snPreSynapticThreshold.LoadData(oXml, "PreSynapticThreshold")
                m_snPreSynapticSaturationLevel.LoadData(oXml, "PreSynapticSaturationLevel")

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                m_snEquilibriumPotential.SaveData(oXml, "EquilibriumPotential")
                m_snMaxSynapticConductance.SaveData(oXml, "MaxSynapticConductance")
                m_snPreSynapticThreshold.SaveData(oXml, "PreSynapticThreshold")
                m_snPreSynapticSaturationLevel.SaveData(oXml, "PreSynapticSaturationLevel")

                oXml.OutOfElem() ' Outof Node Element
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
