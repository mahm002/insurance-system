Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Data
Imports System.Data.SqlClient

Public Class DocumentHandler : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Dim serializer As New JavaScriptSerializer()

        Try
            Dim jsonString As String = New System.IO.StreamReader(context.Request.InputStream).ReadToEnd()
            If String.IsNullOrEmpty(jsonString) Then
                Throw New Exception("No data received")
            End If

            Dim requestData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)

            Dim action As String = requestData("action").ToString()

            Select Case action
                Case "getDocument"
                    GetDocument(context, requestData, serializer)
                Case Else
                    Throw New Exception("Unknown action: " & action)
            End Select

        Catch ex As Exception
            context.Response.Write(serializer.Serialize(New With {
                .success = False,
                .message = "Error: " & ex.Message
            }))
        End Try
    End Sub

    Private Sub GetDocument(ByVal context As HttpContext,
                           ByVal requestData As Dictionary(Of String, Object),
                           ByVal serializer As JavaScriptSerializer)
        Dim polNo As String = If(requestData.ContainsKey("polNo"), requestData("polNo").ToString(), "")
        Dim endNo As String = If(requestData.ContainsKey("endNo"), requestData("endNo").ToString(), "")
        Dim loadNo As String = If(requestData.ContainsKey("loadNo"), requestData("loadNo").ToString(), "")
        Dim branch As String = If(requestData.ContainsKey("branch"), requestData("branch").ToString(), "")

        Dim connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Your query to get document data - ADJUST THIS QUERY TO YOUR DATABASE SCHEMA
            Dim query As String = "SELECT PolNo,OrderNo, PolicyFile.CustNo, Currency, SubIns, EndNo, LoadNo, TOTPRM-InBox As TOTPRM,
            IssuDate, CustName, ExtraInfo.TpName As TpName,
            CASE WHEN Commision <> 0 AND Broker <> 0
            THEN CAST(Commision AS NVARCHAR(100)) +
            CASE WHEN CommisionType = 1 THEN ' ' + EXTRAINFO.TPName ELSE ' %' END + '-' + BrokersInfo.TPName
            ELSE '0' END As Commissioned,
            PolicyFile.Branch
            FROM PolicyFile
            INNER JOIN ExtraInfo ON TP='Cur' AND TpNo=Currency
            INNER JOIN CustomerFile ON CustomerFile.CustNo=PolicyFile.CustNo
            LEFT OUTER JOIN EXTRAINFO As BrokersInfo ON BrokersInfo.TP='Broker' AND PolicyFile.Broker=BrokersInfo.TPNo
            WHERE PolNo=@PolNo AND EndNo=@EndNo AND LoadNo=@LoadNo AND PolicyFile.Branch=@Branch"

            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@PolNo", polNo)
                cmd.Parameters.AddWithValue("@EndNo", endNo)
                cmd.Parameters.AddWithValue("@LoadNo", loadNo)
                cmd.Parameters.AddWithValue("@Branch", branch)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim documentData As New Dictionary(Of String, Object) From {
                            {"polNo", If(reader("PolNo") IsNot DBNull.Value, reader("PolNo").ToString(), "")},
                            {"endNo", If(reader("EndNo") IsNot DBNull.Value, reader("EndNo").ToString(), "")},
                            {"loadNo", If(reader("LoadNo") IsNot DBNull.Value, reader("LoadNo").ToString(), "")},
                            {"custName", If(reader("CustName") IsNot DBNull.Value, reader("CustName").ToString(), "")},
                            {"totprm", If(reader("TOTPRM") IsNot DBNull.Value, Convert.ToDecimal(reader("TOTPRM")), 0)},
                            {"tpName", If(reader("TpName") IsNot DBNull.Value, reader("TpName").ToString(), "")},
                            {"commissioned", If(reader("Commissioned") IsNot DBNull.Value, reader("Commissioned").ToString(), "")}
                        }

                        context.Response.Write(serializer.Serialize(New With {
                            .success = True,
                            .document = documentData
                        }))
                    Else
                        ' Return empty data for "Other" receipts
                        If polNo = "أخرى" Or polNo = "أخـرى" Then
                            Dim documentData As New Dictionary(Of String, Object) From {
                                {"polNo", "أخرى"},
                                {"endNo", ""},
                                {"loadNo", ""},
                                {"custName", ""},
                                {"totprm", 0},
                                {"tpName", ""},
                                {"commissioned", ""}
                            }

                            context.Response.Write(serializer.Serialize(New With {
                                .success = True,
                                .document = documentData
                            }))
                        Else
                            Throw New Exception("Document not found")
                        End If
                    End If
                End Using
            End Using
        End Using
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class