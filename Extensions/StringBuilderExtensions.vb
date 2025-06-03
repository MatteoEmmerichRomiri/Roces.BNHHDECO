Imports System.Text

Module StringBuilderExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Sub AppendLineFormat(sb As StringBuilder, format As String, ParamArray args() As Object)
        sb.AppendLine(String.Format(format, args))
    End Sub
End Module

