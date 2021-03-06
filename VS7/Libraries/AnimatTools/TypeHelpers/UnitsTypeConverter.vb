Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class UnitsTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If TypeOf destinationType Is String Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso (TypeOf value Is AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits OrElse _
               TypeOf value Is AnimatTools.DataObjects.Physical.Environment.enumMassUnits) Then
                If TypeOf value Is AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits Then
                    Return Util.Environment.DistanceUnits.ToString()
                ElseIf TypeOf value Is AnimatTools.DataObjects.Physical.Environment.enumMassUnits Then
                    Return Util.Environment.MassUnits.ToString()
                End If

                Return ""
            End If

        End Function

    End Class

End Namespace
