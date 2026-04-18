<%@ WebHandler Language="vb" Class="ShowImage" %>


Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.ComponentModel


Public Class ShowImage
    Implements IHttpHandler
    Private seq As Long = 0
    Private empPic() As Byte = Nothing

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim picid As Int32
        If context.Request.QueryString("id") IsNot Nothing Then
            picid = Convert.ToInt32(context.Request.QueryString("id"))
        Else
            Throw New ArgumentException("No parameter specified")
        End If

        ' Convert Byte[] to Bitmap
        Dim newBmp As Bitmap = ConvertToBitmap(ShowAlbumImage(picid))
        If newBmp IsNot Nothing Then
            newBmp.Save(context.Response.OutputStream, ImageFormat.Jpeg)
            newBmp.Dispose()
        End If

    End Sub

    ' Convert byte array to Bitmap (byte[] to Bitmap)
    Protected Function ConvertToBitmap(ByVal bmp() As Byte) As Bitmap
        If bmp IsNot Nothing Then
            Dim tc As TypeConverter = TypeDescriptor.GetConverter(GetType(Bitmap))
            Dim b As Bitmap = CType(tc.ConvertFrom(bmp), Bitmap)
            Return b
        End If
        Return Nothing
    End Function

    Public Function ShowAlbumImage(ByVal picid As Integer) As Byte()
        Dim conn1 As String = ConfigurationManager.ConnectionStrings("AttachDB").ConnectionString
        Dim connection As New SqlConnection(conn1)
        Dim sql As String = "SELECT Image FROM ClaimsAttachments WHERE ID = @ID"
        Dim cmd As New SqlCommand(sql, connection)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.AddWithValue("@ID", picid)
        Try
            connection.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read() Then
                seq = dr.GetBytes(0, 0, Nothing, 0, Integer.MaxValue) - 1
                empPic = New Byte(seq){}
                dr.GetBytes(0, 0, empPic, 0, Convert.ToInt32(seq))
                connection.Close()
            End If

            Return empPic

        Catch
            Return Nothing
        Finally
            connection.Close()
        End Try
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


End Class