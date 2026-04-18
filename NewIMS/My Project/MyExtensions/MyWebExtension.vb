#If _MyType <> "Empty" Then

Namespace My
    ''' <summary>
    ''' Module used to define the properties that are available in the My Namespace for Web projects.
    ''' </summary>
    ''' <remarks></remarks>
    <HideModuleName()>
    Friend Module MyWebExtension
        Private s_Computer As New ThreadSafeObjectProvider(Of Devices.ServerComputer)
        Private s_User As New ThreadSafeObjectProvider(Of ApplicationServices.WebUser)
        Private s_Log As New ThreadSafeObjectProvider(Of Logging.AspLog)

        ''' <summary>
        ''' Returns information about the host computer.
        ''' </summary>
        Friend ReadOnly Property Computer() As Devices.ServerComputer
            Get
                Return s_Computer.GetInstance()
            End Get
        End Property

        ''' <summary>
        ''' Returns information for the current Web user.
        ''' </summary>
        Friend ReadOnly Property User() As Microsoft.VisualBasic.ApplicationServices.WebUser
            Get
                Return s_User.GetInstance()
            End Get
        End Property

        ''' <summary>
        ''' Returns Request object.
        ''' </summary>
        <ComponentModel.Design.HelpKeyword("My.Request")>
        Friend ReadOnly Property Request() As HttpRequest
            <DebuggerHidden()>
            Get
                Dim CurrentContext As HttpContext = HttpContext.Current
                If CurrentContext IsNot Nothing Then
                    Return CurrentContext.Request
                End If
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Returns Response object.
        ''' </summary>
        <ComponentModel.Design.HelpKeyword("My.Response")>
        Friend ReadOnly Property Response() As HttpResponse
            <DebuggerHidden()>
            Get
                Dim CurrentContext As HttpContext = HttpContext.Current
                If CurrentContext IsNot Nothing Then
                    Return CurrentContext.Response
                End If
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Returns the Asp log object.
        ''' </summary>
        Friend ReadOnly Property Log() As Logging.AspLog
            Get
                Return s_Log.GetInstance()
            End Get
        End Property

    End Module
End Namespace

#End If