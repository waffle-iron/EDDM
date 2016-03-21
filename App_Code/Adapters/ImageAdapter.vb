Imports System
Imports System.Data
Imports System.Collections
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Namespace CSSFriendly
    Public Class ImageAdapter
        Inherits System.Web.UI.WebControls.Adapters.WebControlAdapter

        Protected This As WebControlAdapterExtender = Nothing
        Private _extender As WebControlAdapterExtender = Nothing
        Private ReadOnly Property Extender() As WebControlAdapterExtender
            Get
                If ((IsNothing(_extender) AndAlso (Not IsNothing(Control))) OrElse _
                    ((Not IsNothing(_extender)) AndAlso (Not Control.Equals(_extender.AdaptedControl)))) Then
                    _extender = New WebControlAdapterExtender(Control)
                End If

                System.Diagnostics.Debug.Assert(Not IsNothing(_extender), "CSS Friendly adapters internal error", "Null extender instance")
                Return _extender
            End Get
        End Property

        '/ ///////////////////////////////////////////////////////////////////////////////
        '/ PROTECTED        

        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            MyBase.OnInit(e)

            If (Extender.AdapterEnabled) Then
                RegisterScripts()
            End If
        End Sub

        Protected Overrides Sub RenderBeginTag(ByVal writer As HtmlTextWriter)
            If (Extender.AdapterEnabled) Then
                If TypeOf Control Is ImageButton Then
                    MyBase.RenderBeginTag(writer)
                Else
                    Dim oImg As Image = Control
                    If (Not IsNothing(oImg)) Then
                        If oImg.Visible Then
                            writer.WriteBeginTag("img")
                            writer.WriteAttribute("id", oImg.ClientID)
                            writer.WriteAttribute("name", oImg.UniqueID)
                            writer.WriteAttribute("src", oImg.ResolveClientUrl(oImg.ImageUrl))
                            writer.WriteAttribute("alt", oImg.AlternateText)
                            '-- Add this code specifically to add a title to the image
                            '-- so that in the Mac browsers, the alt text can be displayed as the title
                            If String.IsNullOrEmpty(oImg.ToolTip) Then
                                writer.WriteAttribute("title", oImg.AlternateText)
                            Else
                                writer.WriteAttribute("title", oImg.ToolTip)
                            End If
                            If Not String.IsNullOrEmpty(oImg.CssClass) Then
                                writer.WriteAttribute("class", oImg.CssClass)
                            End If
                            If Not String.IsNullOrEmpty(oImg.DescriptionUrl) Then
                                writer.WriteAttribute(HtmlTextWriterAttribute.Longdesc, oImg.ResolveClientUrl(oImg.DescriptionUrl))
                            End If
                            If Not String.IsNullOrEmpty(oImg.TabIndex) Then
                                writer.WriteAttribute("tabindex", oImg.TabIndex.ToString)
                            End If
                            '-- Any inline styles being applied?
                            If oImg.Style.Count > 0 Then
                                Dim oStyleSB As New StringBuilder
                                For Each sAttr As String In oImg.Style.Keys
                                    oStyleSB.Append(sAttr & ":" & oImg.Style(sAttr).ToString & ";")
                                Next
                                writer.WriteAttribute("style", oStyleSB.ToString)
                            End If
                            '-- Any inline attributes specified?
                            If oImg.Attributes.Count > 0 Then
                                For Each sAttr As String In oImg.Attributes.Keys
                                    writer.WriteAttribute(sAttr, oImg.Attributes(sAttr).ToString)
                                Next
                            End If
                            writer.Write(HtmlTextWriter.SelfClosingTagEnd)
                        End If
                    End If
                End If
            Else
                MyBase.RenderBeginTag(writer)
            End If
        End Sub

        Protected Overrides Sub RenderEndTag(ByVal writer As HtmlTextWriter)
        End Sub

        Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
            MyBase.RenderContents(writer)
        End Sub


        '/ ///////////////////////////////////////////////////////////////////////////////
        '/ PRIVATE        

        Private Sub RegisterScripts()
        End Sub

    End Class
End Namespace
