Imports System.Data
Imports NTSInformatica.CLN__STD


Public Class CLEHHDECO
    Inherits CLE__BASE

    Public oCldDeco As CLDHHDECO  'oggetto DAL

    Private Moduli_P As Integer = CLN__STD.bsModAll

    Private ModuliExt_P As Integer = CLN__STD.bsModExtAll
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

    Public CodArt As String = String.Empty
    Public descrArt As String = String.Empty

    Public listaMessaggiErrore As New List(Of String)

    Public ReadOnly Property Moduli() As Integer
        Get
            Return Moduli_P
        End Get
    End Property
    Public ReadOnly Property ModuliExt() As Integer
        Get
            Return ModuliExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliSup() As Integer
        Get
            Return ModuliSup_P
        End Get
    End Property
    Public ReadOnly Property ModuliSupExt() As Integer
        Get
            Return ModuliSupExt_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtn() As Integer
        Get
            Return ModuliPtn_P
        End Get
    End Property
    Public ReadOnly Property ModuliPtnExt() As Integer
        Get
            Return ModuliPtnExt_P
        End Get
    End Property

    Public Overrides Function Init(ByRef App As CLE__APP,
                              ByRef oScriptEngine As INT__SCRIPT, ByRef oCleLbmenu As Object, ByVal strTabella As String,
                              ByVal bFiller1 As Boolean, ByVal strFiller1 As String,
                              ByVal strFiller2 As String) As Boolean
        If MyBase.strNomeDal = "BD__BASE" Then MyBase.strNomeDal = "BDHHDECO"
        MyBase.Init(App, oScriptEngine, oCleLbmenu, strTabella, False, "", "")
        oCldDeco = CType(MyBase.ocldBase, CLDHHDECO)
        oCldDeco.Init(oApp)

        Return True
    End Function

    Public Overrides Function Apri(ByVal strDitta As String, ByRef ds As System.Data.DataSet) As Boolean
        Dim dReturn As Boolean = False
        Try
            '--------------------------------------
            'Tengo un puntatore al datatable che verrà usato nell'UI, quindi chiedo i dati al DAL
            strDittaCorrente = strDitta
            dReturn = oCldDeco.GetHHDECO(strDittaCorrente, CodArt, ds)
            If dReturn = False Then Return False

            '--------------------------------------------------------------
            'Imposto i valori di default della tabella: con la riga che segue prendo prima i valori dal database
            oCldDeco.SetTableDefaultValueFromDB(strNomeTabella, ds)

            SetDefaultValue(ds)

            dsShared = ds

            '--------------------------------------
            'Creo gli eventi per la gestione del Datatable dentro l'Entity
            AddHandler dsShared.Tables(strNomeTabella).ColumnChanging, AddressOf BeforeColUpdate
            AddHandler dsShared.Tables(strNomeTabella).ColumnChanged, AddressOf AfterColUpdate

            'bHasChanges = False

            Return True

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Function

    Public Overridable Function ApriZoom(ByVal strDitta As String, ByRef ds As System.Data.DataSet) As Boolean
        'Dim dReturn As Boolean = False

        Try

            'Return oCldDeco.GetHHTABELLAZOOM(strDittaCorrente, ds)

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Sub SetDefaultValue(ByRef ds As DataSet)
        Try

            'Imposto i valori di default diversi da quelli impostati nel database
            ds.Tables(strNomeTabella).Columns("codditt").DefaultValue = "ROCES"
            ''ds.Tables(strNomeTabella).Columns("au_codart").DefaultValue = CodArt
            ''ds.Tables(strNomeTabella).Columns("xx_descr").DefaultValue = descrArt
            'ds.Tables(strNomeTabella).Columns("am_note").DefaultValue = "NOTA"

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub

    Public Overrides Sub BeforeColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
        Dim strErr As String = ""
        Try
            'memorizzo il valore corrente di cella per testarlo nella AfterColUpdate
            'solo se il dato è uguale a quello precedentemente contenuto nella cella
            If ValoriUguali(e.ProposedValue.ToString, e.Row(e.Column.ColumnName).ToString) Then
                strPrevCelValue += e.Column.ColumnName.ToUpper + ";"
                Return
            End If
            '-------------------------------------------------------------
            'controllo che in una cella short non venga inserito un numero troppo grande
            If Not CheckCellaShort(e, strErr) Then Throw New NTSException(strErr)
            '-------------------------------------------------------------
            'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
            Dim strFunction As String = "BeforeColUpdate_" & e.Column.ColumnName.ToLower
            Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
            If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})

        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub

    Public Overrides Sub AfterColUpdate(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)
        Try
            'non valido la colonna se il dato non è cambiato
            If strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";") > -1 Then
                strPrevCelValue = strPrevCelValue.Remove(strPrevCelValue.IndexOf(e.Column.ColumnName.ToUpper + ";"), e.Column.ColumnName.ToUpper.Length + 1)
                Return
            End If

            bHasChanges = True

            'comunico che una cella è cambiata, per fare in modo che se il dato è contenuto in una griglia 
            'vengano fatte le routine di validazione del caso
            ThrowRemoteEvent(New NTSEventArgs("GRIAGG", e.Column.Table.TableName & "§" & e.Column.ColumnName))

            e.Row.EndEdit()
            e.Row.EndEdit()

            '-------------------------------------------------------------
            'cerco e, se la trovo, eseguo la funzione specifica per la colonna modificata
            Dim strFunction As String = "AfterColUpdate_" & e.Column.ColumnName.ToLower
            Dim fun As System.Reflection.MethodInfo = Me.GetType.GetMethod(strFunction)  'occhio: è case_sensitive!!!!
            If Not fun Is Nothing Then fun.Invoke(Me, New Object() {sender, e})
        Catch ex As Exception
            '--------------------------------------------------------------

            CLN__STD.GestErr(ex, Me, "")

            '--------------------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub BeforeColUpdate_tb_codstato(ByVal sender As Object, ByVal e As DataColumnChangeEventArgs)

        Try
            'Dim strCodStato As String = NTSCStr(e.ProposedValue)
            'Dim dttRecord As New DataTable
            'Dim strDescCodStato As String = ""

            'If Not oCldDeco.ValCodiceDb(strCodStato, strDittaCorrente, "TABSTAT", "S", strDescCodStato, dttRecord) Then

            '    ThrowRemoteEvent(New NTSEventArgs("", "Codice stato non esistente, verrà ripristinato quello precedente"))

            '    e.ProposedValue = e.Row(e.Column.ColumnName)

            '    Return

            'End If

            'If dttRecord IsNot Nothing Then
            '    If dttRecord.Rows.Count = 1 Then

            '        If NTSCStr(dttRecord.Rows(0)!tb_codstat) IsNot String.Empty Then

            '            strDescCodStato = NTSCStr(dttRecord.Rows(0)!tb_codstat)

            '            e.Row!xx_descstato = strDescCodStato

            '        End If
            '    End If
            'End If

            'Return

        Catch ex As Exception

            CLN__STD.GestErr(ex, Me, "")

        End Try



    End Sub

    Public Overridable Function TestUbicaz(ByRef row As DataRow) As Boolean
        Try
            Dim sUbicaz As String = NTSCStr(row!au_ubicaz)

            If String.IsNullOrEmpty(sUbicaz) Then
                ThrowRemoteEvent(New NTSEventArgs("", "L'ubicazione è obbligatoria!"))
                Return False
            End If

            If Not oCldDeco.CheckUbicazioneExists(sUbicaz, strDittaCorrente) Then
                ThrowRemoteEvent(New NTSEventArgs("", "L'ubicazione inserita non è valida!"))
                row!au_ubicaz = ""
                Return False
            End If

            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Function TestPreSalva() As Boolean
        'Eseguo i controlli necessari prima di fare il salvataggio della riga
        Dim dtrTmp As DataRow() = dsShared.Tables(strNomeTabella).Select(Nothing, Nothing, DataViewRowState.Added Or DataViewRowState.ModifiedCurrent)

        Try

            For Each row As DataRow In dtrTmp

                ''MUST DA MODIFICARE
                'If Not TestUbicaz(row) Then Return False

                Dim codiceDitta As String = NTSCStr(row!codditt)
                If codiceDitta <> "ROCES" Then
                    listaMessaggiErrore.Add("Attenzione! Il codice ditta deve essere 'ROCES'.")
                    Return False
                End If

                '----------------------------------------

                Dim contoPrincipale As String = NTSCStr(row!de_contoprincipale)

                If contoPrincipale = String.Empty Then
                    listaMessaggiErrore.Add("Attenzione! Non hai inserito il conto principale.")
                    Return False
                End If

                Dim descrizioneSecondario As String = String.Empty

                'If Not ocldBase.ValCodiceDb(contoPrincipale, "REGINATO", "TABMARC", "S", descrizioneSecondario) Then
                '    listaMessaggiErrore.Add("Attenzione! Codice marca inesistente.")
                '    Return False
                'End If

                'row!xx_ = descrizioneSecondario

                '----------------------------------------

                Dim contoSecondario As String = NTSCStr(row!de_contosecondario)

                If contoSecondario = String.Empty Then
                    listaMessaggiErrore.Add("Attenzione! Non hai inserito il conto secondario.")
                    Return False
                End If

                Dim descrizionePrimario As String = String.Empty

                'If Not ocldBase.ValCodiceDb(contoPrincipale, "REGINATO", "TABMARC", "S", descrizionePrimario) Then
                '    listaMessaggiErrore.Add("Attenzione! Codice marca inesistente.")
                '    Return False
                'End If

                'row!xx_ = descrizionePrimario

                '----------------------------------------


            Next

            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overrides Function Salva(ByVal bDelete As Boolean) As Boolean

        Dim bResult As Boolean = False
        Try
            '----------------------------------------
            'controlli pre-salvataggio (solo se non è una delete)
            If Not bDelete Then
                If Not TestPreSalva() Then Return False
            End If

            '----------------------------------------
            'chiamo il dal per salvare
            bResult = oCldDeco.ScriviTabellaSemplice(strDittaCorrente, strNomeTabella, dsShared.Tables(strNomeTabella), "", "", "")

            If bResult Then
                bHasChanges = False
            End If

            Return bResult

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

End Class
