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

    Public Class MotorVelocity
        Inherits AnimatTools.DataObjects.ExternalStimuli.BodyPartStimulus

#Region " Attributes "

        Protected m_snVelocity As ScaledNumber
        Protected m_bDisableWhenDone As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property BodyPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doBodyPart
            End Get
            Set(ByVal Value As DataObjects.Physical.BodyPart)
                m_doBodyPart = Value

                SetVelocityUnits()
            End Set
        End Property

        Public Overridable Property Velocity() As ScaledNumber
            Get
                Return m_snVelocity
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    m_snVelocity.CopyData(Value)
                End If
            End Set
        End Property

        Public Overrides Property Equation() As String
            Get
                Return m_strEquation
            End Get
            Set(ByVal Value As String)
                'Lets verify the equation before we use it.
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.AddVariable("p")
                oMathEval.AddVariable("v")
                oMathEval.Equation = Value
                oMathEval.Parse()

                m_strEquation = Value
            End Set
        End Property

        Public Overridable Property DisableWhenDone() As Boolean
            Get
                Return m_bDisableWhenDone
            End Get
            Set(ByVal Value As Boolean)
                m_bDisableWhenDone = Value
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Motor Velocity"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "AnimatTools.MotorVelocity_Large.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This stimulus sets the velocity/position of a motorized joint."
            End Get
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "MotorVelocity"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceNodeImageName() As String
            Get
                Return "AnimatTools.MotorVelocity.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_snVelocity = New ScaledNumber(Me, "Velocity", 0, ScaledNumber.enumNumericScale.None, "rad/s", "rad/s")

            If Not Util.Environment Is Nothing Then
                m_snVelocity.SetFromValue(0, Util.Environment.DistanceUnits)
            End If

        End Sub

        Protected Overridable Sub SetVelocityUnits()
            If Not m_doBodyPart Is Nothing AndAlso TypeOf m_doBodyPart Is AnimatTools.DataObjects.Physical.Joint Then
                Dim doJoint As AnimatTools.DataObjects.Physical.Joint = DirectCast(m_doBodyPart, AnimatTools.DataObjects.Physical.Joint)

                Dim strUnits As String = doJoint.ScaleUnits
                m_snVelocity.SetScaleUnits(strUnits, strUnits)
            End If

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.MotorVelocity = DirectCast(doOriginal, DataObjects.ExternalStimuli.MotorVelocity)

            m_snVelocity = DirectCast(doPart.m_snVelocity.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.MotorVelocity = New DataObjects.ExternalStimuli.MotorVelocity(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snVelocity.ClearIsDirty()
        End Sub

        Public Overrides Function SaveStimulusToXml() As String

            Dim oXml As New AnimatTools.Interfaces.StdXml

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddElement("Stimuli")
            SaveXml(oXml)

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveXml(ByRef oXml As Interfaces.StdXml)

            If m_doStructure Is Nothing Then
                Throw New System.Exception("No structure was defined for the stimulus '" & m_strName & "'.")
            End If

            If m_doBodyPart Is Nothing Then
                Throw New System.Exception("No bodypart was defined for the stimulus '" & m_strName & "'.")
            End If

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            oXml.AddChildElement("StructureID", m_doStructure.ID)
            oXml.AddChildElement("JointID", m_doBodyPart.ID)

            oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
            oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)

            If m_eValueType = enumValueType.Constant Then
                oXml.AddChildElement("Velocity", m_snVelocity.ActualValue)
            Else
                'We need to convert the infix equation to postfix
                Dim oMathEval As New MathStringEval
                oMathEval.AddVariable("t")
                oMathEval.Equation = m_strEquation
                oMathEval.Parse()
                oXml.AddChildElement("Velocity", oMathEval.PostFix)
            End If

            oXml.AddChildElement("DisableMotorWhenDone", m_bDisableWhenDone)

            oXml.OutOfElem()
        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            Dim strInputStim As String
            If Not m_doBodyPart Is Nothing AndAlso TypeOf m_doBodyPart Is AnimatTools.DataObjects.Physical.Joint Then
                Dim doJoint As AnimatTools.DataObjects.Physical.Joint = DirectCast(m_doBodyPart, AnimatTools.DataObjects.Physical.Joint)
                strInputStim = doJoint.InputStimulus
                SetVelocityUnits()
            Else
                strInputStim = "Velocity"
            End If

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Value Type", m_eValueType.GetType(), "ValueType", _
                                        "Stimulus Properties", "Determines if a constant or an equation is used to determine the velocity/position.", m_eValueType))

            If m_eValueType = enumValueType.Equation Then
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Equation", m_strEquation.GetType(), "Equation", _
                                            "Stimulus Properties", "If setup to use equations, then this is the one used.", m_strEquation))
            Else
                Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snVelocity.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec(strInputStim, pbNumberBag.GetType(), "Velocity", _
                                            "Stimulus Properties", "The velocity/position to move the motorized joint.", pbNumberBag, _
                                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Disable When Done", m_bDisableWhenDone.GetType(), "DisableWhenDone", _
                                        "Stimulus Properties", "If this is true then the motor is disabled at the end of the simulus period.", m_bDisableWhenDone))

        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatTools.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatTools.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)
            m_snVelocity.ActualValue = m_snVelocity.ActualValue * fltDistanceChange
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            SetVelocityUnits()

            oXml.IntoElem()

            If ScaledNumber.IsValidXml(oXml, "Velocity") Then
                m_snVelocity.LoadData(oXml, "Velocity")
            Else
                Dim fltVelocity As Single = oXml.GetChildFloat("Velocity", CSng(m_snVelocity.ActualValue))
                m_snVelocity.ActualValue = fltVelocity
            End If

            m_bDisableWhenDone = oXml.GetChildBool("DisableMotorWhenDone", m_bDisableWhenDone)

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            m_snVelocity.SaveData(oXml, "Velocity")
            oXml.AddChildElement("DisableMotorWhenDone", m_bDisableWhenDone)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace
