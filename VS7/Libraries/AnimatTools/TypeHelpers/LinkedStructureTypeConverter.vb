Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports System.Text.RegularExpressions
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class LinkedStructureTypeConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean

            If Util.IsTypeOf(destinationType, GetType(AnimatTools.TypeHelpers.LinkedStructureList)) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)

        End Function

        Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object

            If Not value Is Nothing AndAlso Util.IsTypeOf(value.GetType, GetType(AnimatTools.TypeHelpers.LinkedStructureList), False) Then
                Dim thStructure As AnimatTools.TypeHelpers.LinkedStructureList = DirectCast(value, AnimatTools.TypeHelpers.LinkedStructureList)

                If Not thStructure.PhysicalStructure Is Nothing Then
                    Return thStructure.PhysicalStructure.Name
                Else
                    Return ""
                End If
            End If

        End Function

    End Class

End Namespace
