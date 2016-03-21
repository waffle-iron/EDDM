Imports Microsoft.VisualBasic
Imports System.Data
Imports System.IO
Imports System.Xml

Public Class cmsFSComparer
    Implements IComparer

    Public Enum ComparisonType
        Name = 1
        FileSize = 2
        Type = 3
        Modified = 4
    End Enum

    Private _ComparisonMethod As ComparisonType
    Public Property ComparisonMethod() As ComparisonType
        Get
            Return _ComparisonMethod
        End Get
        Set(ByVal value As ComparisonType)
            _ComparisonMethod = value
        End Set
    End Property

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim oFS1 As cmsFSItem
        Dim oFS2 As cmsFSItem

        If TypeOf x Is cmsFSItem Then
            oFS1 = DirectCast(x, cmsFSItem)
        Else
            Throw New ArgumentException("Object is not of type cmsFSItem")
        End If

        If TypeOf y Is cmsFSItem Then
            oFS2 = DirectCast(y, cmsFSItem)
        Else
            Throw New ArgumentException("Object is not of type cmsFSItem")
        End If

        Return oFS2.CompareTo(oFS1, ComparisonMethod)
    End Function
End Class

Public Class cmsFSImageInfo
    Private _Width As Integer = 0
    Private _Height As Integer = 0

    Public Property Width() As Integer
        Get
            Return _Width
        End Get
        Set(ByVal value As Integer)
            _Width = value
        End Set
    End Property

    Public Property Height() As Integer
        Get
            Return _Height
        End Get
        Set(ByVal value As Integer)
            _Height = value
        End Set
    End Property

    Public Sub New(ByVal oFile As System.IO.FileInfo)
        Dim oBmp As System.Drawing.Bitmap = Nothing
        Try
            oBmp = New System.Drawing.Bitmap(oFile.FullName)
            Me.Width = oBmp.Width
            Me.Height = oBmp.Height
        Catch ex As Exception
        Finally
            If oBmp IsNot Nothing Then
                oBmp.Dispose()
            End If
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Return Width & " x " & Height
    End Function
End Class

<Serializable()> _
Public Class cmsFSItem
    Private _Name As String = ""
    Private _FileSize As Long = 0
    Private _SizeName As String = ""
    Private _Type As String = ""
    Private _Modified As DateTime
    Private _ImageInfo As cmsFSImageInfo = Nothing

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property FileSize() As Long
        Get
            Return _FileSize
        End Get
        Set(ByVal value As Long)
            _FileSize = value
        End Set
    End Property

    Public Property Type() As String
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            If value.StartsWith(".") Then
                value = value.Substring(1)
            End If
            _Type = value.ToUpper
        End Set
    End Property

    Public Property Modified() As DateTime
        Get
            Return _Modified
        End Get
        Set(ByVal value As DateTime)
            _Modified = value
        End Set
    End Property

    Public ReadOnly Property Dimensions() As String
        Get
            If _ImageInfo IsNot Nothing Then
                Return _ImageInfo.ToString
            Else
                Return ""
            End If
        End Get
    End Property

    Public Sub New(ByVal oFI As System.IO.FileInfo)
        Me.Name = oFI.Name
        Me.FileSize = oFI.Length
        Me.Type = oFI.Extension
        Me.Modified = oFI.LastWriteTime

        Dim sExt As String = oFI.Extension.ToLower
        If sExt = ".jpg" Or sExt = ".gif" Or sExt = ".png" Then
            Me._ImageInfo = New cmsFSImageInfo(oFI)
        End If
    End Sub

    Public ReadOnly Property SizeName() As String
        Get
            Return FormatFileSize(FileSize.ToString)
        End Get
    End Property

    Public Function CompareTo(ByVal oFS2 As cmsFSItem, ByVal ComparisonMethod As cmsFSComparer.ComparisonType) As Integer
        Select Case ComparisonMethod
            Case cmsFSComparer.ComparisonType.Name
                Return Me.Name.CompareTo(oFS2.Name)
            Case cmsFSComparer.ComparisonType.FileSize
                Return Me.FileSize.CompareTo(oFS2.FileSize)
            Case cmsFSComparer.ComparisonType.Type
                Return Me.Type.CompareTo(oFS2.Type)
            Case cmsFSComparer.ComparisonType.Modified
                Return Me.Modified.CompareTo(oFS2.Modified)
            Case Else
                Return Me.Name.CompareTo(oFS2.Name)
        End Select
    End Function

    Private Function FormatFileSize(ByVal sSize As String) As String
        Dim dSize As Decimal = Decimal.Parse(sSize)
        Dim aFormat() As String = {"{0:N} bytes", "{0:N2} KB", "{0:N2} MB", "{0:N2} GB", "{0:N2} TB", "{0:N2} PB", "{0} EB"}

        Dim iFormat As Integer = 0
        While iFormat < aFormat.Length And dSize >= 1024
            dSize = (100 * dSize / 1024) / 100.0
            iFormat = iFormat + 1
        End While

        Return String.Format(aFormat(iFormat), dSize)
    End Function
