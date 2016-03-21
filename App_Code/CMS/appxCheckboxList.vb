Imports System
Imports Microsoft.VisualBasic
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Globalization

Namespace appx.Web
    Public Class CheckboxList
        Inherits System.Web.UI.WebControls.CheckBoxList
        Implements IRepeatInfoUser

        Protected Overrides Sub RenderItem(ByVal itemType As System.Web.UI.WebControls.ListItemType, ByVal repeatIndex As Integer, ByVal repeatInfo As System.Web.UI.WebControls.RepeatInfo, ByVal writer As System.Web.UI.HtmlTextWriter)
            writer.WriteBeginTag("input")
            writer.WriteAttribute("type", "checkbox")
            writer.WriteAttribute("name", UniqueID + Me.IdSeparator + repeatIndex.ToString(NumberFormatInfo.InvariantInfo))
            writer.WriteAttribute("id", ClientID & "_" & repeatIndex.ToString(NumberFormatInfo.InvariantInfo))
            writer.WriteAttribute("value", Items(repeatIndex).Value)
            If Items(repeatIndex).Selected Then
                writer.WriteAttribute("checked", "checked")
            End If
            Dim oAttrs As System.Web.UI.AttributeCollection = Items(repeatIndex).Attributes
            For Each oKey As String In oAttrs.Keys
                writer.WriteAttribute(oKey, oAttrs(oKey))
            Next
            writer.Write(">")
            writer.WriteBeginTag("label")
            writer.WriteAttribute("for", ClientID & "_" & repeatIndex.ToString(NumberFormatInfo.InvariantInfo))
            writer.Write(">")
            writer.Write(Items(repeatIndex).Text)
            writer.WriteEndTag("label")
        End Sub
    End Class
End Namespace
