Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class SynapseTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso Util.IsTypeOf(value.GetType(), GetType(DataObjects.Behavior.SynapseType)) Then

                        Dim stType As DataObjects.Behavior.SynapseType = DirectCast(value, DataObjects.Behavior.SynapseType)

                        Dim frmSynapseType As New Forms.SelectSynapseType
                        frmSynapseType.Origin = Nothing
                        frmSynapseType.Destination = Nothing
                        frmSynapseType.CompatibleLinks = Nothing
                        frmSynapseType.NeuralModule = stType.NeuralModule
                        frmSynapseType.SelectedSynapseType = stType

                        If frmSynapseType.ShowDialog() = DialogResult.OK Then
                            Return frmSynapseType.SelectedSynapseType
                        Else
                            Return stType
                        End If

                    End If
                End If

                Return value

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Function

        Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
            If (Not context Is Nothing And Not context.Instance Is Nothing) Then
                ' we'll show a modal form
                Return UITypeEditorEditStyle.Modal
            End If
            Return MyBase.GetEditStyle(context)
        End Function
    End Class

End Namespace
