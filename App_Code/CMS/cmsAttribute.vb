Namespace appx
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
    Public Class cmsAttribute
        Inherits Attribute

        Public Enum DataValueType
            Free = 1
            CSVideo = 2
            CSVideoList = 3
            CMSImage = 4
            CMSImageList = 5
            URLList = 6
            CMSDirectory = 7
            CMSSurvey = 8
            CMSProtectedDirectory = 9
            CMSUserFunctionList = 10
            MultiLine = 11
            Bool = 12
            NewsBlogCategory = 13

            '-- White Label specific codes
            WLProduct = 201
        End Enum

        Private _MyValueType As DataValueType = DataValueType.Free
        Public Property MyValueType() As DataValueType
            Get
                Return _MyValueType
            End Get
            Set(ByVal value As DataValueType)
                _MyValueType = value
            End Set
        End Property

        Public Sub New()
            MyValueType = DataValueType.Free
        End Sub

        Public Sub New(ByVal ValueType As DataValueType)
            Me.MyValueType = ValueType
        End Sub
    End Class
End Namespace
