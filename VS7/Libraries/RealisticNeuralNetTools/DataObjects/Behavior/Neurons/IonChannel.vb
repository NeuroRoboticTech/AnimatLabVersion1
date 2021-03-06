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

Namespace DataObjects.Behavior.Neurons

    Public Class IonChannel
        Inherits AnimatTools.DataObjects.DragObject

#Region " Attributes "

        Protected m_strSymbol As String = ""

        'Params for I = Gmax*m^mpower*h^hpower*(Vm-EqPot)
        Protected m_snGmax As ScaledNumber
        Protected m_iMPower As Integer = 1
        Protected m_iHPower As Integer = 1
        Protected m_snEquilibriumPotential As ScaledNumber

        'Params for dm/dt = (Minf(V)-m)/n1*Tm(V)
        Protected m_snMinit As ScaledNumber
        Protected m_snNm As ScaledNumber
        Protected m_gnMinf As AnimatTools.DataObjects.Gain
        Protected m_gnTm As AnimatTools.DataObjects.Gain

        'Params for dh/dt = (Hinf(V)-h)/n2*Th(V)
        Protected m_snHinit As ScaledNumber
        Protected m_snNh As ScaledNumber
        Protected m_gnHinf As AnimatTools.DataObjects.Gain
        Protected m_gnTh As AnimatTools.DataObjects.Gain

        Protected m_liListItem As ListViewItem

#End Region

#Region " Properties "

