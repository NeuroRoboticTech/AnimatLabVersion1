Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Forms
Imports AnimatTools.Framework
Imports Gigasoft
Imports Gigasoft.ProEssentials.Api
Imports System.IO

Namespace Forms.Charts

    Public Class LineChart
        Inherits AnimatTools.Forms.Tools.DataChart

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents ctrlGraph As Gigasoft.ProEssentials.Pesgo
        Private WithEvents m_Timer As New Timer

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlGraph = New Gigasoft.ProEssentials.Pesgo
            Me.SuspendLayout()
            '
            'ctrlGraph
            '
            Me.ctrlGraph.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlGraph.Location = New System.Drawing.Point(0, 0)
            Me.ctrlGraph.Name = "ctrlGraph"
            Me.ctrlGraph.Size = New System.Drawing.Size(292, 266)
            Me.ctrlGraph.TabIndex = 0
            Me.ctrlGraph.Text = "Data Graph"
            Me.ctrlGraph.PeSpecial.Custom = False
            Me.ctrlGraph.PeSpecial.CustomParms = False
            Me.ctrlGraph.AllowDrop = True
            Me.ctrlGraph.PeSpecial.CustomParms = False
            '
            'LineChart
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.ctrlGraph)
            Me.Name = "Line Chart"
            Me.Text = "LineChart"
            Me.ResumeLayout(False)

            m_Timer.Enabled = False
        End Sub

#End Region

#Region " Enums "

        Public Enum enumFontSize
            Small
            Medium
            Large
        End Enum

        Public Enum enumBorderType
            DropShadow
            SingleLine
            NoBorder
            Inset
        End Enum

        Public Enum enumViewingStyle
            NoStyle
            LightInset
            LightShadow
            LightLine
            LightNoBorder
            MediumInset
            MediumShadow
            MediumLine
            MediumNoBorder
            DarkInset
            DarkShadow
            DarkLine
            DarkNoBorder
        End Enum

        Public Enum enumGradientStyle
            NoGradient
            Vertical
            Horizontal
        End Enum

        Public Enum enumBitmapType
            None = 0
            Custom = 1
            A = 1700
            B = 1701
            C = 1702
            D = 1703
            E = 1704
            F = 1705
            G = 1706
            H = 1707
            I = 1708
            J = 1709
            K = 1710
        End Enum

        Public Enum enumGridLineControl
            None
            XAxis
            YAxis
            Both
        End Enum

        Public Enum enumGridLineStyle
            Thin
            Thick
            Dot
            Dash
            OnePixel
        End Enum

        Public Enum enumScaleControl
            None
            Min
            Max
            MinMax
        End Enum

        Public Enum enumAxisStyle
            GroupAllAxes
            SeparateAxes
        End Enum

#End Region

