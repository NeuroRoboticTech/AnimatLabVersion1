Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls

Namespace DataObjects.Behavior

    Public MustInherit Class DiagramError

#Region " Enums "

        Public Enum enumErrorLevel
            Warning
            [Error]
        End Enum

        Public Enum enumErrorTypes
            JointNotSet
            RigidBodyNotSet
            MuscleNotSet
            MuscleSpindleNotSet
            DataTypeNotSet
            IsolatedNode
            EmptyName
            NodeNotSet
            MuscleIDDuplicate
        End Enum
#End Region

#Region " Attributes "

        Protected m_eLevel As enumErrorLevel
        Protected m_eType As enumErrorTypes
        Protected m_strMessage As String
        Protected m_liItem As ListViewItem

#End Region

#Region " Properties "

        Public MustOverride ReadOnly Property ItemType() As String
        Public MustOverride ReadOnly Property ItemName() As String
        Public MustOverride ReadOnly Property ID() As String

        Public Overridable Property ErrorLevel() As enumErrorLevel
            Get
                Return m_eLevel
            End Get
            Set(ByVal Value As enumErrorLevel)
                m_eLevel = Value
            End Set
        End Property

        Public Overridable Property Message() As String
            Get
                Return m_strMessage
            End Get
            Set(ByVal Value As String)
                m_strMessage = Value
            End Set
        End Property

        Public Overridable Property ErrorType() As enumErrorTypes
            Get
                Return m_eType
            End Get
            Set(ByVal Value As enumErrorTypes)
                m_eType = Value
            End Set
        End Property

        Public Overridable Property ListItem() As ListViewItem
            Get
                Return m_liItem
            End Get
            Set(ByVal Value As ListViewItem)
                m_liItem = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overridable Sub DoubleClicked(ByVal beEditor As Forms.Behavior.Editor)

        End Sub

        Public Function ImageIndex(ByVal frmErrors As Forms.Behavior.Errors) As Integer
            If m_eLevel = enumErrorLevel.Error Then
                Return frmErrors.SmallImages.GetImageIndex("AnimatTools.ErrorSmall.gif")
            Else
                Return frmErrors.SmallImages.GetImageIndex("AnimatTools.WarningSmall.gif")
            End If
        End Function

#End Region

    End Class

End Namespace