End Class

Public Class cmsFSCache
    Public Shared BasePath As String = "/cmsimages"

    Public Shared Function GetDir() As String
        Dim SiteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        Dim sCK As String = "cmsFS" & SiteId
        Return GetDir(Path.Combine(BasePath, SiteId), sCK)
    End Function

    Public Shared Function GetDir(ByVal sDir As String, ByVal sCacheKey As String) As String
        Dim oCMSFS As New cmsFSCache
        If HttpContext.Current.Cache(sCacheKey) Is Nothing Then
            oCMSFS.CacheDir(sDir, sCacheKey)
        End If
        Dim sXML As String = HttpContext.Current.Cache(sCacheKey).ToString
        If String.IsNullOrEmpty(sXML) Then
            oCMSFS.CacheDir(sDir, sCacheKey)
        End If
        sXML = HttpContext.Current.Cache(sCacheKey).ToString
        Return sXML
    End Function

    Public Shared Function GetEmptyDataSet() As DataSet
        Dim oDS As New DataSet
        Dim oTbl As New DataTable
        oTbl.Columns.Add(New DataColumn("Name"))
        oTbl.Columns.Add(New DataColumn("Created"))
        oTbl.Columns.Add(New DataColumn("Modified"))
        oTbl.Columns.Add(New DataColumn("Size"))
        oTbl.Columns.Add(New DataColumn("SizeName"))
        oTbl.Columns.Add(New DataColumn("Type"))
        oTbl.Columns.Add(New DataColumn("Path"))
        oTbl.Columns.Add(New DataColumn("Description"))
        oTbl.Columns.Add(New DataColumn("Icon"))
        oTbl.Columns.Add(New DataColumn("IsImage"))
        oTbl.Columns.Add(New DataColumn("IsMedia"))
        oTbl.Columns.Add(New DataColumn("IsFlash"))
        oTbl.Columns.Add(New DataColumn("IsFlashVideo"))
        oDS.Tables.Add(oTbl)
        Return oDS
    End Function

    Public Shared Sub ReCache(ByVal sPath As String)
        Dim oCMSFS As New cmsFSCache
        oCMSFS.ReCacheDir(sPath)
    End Sub

    Private Sub CacheDir()
        Dim SiteId As Integer = appxCMS.Util.CMSSettings.GetSiteId
        Dim sBase As String = HttpContext.Current.Server.MapPath(Path.Combine(BasePath, SiteId))
        Dim sCK As String = "cmsFS" & SiteId
        CacheDir(sBase, sCK)
    End Sub

    Private Sub CacheDir(ByVal sBasePath As String, ByVal sCacheKey As String)
        Dim sXML As String = FlatToHier(sBasePath, True, False)
        HttpContext.Current.Cache.Insert(sCacheKey, sXML)
    End Sub

    Private Sub ReCacheDir(ByVal sReplacePath As String)
        If HttpContext.Current.Cache("cmsFS") Is Nothing Then
            CacheDir()
            Exit Sub
        End If

        Dim sXML As String = HttpContext.Current.Cache("cmsFS")
        Dim oXML As New XmlDocument
        oXML.LoadXml(sXML)
        Dim oRemDir As XmlNode = oXML.SelectSingleNode("//directory[@Path='" & sReplacePath & "']")
        If oRemDir IsNot Nothing Then
            oRemDir.ParentNode.RemoveChild(oRemDir)
        End If

        Dim oFs As XmlNode = oXML.SelectSingleNode("//fs")
        If oFs.ChildNodes.Count = 0 Then
            Me.CacheDir()
            Exit Sub
        End If

        DirRecurse(New DirectoryInfo(sReplacePath), oXML, True)
        HttpContext.Current.Cache.Remove("cmsFS")
        HttpContext.Current.Cache.Insert("cmsFS", oXML.OuterXml)
    End Sub

    Protected Function FlatToHier(ByVal sSourceDir As String, ByVal bIncludeFiles As Boolean, Optional ByVal bLevelIndent As Boolean = False) As String
        Dim oXML As New XmlDocument
        oXML.LoadXml("<fs/>")

        If sSourceDir.StartsWith("/") Then
            sSourceDir = HttpContext.Current.Server.MapPath(sSourceDir)
        End If

        If Not Directory.Exists(sSourceDir) Then
            Try
                Directory.CreateDirectory(sSourceDir)
            Catch ex As Exception

            End Try
        End If

        If Directory.Exists(sSourceDir) Then
            Dim oDir As New DirectoryInfo(sSourceDir)

            Dim oPNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(oXML.SelectSingleNode("//fs"), "directory", "")
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "Name", oDir.Name)
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "Created", oDir.CreationTime)
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "Modified", oDir.LastWriteTime)
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "Size", 0)
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "SizeName", "")
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "Type", "directory")
            xmlhelp.AddOrUpdateXMLAttribute(oPNode, "Path", oDir.FullName)

            DirRecurse(oDir, oXML, bIncludeFiles)
        End If

        Return oXML.OuterXml
    End Function

    Protected Sub DirRecurse(ByVal oDir As DirectoryInfo, ByRef oXML As XmlDocument, ByVal bIncludeFiles As Boolean)
        Dim aExclude As New ArrayList
        aExclude.Add("_surveylayout")
        aExclude.Add("_tmp")
        aExclude.Add("mbattachments")
        aExclude.Add("surveycrmconfig")
        aExclude.Add("webcharts")
        aExclude.Add("userimages")

        Dim oPNode As XmlNode = oXML.SelectSingleNode("//directory[@Path='" & oDir.Parent.FullName & "']")
        If ((oDir.Attributes And FileAttributes.System) <> FileAttributes.System) _
        And ((oDir.Attributes And FileAttributes.Hidden) <> FileAttributes.Hidden) And Not aExclude.Contains(oDir.Name.ToLower) Then
            Dim oDirNode As XmlNode = Nothing
            If oPNode IsNot Nothing Then

                oDirNode = xmlhelp.AddOrUpdateXMLNode(oPNode, "directory", "")
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Name", oDir.Name)
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Created", oDir.CreationTime)
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Modified", oDir.LastWriteTime)
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Size", 0)
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "SizeName", "")
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Type", "directory")
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Path", oDir.FullName)
                xmlhelp.AddOrUpdateXMLAttribute(oDirNode, "Icon", VirtualPathUtility.ToAbsolute("~/admin/images/folder.png"))
            Else
                oDirNode = oXML.SelectSingleNode("//directory[@Path='" & oDir.FullName & "']")
            End If

            For Each oSubDir As DirectoryInfo In oDir.GetDirectories
                If ((oSubDir.Attributes And FileAttributes.System) <> FileAttributes.System) _
                    And ((oSubDir.Attributes And FileAttributes.Hidden) <> FileAttributes.Hidden) Then
                    DirRecurse(oSubDir, oXML, bIncludeFiles)
                End If
            Next

            If bIncludeFiles Then
                If oDirNode IsNot Nothing Then
                    For Each oFile As FileInfo In oDir.GetFiles
                        If ((oFile.Attributes And FileAttributes.System) <> FileAttributes.System) _
                        And ((oFile.Attributes And FileAttributes.Hidden) <> FileAttributes.Hidden) Then
                            If oFile.Extension.ToLowerInvariant <> ".description" Then
                                Dim oFileNode As XmlNode = xmlhelp.AddOrUpdateXMLNode(oDirNode, "file", "")
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Name", oFile.Name)
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Created", oFile.CreationTime)
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Modified", oFile.LastWriteTime)
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Size", oFile.Length)
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "SizeName", FormatFileSize(oFile.Length))
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Type", oFile.Extension)
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Path", oFile.FullName)

                                '-- Check for a description file
                                Dim sDescFile As String = oFile.FullName & ".description"
                                Dim sDesc As String = ""
                                If File.Exists(sDescFile) Then
                                    Using oSR As StreamReader = File.OpenText(sDescFile)
                                        sDesc = oSR.ReadToEnd
                                    End Using
                                End If
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Description", sDesc)

                                '-- Determine the icon to use
                                Dim bIsImage As Boolean = False
                                Dim bIsMedia As Boolean = False
                                Dim bIsFlash As Boolean = False
                                Dim bIsFlashVid As Boolean = False
                                Dim sExt As String = oFile.Extension.ToLowerInvariant
                                Dim sIcon As String = "~/admin/images/page_white.png"
                                Select Case sExt
                                    Case ".jpg", ".jpeg", ".png", ".gif", ".bmp"
                                        sIcon = "~/admin/images/page_white_picture.png"
                                        bIsImage = True
                                    Case ".doc", ".docx"
                                        sIcon = "~/admin/images/page_white_word.png"
                                    Case ".xls", ".xlsx", ".csv"
                                        sIcon = "~/admin/images/page_white_excel.png"
                                    Case ".ppt", ".pptx"
                                        sIcon = "~/admin/images/page_white_powerpoint.png"
                                    Case ".swf"
                                        sIcon = "~/admin/images/page_white_flash.png"
                                        bIsFlash = True
                                    Case ".flv"
                                        sIcon = "~/admin/images/page_white_flash.png"
                                        bIsFlashVid = True
                                    Case ".pdf"
                                        sIcon = "~/admin/images/page_white_acrobat.png"
                                    Case ".zip", ".rar", ".gz"
                                        sIcon = "~/admin/images/page_white_zip.png"
                                    Case ".aif", ".aac", ".au", ".gsm", ".mid", ".midi", ".mp3", ".m4a", ".snd", ".ram", ".rm", ".wav", ".wma"
                                        sIcon = "~/admin/images/page_white_cd.png"
                                        bIsMedia = True
                                    Case ".asx", ".asf", ".avi", ".mov", ".mpg", ".mpeg", ".mp4", ".qt", ".ra", ".smil", ".wmv", ".3g2", ".3gp"
                                        sIcon = "~/admin/images/page_white_dvd.png"
                                        bIsMedia = True
                                End Select
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "Icon", VirtualPathUtility.ToAbsolute(sIcon))
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "IsImage", Convert.ToInt32(bIsImage))
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "IsMedia", Convert.ToInt32(bIsMedia))
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "IsFlash", Convert.ToInt32(bIsFlash))
                                xmlhelp.AddOrUpdateXMLAttribute(oFileNode, "IsFlashVideo", Convert.ToInt32(bIsFlashVid))
                            End If
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Private Function FormatFileSize(ByVal sSize As String) As String
        Dim dSize As Decimal = Decimal.Parse(sSize)
        Dim aFormat() As String = {"{0:N} bytes", "{0:N2} KB", "{0:N2} MB", "{0:N2} GB", "{0:N2} TB", "{0:N2} PB", "{0} EB"}

        Dim iFormat As Integer = 0
        While iFormat < aFormat.Length And dSize >= 1024
            dSize = (100 * dSize / 1024) / 100.0
            iFormat = iFormat + 1
        End While

        Return String.Format(aFormat(iFormat), dSize)
    End Function
End Class