#Region " Attributes "

        Protected Const m_iNullValue As Integer = -99999

        'Protected m_graphStream As System.IO.MemoryStream
        'Protected m_aryData() As Byte
        'Protected m_xmlData As AnimatTools.Interfaces.StdXml

        Protected m_Image As System.Drawing.Image
        Protected m_bAddedHandlers As Boolean = False

        Protected m_iSubsets As Integer = 0
        Protected m_iPrevSubsets As Integer = -1

        Protected m_iPointsToKeep As Integer = 15000

        Protected m_nRealTimeCounter As Int32 = 1
        Protected m_nSinCounter As Int32 = 1

        Protected m_dblYAxisOffset As Double = 0

        Protected m_strMainTitle As String
        Protected m_strSubTitle As String
        Protected m_strXAxisLabel As String = "Time (s)"
        Protected m_strYAxisLabel As String
        Protected m_bAutoScaleData As Boolean
        Protected m_eXAutoScale As enumScaleControl = enumScaleControl.None
        Protected m_dblXMin As Double
        Protected m_dblXMax As Double
        Protected m_ptXAxisSize As PointF
        Protected m_iXAxisScaleValue As Integer
        Protected m_ptYAxisSize As PointF
        Protected m_eFontSize As enumFontSize
        Protected m_eBorderType As enumBorderType = enumBorderType.DropShadow
        Protected m_eViewingStyle As enumViewingStyle
        Protected m_bBitmapGradients As Boolean
        Protected m_DeskColor As System.Drawing.Color
        Protected m_eDeskGradientStyle As enumGradientStyle
        Protected m_DeskGradientStart As System.Drawing.Color
        Protected m_DeskGradientEnd As System.Drawing.Color
        Protected m_GraphBackColor As System.Drawing.Color
        Protected m_GraphForeColor As System.Drawing.Color
        Protected m_eGraphGradientStyle As enumGradientStyle
        Protected m_GraphGradientStart As System.Drawing.Color
        Protected m_GraphGradientEnd As System.Drawing.Color
        Protected m_eDeskBitmapType As enumBitmapType
        Protected m_eGraphBitmapType As enumBitmapType
        Protected m_XAxisColor As System.Drawing.Color
        Protected m_eGridLineControl As enumGridLineControl = enumGridLineControl.Both
        Protected m_bGridInFront As Boolean
        Protected m_eGridLineStyle As enumGridLineStyle = enumGridLineStyle.Thin
        Protected m_MainTitleFont As Font
        Protected m_SubTitleFont As Font
        Protected m_LabelFont As Font
        Protected m_eMultiAxisStyle As enumAxisStyle = enumAxisStyle.SeparateAxes
        Protected m_iMultiAxisSeperatorSize As Integer = 5

        Protected m_fltAxisFontScale As Single = 1
        Protected m_fltGridFontScale As Single = 1
        Protected m_fltTitleFontScale As Single = 1
        Protected m_fltLegendFontScale As Single = 1

        Protected m_bSaveDataWhenClosed As Boolean = False
        Protected m_bUpdateChartAtEnd As Boolean = True

        Protected m_menuMain As MenuControl
        Protected m_barMain As Crownwood.Magic.Toolbars.ToolbarControl

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "LicensedAnimatTools.LineChart.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Image() As System.Drawing.Image
            Get
                If m_Image Is Nothing Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load("LicensedAnimatTools")
                    m_Image = ImageManager.LoadImage(myAssembly, Me.ImageName)
                End If

                Return m_Image
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "LicensedAnimatTools.LineChart.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This is a chart that allows the user to plot any variables in the system over time on multiple axis."
            End Get
        End Property

        Public Overrides Property MainTitle() As String
            Get
                Return ctrlGraph.PeString.MainTitle
            End Get
            Set(ByVal Value As String)
                m_strMainTitle = Value
                ctrlGraph.PeString.MainTitle = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property SubTitle() As String
            Get
                Return ctrlGraph.PeString.SubTitle
            End Get
            Set(ByVal Value As String)
                m_strSubTitle = Value
                ctrlGraph.PeString.SubTitle = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property XAxisLabel() As String
            Get
                Return ctrlGraph.PeString.XAxisLabel
            End Get
            Set(ByVal Value As String)
                m_strXAxisLabel = Value
                ctrlGraph.PeString.XAxisLabel = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property YAxisLabel() As String
            Get
                Return ctrlGraph.PeString.YAxisLabel
            End Get
            Set(ByVal Value As String)
                m_strYAxisLabel = Value
                ctrlGraph.PeString.YAxisLabel = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property AutoScaleData() As Boolean
            Get
                Return ctrlGraph.PeData.AutoScaleData
            End Get
            Set(ByVal Value As Boolean)
                m_bAutoScaleData = Value
                ctrlGraph.PeData.AutoScaleData = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property UpdateChartAtEnd() As Boolean
            Get
                Return m_bUpdateChartAtEnd
            End Get
            Set(ByVal Value As Boolean)
                m_bUpdateChartAtEnd = Value

                If m_bUpdateChartAtEnd AndAlso Not m_bSetStartEndTime Then
                    Me.SetStartEndTime = True
                Else
                    Me.Viewer.RefreshProperties()
                End If

            End Set
        End Property

        Public Overrides Property SetStartEndTime() As Boolean
            Get
                Return m_bSetStartEndTime
            End Get
            Set(ByVal Value As Boolean)
                m_bSetStartEndTime = Value
                Me.Viewer.RefreshProperties()

                If m_bSetStartEndTime Then
                    Me.XAutoScale = enumScaleControl.MinMax
                    Me.XMin = Me.CollectStartTime.ActualValue
                    Me.XMax = Me.CollectEndTime.ActualValue
                Else
                    Me.XAutoScale = enumScaleControl.None
                End If
            End Set
        End Property

        Public Overrides Property CollectStartTime() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snCollectStartTime
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                MyBase.CollectStartTime = Value

                If m_bSetStartEndTime Then
                    Me.XAutoScale = enumScaleControl.MinMax
                    Me.XMin = Me.CollectStartTime.ActualValue
                    Me.XMax = Me.CollectEndTime.ActualValue
                End If
            End Set
        End Property

        Public Overrides Property CollectEndTime() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snCollectEndTime
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
                MyBase.CollectEndTime = Value

                If m_bSetStartEndTime Then
                    Me.XAutoScale = enumScaleControl.MinMax
                    Me.XMin = Me.CollectStartTime.ActualValue
                    Me.XMax = Me.CollectEndTime.ActualValue
                End If
            End Set
        End Property

        Public Overridable Property XAutoScale() As enumScaleControl
            Get
                Return DirectCast([Enum].Parse(GetType(enumScaleControl), ctrlGraph.PeGrid.Configure.ManualScaleControlX.ToString, True), enumScaleControl)
            End Get
            Set(ByVal Value As enumScaleControl)

                m_eXAutoScale = Value
                ctrlGraph.PeGrid.Configure.ManualScaleControlX = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.ManualScaleControl), Value.ToString, True), Gigasoft.ProEssentials.Enums.ManualScaleControl)

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Refresh()

            End Set
        End Property

        Public Overridable Property XMin() As Double
            Get
                Return ctrlGraph.PeGrid.Configure.ManualMinX
            End Get
            Set(ByVal Value As Double)

                m_dblXMin = Value - m_dblYAxisOffset
                ctrlGraph.PeGrid.Configure.ManualMinX = m_dblXMin

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Refresh()

            End Set
        End Property

        Public Overridable Property XMax() As Double
            Get
                Return ctrlGraph.PeGrid.Configure.ManualMaxX
            End Get
            Set(ByVal Value As Double)

                m_dblXMax = Value
                ctrlGraph.PeGrid.Configure.ManualMaxX = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Refresh()

            End Set
        End Property

        Public Overridable Property YAxisOffset() As Double
            Get
                Return m_dblYAxisOffset
            End Get
            Set(ByVal Value As Double)

                m_dblYAxisOffset = Value
                m_dblXMin = Me.XMin - m_dblYAxisOffset
                ctrlGraph.PeGrid.Configure.ManualMinX = m_dblXMin

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Refresh()

            End Set
        End Property

        Public Overrides Property XAxisSize() As PointF
            Get
                Dim szSize As New PointF(CSng(ctrlGraph.PeGrid.Configure.ManualMinX), CSng(ctrlGraph.PeGrid.Configure.ManualMaxX))
                Return szSize
            End Get
            Set(ByVal Value As PointF)
                m_ptXAxisSize = Value
                ctrlGraph.PeGrid.Configure.ManualMinX = Value.X
                ctrlGraph.PeGrid.Configure.ManualMaxX = Value.Y
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property XAxisScaleValue() As Integer
            Get
                Return ctrlGraph.PeData.ScaleForXData
            End Get
            Set(ByVal Value As Integer)

                m_iXAxisScaleValue = Value
                ctrlGraph.PeData.ScaleForXData = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Refresh()

            End Set
        End Property

        Public Overrides Property YAxisSize() As PointF
            Get
                Dim szSize As New PointF(CSng(ctrlGraph.PeGrid.Configure.ManualMinY), CSng(ctrlGraph.PeGrid.Configure.ManualMaxY))
            End Get
            Set(ByVal Value As PointF)
                m_ptYAxisSize = Value
                ctrlGraph.PeGrid.Configure.ManualMinY = Value.X
                ctrlGraph.PeGrid.Configure.ManualMaxY = Value.Y
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property FontSize() As enumFontSize
            Get
                Return DirectCast([Enum].Parse(GetType(enumFontSize), ctrlGraph.PeFont.FontSize.ToString, True), enumFontSize)
            End Get
            Set(ByVal Value As enumFontSize)
                m_eFontSize = Value
                ctrlGraph.PeFont.FontSize = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.FontSize), Value.ToString, True), Gigasoft.ProEssentials.Enums.FontSize)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property BorderType() As enumBorderType
            Get
                Return DirectCast([Enum].Parse(GetType(enumBorderType), ctrlGraph.PeConfigure.BorderTypes.ToString, True), enumBorderType)
            End Get
            Set(ByVal Value As enumBorderType)
                m_eBorderType = Value
                ctrlGraph.PeConfigure.BorderTypes = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.TABorder), Value.ToString, True), Gigasoft.ProEssentials.Enums.TABorder)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property ViewingStyle() As enumViewingStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumViewingStyle), ctrlGraph.PeColor.QuickStyle.ToString, True), enumViewingStyle)
            End Get
            Set(ByVal Value As enumViewingStyle)
                m_eViewingStyle = Value
                ctrlGraph.PeColor.QuickStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.QuickStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.QuickStyle)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
                Me.Viewer.HierarchyBar.PropertyData = Me.Properties

                InitializeChartData()
            End Set
        End Property

        Public Overridable Property BitmapGradients() As Boolean
            Get
                Return ctrlGraph.PeColor.BitmapGradientMode
            End Get
            Set(ByVal Value As Boolean)
                m_bBitmapGradients = Value
                ctrlGraph.PeColor.BitmapGradientMode = Value
                ctrlGraph.PeColor.QuickStyle = ctrlGraph.PeColor.QuickStyle
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()

                InitializeChartData()
            End Set
        End Property

        Public Overridable Property DeskColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.Desk
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_DeskColor = Value
                ctrlGraph.PeColor.Desk = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property DeskGradientStyle() As enumGradientStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumGradientStyle), ctrlGraph.PeColor.DeskGradientStyle.ToString, True), enumGradientStyle)
            End Get
            Set(ByVal Value As enumGradientStyle)
                m_eDeskGradientStyle = Value
                ctrlGraph.PeColor.DeskGradientStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GradientStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.GradientStyle)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property DeskGradientStart() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.DeskGradientStart
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_DeskGradientStart = Value
                ctrlGraph.PeColor.DeskGradientStart = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property DeskGradientEnd() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.DeskGradientEnd
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_DeskGradientEnd = Value
                ctrlGraph.PeColor.DeskGradientEnd = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GraphBackColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.GraphBackground
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_GraphBackColor = Value
                ctrlGraph.PeColor.GraphBackground = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GraphForeColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.GraphForeground
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_GraphForeColor = Value
                ctrlGraph.PeColor.GraphForeground = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GraphGradientStyle() As enumGradientStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumGradientStyle), ctrlGraph.PeColor.GraphGradientStyle.ToString, True), enumGradientStyle)
            End Get
            Set(ByVal Value As enumGradientStyle)
                m_eGraphGradientStyle = Value
                ctrlGraph.PeColor.GraphGradientStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GradientStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.GradientStyle)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GraphGradientStart() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.DeskGradientStart
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_GraphGradientStart = Value
                ctrlGraph.PeColor.GraphGradientStart = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GraphGradientEnd() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.DeskGradientEnd
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_GraphGradientEnd = Value
                ctrlGraph.PeColor.GraphGradientEnd = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property DeskBitmapType() As enumBitmapType
            Get
                If ctrlGraph.PeColor.DeskBmpFilename = "" Then
                    Return enumBitmapType.None
                ElseIf IsNumeric(ctrlGraph.PeColor.DeskBmpFilename) Then
                    Return DirectCast([Enum].Parse(GetType(enumBitmapType), ctrlGraph.PeColor.DeskBmpFilename, True), enumBitmapType)
                Else
                    Return enumBitmapType.Custom
                End If
            End Get
            Set(ByVal Value As enumBitmapType)
                If Value = enumBitmapType.Custom OrElse Value = enumBitmapType.None Then
                Else
                    m_eDeskBitmapType = Value
                    ctrlGraph.PeColor.DeskBmpFilename = CInt(Value).ToString()
                End If
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GraphBitmapType() As enumBitmapType
            Get
                If ctrlGraph.PeColor.GraphBmpFilename = "" Then
                    Return enumBitmapType.None
                ElseIf IsNumeric(ctrlGraph.PeColor.GraphBmpFilename) Then
                    Return DirectCast([Enum].Parse(GetType(enumBitmapType), ctrlGraph.PeColor.GraphBmpFilename, True), enumBitmapType)
                Else
                    Return enumBitmapType.Custom
                End If
            End Get
            Set(ByVal Value As enumBitmapType)
                If Value = enumBitmapType.Custom OrElse Value = enumBitmapType.None Then
                Else
                    m_eGraphBitmapType = Value
                    ctrlGraph.PeColor.GraphBmpFilename = CInt(Value).ToString()
                End If
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property XAxisColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.XAxis
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_XAxisColor = Value
                ctrlGraph.PeColor.XAxis = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GridLineControl() As enumGridLineControl
            Get
                Return DirectCast([Enum].Parse(GetType(enumGridLineControl), ctrlGraph.PeGrid.LineControl.ToString, True), enumGridLineControl)
            End Get
            Set(ByVal Value As enumGridLineControl)
                m_eGridLineControl = Value
                ctrlGraph.PeGrid.LineControl = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GridLineControl), Value.ToString, True), Gigasoft.ProEssentials.Enums.GridLineControl)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GridInFront() As Boolean
            Get
                Return ctrlGraph.PeGrid.InFront
            End Get
            Set(ByVal Value As Boolean)
                m_bGridInFront = Value
                ctrlGraph.PeGrid.InFront = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GridLineStyle() As enumGridLineStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumGridLineStyle), ctrlGraph.PeGrid.Style.ToString, True), enumGridLineStyle)
            End Get
            Set(ByVal Value As enumGridLineStyle)
                m_eGridLineStyle = Value
                ctrlGraph.PeGrid.Style = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GridStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.GridStyle)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property MainTitleFont() As Font
            Get
                Dim eStyle As System.Drawing.FontStyle
                If ctrlGraph.PeFont.MainTitle.Bold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
                If ctrlGraph.PeFont.MainTitle.Underline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline

                Return New Font(ctrlGraph.PeFont.MainTitle.Font, 16, eStyle)
            End Get
            Set(ByVal Value As Font)
                ctrlGraph.PeFont.MainTitle.Font = Value.Name
                ctrlGraph.PeFont.MainTitle.Bold = Value.Bold
                ctrlGraph.PeFont.MainTitle.Underline = Value.Underline
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()

                m_MainTitleFont = Me.MainTitleFont
            End Set
        End Property

        Public Overridable Property SubTitleFont() As Font
            Get
                Dim eStyle As System.Drawing.FontStyle
                If ctrlGraph.PeFont.SubTitle.Bold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
                If ctrlGraph.PeFont.SubTitle.Underline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline

                Return New Font(ctrlGraph.PeFont.SubTitle.Font, 16, eStyle)
            End Get
            Set(ByVal Value As Font)
                ctrlGraph.PeFont.SubTitle.Font = Value.Name
                ctrlGraph.PeFont.SubTitle.Bold = Value.Bold
                ctrlGraph.PeFont.SubTitle.Underline = Value.Underline
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()

                m_SubTitleFont = Me.SubTitleFont
            End Set
        End Property

        Public Overridable Property LabelFont() As Font
            Get
                Dim eStyle As System.Drawing.FontStyle
                If ctrlGraph.PeFont.Label.Bold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
                If ctrlGraph.PeFont.Label.Underline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline

                Return New Font(ctrlGraph.PeFont.Label.Font, 16, eStyle)
            End Get
            Set(ByVal Value As Font)
                ctrlGraph.PeFont.Label.Font = Value.Name
                ctrlGraph.PeFont.Label.Bold = Value.Bold
                ctrlGraph.PeFont.Label.Underline = Value.Underline
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()

                m_LabelFont = Me.LabelFont
            End Set
        End Property

        Public Overridable Property PointsToKeep() As Integer
            Get
                Return m_iPointsToKeep
            End Get
            Set(ByVal Value As Integer)
                If Value < 10 Then
                    Throw New System.Exception("The number of points to keep must be greater than 10.")
                End If

                m_iPointsToKeep = Value
                Me.ctrlGraph.PeData.Points = m_iPointsToKeep

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property MultiAxisStyle() As enumAxisStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumAxisStyle), ctrlGraph.PeGrid.Option.MultiAxisStyle.ToString, True), enumAxisStyle)
            End Get
            Set(ByVal Value As enumAxisStyle)
                m_eMultiAxisStyle = Value
                ctrlGraph.PeGrid.Option.MultiAxisStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.MultiAxisStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.MultiAxisStyle)
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property MultiAxisSeperatorSize() As Integer
            Get
                Return ctrlGraph.PeGrid.Option.MultiAxisSeparatorSize
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 OrElse Value > 300 Then
                    Throw New System.Exception("The size of the axis seperator must be between 0 and 300.")
                End If

                m_iMultiAxisSeperatorSize = Value
                ctrlGraph.PeGrid.Option.MultiAxisSeparatorSize = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property AxisFontScale() As Single
            Get
                Return ctrlGraph.PeFont.SizeAxisLabelCntl
            End Get
            Set(ByVal Value As Single)
                If Value < 0.5 OrElse Value > 2 Then
                    Throw New System.Exception("The size of the axis font scale must be between 0.5 and 2.")
                End If

                m_fltAxisFontScale = Value
                ctrlGraph.PeFont.SizeAxisLabelCntl = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property GridFontScale() As Single
            Get
                Return ctrlGraph.PeFont.SizeGridNumberCntl
            End Get
            Set(ByVal Value As Single)
                If Value < 0.5 OrElse Value > 2 Then
                    Throw New System.Exception("The size of the grid font scale must be between 0.5 and 2.")
                End If

                m_fltGridFontScale = Value
                ctrlGraph.PeFont.SizeGridNumberCntl = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property TitleFontScale() As Single
            Get
                Return ctrlGraph.PeFont.SizeTitleCntl
            End Get
            Set(ByVal Value As Single)
                If Value < 0.5 OrElse Value > 2 Then
                    Throw New System.Exception("The size of the title font scale must be between 0.5 and 2.")
                End If

                m_fltTitleFontScale = Value
                ctrlGraph.PeFont.SizeTitleCntl = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property LegendFontScale() As Single
            Get
                Return ctrlGraph.PeFont.SizeLegendCntl
            End Get
            Set(ByVal Value As Single)
                If Value < 0.5 OrElse Value > 2 Then
                    Throw New System.Exception("The size of the legend font scale must be between 0.5 and 2.")
                End If

                m_fltLegendFontScale = Value
                ctrlGraph.PeFont.SizeLegendCntl = Value

                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overridable Property SubSets() As Integer
            Get
                Return m_iSubsets
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Throw New System.Exception("The subset value must not be less than zero.")
                End If

                m_iPrevSubsets = m_iSubsets
                m_iSubsets = Value
            End Set
        End Property

        Public Overridable Property PrevSubSets() As Integer
            Get
                Return m_iPrevSubsets
            End Get
            Set(ByVal Value As Integer)
                m_iPrevSubsets = Value
            End Set
        End Property

        Public Overridable Property SaveDataWhenClosed() As Boolean
            Get
                Return m_bSaveDataWhenClosed
            End Get
            Set(ByVal Value As Boolean)
                m_bSaveDataWhenClosed = Value
            End Set
        End Property

        Public Overrides ReadOnly Property Chart() As Object
            Get
                Return Me.ctrlGraph
            End Get
        End Property

        Public Overrides ReadOnly Property SimToolModuleName() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property SimToolClassType() As String
            Get
                Return "MemoryChart"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByRef frmApplication As AnimatApplication, _
                                        Optional ByVal frmMdiParent As MdiChild = Nothing, _
                                        Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmApplication, frmMdiParent, frmParent)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("LicensedAnimatTools")

            Me.Icon = m_frmApplication.SmallImages.LoadIcon(myAssembly, "LicensedAnimatTools.LineChart.ico")
            CreateImageManager()

            ctrlGraph.PeString.MainTitle = ""
            ctrlGraph.PeString.SubTitle = ""
            ctrlGraph.PeGrid.Option.MultiAxisStyle = ProEssentials.Enums.MultiAxisStyle.SeparateAxes
            ctrlGraph.PeUserInterface.Allow.Customization = False

            Me.ctrlGraph.PeData.NullDataValue = m_iNullValue
            Me.ctrlGraph.PeData.NullDataValueX = m_iNullValue
            Me.ctrlGraph.PeGrid.Option.MultiAxesSeparators = ProEssentials.Enums.MultiAxesSeparators.Thick

            Me.ctrlGraph.PeData.Points = 10000
            Me.ctrlGraph.PeUserInterface.Dialog.RandomPointsToExport = True
            Me.ctrlGraph.PeUserInterface.Allow.FocalRect = False
            Me.ctrlGraph.PePlot.Allow.Bar = False
            'Me.ctrlGraph.PeUserInterface.Allow.Popup = False
            Me.ctrlGraph.PeConfigure.PrepareImages = True
            Me.ctrlGraph.PeConfigure.CacheBmp = True
            Me.ctrlGraph.PeFont.Fixed = True
            Me.ctrlGraph.PeUserInterface.Allow.Zooming = ProEssentials.Enums.AllowZooming.Horizontal
            ctrlGraph.PeData.ScaleForXData = 0

            'Me.ctrlGraph.PeGrid.Configure.ManualScaleControlY = ProEssentials.Enums.ManualScaleControl.MinMax
            'Me.ctrlGraph.PeGrid.Configure.ManualMinY = 1
            'Me.ctrlGraph.PeGrid.Configure.ManualMaxY = 200

            ''// Set Manual X scale //
            'Me.ctrlGraph.PeGrid.Configure.ManualScaleControlX = ProEssentials.Enums.ManualScaleControl.MinMax
            'Me.ctrlGraph.PeGrid.Configure.ManualMinX = 1
            'Me.ctrlGraph.PeGrid.Configure.ManualMaxX = 200

            InitializeChartData()
            ClearCharts()

            Me.ctrlGraph.PeFunction.ReinitializeResetImage()
            Me.Refresh()


            'Me.ctrlGraph.PointsToGraph = 20
            'Me.ctrlGraph.PointsToGraphInit = PEPTGI_LASTPOINTS 'Show Last Points Initially

            If Not m_bAddedHandlers Then
                AddHandler m_frmApplication.SimulationStarting, AddressOf Me.OnSimulationStarting
                AddHandler m_frmApplication.SimulationResuming, AddressOf Me.OnSimulationResuming
                AddHandler m_frmApplication.SimulationStarted, AddressOf Me.OnSimulationStarted
                AddHandler m_frmApplication.SimulationPaused, AddressOf Me.OnSimulationPaused
                AddHandler m_frmApplication.SimulationStopped, AddressOf Me.OnSimulationStopped
                m_bAddedHandlers = True
            End If

            m_aryAxisList.Clear()
            Dim doAxis As New DataObjects.Charting.Pro2DAxis(Me)
            doAxis.WorkingAxis = 0
            doAxis.Name = "Y Axis 1"
            'Setting the name on the axis adds it to the axis list

        End Sub

        Public Overrides Function CreateMenu() As MenuControl
            If m_frmApplication Is Nothing Then Throw New System.Exception("Application object is not defined.")

            m_menuMain = m_frmApplication.CreateDefaultMenu()

            Dim mcFile As MenuCommand = m_menuMain.MenuCommands("File")
            Dim mcSep As MenuCommand = New MenuCommand("-")
            Dim mcPrint As New MenuCommand("Print", "Print", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.Print.gif"), _
                                            New EventHandler(AddressOf Me.OnPrint))

            Dim iIndex As Integer = mcFile.MenuCommands.IndexOf(mcFile.MenuCommands("Exit"))
            mcFile.MenuCommands.Insert(iIndex, mcSep)
            iIndex = mcFile.MenuCommands.IndexOf(mcSep)
            mcFile.MenuCommands.Insert(iIndex, mcPrint)

            Dim mcEdit As MenuCommand = m_menuMain.MenuCommands("Edit")

            Dim mcSep2 As MenuCommand = New MenuCommand("-")
            Dim mcDelete As New MenuCommand("Delete", "Delete", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.Delete.gif"), _
                                            Shortcut.Del, New EventHandler(AddressOf Me.OnDelete))
            Dim mcAddAxis As New MenuCommand("Add Chart Axis", "AddAxis", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.AddChartAxis.gif"), _
                                            New EventHandler(AddressOf Me.OnAddAxis))
            Dim mcAddItem As New MenuCommand("Add Chart Item", "AddItem", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.AddChartItem.gif"), _
                                            New EventHandler(AddressOf Me.OnAddItem))
            Dim mcSep3 As MenuCommand = New MenuCommand("-")
            Dim mcViewData As New MenuCommand("View Data", "ViewData", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.ViewData.gif"), _
                                            New EventHandler(AddressOf Me.OnViewData))
            Dim mcExportData As New MenuCommand("Export Data", "ExportData", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.ExportData.gif"), _
                                            New EventHandler(AddressOf Me.OnExportData))
            Dim mcSetZoom As New MenuCommand("Set Zoom Axis", "SetZoomAxis", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("AnimatTools.SetZoomAxis.gif"), _
                                            New EventHandler(AddressOf Me.OnSetZoomAxis))

            mcEdit.MenuCommands.AddRange(New MenuCommand() {mcSep2, mcDelete, mcAddAxis, mcAddItem, mcSep3, mcViewData, mcExportData, mcSetZoom})


            Return m_menuMain
        End Function

        Public Overrides Function CreateToolbar(ByRef menuDefault As MenuControl) As Crownwood.Magic.Toolbars.ToolbarControl

            m_barMain = m_frmApplication.CreateDefaultToolbar(m_menuMain)

            Dim btnPrint As New ToolBarButton
            btnPrint.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.Print.gif")
            btnPrint.ToolTipText = "Print Chart"

            Dim btnDelete As New ToolBarButton
            btnDelete.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.Delete.gif")
            btnDelete.ToolTipText = "Delete"

            Dim btnAddAxis As New ToolBarButton
            btnAddAxis.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.AddChartAxis.gif")
            btnAddAxis.ToolTipText = "Add Chart Axis"

            Dim btnAddItem As New ToolBarButton
            btnAddItem.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.AddChartItem.gif")
            btnAddItem.ToolTipText = "Add Chart Data Item"

            Dim btnViewData As New ToolBarButton
            btnViewData.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.ViewData.gif")
            btnViewData.ToolTipText = "List all data points for viewing"

            Dim btnExportData As New ToolBarButton
            btnExportData.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.ExportData.gif")
            btnExportData.ToolTipText = "Export data to tab-delimited text file"

            Dim btnSetZoomAxis As New ToolBarButton
            btnSetZoomAxis.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("AnimatTools.SetZoomAxis.gif")
            btnSetZoomAxis.ToolTipText = "Manually specify the zoom axis"

            m_barMain.Buttons.AddRange(New ToolBarButton() {btnPrint, btnDelete, btnAddAxis, btnAddItem, btnViewData, btnExportData, btnSetZoomAxis})

            m_barMain.ButtonManager.SetButtonMenuItem(btnPrint, m_menuMain.MenuCommands.FindMenuCommand("Print"))
            m_barMain.ButtonManager.SetButtonMenuItem(btnDelete, m_menuMain.MenuCommands.FindMenuCommand("Delete"))
            m_barMain.ButtonManager.SetButtonMenuItem(btnAddAxis, m_menuMain.MenuCommands.FindMenuCommand("AddAxis"))
            m_barMain.ButtonManager.SetButtonMenuItem(btnAddItem, m_menuMain.MenuCommands.FindMenuCommand("AddItem"))
            m_barMain.ButtonManager.SetButtonMenuItem(btnViewData, m_menuMain.MenuCommands.FindMenuCommand("ViewData"))
            m_barMain.ButtonManager.SetButtonMenuItem(btnExportData, m_menuMain.MenuCommands.FindMenuCommand("ExportData"))
            m_barMain.ButtonManager.SetButtonMenuItem(btnSetZoomAxis, m_menuMain.MenuCommands.FindMenuCommand("SetZoomAxis"))

            Return m_barMain
        End Function

        Protected Overridable Sub CreateImageManager()

            Try

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.Delete.gif")
                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.AddChartAxis.gif")
                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.AddChartItem.gif")
                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.Print.gif")
                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.ViewData.gif")
                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.ExportData.gif")
                m_frmApplication.SmallImages.AddImage(myAssembly, "AnimatTools.SetZoomAxis.gif")

                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.Delete.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.AddChartAxis.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.AddChartItem.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.Print.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.ViewData.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.ExportData.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "AnimatTools.SetZoomAxis.gif")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone() As AnimatTools.Forms.Tools.ToolForm

            Try
                Return New Forms.Charts.LineChart
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Function

        Public Overrides Sub DrawGainChart()

            If Me.ctrlGraph Is Nothing Then Return

            ctrlGraph.PeString.XAxisLabel = m_gnGain.IndependentUnits
            ctrlGraph.PeString.YAxisLabel = m_gnGain.DependentUnits
            ctrlGraph.PeUserInterface.Allow.Customization = False
            If m_gnGain.SelectableGain Then
                Dim doParent As AnimatTools.DataObjects.DragObject = m_gnGain.DraggableParent

                If Not doParent Is Nothing AndAlso Not doParent.IncomingDataType Is Nothing AndAlso Not doParent.DataTypes.Value Is Nothing Then
                    ctrlGraph.PeString.XAxisLabel = doParent.DataTypes.Value.AxisTitle
                    ctrlGraph.PeString.YAxisLabel = doParent.IncomingDataType.AxisTitle

                    ctrlGraph.PeString.MultiBottomTitles(0) = "|Y Axis: " & doParent.IncomingDataType.LimitText & "|"
                    ctrlGraph.PeString.MultiBottomTitles(1) = "|X Axis: " & doParent.DataTypes.Value.LimitText & "|"
                    ctrlGraph.PeString.MultiBottomTitles(2) = "|Maximum Suggested Ranges|"
                    ctrlGraph.PeString.MultiBottomTitles(3) = "|  |"
                End If
            End If

            Dim dblXMin As Double = m_gnGain.LowerLimit.ActualValue
            Dim dblXMax As Double = m_gnGain.UpperLimit.ActualValue

            Dim dblStep As Double = Math.Abs(dblXMax - dblXMin) / 200
            Dim dblX As Double = dblXMin
            Dim dblY As Double = 0

            ctrlGraph.PeGrid.Style = ProEssentials.Enums.GridStyle.Dot
            ctrlGraph.PePlot.SubsetColors(0) = Color.Red
            ctrlGraph.PePlot.SubsetLineTypes(0) = ProEssentials.Enums.LineType.MediumSolid
            ctrlGraph.PeString.SubTitle = m_gnGain.GainEquation
            ctrlGraph.PeData.Points = 200
            ctrlGraph.PeData.Subsets = 1
            ctrlGraph.PeGrid.Configure.ManualMinX = dblXMin
            ctrlGraph.PeGrid.Configure.ManualMinX = dblXMax
            ctrlGraph.PeData.ScaleForXData = 0
            ctrlGraph.PeData.ScaleForYData = 0

            For iPoint As Integer = 0 To 199
                dblY = m_gnGain.CalculateGain(dblX)

                ctrlGraph.PeData.X(0, iPoint) = CSng(dblX)
                ctrlGraph.PeData.Y(0, iPoint) = CSng(dblY)

                dblX = dblX + dblStep
            Next

            ctrlGraph.PeFunction.ReinitializeResetImage()
            Me.Refresh()
        End Sub

        Protected Overridable Sub InitializeChartData()

            ctrlGraph.PeUserInterface.Allow.Customization = False
            m_strMainTitle = ctrlGraph.PeString.MainTitle
            m_strSubTitle = ctrlGraph.PeString.SubTitle
            m_strXAxisLabel = ctrlGraph.PeString.XAxisLabel
            m_strYAxisLabel = ctrlGraph.PeString.YAxisLabel
            m_bAutoScaleData = ctrlGraph.PeData.AutoScaleData
            m_eXAutoScale = DirectCast([Enum].Parse(GetType(enumScaleControl), ctrlGraph.PeGrid.Configure.ManualScaleControlX.ToString, True), enumScaleControl)
            m_dblXMin = ctrlGraph.PeGrid.Configure.ManualMinX
            m_dblXMax = ctrlGraph.PeGrid.Configure.ManualMaxX
            m_ptXAxisSize = New PointF(CSng(m_dblXMin), CSng(m_dblXMax))
            m_iXAxisScaleValue = ctrlGraph.PeData.ScaleForXData
            m_ptXAxisSize = New PointF(CSng(ctrlGraph.PeGrid.Configure.ManualMinY), CSng(ctrlGraph.PeGrid.Configure.ManualMaxY))
            m_eFontSize = DirectCast([Enum].Parse(GetType(enumFontSize), ctrlGraph.PeFont.FontSize.ToString, True), enumFontSize)
            m_fltAxisFontScale = ctrlGraph.PeFont.SizeAxisLabelCntl
            m_fltGridFontScale = ctrlGraph.PeFont.SizeGridNumberCntl
            m_fltTitleFontScale = ctrlGraph.PeFont.SizeTitleCntl
            m_fltLegendFontScale = ctrlGraph.PeFont.SizeLegendCntl

            m_eBorderType = DirectCast([Enum].Parse(GetType(enumBorderType), ctrlGraph.PeConfigure.BorderTypes.ToString, True), enumBorderType)
            m_eViewingStyle = DirectCast([Enum].Parse(GetType(enumViewingStyle), ctrlGraph.PeColor.QuickStyle.ToString, True), enumViewingStyle)
            m_bBitmapGradients = ctrlGraph.PeColor.BitmapGradientMode
            m_DeskColor = ctrlGraph.PeColor.Desk
            m_eDeskGradientStyle = DirectCast([Enum].Parse(GetType(enumGradientStyle), ctrlGraph.PeColor.DeskGradientStyle.ToString, True), enumGradientStyle)
            m_DeskGradientStart = ctrlGraph.PeColor.DeskGradientStart
            m_DeskGradientEnd = ctrlGraph.PeColor.DeskGradientEnd
            m_GraphBackColor = ctrlGraph.PeColor.GraphBackground
            m_GraphForeColor = ctrlGraph.PeColor.GraphForeground
            m_eGraphGradientStyle = DirectCast([Enum].Parse(GetType(enumGradientStyle), ctrlGraph.PeColor.GraphGradientStyle.ToString, True), enumGradientStyle)
            m_GraphGradientStart = ctrlGraph.PeColor.GraphGradientStart
            m_GraphGradientEnd = ctrlGraph.PeColor.GraphGradientEnd
            m_XAxisColor = ctrlGraph.PeColor.XAxis

            If ctrlGraph.PeColor.DeskBmpFilename.Trim.Length = 0 Then
                m_eDeskBitmapType = enumBitmapType.None
            Else
                m_eDeskBitmapType = DirectCast([Enum].Parse(GetType(enumBitmapType), ctrlGraph.PeColor.DeskBmpFilename, True), enumBitmapType)
            End If

            If ctrlGraph.PeColor.GraphBmpFilename.Trim.Length = 0 Then
                m_eGraphBitmapType = enumBitmapType.None
            Else
                m_eGraphBitmapType = DirectCast([Enum].Parse(GetType(enumBitmapType), ctrlGraph.PeColor.GraphBmpFilename, True), enumBitmapType)
            End If

            m_eGridLineControl = DirectCast([Enum].Parse(GetType(enumGridLineControl), ctrlGraph.PeGrid.LineControl.ToString, True), enumGridLineControl)
            m_eGridLineStyle = DirectCast([Enum].Parse(GetType(enumGridLineStyle), ctrlGraph.PeGrid.Style.ToString, True), enumGridLineStyle)
            m_bGridInFront = ctrlGraph.PeGrid.InFront

            Dim eStyle As System.Drawing.FontStyle
            If ctrlGraph.PeFont.MainTitle.Bold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
            If ctrlGraph.PeFont.MainTitle.Underline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline
            m_MainTitleFont = New Font(ctrlGraph.PeFont.MainTitle.Font, 16, eStyle)

            eStyle = New System.Drawing.FontStyle
            If ctrlGraph.PeFont.SubTitle.Bold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
            If ctrlGraph.PeFont.SubTitle.Underline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline
            m_SubTitleFont = New Font(ctrlGraph.PeFont.SubTitle.Font, 16, eStyle)

            eStyle = New System.Drawing.FontStyle
            If ctrlGraph.PeFont.Label.Bold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
            If ctrlGraph.PeFont.Label.Underline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline
            m_LabelFont = New Font(ctrlGraph.PeFont.Label.Font, 16, eStyle)

            m_eMultiAxisStyle = DirectCast([Enum].Parse(GetType(enumAxisStyle), ctrlGraph.PeGrid.Option.MultiAxisStyle.ToString, True), enumAxisStyle)
            m_iMultiAxisSeperatorSize = ctrlGraph.PeGrid.Option.MultiAxisSeparatorSize

            Dim doAxis As AnimatTools.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatTools.DataObjects.Charting.Axis)
                doAxis.InitializeChartData()
            Next

            If m_strXAxisLabel.Trim.Length = 0 OrElse m_strXAxisLabel = "X Axis" Then
                m_strXAxisLabel = "Time (s)"
            End If

            If m_bSetStartEndTime Then
                Me.XAutoScale = enumScaleControl.MinMax
                Me.XMin = Me.CollectStartTime.ActualValue
                Me.XMax = Me.CollectEndTime.ActualValue
            End If

        End Sub

        Protected Overridable Sub RebuildChart()

            Me.ctrlGraph.PeFunction.Reset()

            Me.ctrlGraph.PeData.NullDataValue = m_iNullValue
            Me.ctrlGraph.PeData.NullDataValueX = m_iNullValue

            Me.ctrlGraph.PeData.Points = m_iPointsToKeep
            Me.ctrlGraph.PeUserInterface.Dialog.RandomPointsToExport = True
            Me.ctrlGraph.PeUserInterface.Allow.FocalRect = False
            Me.ctrlGraph.PePlot.Allow.Bar = False
            Me.ctrlGraph.PeConfigure.PrepareImages = True
            Me.ctrlGraph.PeConfigure.CacheBmp = True
            Me.ctrlGraph.PeFont.Fixed = True

            Me.ctrlGraph.PeUserInterface.Allow.Zooming = ProEssentials.Enums.AllowZooming.Horizontal
            ctrlGraph.PeUserInterface.Allow.Customization = False
            ctrlGraph.PeData.ScaleForXData = 0

            ctrlGraph.PeString.MainTitle = m_strMainTitle
            ctrlGraph.PeString.SubTitle = m_strSubTitle
            ctrlGraph.PeString.XAxisLabel = m_strXAxisLabel
            ctrlGraph.PeString.YAxisLabel = m_strYAxisLabel
            ctrlGraph.PeData.AutoScaleData = m_bAutoScaleData
            ctrlGraph.PeGrid.Configure.ManualScaleControlX = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.ManualScaleControl), m_eXAutoScale.ToString, True), Gigasoft.ProEssentials.Enums.ManualScaleControl)
            ctrlGraph.PeGrid.Configure.ManualMinX = m_dblXMin
            ctrlGraph.PeGrid.Configure.ManualMaxX = m_dblXMax
            ctrlGraph.PeData.ScaleForXData = m_iXAxisScaleValue
            ctrlGraph.PeGrid.Configure.ManualMinY = m_ptYAxisSize.X
            ctrlGraph.PeGrid.Configure.ManualMaxY = m_ptYAxisSize.Y
            ctrlGraph.PeFont.FontSize = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.FontSize), m_eFontSize.ToString, True), Gigasoft.ProEssentials.Enums.FontSize)

            ctrlGraph.PeConfigure.BorderTypes = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.TABorder), m_eBorderType.ToString, True), Gigasoft.ProEssentials.Enums.TABorder)
            ctrlGraph.PeColor.BitmapGradientMode = m_bBitmapGradients
            ctrlGraph.PeColor.QuickStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.QuickStyle), m_eViewingStyle.ToString, True), Gigasoft.ProEssentials.Enums.QuickStyle)
            ctrlGraph.PeColor.Desk = m_DeskColor
            ctrlGraph.PeColor.DeskGradientStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GradientStyle), m_eDeskGradientStyle.ToString, True), Gigasoft.ProEssentials.Enums.GradientStyle)
            ctrlGraph.PeColor.DeskGradientStart = m_DeskGradientStart
            ctrlGraph.PeColor.DeskGradientEnd = m_DeskGradientEnd
            ctrlGraph.PeColor.GraphBackground = m_GraphBackColor
            ctrlGraph.PeColor.GraphForeground = m_GraphForeColor
            ctrlGraph.PeColor.GraphGradientStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GradientStyle), m_eGraphGradientStyle.ToString, True), Gigasoft.ProEssentials.Enums.GradientStyle)
            ctrlGraph.PeColor.GraphGradientStart = m_GraphGradientStart
            ctrlGraph.PeColor.GraphGradientEnd = m_GraphGradientEnd

            If Not (m_eDeskBitmapType = enumBitmapType.Custom OrElse m_eDeskBitmapType = enumBitmapType.None) Then
                ctrlGraph.PeColor.DeskBmpFilename = CInt(m_eDeskBitmapType).ToString()
            End If

            If Not (m_eGraphBitmapType = enumBitmapType.Custom OrElse m_eGraphBitmapType = enumBitmapType.None) Then
                ctrlGraph.PeColor.GraphBmpFilename = CInt(m_eGraphBitmapType).ToString()
            End If

            ctrlGraph.PeColor.XAxis = m_XAxisColor

            ctrlGraph.PeGrid.LineControl = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GridLineControl), m_eGridLineControl.ToString, True), Gigasoft.ProEssentials.Enums.GridLineControl)
            ctrlGraph.PeGrid.Style = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GridStyle), m_eGridLineStyle.ToString, True), Gigasoft.ProEssentials.Enums.GridStyle)
            ctrlGraph.PeGrid.InFront = m_bGridInFront

            If Not m_MainTitleFont Is Nothing Then
                ctrlGraph.PeFont.MainTitle.Font = m_MainTitleFont.Name
                ctrlGraph.PeFont.MainTitle.Bold = m_MainTitleFont.Bold
                ctrlGraph.PeFont.MainTitle.Underline = m_MainTitleFont.Underline
            End If

            If Not m_SubTitleFont Is Nothing Then
                ctrlGraph.PeFont.SubTitle.Font = m_SubTitleFont.Name
                ctrlGraph.PeFont.SubTitle.Bold = m_SubTitleFont.Bold
                ctrlGraph.PeFont.SubTitle.Underline = m_SubTitleFont.Underline
            End If

            If Not m_LabelFont Is Nothing Then
                ctrlGraph.PeFont.Label.Font = m_LabelFont.Name
                ctrlGraph.PeFont.Label.Bold = m_LabelFont.Bold
                ctrlGraph.PeFont.Label.Underline = m_LabelFont.Underline
            End If

            ctrlGraph.PeGrid.Option.MultiAxisStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.MultiAxisStyle), m_eMultiAxisStyle.ToString, True), Gigasoft.ProEssentials.Enums.MultiAxisStyle)
            ctrlGraph.PeGrid.Option.MultiAxisSeparatorSize = m_iMultiAxisSeperatorSize

            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuText(0) = "|"
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuText(1) = "Print"
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuText(2) = "Set Zoom Axis"
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuText(3) = "View Data"
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuText(4) = "Export Data"

            Me.ctrlGraph.PeUserInterface.Menu.CustomMenu(0, 0) = ProEssentials.Enums.MenuControl.Show
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenu(1, 0) = ProEssentials.Enums.MenuControl.Show
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenu(2, 0) = ProEssentials.Enums.MenuControl.Show
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenu(3, 0) = ProEssentials.Enums.MenuControl.Show
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenu(4, 0) = ProEssentials.Enums.MenuControl.Show

            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuLocation(0) = ProEssentials.Enums.CustomMenuLocation.Bottom
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuLocation(1) = ProEssentials.Enums.CustomMenuLocation.Bottom
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuLocation(2) = ProEssentials.Enums.CustomMenuLocation.Bottom
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuLocation(3) = ProEssentials.Enums.CustomMenuLocation.Bottom
            Me.ctrlGraph.PeUserInterface.Menu.CustomMenuLocation(4) = ProEssentials.Enums.CustomMenuLocation.Bottom

            ctrlGraph.PeFont.SizeAxisLabelCntl = m_fltAxisFontScale
            ctrlGraph.PeFont.SizeGridNumberCntl = m_fltGridFontScale
            ctrlGraph.PeFont.SizeTitleCntl = m_fltTitleFontScale
            ctrlGraph.PeFont.SizeLegendCntl = m_fltLegendFontScale

        End Sub

        Public Overrides Sub UpdateChartConfiguration(ByVal bReconfigureData As Boolean)

            If m_aryAxisList.Count = 0 Then
                Return
            End If

            Dim aryOldX(,) As Single
            Dim aryOldY(,) As Single
            'If bReconfigureData Then
            aryOldX = Me.ctrlGraph.PeData.X.Copy()
            aryOldY = Me.ctrlGraph.PeData.Y.Copy()
            'End If

            RebuildChart()

            'Lets remove any axis's that do not have any data attached.
            RemoveUnusedAxis()

            'Lets get the number of subsets needed.
            Dim iSubSets As Integer
            Dim doAxis As AnimatTools.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatTools.DataObjects.Charting.Axis)
                iSubSets = iSubSets + doAxis.DataColumns.Count
            Next

            Me.ctrlGraph.PeData.Subsets = iSubSets

            'Lets order the array of axis's using the WorkingAxis for each
            Dim iAxisCount As Integer = m_aryAxisList.Count - 1
            Dim aryAxislist(iAxisCount) As AnimatTools.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatTools.DataObjects.Charting.Axis)

                If doAxis.WorkingAxis > iAxisCount Then
                    Throw New System.Exception("The working axis for '" & doAxis.Name & "' is larger than the available axis list.")
                End If

                aryAxislist(doAxis.WorkingAxis) = doAxis
            Next

            Dim iSubset As Integer = 0
            For iAxis As Integer = 0 To iAxisCount
                doAxis = aryAxislist(iAxis)
                doAxis.UpdateChartConfiguration(iSubset)
            Next
            Me.ctrlGraph.PeGrid.WorkingAxis = 0 'Always reset WorkingAxis to zero

            Me.ctrlGraph.PeData.Points = m_iPointsToKeep

            'If bReconfigureData Then
            Dim aryNewX(Me.ctrlGraph.PeData.Subsets - 1, Me.ctrlGraph.PeData.Points - 1) As Single
            Dim aryNewY(Me.ctrlGraph.PeData.Subsets - 1, Me.ctrlGraph.PeData.Points - 1) As Single

            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatTools.DataObjects.Charting.Axis)
                doAxis.ReconfigureChartData(aryOldX, aryOldY, aryNewX, aryNewY)
            Next

            Me.ctrlGraph.PeData.X.CopyFrom(aryNewX)
            Me.ctrlGraph.PeData.Y.CopyFrom(aryNewY)
            'End If

            Me.ctrlGraph.PeFunction.ReinitializeResetImage()
            Me.Refresh()


            'We need to check to see if any of the data columns must have AutoCollectInterval to true.
            'If this is the case then set this value to true for the graph.
            If Not Me.AutoCollectDataInterval AndAlso RequiresAutoDataCollectInterval() Then
                Me.AutoCollectDataInterval = True
            End If

        End Sub

        Protected Overridable Sub PrepareForCharting()
            'The simulation is starting so we need to go through and add this chart and all of its
            'data columns so we can recieve data during the simulation run.

            If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
               AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then

                'If we are updating the chart at the end of the collection interval then
                'we need to setup a few variables.
                If m_bUpdateChartAtEnd Then
                    Me.CollectTimeWindow.ActualValue = (Me.CollectEndTime.ActualValue - Me.CollectStartTime.ActualValue) + 1
                    Me.PointsToKeep = CInt((Me.CollectTimeWindow.ActualValue / Me.CollectDataInterval.ActualValue) + 0.5)
                End If

                'Lets recreate the charts
                Dim frmSim As AnimatTools.Forms.SimulatorView = GetSimulatorView()

                'If we do not find a datachart with this id then add one.
                If Not frmSim.Simulator.FindDataChart(m_strID, False) Then
                    Dim strXml As String = Me.SaveChartToXml()
                    frmSim.Simulator.AddDataChart(Me.SimToolModuleName, Me.SimToolClassType, strXml)
                    ClearCharts()
                End If

                Dim doAxis As AnimatTools.DataObjects.Charting.Axis
                For Each deEntry As DictionaryEntry In m_aryAxisList
                    doAxis = DirectCast(deEntry.Value, AnimatTools.DataObjects.Charting.Axis)
                    doAxis.PrepareForCharting()
                Next

                UpdateChartConfiguration(True)
            End If

        End Sub

        Public Overrides Sub AfterDropped()

            'If the simulator is loaded and paused then we need to add this chart to the simulation.
            If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
               AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then

                If Not Util.Application.SimulationView.Simulator.FindDataChart(m_strID, False) Then
                    Dim strXml As String = Me.SaveChartToXml()
                    Util.Application.SimulationView.Simulator.AddDataChart(Me.SimToolModuleName, Me.SimToolClassType, strXml)
                End If
            End If

        End Sub

        Protected Overridable Sub UpdateChartData()
            Try

                Dim aryXData(,) As Single
                Dim aryYData(,) As Single
                Dim iRowCount As Integer

                If Not m_bUpdateChartAtEnd OrElse (m_bUpdateChartAtEnd AndAlso ((Util.Application.SimulationView.Simulator.CurrentMillisecond / 1000) > m_snCollectEndTime.ActualValue)) Then
                    iRowCount = Util.Application.SimulationView.Simulator.RetrieveChartData(m_strID, aryXData, aryYData)

                    If iRowCount > Me.ctrlGraph.PeData.Points Then
                        If Me.UpdateChartAtEnd Then
                            Me.ctrlGraph.PeData.Points = iRowCount
                        Else
                            iRowCount = Me.ctrlGraph.PeData.Points
                        End If
                    End If

                    If iRowCount > 0 Then
                        'Debug.WriteLine("Updating chart data. " & Now())

                        Gigasoft.ProEssentials.Api.PEvset(Me.ctrlGraph.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.AppendXData, aryXData, iRowCount)
                        Gigasoft.ProEssentials.Api.PEvset(Me.ctrlGraph.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.AppendYData, aryYData, iRowCount)

                        ctrlGraph.PeData.ScaleForXData = 0
                        ctrlGraph.PeData.ScaleForYData = 0

                        Me.ctrlGraph.PeFunction.ReinitializeResetImage()
                        Me.ctrlGraph.Refresh()

                        'If we are updating it at the end then we should only get the data once.
                        If m_bUpdateChartAtEnd Then
                            m_Timer.Enabled = False
                        End If
                    End If
                End If

            Catch ex As System.Exception
                m_Timer.Enabled = False
            End Try

        End Sub

        Protected Overridable Sub ConfigChartForRunningSimulation()
            Me.ctrlGraph.PeUserInterface.Allow.Zooming = ProEssentials.Enums.AllowZooming.None

            'Undo the current zoom if any.
            Me.ctrlGraph.PeGrid.Zoom.Mode = False
        End Sub

        Protected Overridable Sub ConfigChartForPausedSimulation()
            Me.ctrlGraph.PeUserInterface.Allow.Zooming = ProEssentials.Enums.AllowZooming.Horizontal
        End Sub

        Protected Overridable Sub ClearCharts()

            For iSubSet As Integer = 0 To Me.ctrlGraph.PeData.Subsets - 1
                For iPoint As Integer = 0 To Me.ctrlGraph.PeData.Points - 1
                    Me.ctrlGraph.PeData.X(iSubSet, iPoint) = 0
                    Me.ctrlGraph.PeData.Y(iSubSet, iPoint) = m_iNullValue
                Next
            Next

        End Sub

        Protected Overridable Sub SetZoomAxis()
            Dim frmZoomAxis As New ZoomAxis

            frmZoomAxis.m_fltMin = CSng(Me.ctrlGraph.PeGrid.Configure.ManualMinX)
            frmZoomAxis.m_fltMax = CSng(Me.ctrlGraph.PeGrid.Configure.ManualMaxX)

            If frmZoomAxis.ShowDialog = DialogResult.OK Then
                Me.ctrlGraph.PeGrid.Zoom.MinX = frmZoomAxis.m_fltMin
                Me.ctrlGraph.PeGrid.Zoom.MaxX = frmZoomAxis.m_fltMax
                Me.ctrlGraph.PeGrid.Zoom.Mode = True
                Me.ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End If

        End Sub

        Protected Overridable Sub PackageChartDataIntoArrays(ByRef aryNames() As String, ByRef aryData(,) As Single)

            ReDim aryNames(Me.ctrlGraph.PeData.Subsets)

            aryNames(0) = "Time"
            For iSubSet As Integer = 1 To Me.ctrlGraph.PeData.Subsets
                aryNames(iSubSet) = Me.ctrlGraph.PeString.SubsetLabels(iSubSet - 1)
            Next

            'Lets first go through and see where the first valid piece of data is in the points array.
            Dim iPoint As Integer = 0
            Dim bDone As Boolean = False
            While iPoint < Me.ctrlGraph.PeData.Points AndAlso Not bDone
                If Me.ctrlGraph.PeData.Y(0, iPoint) <> Me.ctrlGraph.PeData.NullDataValue Then
                    bDone = True
                End If
                If Not bDone Then iPoint = iPoint + 1
            End While

            ReDim aryData(Me.ctrlGraph.PeData.Points - iPoint - 1, Me.ctrlGraph.PeData.Subsets)

            Dim iNum As Integer = aryData.GetLength(1)

            Dim iD As Integer = 0
            For iP As Integer = iPoint To Me.ctrlGraph.PeData.Points - 1
                aryData(iD, 0) = Me.ctrlGraph.PeData.X(0, iP)

                For iSubset As Integer = 0 To Me.ctrlGraph.PeData.Subsets - 1
                    aryData(iD, iSubset + 1) = Me.ctrlGraph.PeData.Y(iSubset, iP)
                Next

                iD = iD + 1
            Next

        End Sub

        Protected Overridable Sub ViewChartData()
            Dim frmLineData As New LineData

            PackageChartDataIntoArrays(frmLineData.m_aryNames, frmLineData.m_aryData)
            frmLineData.ShowDialog()
        End Sub

        Protected Overridable Sub ExportChartData()
            Dim strFile As String = Util.Application.ProjectPath & Me.Title & ".txt"

            Dim sr As StreamWriter = File.CreateText(strFile)

            Dim aryNames() As String
            Dim aryData(,) As Single

            PackageChartDataIntoArrays(aryNames, aryData)

            Dim strLine As String = ""
            For Each strName As String In aryNames
                strLine = strLine & strName & vbTab
            Next
            sr.WriteLine(strLine)

            Dim iLines As Integer = aryData.GetLength(0) - 1
            Dim iCols As Integer = aryData.GetLength(1) - 1
            For iLine As Integer = 0 To iLines
                strLine = ""

                For iCol As Integer = 0 To iCols
                    strLine = strLine & aryData(iLine, iCol) & vbTab
                Next
                sr.WriteLine(strLine)
            Next

            sr.Close()

        End Sub

        Protected Overrides Sub BuildProperties()

            Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Update Chart At End", GetType(Boolean), "UpdateChartAtEnd", _
                                        "Data Properties", "If this is true then the chart will not update its graphics until all data is collected for the time period.", Me.UpdateChartAtEnd))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Y Axis Offset", GetType(Double), "YAxisOffset", _
                                        "Data Properties", "Offset of the data from the Y Axis.", Me.YAxisOffset))

            If Not Me.UpdateChartAtEnd Then
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Set Start/End Time", GetType(Boolean), "SetStartEndTime", _
                                            "Data Properties", "Sets whether you want to set the start/end time, or if you just want to collect data continuosly.", Me.SetStartEndTime, Me.UpdateChartAtEnd))

                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Points To Keep", m_iPointsToKeep.GetType, "PointsToKeep", _
                                            "Data Properties", "Sets how many points that this chart will keep for each data column.", m_iPointsToKeep, Me.UpdateChartAtEnd))


                pbNumberBag = m_snCollectTimeWindow.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Collect Time Window", pbNumberBag.GetType(), "CollectTimeWindow", _
                                            "Data Properties", "Sets how much data the memory chart in the simulation can hold.", _
                                            pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter), Me.UpdateChartAtEnd))
            End If

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Auto CollectInterval", GetType(Boolean), "AutoCollectDataInterval", _
                                        "Data Properties", "If this is true then the collect data interval is automatically calculated from the data items added." & _
                                        "The smallest timestep for the items added is used.", Me.AutoCollectDataInterval, Me.RequiresAutoDataCollectInterval))

            pbNumberBag = m_snCollectDataInterval.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Collect Data Interval", pbNumberBag.GetType(), "CollectDataInterval", _
                                        "Data Properties", "Sets how often in milliseconds that data is collected during the simulation.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter), m_bAutoCollectDataInterval))

            If m_bSetStartEndTime Then
                pbNumberBag = m_snCollectStartTime.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Start Time", pbNumberBag.GetType(), "CollectStartTime", _
                                            "Data Properties", "Sets the time where this chart should begin collecting data.", _
                                            pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

                pbNumberBag = m_snCollectEndTime.Properties
                m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("End Time", pbNumberBag.GetType(), "CollectEndTime", _
                                            "Data Properties", "Sets how time where this chart should stop collecting data.", _
                                            pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            pbNumberBag = m_snUpdateDataInterval.Properties
            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Update Data Interval", pbNumberBag.GetType(), "UpdateDataInterval", _
                                        "Data Properties", "Sets how often the data chart polls to see if new data points have been collected.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Save Data When Closed", m_bSaveDataWhenClosed.GetType, "SaveDataWhenClosed", _
                                        "Data Properties", "If this is true then the collected data points will be stored in the file and will be visible again " & _
                                        "when this chart is re-opened.", m_bSaveDataWhenClosed))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Title", GetType(String), "MainTitle", _
                                        "Graphical Properties", "Sets the main title for this chart.", Me.MainTitle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("SubTitle", GetType(String), "SubTitle", _
                                        "Graphical Properties", "Sets the subtitle for this chart.", Me.SubTitle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("AutoScale", GetType(Boolean), "AutoScaleData", _
                                        "Graphical Properties", "Determines if the axis settins are automatically scaled to fit the data.", Me.AutoScaleData))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("X AutoScale", GetType(enumScaleControl), "XAutoScale", _
                                        "Graphical Properties", "Determines whether the x axis is automatically scaled to fit the data."))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("X Min Value", GetType(Double), "XMin", _
                                        "Graphical Properties", "Sets the minimum size of the x axis.", Me.XMin))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("X Max Value", GetType(Double), "XMax", _
                                        "Graphical Properties", "Sets the maximum size of the x axis.", Me.XMax))

            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("XAxisSize", GetType(PointF), "XAxisSize", _
            '                            "Graphical Properties", "Sets the size of the x axis.", Me.XAxisSize))

            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("YAxisSize", GetType(PointF), "YAxisSize", _
            '                            "Graphical Properties", "Sets the size of the y axis.", Me.YAxisSize, Not Me.AutoScaleData))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Font Size", GetType(enumFontSize), "FontSize", _
                                        "Graphical Properties", "Determines the global size of the font used in this chart.", Me.FontSize))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Border Type", GetType(enumBorderType), "BorderType", _
                                        "Graphical Properties", "Determines the type of border for the chart.", Me.BorderType))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Viewing Style", GetType(enumViewingStyle), "ViewingStyle", _
                                        "Graphical Properties", "Determines the look-and-feel of the chart.", Me.ViewingStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Bitmap Gradients", GetType(Boolean), "BitmapGradients", _
                                        "Graphical Properties", "Determines whether bitmap gradients are used in the viewing style.", Me.BitmapGradients))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Desk Color", GetType(System.Drawing.Color), "DeskColor", _
                                        "Graphical Properties", "Determines whether color of the desktop.", Me.DeskColor))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Desk Gradient Style", GetType(enumGradientStyle), "DeskGradientStyle", _
                                        "Graphical Properties", "Determines the style of gradient to use for the desktop.", Me.DeskGradientStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Desk Gradient Start", GetType(System.Drawing.Color), "DeskGradientStart", _
                                        "Graphical Properties", "Determines the starting color for the desktop gradient.", Me.DeskGradientStart))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Desk Gradient End", GetType(System.Drawing.Color), "DeskGradientEnd", _
                                        "Graphical Properties", "Determines the ending color for the desktop gradient.", Me.DeskGradientEnd))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Graph Backcolor", GetType(System.Drawing.Color), "GraphBackColor", _
                                        "Graphical Properties", "Determines whether background color of the graph.", Me.GraphBackColor))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Graph Forecolor", GetType(System.Drawing.Color), "GraphForeColor", _
                                        "Graphical Properties", "Determines whether foreground color of the graph.", Me.GraphForeColor))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Graph Gradient Style", GetType(enumGradientStyle), "GraphGradientStyle", _
                                        "Graphical Properties", "Determines the style of gradient to use for the graph.", Me.GraphGradientStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Graph Gradient Start", GetType(System.Drawing.Color), "GraphGradientStart", _
                                        "Graphical Properties", "Determines the starting color for the graph gradient.", Me.GraphGradientStart))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Graph Gradient End", GetType(System.Drawing.Color), "GraphGradientEnd", _
                                        "Graphical Properties", "Determines the ending color for the graph gradient.", Me.GraphGradientEnd))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Desk Bitmap Type", GetType(enumBitmapType), "DeskBitmapType", _
                                        "Graphical Properties", "Determines the style of gradient to use for the desktop.", Me.DeskBitmapType))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Graph Bitmap Type", GetType(enumBitmapType), "GraphBitmapType", _
                                        "Graphical Properties", "Determines the style of gradient to use for the desktop.", Me.GraphBitmapType))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("X Axis Color", GetType(System.Drawing.Color), "XAxisColor", _
                                        "Graphical Properties", "Determines the color of the x axis.", Me.XAxisColor))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Grid Line Control", GetType(enumGridLineControl), "GridLineControl", _
                                        "Graphical Properties", "Determines how the grid lines are drawn.", Me.GridLineControl))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Grid Line Style", GetType(enumGridLineStyle), "GridLineStyle", _
                                        "Graphical Properties", "Determines how the grid lines are drawn.", Me.GridLineStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Grid In Front", GetType(Boolean), "GridInFront", _
                                        "Graphical Properties", "Determines whether the grid is drawn in front of the data.", Me.GridInFront))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Title Font", GetType(Font), "MainTitleFont", _
                                        "Graphical Properties", "Sets the font to use for the main title.", Me.MainTitleFont))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Subtitle Font", GetType(Font), "SubTitleFont", _
                                        "Graphical Properties", "Sets the font to use for the subtitle.", Me.SubTitleFont))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Label Font", GetType(Font), "LabelFont", _
                                        "Graphical Properties", "Sets the font to use for the labels like the x and y axis and the legend.", Me.LabelFont))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("X Axis Scale", GetType(Integer), "XAxisScaleValue", _
                                        "Graphical Properties", "Determines the exponent to use when this axis of the graph. If this is set to -9 then " & _
                                        "all the numeric values will be drawn with n standing for nano."))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("MultiAxis Style", GetType(enumAxisStyle), "MultiAxisStyle", _
                                        "Graphical Properties", "Determines how multiple axis are drawn together.", Me.MultiAxisStyle))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("MultiAxis Seperator Size", GetType(Integer), "MultiAxisSeperatorSize", _
                                        "Graphical Properties", "Determines how large a gap to draw between each axis. " & _
                                        "The MultiAxis Style property must be set to SeparateAxes.", Me.MultiAxisSeperatorSize))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Axis Font Scale", GetType(Single), "AxisFontScale", _
                                        "Graphical Properties", "Sets the scale of the font for the axis. Valid values range from 0.5 to 2.", Me.AxisFontScale))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Grid Font Scale", GetType(Single), "GridFontScale", _
                                        "Graphical Properties", "Sets the scale of the font for the grid numbers. Valid values range from 0.5 to 2.", Me.GridFontScale))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Title Font Scale", GetType(Single), "TitleFontScale", _
                                        "Graphical Properties", "Sets the scale of the font for the title. Valid values range from 0.5 to 2.", Me.TitleFontScale))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Legend Font Scale", GetType(Single), "LegendFontScale", _
                                        "Graphical Properties", "Sets the scale of the font for the title. Valid values range from 0.5 to 2.", Me.LegendFontScale))

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem() 'Into Form Element

            m_bSaveDataWhenClosed = oXml.GetChildBool("SaveDataWhenClosed", m_bSaveDataWhenClosed)
            m_bUpdateChartAtEnd = oXml.GetChildBool("UpdateChartAtEnd", False)
            m_iPointsToKeep = oXml.GetChildInt("PointsToKeep", m_iPointsToKeep)
            m_dblYAxisOffset = oXml.GetChildDouble("YAxisOffset", m_dblYAxisOffset)

            'Load the graph configuration info
            If oXml.FindChildElement("GraphConfig", False) Then
                Dim aryData() As Byte = oXml.GetChildByteArray("GraphConfig")

                Dim graphStream As New System.IO.MemoryStream
                graphStream.Write(aryData, 0, aryData.Length)
                graphStream.Position = 0
                Me.ctrlGraph.PeFunction.LoadObjectFromStream(graphStream)
            End If

            oXml.OutOfElem() 'Outof Form Element

            InitializeChartData()
            Me.ctrlGraph.PeFunction.ReinitializeResetImage()
            Me.Refresh()

            UpdateChartConfiguration(False)

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            oXml.AddChildElement("SaveDataWhenClosed", m_bSaveDataWhenClosed)
            oXml.AddChildElement("UpdateChartAtEnd", m_bUpdateChartAtEnd)
            oXml.AddChildElement("PointsToKeep", m_iPointsToKeep)
            oXml.AddChildElement("YAxisOffset", m_dblYAxisOffset)

            Dim graphTempStream As System.IO.MemoryStream

            'If we have a lot of data then saving that data makes the chart files
            'massive for no real benefit. All it buys you is that you get to see the
            'last data run when you load the project up. But that is of very limited
            'usefulness. It makes more sense to just not store that data.
            If Not m_bSaveDataWhenClosed Then
                'We want to keep the data this is currently displayed on the graph.
                'so lets save it off into a temporary array, then get rid of the data
                'for the save method.
                graphTempStream = Me.ctrlGraph.PeFunction.SaveObjectToStream()

                Me.ctrlGraph.PeData.X.Clear()
                Me.ctrlGraph.PeData.Y.Clear()
            End If

            'save the graph configuration info
            Dim graphStream As System.IO.MemoryStream = Me.ctrlGraph.PeFunction.SaveObjectToStream()
            Dim aryData(CInt(graphStream.Length)) As Byte
            graphStream.Read(aryData, 0, CInt(graphStream.Length))
            oXml.AddChildElement("GraphConfig", aryData)

            If Not m_bSaveDataWhenClosed Then
                'Now lets put the original graph data back.
                Me.ctrlGraph.PeFunction.LoadObjectFromStream(graphTempStream)

                Me.ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.ctrlGraph.Refresh()
            End If

            oXml.OutOfElem()

        End Sub

        'This is used to save the xml out for when we add a chart to the opened simulation.
        Public Overrides Function SaveChartToXml() As String

            Dim oXml As New Interfaces.StdXml
            oXml.AddElement("ChartConfiguration")
            oXml.AddChildElement("DataChart")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)

            'oXml.AddChildElement("UpdateChartAtEnd", m_bUpdateChartAtEnd)
            oXml.AddChildElement("SetStartTime", m_bSetStartEndTime)
            oXml.AddChildElement("StartTime", CSng(m_snCollectStartTime.ActualValue))
            oXml.AddChildElement("EndTime", CSng(m_snCollectEndTime.ActualValue))
            oXml.AddChildElement("CollectTimeWindow", CSng(m_snCollectTimeWindow.ActualValue))
            oXml.AddChildElement("CollectInterval", CSng(m_snCollectDataInterval.ActualValue))
            oXml.AddChildElement("AlwaysActive", Not m_bSetStartEndTime)

            oXml.OutOfElem()

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveXml(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("DataChart")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", "FileChart")
            oXml.AddChildElement("OutputFilename", Me.Title & ".txt")
            oXml.AddChildElement("Name", Me.Title)

            'For data charts we will always use a start and end time, and we will always
            'set the collect time window to default to this start/end time.
            oXml.AddChildElement("SetStartTime", True)
            oXml.AddChildElement("CollectTimeWindow", -1)
            oXml.AddChildElement("AlwaysActive", False)

            oXml.AddChildElement("StartTime", CSng(m_snCollectStartTime.ActualValue))
            oXml.AddChildElement("EndTime", CSng(m_snCollectEndTime.ActualValue))
            oXml.AddChildElement("CollectInterval", CSng(m_snCollectDataInterval.ActualValue))

            oXml.AddChildElement("DataColumns")
            oXml.IntoElem()
            Dim doAxis As AnimatTools.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatTools.DataObjects.Charting.Axis)
                doAxis.SaveXml(oXml)
            Next
            oXml.OutOfElem() ' Outof DataColumns

            oXml.OutOfElem() ' Outof DataChart

        End Sub

        Public Overrides Sub OnContentClosing(ByVal e As System.ComponentModel.CancelEventArgs)
            MyBase.OnContentClosing(e)

            If Not e.Cancel AndAlso m_bAddedHandlers Then
                RemoveHandler m_frmApplication.SimulationStarting, AddressOf Me.OnSimulationStarting
                RemoveHandler m_frmApplication.SimulationResuming, AddressOf Me.OnSimulationResuming
                RemoveHandler m_frmApplication.SimulationStarted, AddressOf Me.OnSimulationStarted
                RemoveHandler m_frmApplication.SimulationPaused, AddressOf Me.OnSimulationPaused
                RemoveHandler m_frmApplication.SimulationStopped, AddressOf Me.OnSimulationStopped
                m_bAddedHandlers = False

                If Not Util.Application.SimulationView Is Nothing AndAlso Not Util.Application.SimulationView.Simulator Is Nothing _
                   AndAlso Util.Application.SimulationView.Simulator.Loaded AndAlso Util.Application.SimulationView.Simulator.Paused Then

                    If Util.Application.SimulationView.Simulator.FindDataChart(m_strID, False) Then
                        Util.Application.SimulationView.Simulator.RemoveDataChart(m_strID)
                    End If
                End If

            End If
        End Sub

