Imports System.Data
Imports NTSInformatica.CLN__STD
Imports System.Text


Public Class CLDHHDECO
    Inherits CLD__BASE

    Public Overridable Function GetHHDECO(ByVal strDitta As String, ByRef codArt As String, ByRef dsOut As DataSet) As Boolean ''MUST DA MODIFICARE
        Try
            Dim sSql As New StringBuilder
            Dim nomeTabella As String = "HHDECODICA"

            sSql.Clear()
            sSql.AppendLineFormat("SELECT 1 FROM information_schema.tables WHERE table_name = {0}", CStrSQL(nomeTabella))

            ' Eseguiamo la query per verificare l'esistenza della tabella
            Dim dtCheck As DataTable = OpenRecordset(sSql.ToString(), CLE__APP.DBTIPO.DBAZI)

            ' Se la tabella non esiste, ritorniamo False
            If dtCheck Is Nothing OrElse dtCheck.Rows.Count = 0 Then Return False

            'So che la tabella esiste quindi posso fare la select
            sSql.Clear()
            sSql.AppendLineFormat("SELECT *")
            sSql.AppendLineFormat("FROM {0} as tab1", nomeTabella)

            dsOut = OpenRecordset(sSql.ToString, CLE__APP.DBTIPO.DBAZI, nomeTabella)

            If dsOut Is Nothing Then Return False

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------
            'Non eseguo la gestione errori standard ma giro l'errore 
            'direttamente al componente entity che mi ha chiamato
            CLN__STD.GestErr(ex, Me, "")
            '--------------------------------------------------------------
        End Try
    End Function

End Class
