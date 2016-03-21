' 
' 
'LineSpanArticulator 
'Copyright (C) 2009 Raymond Glover 
' 
'This library is free software; you can redistribute it and/or 
'modify it under the terms of the GNU Lesser General Public 
'License as published by the Free Software Foundation; either 
'version 2.1 of the License, or (at your option) any later version. 
' 
'This library is distributed in the hope that it will be useful, 
'but WITHOUT ANY WARRANTY; without even the implied warranty of 
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
'Lesser General Public License for more details. 
' 
'You should have received a copy of the GNU Lesser General Public 
'License along with this library; if not, write to the Free Software 
'Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA 
' 
' 
'Converted to VB.Net from C# by Misty Rae McKinley, Application X


Imports System
Imports System.Reflection
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Cynosura
    <Flags()> _
    Public Enum TemporalGroupType

        <TimeSpan("year", Days:=365)> _
        year = 1

        <TimeSpan("month", Days:=30, Hours:=12)> _
        month = 2

        <TimeSpan("week", Days:=7)> _
        week = 4

        <TimeSpan("day", Days:=1)> _
        day = 8

        <TimeSpan("hour", Hours:=1)> _
        hour = 16

        <TimeSpan("minute", Minutes:=1)> _
        minute = 32

        <TimeSpan("second", Seconds:=1)> _
        second = 64
    End Enum

    Friend Class TimeSpanAttribute
        Inherits Attribute
        Private _Days As Integer
        Public Property Days() As Integer
            Get
                Return _Days
            End Get
            Set(ByVal value As Integer)
                _Days = value
            End Set
        End Property

        Private _Hours As Integer
        Public Property Hours() As Integer
            Get
                Return _Hours
            End Get
            Set(ByVal value As Integer)
                _Hours = value
            End Set
        End Property

        Private _Minutes As Integer
        Public Property Minutes() As Integer
            Get
                Return _Minutes
            End Get
            Set(ByVal value As Integer)
                _Minutes = value
            End Set
        End Property

        Private _Seconds As Integer
        Public Property Seconds() As Integer
            Get
                Return _Seconds
            End Get
            Set(ByVal value As Integer)
                _Seconds = value
            End Set
        End Property

        Private _Name As String
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Private Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Sub New(ByVal name As String)
            Me.Name = name
        End Sub

        ''' <summary> 
        ''' Calculates and returns the Timespan for this attributes state 
        ''' </summary> 
        Public Function GetTimeSpan() As TimeSpan
            Return New TimeSpan(Me.Days, Me.Hours, Me.Minutes, Me.Seconds)
        End Function

        ''' <summary> 
        ''' Uses reflection to retrieve an instance of this attribute 
        ''' on a given enum 
        ''' </summary> 
        Public Shared Function RetrieveAttribute(ByVal target As [Enum]) As TimeSpanAttribute
            Dim attributes As Object() = target.[GetType]().GetField(target.ToString()).GetCustomAttributes(GetType(TimeSpanAttribute), True)

            If attributes IsNot Nothing AndAlso attributes.Length > 0 Then
                Return DirectCast(attributes(0), TimeSpanAttribute)
            Else
                Return Nothing
            End If
        End Function

    End Class

    Public Module TimeSpanArticulator
        'Sub New()
        'End Sub
        Private ReadOnly Seperator As String = ","
        Private ReadOnly Plural As String = "s"
        Private ReadOnly [And] As String = "and"
        Private ReadOnly Space As String = " "

        Private Const defaultAccuracy As TemporalGroupType = TemporalGroupType.hour Or TemporalGroupType.day Or TemporalGroupType.week Or TemporalGroupType.month Or TemporalGroupType.year

        Friend Class TemporalGrouping
            ''' <summary> 
            ''' The type of the temporal grouping 
            ''' e.g. 'hour' or 'day' 
            ''' </summary> 
            Private _Type As TemporalGroupType
            Friend Property Type() As TemporalGroupType
                Get
                    Return _Type
                End Get
                Private Set(ByVal value As TemporalGroupType)
                    _Type = value
                End Set
            End Property

            ''' <summary> 
            ''' The size of the temporal grouping. 
            ''' e.g. '1' hour, or '3' hours 
            ''' </summary> 
            Private _Magnitude As Integer
            Friend Property Magnitude() As Integer
                Get
                    Return _Magnitude
                End Get
                Private Set(ByVal value As Integer)
                    _Magnitude = value
                End Set
            End Property

            Friend Sub New(ByVal type As TemporalGroupType, ByVal magnitude As Integer)
                Me.Type = type
                Me.Magnitude = magnitude
            End Sub

            Public Overloads Overrides Function ToString() As String
                Dim result As String = Me.Magnitude.ToString()

                result += " " & TimeSpanAttribute.RetrieveAttribute(Me.Type).Name

                If Me.Magnitude > 1 Then
                    result += Plural
                End If

                Return result
            End Function
        End Class

        ' a cache of all the TemporalGroupTypes 
        Private groupTypes As List(Of TemporalGroupType)

        ' static contructor 
        Sub New()
            groupTypes = New List(Of TemporalGroupType)(TryCast([Enum].GetValues(GetType(TemporalGroupType)), IEnumerable(Of TemporalGroupType)))
        End Sub

        ''' <summary> 
        ''' Articulates a given TimeSpan using the default accuracy 
        ''' </summary> 
        ''' <param name="span">The TimeSpan to articulate</param> 
        Public Function Articulate(ByVal span As TimeSpan) As String
            Return Articulate(span, defaultAccuracy)
        End Function

        ''' <param name="span">The TimeSpan to articulate</param> 
        ''' <param name="accuracy">Accuracy Flags</param> 
        Public Function Articulate(ByVal span As TimeSpan, ByVal accuracy As TemporalGroupType) As String
            ' populate a list with temporalgroupings. Each temporal grouping 
            ' represents a particular element of the articulation, ordered 
            ' accoring to the temporal duration of each element. 

            Dim groupings As New List(Of TemporalGrouping)(4)

            ' foreach possible temporal type (day/hour/minute etc.) 
            For Each type As TemporalGroupType In groupTypes
                ' if the temporal type isn't specified in the accuracy, skip. 
                If (accuracy And type) <> type Then
                    Continue For
                End If

                ' get the timespan for this temporal type 
                Dim ts As TimeSpan = TimeSpanAttribute.RetrieveAttribute(type).GetTimeSpan()

                If span.Ticks >= ts.Ticks Then
                    ' divide the current timespan with the temporal group span 
                    Dim magnitude As Integer = CInt((span.Ticks / ts.Ticks))

                    groupings.Add(New TemporalGrouping(type, magnitude))

                    span = New TimeSpan(span.Ticks Mod ts.Ticks)
                End If
            Next

            Return Textify(groupings)
        End Function


        ''' <summary> 
        ''' converts a list of groupings into text 
        ''' </summary> 
        Private Function Textify(ByVal groupings As IList(Of TemporalGrouping)) As String
            Dim result As String = [String].Empty

            For i As Integer = 0 To groupings.Count - 1
                Dim groupingStr As String = groupings(i).ToString()

                If i > 0 Then
                    If i = groupings.Count - 1 Then
                        ' this is the last element. Add an "and" 
                        ' between this and the last. 
                        result += Space & [And] & Space
                    Else
                        ' add comma between this and the next element 
                        result += Seperator & Space
                    End If
                End If

                result += groupingStr
            Next

            Return result
        End Function
    End Module
End Namespace