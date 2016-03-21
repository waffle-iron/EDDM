Imports Microsoft.VisualBasic
Imports System.Net
Imports System.Net.WebRequest
Imports System.Net.WebResponse
Imports System.IO
Imports System.Collections.Generic

Public Class PostResponseData
    Private _CookieJar As CookieContainer
    Public Property CookieJar() As CookieContainer
        Get
            Return _CookieJar
        End Get
        Set(ByVal value As CookieContainer)
            _CookieJar = value
        End Set
    End Property

    Private _ResponseHTML As String = ""
    Public Property ResponseHTML() As String
        Get
            Return _ResponseHTML
        End Get
        Set(ByVal value As String)
            _ResponseHTML = value
        End Set
    End Property
End Class

Public Class httpHelp
    Public Shared Function PostXMLURLPage(ByVal sURL As String, ByVal aPostData As Hashtable, Optional ByRef CookieJar As CookieContainer = Nothing, Optional ByVal Credentials As CredentialCache = Nothing) As String
        If CookieJar Is Nothing Then
            CookieJar = New CookieContainer
        End If

        Dim oResponseData As New PostResponseData

        Dim oRequestURI As New Uri(sURL)

        Dim sPage As String = ""
        Try
            Dim oRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(sURL), HttpWebRequest)
            If Credentials IsNot Nothing Then
                oRequest.Credentials = Credentials
                oRequest.PreAuthenticate = True
            Else
                oRequest.UseDefaultCredentials = True
            End If
            oRequest.ContentType = "application/x-www-form-urlencoded"
            oRequest.Accept = "text/html"
            oRequest.Method = "POST"

            If aPostData.Count > 0 Then
                Dim oPostData As New List(Of String)
                Dim oEnum As IDictionaryEnumerator = aPostData.GetEnumerator
                While oEnum.MoveNext
                    oPostData.Add(oEnum.Key & "=" & HttpContext.Current.Server.UrlEncode(oEnum.Value))
                End While
                Dim sPostData As String = String.Join("&", oPostData.ToArray())
                Dim oBuffer As Byte() = Encoding.UTF8.GetBytes(sPostData)

                oRequest.ContentLength = oBuffer.Length

                Dim oRStream As Stream = oRequest.GetRequestStream
                oRStream.Write(oBuffer, 0, oBuffer.Length)
                oRStream.Flush()
                oRStream.Close()

            End If

            oRequest.CookieContainer = CookieJar

            Dim oResponse As System.Net.HttpWebResponse = DirectCast(oRequest.GetResponse, HttpWebResponse)
            If oResponse.StatusCode = HttpStatusCode.OK Then
                '-- The query went OK
                Dim oResponseStream As Stream = oResponse.GetResponseStream
                Dim oStreamReader As New StreamReader(oResponseStream)
                sPage = oStreamReader.ReadToEnd.Trim
                oStreamReader.Dispose()
                oResponseStream.Close()
                oResponseStream.Dispose()
            Else
                Dim oResponseStream As Stream = oResponse.GetResponseStream
                Dim oStreamReader As New StreamReader(oResponseStream)
                sPage = "Server Status: " & oResponse.StatusDescription & "<div>" & oStreamReader.ReadToEnd.Trim & "</div>"
                oStreamReader.Dispose()
                oResponseStream.Close()
                oResponseStream.Dispose()
            End If

            oResponse.Close()
        Catch ex As Exception
            Return ex.Message & "<div>" & ex.StackTrace & "</div>"
        End Try

        Return sPage
    End Function

    Public Shared Function GetXMLURLPage(ByVal sURL As String, ByVal aRequestData As Hashtable, Optional ByRef CookieJar As CookieContainer = Nothing) As String
        If CookieJar Is Nothing Then
            CookieJar = New CookieContainer
        End If

        Dim sPage As String = ""
        Try
            Dim sRequestURL As String = sURL
            If aRequestData IsNot Nothing Then
                Dim oPostData As New List(Of String)
                Dim oEnum As IDictionaryEnumerator = aRequestData.GetEnumerator
                While oEnum.MoveNext
                    oPostData.Add(oEnum.Key & "=" & HttpContext.Current.Server.UrlEncode(oEnum.Value))
                End While
                Dim sPostData As String = String.Join("&", oPostData.ToArray())
                sRequestURL = String.Concat(sRequestURL, "?", sPostData)
            End If
            Dim oRequestURI As New Uri(sRequestURL)

            Dim oRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(oRequestURI), HttpWebRequest)
            oRequest.Credentials = System.Net.CredentialCache.DefaultCredentials
            oRequest.Accept = "text/html"
            oRequest.Method = "GET"
            oRequest.CookieContainer = CookieJar
            Dim oResponse As System.Net.HttpWebResponse = DirectCast(oRequest.GetResponse, HttpWebResponse)
            If oResponse.StatusCode = HttpStatusCode.OK Then
                '-- The query went OK
                Dim oResponseStream As Stream = oResponse.GetResponseStream
                Dim oStreamReader As New StreamReader(oResponseStream)
                sPage = oStreamReader.ReadToEnd.Trim
                oStreamReader.Dispose()
                oResponseStream.Close()
                oResponseStream.Dispose()
            End If
            oResponse.Close()
        Catch ex As Exception

        End Try
        Return sPage
    End Function

    Public Shared Function GetXMLURLPage(ByVal sURL As String) As String
        Return GetXMLURLPage(sURL, Nothing, Nothing)
    End Function

    Public Shared Function XMLURLPageRequest(ByVal sURL As String, ByVal aPostData As Hashtable, ByVal sMethod As String, Optional ByRef CookieJar As CookieContainer = Nothing, Optional ByVal Credentials As CredentialCache = Nothing) As String
        If CookieJar Is Nothing Then
            CookieJar = New CookieContainer
        End If

        Dim oResponseData As New PostResponseData

        Dim oRequestURI As New Uri(sURL)

        Dim sPage As String = ""
        Try
            Dim oRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(sURL), HttpWebRequest)
            If Credentials IsNot Nothing Then
                oRequest.Credentials = Credentials
                oRequest.PreAuthenticate = True
            Else
                oRequest.UseDefaultCredentials = True
            End If
            If sMethod.ToUpperInvariant = "POST" Then
                oRequest.ContentType = "application/x-www-form-urlencoded"
            End If
            oRequest.Accept = "text/html"
            oRequest.Method = sMethod

            If aPostData.Count > 0 Then
                Dim oPostData As New List(Of String)
                Dim oEnum As IDictionaryEnumerator = aPostData.GetEnumerator
                While oEnum.MoveNext
                    oPostData.Add(oEnum.Key & "=" & HttpContext.Current.Server.UrlEncode(oEnum.Value))
                End While
                Dim sPostData As String = String.Join("&", oPostData.ToArray())

                Dim oBuffer As Byte() = Encoding.UTF8.GetBytes(sPostData)

                oRequest.ContentLength = oBuffer.Length

                Dim oRStream As Stream = oRequest.GetRequestStream
                oRStream.Write(oBuffer, 0, oBuffer.Length)
                oRStream.Flush()
                oRStream.Close()

            End If

            oRequest.CookieContainer = CookieJar

            Dim oResponse As System.Net.HttpWebResponse = DirectCast(oRequest.GetResponse, HttpWebResponse)
            If oResponse.StatusCode = HttpStatusCode.OK Then
                '-- The query went OK
                Dim oResponseStream As Stream = oResponse.GetResponseStream
                Dim oStreamReader As New StreamReader(oResponseStream)
                sPage = oStreamReader.ReadToEnd.Trim
                oStreamReader.Dispose()
                oResponseStream.Close()
                oResponseStream.Dispose()
            Else
                Dim oResponseStream As Stream = oResponse.GetResponseStream
                Dim oStreamReader As New StreamReader(oResponseStream)
                sPage = "Server Status: " & oResponse.StatusDescription & "<div>" & oStreamReader.ReadToEnd.Trim & "</div>"
                oStreamReader.Dispose()
                oResponseStream.Close()
                oResponseStream.Dispose()
            End If

            oResponse.Close()
        Catch ex As Exception
            Return ex.Message & "<div>" & ex.StackTrace & "</div>"
        End Try

        Return sPage
    End Function

End Class