#Region " Drag ObjectProperties "

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "RealisticNeuralNetTools.IonChannel.gif"
            End Get
        End Property


        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return Me.ImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnModuleName() As String
            Get
                Return "RealisticNeuralNet"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnClassType() As String
            Get
                Return "IonChannelData"
            End Get
        End Property

        Public Overrides ReadOnly Property StructureID() As String
            Get
                If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Behavior.Neurons.Spiking Then
                    Dim doNeuron As DataObjects.Behavior.Neurons.Spiking = DirectCast(Me.Parent, DataObjects.Behavior.Neurons.Spiking)
                    If Not doNeuron.Organism Is Nothing Then
                        Return doNeuron.Organism.ID
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            End Get
        End Property

        'Tells if this data item can be added to a chart object. certain items like graphical node can not.
        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
                If Not m_liListItem Is Nothing Then
                    m_liListItem.Text = m_strName
                End If
            End Set
        End Property

        Public Overridable Property Symbol() As String
            Get
                Return m_strSymbol
            End Get
            Set(ByVal Value As String)
                m_strSymbol = Value
            End Set
        End Property

        Public Overridable Property Gmax() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snGmax
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.1 Then
                    Throw New System.Exception("The maximum conductance must be greater than zero and less than 100 mS.")
                End If

                m_snGmax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MPower() As Integer
            Get
                Return m_iMPower
            End Get
            Set(ByVal Value As Integer)
                m_iMPower = Value
            End Set
        End Property

        Public Overridable Property HPower() As Integer
            Get
                Return m_iHPower
            End Get
            Set(ByVal Value As Integer)
                m_iHPower = Value
            End Set
        End Property

        Public Overridable Property EquilibriumPotential() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < -0.2 OrElse Value.ActualValue > 0.2 Then
                    Throw New System.Exception("The maximum conductance must be between -200 mV and 200 mV.")
                End If

                m_snEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Minit() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snMinit
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The initial activation value (m) must be greater than zero and less than one.")
                End If

                m_snMinit.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Nm() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snNm
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                m_snNm.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Minf() As AnimatTools.DataObjects.Gain
            Get
                Return m_gnMinf
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Gain)
                m_gnMinf = Value
            End Set
        End Property

        Public Overridable Property Tm() As AnimatTools.DataObjects.Gain
            Get
                Return m_gnTm
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Gain)
                m_gnTm = Value
            End Set
        End Property

        Public Overridable Property Hinit() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snHinit
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The initial inactivation value (h) must be greater than zero and less than one.")
                End If

                m_snHinit.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Nh() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snNh
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                m_snNh.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Hinf() As AnimatTools.DataObjects.Gain
            Get
                Return m_gnHinf
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Gain)
                m_gnHinf = Value
            End Set
        End Property

        Public Overridable Property Th() As AnimatTools.DataObjects.Gain
            Get
                Return m_gnTh
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Gain)
                m_gnTh = Value
            End Set
        End Property

        Public Overridable Property ListItem() As ListViewItem
            Get
                Return m_liListItem
            End Get
            Set(ByVal Value As ListViewItem)
                m_liListItem = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_snGmax = New AnimatTools.Framework.ScaledNumber(Me, "Gmax", 5, AnimatTools.Framework.ScaledNumber.enumNumericScale.nano, "Siemens", "S")
            m_snEquilibriumPotential = New AnimatTools.Framework.ScaledNumber(Me, "EquilibriumPotential", -30, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

            m_snMinit = New AnimatTools.Framework.ScaledNumber(Me, "Minit", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snNm = New AnimatTools.Framework.ScaledNumber(Me, "Nm", 1, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_snHinit = New AnimatTools.Framework.ScaledNumber(Me, "Hinit", 0, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snNh = New AnimatTools.Framework.ScaledNumber(Me, "Nh", 1, AnimatTools.Framework.ScaledNumber.enumNumericScale.None, "", "")

            m_gnMinf = New DataObjects.Gains.IonChannelSigmoid(Me, "Minf", "Volts", "", False, False, False)
            m_gnTm = New DataObjects.Gains.IonChannelSigmoid(Me, "Tm", "Volts", "", False, False, False)
            m_gnHinf = New DataObjects.Gains.IonChannelSigmoid(Me, "Hinf", "Volts", "", False, False, False)
            m_gnTh = New DataObjects.Gains.IonChannelSigmoid(Me, "Th", "Volts", "", False, False, False)

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("G", "Conductance", "Siemens", "S", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("M", "Activation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("H", "Inactivation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("I", "Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("Act", "Total Activation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("Tm", "Activation Time Constant", "Seconds", "s", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("Th", "Inactivation Time Constant", "Seconds", "s", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("Minf", "Minf", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("Hinf", "Hinf", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thDataTypes.ID = "I"

        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As AnimatTools.DataObjects.DragObject

            Dim oOrg As Object = Util.Environment.FindOrganism(strStructureName, bThrowError)
            If oOrg Is Nothing Then Return Nothing

            Dim doOrganism As AnimatTools.DataObjects.Physical.Organism = DirectCast(oOrg, AnimatTools.DataObjects.Physical.Organism)
            Dim doNode As AnimatTools.DataObjects.Behavior.Node

            If Not doOrganism Is Nothing Then

                Dim doNeuron As DataObjects.Behavior.Neurons.Spiking
                For Each deEntry As DictionaryEntry In doOrganism.BehavioralNodes
                    If TypeOf deEntry.Value Is DataObjects.Behavior.Neurons.Spiking Then
                        doNeuron = DirectCast(deEntry.Value, DataObjects.Behavior.Neurons.Spiking)

                        If doNeuron.IonChannels.Contains(strDataItemID) Then
                            Return doNeuron.IonChannels(strDataItemID)
                        End If
                    End If
                Next

            End If

            If bThrowError AndAlso Not doNode Is Nothing Then
                Throw New System.Exception("The drag object with id '" & strDataItemID & "' was not found.")
            End If

            Return doNode

        End Function

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()

            'Channel properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strName.GetType, "Name", _
                                        "Channel Properties", "The name for this channel.", m_strName))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Symbol", m_strSymbol.GetType, "Symbol", _
                                        "Channel Properties", "A symbol for this channel.", m_strSymbol))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Enabled", m_bEnabled.GetType, "Enabled", _
                                        "Channel Properties", "Determines whether this channel is active and generates currents in the neuron.", m_bEnabled))

            'Ion Current properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("M Power", m_iMPower.GetType, "MPower", _
                                        "Ion Current", "The power to which the activation variable is raised.", m_iMPower))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("H Power", m_iHPower.GetType, "HPower", _
                                        "Ion Current", "The power to which the inactivation variable is raised.", m_iHPower))

            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snGmax.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Gmax", pbNumberBag.GetType(), "Gmax", _
                                        "Ion Current", "The maximum conductance for this ion channel. " & _
                                        "Acceptable values are in the range 0 to 100 mS.", pbNumberBag, _
                                        "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEquilibriumPotential.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Equilibrium Potential", pbNumberBag.GetType(), "EquilibriumPotential", _
                                        "Ion Current", "The equilibrium potential for this ion channel. Acceptable values are in the range -200 to 200 mV.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            'Activation properties
            pbNumberBag = m_snMinit.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("M Initial", pbNumberBag.GetType(), "Minit", _
                                        "Activation", "The initial value of the activation variable. Acceptable values are in the range 0 to 1.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snNm.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("N", pbNumberBag.GetType(), "Nm", _
                                        "Activation", "The value of a scaling variable multiplied to the denominator of the activation derivative.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_gnMinf.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Minf", pbNumberBag.GetType(), "Minf", _
                                        "Activation", "A gain function that describes the equation used in the numerator of the activation derivative.", _
                                        pbNumberBag, GetType(AnimatTools.TypeHelpers.GainTypeEditor), GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_gnTm.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Tm", pbNumberBag.GetType(), "Tm", _
                                        "Activation", "A gain function that describes the equation used in the denominator of the activation derivative.", _
                                        pbNumberBag, GetType(AnimatTools.TypeHelpers.GainTypeEditor), GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

            'Inactivation properties
            pbNumberBag = m_snHinit.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("H Initial", pbNumberBag.GetType(), "Hinit", _
                                        "Inactivation", "The initial value of the inactivation variable. Acceptable values are in the range 0 to 1.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snNh.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("N", pbNumberBag.GetType(), "Nh", _
                                        "Inactivation", "The value of a scaling variable multiplied to the denominator of the inactivation derivative.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_gnHinf.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Hinf", pbNumberBag.GetType(), "Hinf", _
                                        "Inactivation", "A gain function that describes the equation used in the numerator of the inactivation derivative.", _
                                        pbNumberBag, GetType(AnimatTools.TypeHelpers.GainTypeEditor), GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_gnTh.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Th", pbNumberBag.GetType(), "Th", _
                                        "Inactivation", "A gain function that describes the equation used in the denominator of the inactivation derivative.", _
                                        pbNumberBag, GetType(AnimatTools.TypeHelpers.GainTypeEditor), GetType(Crownwood.Magic.Controls.ExpandablePropBagConverter)))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewNode As New DataObjects.Behavior.Neurons.IonChannel(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As DataObjects.Behavior.Neurons.IonChannel = DirectCast(doOriginal, DataObjects.Behavior.Neurons.IonChannel)

            m_strSymbol = bnOrig.m_strSymbol
            m_bEnabled = bnOrig.m_bEnabled
            m_snGmax = DirectCast(bnOrig.m_snGmax.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_iMPower = bnOrig.m_iMPower
            m_iHPower = bnOrig.m_iHPower
            m_snEquilibriumPotential = DirectCast(bnOrig.m_snEquilibriumPotential.Clone(Me, bCutData, doRoot), ScaledNumber)

            m_snMinit = DirectCast(bnOrig.m_snMinit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snNm = DirectCast(bnOrig.m_snNm.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_gnMinf = DirectCast(bnOrig.m_gnMinf.Clone(Me, bCutData, doRoot), AnimatTools.DataObjects.Gain)
            m_gnTm = DirectCast(bnOrig.m_gnTm.Clone(Me, bCutData, doRoot), AnimatTools.DataObjects.Gain)

            m_snHinit = DirectCast(bnOrig.m_snHinit.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snNh = DirectCast(bnOrig.m_snNh.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_gnHinf = DirectCast(bnOrig.m_gnHinf.Clone(Me, bCutData, doRoot), AnimatTools.DataObjects.Gain)
            m_gnTh = DirectCast(bnOrig.m_gnTh.Clone(Me, bCutData, doRoot), AnimatTools.DataObjects.Gain)

            If Not m_Image Is Nothing Then m_Image = DirectCast(bnOrig.m_Image.Clone, System.Drawing.Image)
            If Not m_DragImage Is Nothing Then m_DragImage = DirectCast(bnOrig.m_DragImage.Clone, System.Drawing.Image)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snGmax Is Nothing Then m_snGmax.ClearIsDirty()
            If Not m_snEquilibriumPotential Is Nothing Then m_snEquilibriumPotential.ClearIsDirty()
            If Not m_snMinit Is Nothing Then m_snMinit.ClearIsDirty()
            If Not m_snNm Is Nothing Then m_snNm.ClearIsDirty()
            If Not m_gnMinf Is Nothing Then m_gnMinf.ClearIsDirty()
            If Not m_gnTm Is Nothing Then m_gnTm.ClearIsDirty()
            If Not m_snHinit Is Nothing Then m_snHinit.ClearIsDirty()
            If Not m_snNh Is Nothing Then m_snNh.ClearIsDirty()
            If Not m_gnHinf Is Nothing Then m_gnHinf.ClearIsDirty()
            If Not m_gnTh Is Nothing Then m_gnTh.ClearIsDirty()

        End Sub

        Public Overridable Sub SaveNetwork(ByRef oXml As AnimatTools.Interfaces.StdXml, ByRef nmModule As AnimatTools.DataObjects.Behavior.NeuralModule)

            oXml.AddChildElement("IonChannel")
            oXml.IntoElem()

            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("Gmax", m_snGmax.ActualValue)
            oXml.AddChildElement("MPower", m_iMPower)
            oXml.AddChildElement("HPower", m_iHPower)
            oXml.AddChildElement("EqPot", m_snEquilibriumPotential.ActualValue)

            oXml.AddChildElement("Minit", m_snMinit.ActualValue)
            oXml.AddChildElement("Nm", m_snNm.ActualValue)
            m_gnMinf.SaveNetwork(oXml, "Minf")
            m_gnTm.SaveNetwork(oXml, "Tm")

            oXml.AddChildElement("Hinit", m_snHinit.ActualValue)
            oXml.AddChildElement("Nh", m_snNh.ActualValue)
            m_gnHinf.SaveNetwork(oXml, "Hinf")
            m_gnTh.SaveNetwork(oXml, "Th")

            oXml.OutOfElem() 'Outof IonChannel

        End Sub

        Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatTools.Interfaces.StdXml)

            If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Behavior.Neurons.Spiking Then
                Dim doNeuron As DataObjects.Behavior.Neurons.Spiking = DirectCast(Me.Parent, DataObjects.Behavior.Neurons.Spiking)

                If Not doNeuron.Organism Is Nothing Then
                    oXml.IntoElem()

                    oXml.AddChildElement("OrganismID", doNeuron.Organism.ID)
                    oXml.AddChildElement("NeuronIndex", doNeuron.NodeIndex)
                    oXml.AddChildElement("ChannelID", Me.ID)
                    oXml.AddChildElement("Enabled", m_bEnabled)

                    oXml.OutOfElem() 'Outof ChannelData
                End If
            End If

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.IntoElem()

            m_strID = oXml.GetChildString("ID", m_strID)
            m_strName = oXml.GetChildString("Name", m_strName)
            m_strSymbol = oXml.GetChildString("Symbol", m_strSymbol)
            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

            m_snGmax.LoadData(oXml, "Gmax")
            m_iMPower = oXml.GetChildInt("MPower", m_iMPower)
            m_iHPower = oXml.GetChildInt("HPower", m_iHPower)
            m_snEquilibriumPotential.LoadData(oXml, "EqPot")

            m_snMinit.LoadData(oXml, "Minit")
            m_snNm.LoadData(oXml, "Nm")
            m_gnMinf = Util.LoadGain("Minf", "Minf", Me, oXml)
            m_gnTm = Util.LoadGain("Tm", "Tm", Me, oXml)

            m_snHinit.LoadData(oXml, "Hinit")
            m_snNh.LoadData(oXml, "Nh")
            m_gnHinf = Util.LoadGain("Hinf", "Hinf", Me, oXml)
            m_gnTh = Util.LoadGain("Th", "Th", Me, oXml)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.AddChildElement("IonChannel")
            oXml.IntoElem()  'Into IonChannel Element

            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Symbol", m_strSymbol)
            oXml.AddChildElement("Enabled", m_bEnabled)

            m_snGmax.SaveData(oXml, "Gmax")
            oXml.AddChildElement("MPower", m_iMPower)
            oXml.AddChildElement("HPower", m_iHPower)
            m_snEquilibriumPotential.SaveData(oXml, "EqPot")

            m_snMinit.SaveData(oXml, "Minit")
            m_snNm.SaveData(oXml, "Nm")
            m_gnMinf.SaveData(oXml, "Minf")
            m_gnTm.SaveData(oXml, "Tm")

            m_snHinit.SaveData(oXml, "Hinit")
            m_snNh.SaveData(oXml, "Nh")
            m_gnHinf.SaveData(oXml, "Hinf")
            m_gnTh.SaveData(oXml, "Th")

            oXml.OutOfElem() ' Outof IonChannel Element

        End Sub

#End Region

#End Region

    End Class

End Namespace
