Imports Tecnomatix.Engineering
Imports System.Type
Imports System.IO
Imports System.Exception

<TxPlcLogicBehaviorFunctionAttribute("RobZone")> Public Class RobZone
    Implements ITxPlcLogicBehaviorFunction

    Dim m_typesArray As New ArrayList
    Dim m_namesArray As New ArrayList
    Dim m_returnValueType As TxPlcSignalDataType

    'Constructor
    Public Sub New()
        '   low bits 0-5 are for the interfenrence zone no., 
        '   bit 6 is Zone Request and 
        '   bit 7 is Zone release
        m_typesArray.Add(TxPlcSignalDataType.Byte)
        m_typesArray.Add(TxPlcSignalDataType.Int)
        m_typesArray.Add(TxPlcSignalDataType.Bool)
        m_typesArray.Add(TxPlcSignalDataType.Bool)
        m_namesArray.Add("EntryByte")
        m_namesArray.Add("Rob_No")
        m_namesArray.Add("ZoneReq")
        m_namesArray.Add("ZoneRls")
        m_returnValueType = TxPlcSignalDataType.Int

    End Sub

    'Returns an array containing the function's parameters types
    Public Function ParametersTypes() As ArrayList Implements ITxPlcLogicBehaviorFunction.ParametersTypes

        Return m_typesArray

    End Function

    'Returns an array containing the function's parameters names
    Public Function ParametersNames() As ArrayList Implements ITxPlcLogicBehaviorFunction.ParametersNames

        Return m_namesArray

    End Function


    'Returns the type of the function's return value
    Public Function ReturnValueType() As TxPlcSignalDataType Implements ITxPlcLogicBehaviorFunction.ReturnValueType

        Return m_returnValueType

    End Function

    Public Function Evaluate(ByVal parameters As ArrayList) As TxPlcValue Implements ITxPlcLogicBehaviorFunction.Evaluate

        Dim inVal As TxPlcValue = parameters.Item(0)
        Dim inGoCollZone As Byte = inVal.ByteValue()
        Dim inVal1 As TxPlcValue = parameters.Item(1)
        Dim inRob_No As Integer = inVal1.IntValue()
        Dim inVal2 As TxPlcValue = parameters.Item(2)
        Dim inZoneReq As Boolean = inVal2.BooleanValue()
        Dim inVal3 As TxPlcValue = parameters.Item(3)
        Dim inZoneRls As Boolean = inVal3.BooleanValue()

        Dim OutByte As New TxPlcValue

        Dim outCollZoneFree As Integer = 0
        Dim outCollZoneReleased As Integer = 0
        Dim giCollZoneTemp As Integer = 0
        Static OldinInt As Integer = -1
        Static AllocatedZones(255), zone As Integer

        If ((Not (inZoneReq Or inZoneRls)) And (inGoCollZone = 0)) Or inGoCollZone > 255 Then
            OutByte.IntValue = 0
            Return OutByte
            Exit Function
        End If

        If inZoneReq Then
            If (AllocatedZones(inGoCollZone) = inRob_No) Or (AllocatedZones(inGoCollZone) = 0) Then
                AllocatedZones(inGoCollZone) = inRob_No
                giCollZoneTemp = inGoCollZone
                outCollZoneFree = 256
            End If
        End If

        If inZoneRls Then
            If (AllocatedZones(inGoCollZone) = inRob_No) Then
                AllocatedZones(inGoCollZone) = 0
                giCollZoneTemp = inGoCollZone
            End If
            outCollZoneReleased = 512
        End If

        If inGoCollZone = 255 Then
            For zone = 1 To 254
                If AllocatedZones(zone) = inRob_No Then
                    AllocatedZones(zone) = 0
                End If
            Next
            giCollZoneTemp = inGoCollZone
        End If
        OutByte.IntValue = outCollZoneReleased + outCollZoneFree + giCollZoneTemp

        Return OutByte

    End Function
End Class
