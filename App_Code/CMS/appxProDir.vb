Imports System.IO
Imports appxCMS

Namespace appxProDir
    Public Class FileMeta
#Region "Com Interop"
        <System.Runtime.InteropServices.DllImport("shlwapi", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
        Private Shared Function StrFormatByteSize(ByVal fileSize As Long, _
             ByVal buffer As Text.StringBuilder, _
             ByVal bufferSize As Integer) As Long
        End Function

        Protected Shared Function FormatByteSize(ByVal size As Long) As String
            Dim sb As New Text.StringBuilder(20)
            StrFormatByteSize(size, sb, sb.Capacity)
            Return sb.ToString()
        End Function
#End Region

        Private _Title As String = ""
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Private _Description As String = ""
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Private _SizeInBytes As Integer = 0
        Public Property SizeInBytes() As Integer
            Get
                Return _SizeInBytes
            End Get
            Set(ByVal value As Integer)
                _SizeInBytes = value
            End Set
        End Property

        Public ReadOnly Property Size() As String
            Get
                '-- Convert size in bytes to MB
                Return FormatByteSize(Me.SizeInBytes)
            End Get
        End Property

        Private _MetaFile As String = ""
        Public Property MetaFile() As String
            Get
                Return _MetaFile
            End Get
            Set(ByVal value As String)
                _MetaFile = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal oFI As FileInfo, ByVal sMetaFile As String)
            Dim sName As String = oFI.Name
            Dim sDesc As String = ""

            If File.Exists(sMetaFile) Then
                Dim sMeta As String = File.ReadAllText(sMetaFile)
                Dim oMeta As FileMeta = appxCMS.Util.JavaScriptSerializer.Deserialize(Of FileMeta)(sMeta)
                If oMeta IsNot Nothing Then
                    If Not String.IsNullOrEmpty(oMeta.Title) Then
                        sName = oMeta.Title
                    End If
                    sDesc = oMeta.Description
                End If
            End If

            Me.Title = sName
            Me.Description = sDesc
            Me.SizeInBytes = oFI.Length
        End Sub
    End Class

    Public Class LinkMeta
        Private _Title As String = ""
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Private _Description As String = ""
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Private _NavigateUrl As String = ""
        Public Property NavigateUrl() As String
            Get
                Return _NavigateUrl
            End Get
            Set(ByVal value As String)
                _NavigateUrl = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal sMetaFile As String)
            If File.Exists(sMetaFile) Then
                Dim sMeta As String = File.ReadAllText(sMetaFile)
                Dim oMeta As LinkMeta = appxCMS.Util.JavaScriptSerializer.Deserialize(Of LinkMeta)(sMeta)
                If oMeta IsNot Nothing Then
                    Me.NavigateUrl = oMeta.NavigateUrl
                    If Not String.IsNullOrEmpty(oMeta.Title) Then
                        Me.Title = oMeta.Title
                    Else
                        Me.Title = Me.NavigateUrl
                    End If
                    Me.Description = oMeta.Description
                End If
            End If
        End Sub
    End Class

    Public Class DirMeta
        Private _Title As String = ""
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Private _Description As String = ""
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal sMetaFile As String)
            If File.Exists(sMetaFile) Then
                Dim sMeta As String = File.ReadAllText(sMetaFile)
                Dim oMeta As DirMeta = appxCMS.Util.JavaScriptSerializer.Deserialize(Of DirMeta)(sMeta)
                If oMeta IsNot Nothing Then
                    If Not String.IsNullOrEmpty(oMeta.Title) Then
                        Me.Title = oMeta.Title
                    End If
                    Me.Description = oMeta.Description
                End If
            End If
        End Sub
    End Class
End Namespace
