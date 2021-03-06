Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Text.RegularExpressions

Namespace Framework

    Public Class Util

        'Windows API Declarations
        Declare Function PostMessage _
        Lib "user32" _
        Alias "PostMessageA" _
        (ByVal hwnd As Integer, _
        ByVal wMsg As Integer, _
        ByVal wParam As Integer, _
        ByVal lParam As String) As Integer

        Declare Function GlobalLock _
        Lib "kernel32" _
        (ByVal hMem As Integer) As Integer

        Declare Function GlobalFree _
        Lib "kernel32" _
        (ByVal hMem As Integer) As Integer

        Declare Function hwrite _
        Lib "kernel32" _
        Alias "_hwrite" _
        (ByVal hFile As Integer, _
        ByVal lpBuffer As String, _
        ByVal lBytes As Integer) As Integer

        Declare Function lclose _
        Lib "kernel32" _
        Alias "_lclose" _
        (ByVal hFile As Integer) As Integer

        Public Enum WindowsMessages
            WM_MOVE = &H3
            WM_CLOSE = &H10
            WM_PAINT = &HF
            WM_APP = &H8000
            WM_AM_UPDATE_DATA = &H8001
            WM_AM_SIMULATION_ERROR = &H8002
        End Enum

        Protected Shared m_frmApplication As Forms.AnimatApplication
        Protected Shared m_bCopyInProgress As Boolean = False
        Protected Shared m_bCutInProgress As Boolean = False
        Protected Shared m_bDisableDirtyFlags As Boolean = False
        Protected Shared m_iDisableDirtyCount As Integer = 0
        Protected Shared m_szErrorFormSize As New Size(500, 250)
        Protected Shared m_bDisplayErrorDetails As Boolean = False
        Protected Shared m_bDisplayingError As Boolean = False
        Protected Shared m_doRobotOrganism As DataObjects.Physical.Organism
        Protected Shared m_bExportForStandAloneSim As Boolean = False
        Protected Shared m_bExportStimsInStandAloneSim As Boolean = False
        Protected Shared m_bExportChartsInStandAloneSim As Boolean = False

        Public Shared Property Application() As Forms.AnimatApplication
            Get
                Return m_frmApplication
            End Get
            Set(ByVal Value As Forms.AnimatApplication)
                m_frmApplication = Value
            End Set
        End Property

        Public Shared ReadOnly Property Simulation() As DataObjects.Simulation
            Get
                If Not m_frmApplication Is Nothing Then
                    Return m_frmApplication.Simulation
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Shared ReadOnly Property Environment() As DataObjects.Physical.Environment
            Get
                If Not m_frmApplication Is Nothing AndAlso Not m_frmApplication.Simulation Is Nothing Then
                    Return m_frmApplication.Simulation.Environment
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Shared ReadOnly Property Logger() As AnimatTools.Interfaces.Logger
            Get
                If Not m_frmApplication Is Nothing Then
                    Return m_frmApplication.Logger
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Shared ReadOnly Property ModificationHistory() As AnimatTools.Framework.UndoSystem.ModificationHistory
            Get
                If Not m_frmApplication Is Nothing Then
                    Return m_frmApplication.ModificationHistory
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Shared Property CopyInProgress() As Boolean
            Get
                Return m_bCopyInProgress
            End Get
            Set(ByVal Value As Boolean)
                m_bCopyInProgress = Value
            End Set
        End Property

        Public Shared Property CutInProgress() As Boolean
            Get
                Return m_bCutInProgress
            End Get
            Set(ByVal Value As Boolean)
                m_bCutInProgress = Value
            End Set
        End Property

        Public Shared Property DisableDirtyFlags() As Boolean
            Get
                Return m_bDisableDirtyFlags
            End Get
            Set(ByVal Value As Boolean)
                'If we are trying to disable the dirty flags then lets up the count
                If Value Then
                    m_iDisableDirtyCount = m_iDisableDirtyCount + 1
                    m_bDisableDirtyFlags = True
                Else
                    'If we are trying to enable the flags again lets decrement the count
                    'and if it reaches 0 then set them back to true.
                    m_iDisableDirtyCount = m_iDisableDirtyCount - 1
                    If m_iDisableDirtyCount <= 0 Then
                        m_iDisableDirtyCount = 0
                        m_bDisableDirtyFlags = False
                    End If
                End If
            End Set
        End Property

        Public Shared Property RobotOrganism() As DataObjects.Physical.Organism
            Get
                Return m_doRobotOrganism
            End Get
            Set(ByVal Value As DataObjects.Physical.Organism)
                m_doRobotOrganism = Value
            End Set
        End Property

        Public Shared Property ExportForStandAloneSim() As Boolean
            Get
                Return m_bExportForStandAloneSim
            End Get
            Set(ByVal Value As Boolean)
                m_bExportForStandAloneSim = Value
            End Set
        End Property

        Public Shared Property ExportStimsInStandAloneSim() As Boolean
            Get
                Return m_bExportStimsInStandAloneSim
            End Get
            Set(ByVal Value As Boolean)
                m_bExportStimsInStandAloneSim = Value
            End Set
        End Property

        Public Shared Property ExportChartsInStandAloneSim() As Boolean
            Get
                Return m_bExportChartsInStandAloneSim
            End Get
            Set(ByVal Value As Boolean)
                m_bExportChartsInStandAloneSim = Value
            End Set
        End Property

        Public Shared Function Rand(ByVal dblMin As Double, ByVal dblMax As Double) As Double
            Dim rndNum As System.Random = New System.Random
            Return ((Math.Abs(dblMax - dblMin) * rndNum.NextDouble) + dblMin)
        End Function

        Public Shared Function Rand(ByVal dblMin As Double, ByVal dblMax As Double, ByVal rndNum As System.Random) As Double
            Return ((Math.Abs(dblMax - dblMin) * rndNum.NextDouble) + dblMin)
        End Function

        Public Shared Function IsBlank(ByVal strVal As String) As Boolean
            If strVal.Trim.Length > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Sub SplitPathAndFile(ByVal strFullPath As String, _
                                           ByRef strPath As String, _
                                           ByRef strFile As String)
            Dim aryParts() As String = Split(strFullPath, "\")

            Dim iCount As Integer = aryParts.GetUpperBound(0)
            If iCount <= 0 Then
                strPath = ""
                strFile = strFullPath
            Else
                strFile = aryParts(iCount)
                ReDim Preserve aryParts(iCount - 1)
            End If

            strPath = Join(aryParts, "\")
            If Not IsBlank(strPath) Then strPath += "\"
        End Sub

        Public Shared Function ExtractFilename(ByVal strFullPath As String) As String
            Dim strPath As String, strFile As String
            SplitPathAndFile(strFullPath, strPath, strFile)
            Return strFile
        End Function

        Public Shared Function IsFullPath(ByVal strPath As String) As Boolean
            Dim aryParts() As String = Split(strPath, ":")

            Dim iCount As Integer = aryParts.GetUpperBound(0)
            If iCount > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shared Function VerifyFilePath(ByVal strFilename As String) As String

            If (File.Exists(strFilename)) Then
                Return strFilename
            Else
                Dim strPath As String = ""
                Dim strFile As String = ""
                Util.SplitPathAndFile(strFilename, strPath, strFile)
                strFile = Util.Application.ProjectPath & strFile

                If (File.Exists(strFile)) Then
                    Return strFile
                Else
                    Return ""
                End If
            End If

        End Function

        Public Shared Function GetFilePath(ByVal strProjectPath As String, ByVal strFile As String) As String

            Dim strPath As String
            If Not IsFullPath(strFile) Then
                If Right(strProjectPath, 1) = "\" Then
                    strPath = strProjectPath & strFile
                Else
                    strPath = strProjectPath & "\" & strFile
                End If
            Else
                strPath = strFile
            End If

            Return strPath
        End Function

        Public Shared Function IsFileInProjectDirectory(ByRef strFilename As String) As Boolean

            If strFilename.Trim.Length = 0 Then
                Return False
            End If

            If Not Application.ProjectPath.Trim.Length > 0 AndAlso IsFullPath(strFilename) Then
                Dim strPath As String, strFile As String
                SplitPathAndFile(strFilename, strPath, strFile)
                If strPath.Trim.ToUpper.Substring(0, Application.ProjectPath.Length) = Application.ProjectPath.Trim.ToUpper Then
                    strFilename = strFile
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If

        End Function

        Public Shared Function DetermineFilePath(ByVal strFilename As String, ByRef strPath As String, ByRef strFile As String) As Boolean

            Util.SplitPathAndFile(strFilename, strPath, strFile)

            Dim aryTestPath As String() = Split(strFilename, "\")
            Dim aryProjectPath As String() = Split(Util.Application.ProjectPath, "\")

            'Dont compare the last array value. It is blank for project path.
            Dim iEnd As Integer = UBound(aryProjectPath) - 1
            For iDir As Integer = 0 To iEnd
                If Not aryTestPath(iDir).ToUpper() = aryProjectPath(iDir).ToUpper() Then
                    Return False
                End If
            Next

            'If we got here then all the directories mathed to this point, so it is in the project directory.
            'Now we need to rebuild the file path in case it is in a subdirectory of the project dir.
            strPath = Util.Application.ProjectPath
            Dim iStart As Integer = UBound(aryProjectPath)
            iEnd = UBound(aryTestPath)
            strFile = ""
            For iIndex As Integer = iStart To iEnd
                If iIndex <> iEnd Then
                    strFile = strFile & aryTestPath(iIndex) & "\"
                Else
                    strFile = strFile & aryTestPath(iIndex)
                End If
            Next

            Return True
        End Function

        Public Shared Function GetFileExtension(ByVal strFilename As String) As String

            Dim strExtension As String
            Dim iDot As Integer = InStr(strFilename, ".", CompareMethod.Text)
            If iDot >= 0 Then
                strExtension = strFilename.Substring(iDot, (strFilename.Length - iDot))
            End If

            Return strExtension
        End Function

        Public Shared Sub DisplayError(ByVal exError As System.Exception)

            Try
                If Not m_bDisplayingError Then
                    m_bDisplayingError = True
                    Dim frmError As New AnimatTools.Forms.ErrorDisplay
                    frmError.DisplayErrorDetails = m_bDisplayErrorDetails
                    frmError.Size = m_szErrorFormSize
                    frmError.Exception = exError
                    frmError.ShowDialog()

                    m_bDisplayErrorDetails = frmError.DisplayErrorDetails
                    m_szErrorFormSize = frmError.Size

                    m_bDisplayingError = False
                End If

            Catch ex As System.Exception
                Try
                    MessageBox.Show(exError.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bDisplayingError = False
                Catch ex1 As System.Exception
                    m_bDisplayingError = False
                End Try
            End Try
        End Sub

        Public Shared Function GetErrorDetails(ByVal strParentDetails As String, ByVal ex As System.Exception) As String
            Try
                Dim strDetails As String

                If strParentDetails.Trim.Length = 0 Then
                    strDetails = ex.StackTrace
                Else
                    strDetails = strParentDetails & vbCrLf & ex.StackTrace
                End If

                If Not ex.InnerException Is Nothing Then
                    strDetails = strDetails & vbCrLf & vbCrLf & ex.InnerException.Message
                    strDetails = GetErrorDetails(strDetails, ex.InnerException)
                End If

                Return strDetails
            Catch ex1 As System.Exception
                Return strParentDetails
            End Try
        End Function

        Public Shared Sub CopyDirectory(ByVal strOrigFolder As String, ByVal strNewFolder As String)

            If Not Directory.Exists(strNewFolder) Then
                Directory.CreateDirectory(strNewFolder)

                Dim aryFiles As String() = Directory.GetFiles(strOrigFolder)

                Dim strFilePath As String
                Dim strFile As String
                For Each strFilePath In aryFiles
                    strFile = Util.ExtractFilename(strFilePath)
                    File.Copy((strOrigFolder & "\" & strFile), (strNewFolder & "\" & strFile))
                Next

                'Now copy any directories that exist in that folder also.
                Dim aryDirs As String() = Directory.GetDirectories(strOrigFolder)
                For Each strDir As String In aryDirs
                    Dim aryDirNames As String() = Split(strDir, "\")
                    CopyDirectory(strDir, strNewFolder & "\" & aryDirNames(UBound(aryDirNames)))
                Next
            Else
                Throw New System.Exception("The folder '" & strOrigFolder & "' can not be copied to a new directory '" & _
                                           strNewFolder & "' becuase a directory with that name already exists.")
            End If

        End Sub

        Public Shared Sub FindAssemblies(ByVal strFolder As String, ByRef aryFiles As ArrayList)

            ' Only get files that begin with the letter "c."
            Dim dirs As String() = Directory.GetFiles(strFolder, "*.dll")

            Dim dir As String
            For Each dir In dirs
                aryFiles.Add(dir)
            Next
        End Sub

        Public Shared Function LoadAssembly(ByVal strAssemblyPath As String, Optional ByVal bThrowError As Boolean = True) As System.Reflection.Assembly

            Try
                strAssemblyPath = Util.GetFilePath(Util.Application.ApplicationDirectory, strAssemblyPath)
                Dim assemModule As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(strAssemblyPath)
                If assemModule Is Nothing AndAlso bThrowError Then
                    Throw New System.Exception("Unable to load the assembly '" & strAssemblyPath & "'.")
                End If

                Return assemModule

            Catch ex As System.Exception
                If bThrowError Then
                    Throw ex
                End If
            End Try
        End Function

        Public Shared Function LoadClass(ByVal strAssemblyPath As String, ByVal strClassName As String, _
                                         ByVal doParent As AnimatTools.Framework.DataObject, Optional ByVal bThrowError As Boolean = True) As Object
            Dim aryArgs() As Object = {doParent}
            Return LoadClass(strAssemblyPath, strClassName, aryArgs, bThrowError)
        End Function

        Public Shared Function LoadClass(ByVal strAssemblyPath As String, ByVal strClassName As String, _
                                         Optional ByVal aryArgs() As Object = Nothing, Optional ByVal bThrowError As Boolean = True) As Object

            Try
                If Right(UCase(Trim(strAssemblyPath)), 4) <> ".DLL" Then
                    strAssemblyPath = strAssemblyPath & ".dll"
                End If

                strAssemblyPath = Util.GetFilePath(Util.Application.ApplicationDirectory, strAssemblyPath)
                Dim assemModule As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(strAssemblyPath)
                If assemModule Is Nothing AndAlso bThrowError Then
                    Throw New System.Exception("Unable to load the assembly '" & strAssemblyPath & "'.")
                End If

                Dim oData As Object = assemModule.CreateInstance(strClassName, True, Reflection.BindingFlags.Default, Nothing, aryArgs, Nothing, Nothing)

                If oData Is Nothing AndAlso bThrowError Then
                    Throw New System.Exception("The system was unable to create the class '" & strClassName & _
                                               "' from the assembly '" & assemModule.FullName & "'.")
                End If

                Return oData

            Catch ex As System.Exception
                If bThrowError Then
                    Throw ex
                End If
            End Try
        End Function

        Public Shared Function LoadClass(ByRef assemModule As System.Reflection.Assembly, ByVal strClassName As String, _
                                         ByVal doParent As AnimatTools.Framework.DataObject, Optional ByVal bThrowError As Boolean = True) As Object
            Dim aryArgs() As Object = {doParent}
            Return LoadClass(assemModule, strClassName, aryArgs, bThrowError)
        End Function

        Public Shared Function LoadClass(ByRef assemModule As System.Reflection.Assembly, ByVal strClassName As String, _
                                         Optional ByVal aryArgs() As Object = Nothing, Optional ByVal bThrowError As Boolean = True) As Object

            Try
                Dim oData As Object = assemModule.CreateInstance(strClassName, True, Reflection.BindingFlags.Default, Nothing, aryArgs, Nothing, Nothing)

                If oData Is Nothing AndAlso bThrowError Then
                    Throw New System.Exception("The system was unable to create the class '" & strClassName & _
                                               "' from the assembly '" & assemModule.FullName & "'.")
                End If

                Return oData

            Catch ex As System.Exception
                If bThrowError Then
                    Throw ex
                End If
            End Try
        End Function

        Public Shared Function IsTypeOf(ByVal tpNew As Type, ByVal tpOriginal As Type, Optional ByVal bInheritedOnly As Boolean = True) As Boolean

            If Not tpNew Is Nothing Then
                If tpNew Is tpOriginal Then
                    If bInheritedOnly Then
                        Return False
                    Else
                        Return True
                    End If
                ElseIf tpNew.BaseType Is Nothing Then
                    Return False
                ElseIf tpNew.BaseType Is tpOriginal Then
                    Return True
                Else
                    Return IsTypeOf(tpNew.BaseType, tpOriginal)
                End If
            End If

            Return False
        End Function

        Public Shared Function IsTypeOf(ByVal tpNew As Type, ByVal strOriginal As String, Optional ByVal bInheritedOnly As Boolean = True) As Boolean

            If Not tpNew Is Nothing Then
                If tpNew.ToString = strOriginal Then
                    If bInheritedOnly Then
                        Return False
                    Else
                        Return True
                    End If
                ElseIf tpNew.BaseType Is Nothing Then
                    Return False
                ElseIf tpNew.BaseType.ToString Is strOriginal Then
                    Return True
                Else
                    Return IsTypeOf(tpNew.BaseType, strOriginal)
                End If
            End If

            Return False
        End Function

        Public Overloads Shared Function RootNamespace(ByRef assemModule As System.Reflection.Assembly) As String
            Dim aryTypes() As Type = assemModule.GetTypes()
            If UBound(aryTypes) >= 0 Then
                Dim tpClass As Type = aryTypes(0)
                Dim aryName() As String = Split(tpClass.FullName, ".")

                Return aryName(0)
            End If
        End Function

        Public Overloads Shared Function RootNamespace(ByVal strAssemName As String) As String
            Dim aryName() As String = Split(strAssemName, ".")
            Return aryName(0)
        End Function

        Public Shared Function ModuleName(ByRef assemModule As System.Reflection.Assembly) As String
            Dim strModuleName As String

            Try
                strModuleName = RootNamespace(assemModule) & ".ModuleInformation"
                Dim resMan As System.Resources.ResourceManager = New System.Resources.ResourceManager(strModuleName, assemModule)

                strModuleName = resMan.GetString("ModuleName")

                If strModuleName Is Nothing Then
                    Throw New System.Exception("ModuleName not found.")
                End If

                Return strModuleName

            Catch ex As System.Exception
                Dim aryNames() As String = Split(assemModule.FullName, ",")
                Return aryNames(0)
            End Try

        End Function

        Public Overloads Shared Function ExtractIDCount(ByVal strRootname As String, ByVal aryValues As Collections.AnimatDictionaryBase, Optional ByVal strSeperator As String = "_") As Integer
            Dim strNumber As String
            Dim iNumber As Integer, iMax As Integer = -1

            strRootname = strRootname & strSeperator
            strRootname = strRootname.ToUpper
            Dim doItem As AnimatTools.DataObjects.DragObject
            For Each deEntry As DictionaryEntry In aryValues
                If TypeOf deEntry.Value Is AnimatTools.DataObjects.DragObject Then
                    doItem = DirectCast(deEntry.Value, AnimatTools.DataObjects.DragObject)
                    strNumber = doItem.ItemName

                    If strNumber.Length > strRootname.Length AndAlso Left(strNumber, strRootname.Length).ToUpper = strRootname Then
                        strNumber = Right(strNumber, strNumber.Length - strRootname.Length)

                        If IsNumeric(strNumber) AndAlso Not InStr(strNumber, ".") > 0 AndAlso Not InStr(strNumber, "-") > 0 Then
                            iNumber = CInt(strNumber)
                            If iNumber > iMax OrElse iMax < 0 Then
                                iMax = iNumber
                            End If
                        End If
                    End If
                End If
            Next

            If iMax < 0 Then
                Return 0
            Else
                Return iMax
            End If
        End Function


        Public Overloads Shared Function ExtractIDCount(ByVal strRootname As String, ByVal aryValues As SortedList, Optional ByVal strSeperator As String = "_") As Integer
            Dim strNumber As String
            Dim iNumber As Integer, iMax As Integer = -1

            strRootname = strRootname & strSeperator
            strRootname = strRootname.ToUpper
            Dim doItem As AnimatTools.DataObjects.DragObject
            For Each deEntry As DictionaryEntry In aryValues
                If TypeOf deEntry.Value Is AnimatTools.DataObjects.DragObject Then
                    doItem = DirectCast(deEntry.Value, AnimatTools.DataObjects.DragObject)
                    strNumber = doItem.ItemName

                    If strNumber.Length > strRootname.Length AndAlso Left(strNumber, strRootname.Length).ToUpper = strRootname Then
                        strNumber = Right(strNumber, strNumber.Length - strRootname.Length)

                        If IsNumeric(strNumber) AndAlso Not InStr(strNumber, ".") > 0 AndAlso Not InStr(strNumber, "-") > 0 Then
                            iNumber = CInt(strNumber)
                            If iNumber > iMax OrElse iMax < 0 Then
                                iMax = iNumber
                            End If
                        End If
                    End If
                End If
            Next

            If iMax < 0 Then
                Return 0
            Else
                Return iMax
            End If
        End Function

        Public Overloads Shared Sub SavePoint(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal ptPoint As PointF)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("x", ptPoint.X)
            oXml.SetAttrib("y", ptPoint.Y)
            oXml.OutOfElem()
        End Sub

        Public Overloads Shared Sub SavePoint(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal ptPoint As Point)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("x", ptPoint.X)
            oXml.SetAttrib("y", ptPoint.Y)
            oXml.OutOfElem()
        End Sub

        Public Shared Function LoadPointF(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String) As PointF
            Dim ptPoint As New PointF
            oXml.IntoChildElement(strName)

            Try
                ptPoint.X = oXml.GetAttribFloat("x")
                ptPoint.Y = oXml.GetAttribFloat("y")
            Catch ex As System.Exception
                ptPoint.X = oXml.GetAttribFloat("X")
                ptPoint.Y = oXml.GetAttribFloat("Y")
            End Try
            oXml.OutOfElem()
        End Function

        Public Shared Function LoadPoint(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String) As Point
            Dim ptPoint As New Point
            oXml.IntoChildElement(strName)

            Try
                ptPoint.X = oXml.GetAttribInt("x")
                ptPoint.Y = oXml.GetAttribInt("y")
            Catch ex As System.Exception
                ptPoint.X = oXml.GetAttribInt("X")
                ptPoint.Y = oXml.GetAttribInt("Y")
            End Try

            oXml.OutOfElem()
            Return ptPoint
        End Function

        Public Shared Function LoadColor(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal clDefault As Color) As System.Drawing.Color
            Dim r, g, b, alpha As Integer
            Dim clColor As Color

            If oXml.FindChildElement(strName, False) Then
                oXml.IntoChildElement(strName)

                r = CInt(oXml.GetAttribFloat("Red", False, CSng(clDefault.R / 255)) * 255)
                g = CInt(oXml.GetAttribFloat("Green", False, CSng(clDefault.G / 255)) * 255)
                b = CInt(oXml.GetAttribFloat("Blue", False, CSng(clDefault.B / 255)) * 255)
                alpha = CInt(oXml.GetAttribFloat("Alpha", False, 1) * 255)

                clColor = Color.FromArgb(alpha, r, g, b)

                oXml.OutOfElem()
            Else
                clColor = clDefault
            End If

            Return clColor
        End Function

        Public Shared Function LoadColor(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String) As System.Drawing.Color
            Dim r, g, b, alpha As Integer
            Dim clColor As Color

            oXml.IntoChildElement(strName)

            r = CInt(oXml.GetAttribFloat("Red") * 255)
            g = CInt(oXml.GetAttribFloat("Green") * 255)
            b = CInt(oXml.GetAttribFloat("Blue") * 255)
            alpha = CInt(oXml.GetAttribFloat("Alpha") * 255)

            clColor = Color.FromArgb(alpha, r, g, b)

            oXml.OutOfElem()

            Return clColor
        End Function

        Public Shared Sub SaveColor(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal oColor As System.Drawing.Color)

            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("Red", CSng(oColor.R / 255.0))
            oXml.SetAttrib("Green", CSng(oColor.G / 255.0))
            oXml.SetAttrib("Blue", CSng(oColor.B / 255.0))
            oXml.SetAttrib("Alpha", CSng(oColor.A / 255.0))
            oXml.OutOfElem()

        End Sub

        Public Overloads Shared Sub SaveSize(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal szSize As SizeF)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("Width", szSize.Width)
            oXml.SetAttrib("Height", szSize.Height)
            oXml.OutOfElem()
        End Sub

        Public Overloads Shared Sub SaveSize(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal szSize As Size)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("Width", szSize.Width)
            oXml.SetAttrib("Height", szSize.Height)
            oXml.OutOfElem()
        End Sub

        Public Shared Function LoadSizeF(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String) As SizeF
            Dim szSize As SizeF
            oXml.IntoChildElement(strName)
            szSize.Width = oXml.GetAttribFloat("Width")
            szSize.Height = oXml.GetAttribFloat("Height")
            oXml.OutOfElem()
            Return szSize
        End Function

        Public Shared Function LoadSize(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String) As Size
            Dim szSize As Size
            oXml.IntoChildElement(strName)
            szSize.Width = oXml.GetAttribInt("Width")
            szSize.Height = oXml.GetAttribInt("Height")
            oXml.OutOfElem()
            Return szSize
        End Function

        Public Overloads Shared Sub SaveVector(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal ptVector As Vec3i)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("x", ptVector.X)
            oXml.SetAttrib("y", ptVector.Y)
            oXml.SetAttrib("z", ptVector.Z)
            oXml.OutOfElem()
        End Sub

        Public Shared Function LoadVec3i(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal doParent As AnimatTools.Framework.DataObject) As Vec3i
            Dim ptVector As New Vec3i(doParent)
            oXml.IntoChildElement(strName)
            ptVector.X = oXml.GetAttribInt("x")
            ptVector.Y = oXml.GetAttribInt("y")
            ptVector.Z = oXml.GetAttribInt("z")
            oXml.OutOfElem()
            Return ptVector
        End Function

        Public Overloads Shared Sub SaveVector(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal ptVector As Vec3d)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("x", ptVector.X)
            oXml.SetAttrib("y", ptVector.Y)
            oXml.SetAttrib("z", ptVector.Z)
            oXml.OutOfElem()
        End Sub

        Public Shared Function LoadVec3d(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal doParent As AnimatTools.Framework.DataObject) As Vec3d
            Dim ptVector As New Vec3d(doParent)
            oXml.IntoChildElement(strName)
            ptVector.X = oXml.GetAttribDouble("x")
            ptVector.Y = oXml.GetAttribDouble("y")
            ptVector.Z = oXml.GetAttribDouble("z")
            oXml.OutOfElem()
            Return ptVector
        End Function

        Public Shared Sub SaveFont(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String, ByVal oFont As Font)
            oXml.AddChildElement(strName)
            oXml.IntoElem()
            oXml.SetAttrib("Family", oFont.Name)
            oXml.SetAttrib("Size", oFont.Size)
            oXml.SetAttrib("Bold", oFont.Bold)
            oXml.SetAttrib("Underline", oFont.Underline)
            oXml.SetAttrib("Strikeout", oFont.Strikeout)
            oXml.SetAttrib("Italic", oFont.Italic)
            oXml.OutOfElem()
        End Sub

        Public Shared Function LoadFont(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strName As String) As Font
            oXml.IntoChildElement(strName)
            Dim strFamily As String = oXml.GetAttribString("Family")
            Dim fltSize As Single = oXml.GetAttribFloat("Size")
            Dim bBold As Boolean = oXml.GetAttribBool("Bold")
            Dim bUnderline As Boolean = oXml.GetAttribBool("Underline")
            Dim bStrikeout As Boolean = oXml.GetAttribBool("Strikeout")
            Dim bItalic As Boolean = oXml.GetAttribBool("Italic")
            oXml.OutOfElem()

            Dim eStyle As System.Drawing.FontStyle
            If bBold Then eStyle = eStyle Or System.Drawing.FontStyle.Bold
            If bUnderline Then eStyle = eStyle Or System.Drawing.FontStyle.Underline
            If bStrikeout Then eStyle = eStyle Or System.Drawing.FontStyle.Strikeout
            If bItalic Then eStyle = eStyle Or System.Drawing.FontStyle.Italic

            Return New Font(strFamily, fltSize, eStyle)
        End Function

        Public Function IntersectLineLine(ByVal a1 As Point, ByVal a2 As Point, ByVal b1 As Point, ByVal b2 As Point, ByRef result As Point) As Boolean

            Dim ua_t As Double = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X)
            Dim ub_t As Double = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X)
            Dim u_b As Double = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y)

            If u_b <> 0 Then

                Dim ua As Double = ua_t / u_b
                Dim ub As Double = ub_t / u_b

                If 0 <= ua AndAlso ua <= 1 AndAlso 0 <= ub AndAlso ub <= 1 Then
                    result = New Point(CInt(a1.X + ua * (a2.X - a1.X)), CInt(a1.Y + ua * (a2.Y - a1.Y)))
                    IntersectLineLine = True
                Else
                    IntersectLineLine = False
                End If
            Else
                If ua_t = 0 OrElse ub_t = 0 Then
                    IntersectLineLine = False
                Else
                    IntersectLineLine = False
                End If
            End If
        End Function

        Public Shared Function DegreesToRadians(ByVal fltDegrees As Single) As Single

            If fltDegrees > 360 OrElse fltDegrees < -360 Then
                Dim iMod As Integer = CInt(Math.Abs(fltDegrees / 360))
                fltDegrees = fltDegrees / iMod
            End If

            If fltDegrees = -360 OrElse fltDegrees = 360 Then fltDegrees = 0

            Return CSng((fltDegrees / 180.0) * Math.PI)
        End Function

        Public Shared Function RadiansToDegrees(ByVal fltRadians As Single) As Single
            Return CSng((fltRadians / Math.PI) * 180.0)
        End Function

        Public Shared Function LoadID(ByRef oXml As AnimatTools.Interfaces.StdXml, ByVal strRootName As String, Optional ByVal bUseDefault As Boolean = False, Optional ByVal strDefault As String = "") As String
            Dim strID As String = ""

            If bUseDefault Then
                If oXml.FindChildElement(strRootName & "ID", False) Then
                    strID = oXml.GetChildString(strRootName & "ID", strDefault)
                Else
                    strID = oXml.GetChildString(strRootName & "Key", strDefault)
                End If
            Else
                If oXml.FindChildElement(strRootName & "ID", False) Then
                    strID = oXml.GetChildString(strRootName & "ID")
                Else
                    strID = oXml.GetChildString(strRootName & "Key")
                End If
            End If

            Return strID
        End Function



        Public Shared Function Distance(ByVal vA As AnimatTools.Framework.Vec3d, ByVal vB As AnimatTools.Framework.Vec3d) As Double
            Return Math.Sqrt(Math.Pow(vA.X - vB.X, 2) + Math.Pow(vA.Y - vB.Y, 2) + Math.Pow(vA.Z - vB.Z, 2))
        End Function

        Public Shared Sub Relable(ByVal aryObjects As ArrayList, ByVal strMatch As String, ByVal strReplace As String)

            Try

                ModificationHistory.BeginHistoryGroup()
                Dim doData As AnimatTools.Framework.DataObject
                For Each oObj As Object In aryObjects
                    If TypeOf oObj Is AnimatTools.Framework.DataObject Then
                        doData = DirectCast(oObj, AnimatTools.Framework.DataObject)
                        If Regex.IsMatch(doData.Name, strMatch) Then
                            doData.Name = Regex.Replace(doData.Name, strMatch, strReplace)
                        End If
                    End If
                Next
                ModificationHistory.CommitHistoryGroup()

            Catch ex As System.Exception
                ModificationHistory.AbortHistoryGroup()
                Throw ex
            End Try

        End Sub

        Public Shared Sub RelableSelected(ByVal aryObjects As ArrayList, ByVal strLabel As String, ByVal iStartWith As Integer, ByVal iIncrementBy As Integer)

            Try

                ModificationHistory.BeginHistoryGroup()
                Dim doData As AnimatTools.Framework.DataObject
                Dim iNum As Integer = iStartWith
                For Each oObj As Object In aryObjects
                    If TypeOf oObj Is AnimatTools.Framework.DataObject Then
                        doData = DirectCast(oObj, AnimatTools.Framework.DataObject)
                        doData.Name = Replace(strLabel, "%NUM%", iNum.ToString)
                        iNum = iNum + iIncrementBy
                    End If
                Next
                ModificationHistory.CommitHistoryGroup()

            Catch ex As System.Exception
                ModificationHistory.AbortHistoryGroup()
                Throw ex
            End Try

        End Sub

        Public Shared Function RemoveStringSections(ByVal strText As String, ByVal strDelim As String, ByVal iSectionsToRemove As Integer, Optional ByVal bStartAtRear As Boolean = True) As String

            If strDelim.Length <= 0 Then
                Throw New System.Exception("No Delimiter was specified.")
            End If

            Dim aryVals() As String = strText.Split(strDelim.Chars(0))
            Dim strOut As String = ""

            If aryVals.Length > 0 Then
                'If the last entry is blank then ignore it.
                If aryVals(aryVals.Length - 1).Trim.Length = 0 Then
                    ReDim Preserve aryVals(aryVals.Length - 2)
                End If

                If iSectionsToRemove > aryVals.Length Then
                    Throw New System.Exception("You are attempting to remove " & iSectionsToRemove & " from a string that only has " & aryVals.Length & " sections.")
                End If

                Dim iStart As Integer = 0
                Dim iEnd As Integer = 0

                If aryVals.Length > 0 Then
                    If bStartAtRear Then
                        iStart = 0
                        iEnd = aryVals.Length - iSectionsToRemove - 1
                    Else
                        iStart = iSectionsToRemove
                        iEnd = aryVals.Length - 1
                    End If

                    For iIndex As Integer = iStart To iEnd
                        If iIndex = iStart Then
                            strOut = aryVals(iIndex)
                        Else
                            strOut = strOut & "\" & aryVals(iIndex)
                        End If
                    Next
                End If
            End If

            Return strOut
        End Function

        Public Shared Sub UpdateConfigFile(ByVal strAutoUpdateInterval As String, ByVal dtUpdateTime As DateTime)
            Try
                Dim oXmlRead As New AnimatTools.Interfaces.StdXml
                'Dim readInfo as
                oXmlRead.Load(Util.Application.ApplicationDirectory() & "AnimatLab.config")

                oXmlRead.FindElement("AnimatLabConfig")
                oXmlRead.FindChildElement("")

                Dim strAssemName As String = oXmlRead.GetChildString("AssemblyName")
                Dim strClassName As String = oXmlRead.GetChildString("ClassName")

                oXmlRead = Nothing

                Dim oXmlWrite As New AnimatTools.Interfaces.StdXml

                oXmlWrite.AddElement("AnimatLabConfig")
                oXmlWrite.AddChildElement("AssemblyName", strAssemName)
                oXmlWrite.AddChildElement("ClassName", strClassName)
                oXmlWrite.AddChildElement("UpdateFrequency", strAutoUpdateInterval)
                oXmlWrite.AddChildElement("UpdateTime", dtUpdateTime.Month.ToString() & "/" & dtUpdateTime.Day.ToString & "/" & dtUpdateTime.Year.ToString)

                oXmlWrite.Save(Util.Application.ApplicationDirectory() & "AnimatLab.config")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Shared Function LoadGain(ByVal strGainName As String, ByVal strPropName As String, ByRef oParent As Framework.DataObject, ByRef oXml As AnimatTools.Interfaces.StdXml) As AnimatTools.DataObjects.Gain
            Dim gnGain As AnimatTools.DataObjects.Gain

            Try

                If oXml.FindChildElement(strGainName, False) Then
                    oXml.IntoChildElement(strGainName)
                    Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                    Dim strClassName As String = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, oParent), AnimatTools.DataObjects.Gain)
                    gnGain.LoadData(oXml, strGainName, strPropName)
                End If

                Return gnGain
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Function

        '    Public Function IntersectLineRectangle(ByVal a1 As Point, ByVal a2 As Point, ByVal topRight As Point, ByVal bottomLeft As Point) As Boolean

        '        if(IntersectLineLine(min, topRight, a1, a2);
        '        var inter2 = Intersection.intersectLineLine(topRight, max, a1, a2);
        '        var inter3 = Intersection.intersectLineLine(max, bottomLeft, a1, a2);
        '        var inter4 = Intersection.intersectLineLine(bottomLeft, min, a1, a2);

        'var result = new Intersection("No Intersection");

        'result.appendPoints(inter1.points);
        'result.appendPoints(inter2.points);
        'result.appendPoints(inter3.points);
        'result.appendPoints(inter4.points);

        '        If (result.points.length > 0) Then
        '    result.status = "Intersection";

        'return result;
        '    End Function

    End Class

End Namespace