#End Region

#Region " Events "

#Region " Chart Events "

        Private Sub ctrlGraph_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlGraph.DragDrop
            MyBase.OnDragItemDropped(sender, e)
        End Sub

        Private Sub ctrlGraph_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlGraph.DragEnter
            MyBase.OnDragItemEntered(sender, e)
        End Sub

        Private Sub ctrlGraph_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlGraph.DragLeave
            MyBase.OnDragItemLeave(sender, e)
        End Sub

        Private Sub ctrlGraph_PeCustomizeDlg(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlGraph.PeCustomizeDlg
            m_doFormHelper.IsDirty = True

            If Not Me.Viewer.HierarchyBar.TreeView.SelectedNode Is Nothing AndAlso Me.Viewer.HierarchyBar.TreeView.SelectedNode.Tag Is Me Then
                Me.Viewer.HierarchyBar.PropertyData = Me.Properties
            End If
        End Sub

        Protected Sub OnPrint(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.ctrlGraph.PeFunction.PrintGraph(0, 0, ProEssentials.Enums.DefOrientation.DriverDefault)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not m_frmViewer.HierarchyBar.SelectedItem Is Nothing Then

                    If TypeOf m_frmViewer.HierarchyBar.SelectedItem Is AnimatTools.DataObjects.Charting.Axis Then
                        Dim doAxis As AnimatTools.DataObjects.Charting.Axis = DirectCast(m_frmViewer.HierarchyBar.SelectedItem, AnimatTools.DataObjects.Charting.Axis)
                        doAxis.DeleteAxis()
                    ElseIf TypeOf m_frmViewer.HierarchyBar.SelectedItem Is AnimatTools.DataObjects.Charting.DataColumn Then
                        Dim doItem As AnimatTools.DataObjects.Charting.DataColumn = DirectCast(m_frmViewer.HierarchyBar.SelectedItem, AnimatTools.DataObjects.Charting.DataColumn)
                        doItem.RemoveFromAxis()
                    End If

                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAddItem(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'If Not m_frmViewer.HierarchyBar.SelectedItem Is Nothing Then
                Dim doSelAxis As AnimatTools.DataObjects.Charting.Axis

                If TypeOf m_frmViewer.HierarchyBar.SelectedItem Is AnimatTools.DataObjects.Charting.Axis Then
                    doSelAxis = DirectCast(m_frmViewer.HierarchyBar.SelectedItem, AnimatTools.DataObjects.Charting.Axis)
                ElseIf TypeOf m_frmViewer.HierarchyBar.SelectedItem Is AnimatTools.DataObjects.Charting.DataColumn Then
                    Dim doItem As AnimatTools.DataObjects.Charting.DataColumn = DirectCast(m_frmViewer.HierarchyBar.SelectedItem, AnimatTools.DataObjects.Charting.DataColumn)
                    doSelAxis = doItem.ParentAxis
                Else
                    If Me.AxisList.Count = 1 Then
                        doSelAxis = DirectCast(Me.AxisList.GetItem(0), AnimatTools.DataObjects.Charting.Axis)
                    ElseIf Me.AxisList.Count = 0 Then
                        Me.OnAddAxis(Me, New System.EventArgs)
                        If Me.AxisList.Count = 1 Then
                            doSelAxis = DirectCast(Me.AxisList.GetItem(0), AnimatTools.DataObjects.Charting.Axis)
                        Else
                            MessageBox.Show("Please add an axis to the chart.")
                        End If
                    Else
                        MessageBox.Show("You must select an axis to add an item.")
                    End If
                End If

                If Not doSelAxis Is Nothing Then
                    doSelAxis.AddDataItem()
                End If

                'End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnViewData(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.ViewChartData()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnExportData(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.ExportChartData()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSetZoomAxis(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.SetZoomAxis()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Simulation Events "

        Protected Sub OnSimulationStarting()

            Try
                ConfigChartForRunningSimulation()
                ClearCharts()
                PrepareForCharting()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationResuming()

            Try
                ConfigChartForRunningSimulation()
                PrepareForCharting()

                m_Timer.Interval = CInt(m_snUpdateDataInterval.ActualValue * 1000)
                m_Timer.Enabled = True
                'Debug.WriteLine("Chart Data Resuming")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationStarted()

            Try
                ConfigChartForRunningSimulation()

                m_Timer.Interval = CInt(m_snUpdateDataInterval.ActualValue * 1000)
                m_Timer.Enabled = True
                'Debug.WriteLine("Chart Data Started")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationPaused()

            Try
                ConfigChartForPausedSimulation()

                m_Timer.Enabled = False
                'Debug.WriteLine("Chart Data Pausing")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationStopped()

            Try
                ConfigChartForPausedSimulation()

                m_Timer.Enabled = False
                'Debug.WriteLine("Chart Data Stopped")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Timer.Tick

            Try
                UpdateChartData()
                Application.DoEvents()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Protected Overrides Sub OnAddAxis(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_aryAxisList.Count >= 6 Then
                    Throw New System.Exception("You can not have more than 6 Y Axis.")
                End If

                'ClearCharts()

                Dim iIndex As Integer = Util.ExtractIDCount("Y Axis", m_aryAxisList, " ") + 1
                Dim strName As String = "Y Axis " & iIndex

                Dim doAxis As New DataObjects.Charting.Pro2DAxis(Me)
                doAxis.WorkingAxisInternal = FindAvailableWorkingAxis()
                doAxis.Name = strName

                'Setting the name on the axis adds it to the axis list
                Me.Viewer.HierarchyBar.TreeView.Invalidate()

                If Not doAxis.TreeNode Is Nothing Then
                    doAxis.TreeNode.ExpandAll()
                    Me.Viewer.HierarchyBar.TreeView.SelectedNode = doAxis.TreeNode
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnClearCharts(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                ClearCharts()
                Me.ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlGraph_PeCustomMenu(ByVal sender As Object, ByVal e As Gigasoft.ProEssentials.EventArg.CustomMenuEventArgs) Handles ctrlGraph.PeCustomMenu
            Try
                If e.MenuIndex = 1 Then
                    Me.ctrlGraph.PeFunction.PrintGraph(0, 0, ProEssentials.Enums.DefOrientation.DriverDefault)
                End If

                If e.MenuIndex = 2 Then
                    SetZoomAxis()
                End If

                If e.MenuIndex = 3 Then
                    ViewChartData()
                End If

                If e.MenuIndex = 4 Then
                    ExportChartData()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

