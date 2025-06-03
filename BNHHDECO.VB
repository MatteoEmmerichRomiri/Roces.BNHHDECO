Imports System.Data
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports NTSInformatica.CLN__STD

Public Class FRMHHDECO
    Inherits FRM__CHIL

#Region "MODULI"
    Private Moduli_P As Integer = CLN__STD.bsModAll
    Private ModuliExt_P As Integer = CLN__STD.bsModExtAll
    Private ModuliSup_P As Integer = 0
    Private ModuliSupExt_P As Integer = 0
    Private ModuliPtn_P As Integer = 0
    Private ModuliPtnExt_P As Integer = 0

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
#End Region

#Region "Variabili"
    Public strNomeTabella As String = "HHDECODICA"

    Public nuovaRiga As Boolean = False

    Public oCleDeco As CLEHHDECO

    Public dsHhst As DataSet
    Public dcDeco As BindingSource = New BindingSource()

    Public oCallParams As CLE__CLDP

    Public dsZoom As New DataSet

    Private components As System.ComponentModel.IContainer
    Public WithEvents NtsBarManager1 As NTSInformatica.NTSBarManager
    Public WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Public WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Public WithEvents tlbMain As NTSInformatica.NTSBar
    Public WithEvents tlbCancella As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbRipristina As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbZoom As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbStrumenti As NTSInformatica.NTSBarSubItem
    Public WithEvents tlbImpostaStampante As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbGuida As NTSInformatica.NTSBarButtonItem
    Public WithEvents tlbEsci As NTSInformatica.NTSBarButtonItem
    Public WithEvents pnHhst As NTSInformatica.NTSPanel
    Public WithEvents grDeco As NTSInformatica.NTSGrid
    Friend WithEvents tlbUbicazioni As NTSInformatica.NTSBarButtonItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents de_contoprincipale As NTSGridColumn
    Friend WithEvents de_contosecondario As NTSGridColumn
    Friend WithEvents de_descrcontosecondario As NTSGridColumn
    Friend WithEvents codditt As NTSGridColumn
    Friend WithEvents de_descrcontoprincipale As NTSGridColumn
    Public WithEvents grvDeco As NTSInformatica.NTSGridView

#End Region

    Public Overloads Function Init(ByRef Menu As CLE__MENU, ByRef Param As CLE__CLDP, Optional ByVal Ditta As String = "", Optional ByRef SharedControls As CLE__EVNT = Nothing) As Boolean
        oMenu = Menu
        oApp = oMenu.App
        oCallParams = Param
        If Ditta <> "" Then
            DittaCorrente = Ditta
        Else
            DittaCorrente = oApp.Ditta
        End If

        InitializeComponent()
        Me.MinimumSize = Me.Size

        '------------------------------------------------
        'Creo e attivo l'entity, in più inizializzo la funzione che dovrà rilevare gli eventi dall'ENTITY stesso
        Dim strErr As String = ""
        Dim objTmp As Object = Nothing
        If CLN__STD.NTSIstanziaDll(oApp.ServerDir, oApp.NetDir, "BNHHDECO", "BEHHDECO", objTmp, strErr, False, "", "") = False Then  ''MUST DA CAMBIARE
            oApp.MsgBoxErr(oApp.Tr(Me, 128182574024687500, "ERRORE in fase di creazione Entity:" & vbCrLf & "|" & strErr & "|"))
            Return False
        End If
        oCleDeco = CType(objTmp, CLEHHDECO)
        '------------------------------------------------

        AddHandler oCleDeco.RemoteEvent, AddressOf GestisciEventiEntity
        If oCleDeco.Init(oApp, NTSScript, oMenu.oCleComm, strNomeTabella, False, "", "") = False Then Return False

        'oCleDeco.CodArt = If(oCallParams.strPar1 IsNot Nothing, oCallParams.strPar1, "")

        'Setto i riferimenti alle variabili condivise
        'oCleDeco.CodArt = oCleDeco.CodArt
        'oCleDeco.descrArt = oCleDeco.descrArt

        Return True
    End Function

    Public Overridable Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRMHHDECO))
        Me.NtsBarManager1 = New NTSInformatica.NTSBarManager()
        Me.tlbMain = New NTSInformatica.NTSBar()
        Me.tlbRipristina = New NTSInformatica.NTSBarButtonItem()
        Me.tlbCancella = New NTSInformatica.NTSBarButtonItem()
        Me.tlbStrumenti = New NTSInformatica.NTSBarSubItem()
        Me.tlbImpostaStampante = New NTSInformatica.NTSBarButtonItem()
        Me.tlbGuida = New NTSInformatica.NTSBarButtonItem()
        Me.tlbEsci = New NTSInformatica.NTSBarButtonItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.tlbZoom = New NTSInformatica.NTSBarButtonItem()
        Me.tlbUbicazioni = New NTSInformatica.NTSBarButtonItem()
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
        Me.pnHhst = New NTSInformatica.NTSPanel()
        Me.grDeco = New NTSInformatica.NTSGrid()
        Me.grvDeco = New NTSInformatica.NTSGridView()
        Me.codditt = New NTSInformatica.NTSGridColumn()
        Me.de_contoprincipale = New NTSInformatica.NTSGridColumn()
        Me.de_descrcontoprincipale = New NTSInformatica.NTSGridColumn()
        Me.de_contosecondario = New NTSInformatica.NTSGridColumn()
        Me.de_descrcontosecondario = New NTSInformatica.NTSGridColumn()
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnHhst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnHhst.SuspendLayout()
        CType(Me.grDeco, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grvDeco, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'NtsBarManager1
        '
        Me.NtsBarManager1.AllowCustomization = False
        Me.NtsBarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.tlbMain})
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlTop)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.NtsBarManager1.DockControls.Add(Me.barDockControlRight)
        Me.NtsBarManager1.Form = Me
        Me.NtsBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.tlbCancella, Me.tlbRipristina, Me.tlbZoom, Me.tlbStrumenti, Me.tlbImpostaStampante, Me.tlbGuida, Me.tlbEsci, Me.tlbUbicazioni, Me.BarButtonItem1})
        Me.NtsBarManager1.MaxItemId = 18
        '
        'tlbMain
        '
        Me.tlbMain.BarName = "tlbMain"
        Me.tlbMain.DockCol = 0
        Me.tlbMain.DockRow = 0
        Me.tlbMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
        Me.tlbMain.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbRipristina), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbCancella), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbStrumenti, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbGuida, True), New DevExpress.XtraBars.LinkPersistInfo(Me.tlbEsci)})
        Me.tlbMain.OptionsBar.AllowQuickCustomization = False
        Me.tlbMain.OptionsBar.DisableClose = True
        Me.tlbMain.OptionsBar.DrawDragBorder = False
        Me.tlbMain.OptionsBar.UseWholeRow = True
        Me.tlbMain.Text = "tlbMain"
        '
        'tlbRipristina
        '
        Me.tlbRipristina.Caption = "Ripristina"
        Me.tlbRipristina.Glyph = CType(resources.GetObject("tlbRipristina.Glyph"), System.Drawing.Image)
        Me.tlbRipristina.Id = 3
        Me.tlbRipristina.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F8)
        Me.tlbRipristina.Name = "tlbRipristina"
        Me.tlbRipristina.Visible = True
        '
        'tlbCancella
        '
        Me.tlbCancella.Caption = "Cancella"
        Me.tlbCancella.Glyph = CType(resources.GetObject("tlbCancella.Glyph"), System.Drawing.Image)
        Me.tlbCancella.Id = 2
        Me.tlbCancella.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4)
        Me.tlbCancella.Name = "tlbCancella"
        Me.tlbCancella.Visible = True
        '
        'tlbStrumenti
        '
        Me.tlbStrumenti.Caption = "Strumenti"
        Me.tlbStrumenti.Id = 9
        Me.tlbStrumenti.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.tlbImpostaStampante)})
        Me.tlbStrumenti.Name = "tlbStrumenti"
        Me.tlbStrumenti.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionInMenu
        Me.tlbStrumenti.Visible = True
        '
        'tlbImpostaStampante
        '
        Me.tlbImpostaStampante.Caption = "Imposta stampante"
        Me.tlbImpostaStampante.Id = 10
        Me.tlbImpostaStampante.Name = "tlbImpostaStampante"
        Me.tlbImpostaStampante.Visible = True
        '
        'tlbGuida
        '
        Me.tlbGuida.Caption = "Guida"
        Me.tlbGuida.Id = 13
        Me.tlbGuida.Name = "tlbGuida"
        Me.tlbGuida.Visible = True
        '
        'tlbEsci
        '
        Me.tlbEsci.Caption = "Esci"
        Me.tlbEsci.Id = 14
        Me.tlbEsci.Name = "tlbEsci"
        Me.tlbEsci.Visible = True
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlTop.Size = New System.Drawing.Size(682, 35)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 503)
        Me.barDockControlBottom.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlBottom.Size = New System.Drawing.Size(682, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 35)
        Me.barDockControlLeft.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 468)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(682, 35)
        Me.barDockControlRight.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 468)
        '
        'tlbZoom
        '
        Me.tlbZoom.Caption = "Zoom"
        Me.tlbZoom.Id = 4
        Me.tlbZoom.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5)
        Me.tlbZoom.Name = "tlbZoom"
        Me.tlbZoom.Visible = True
        '
        'tlbUbicazioni
        '
        Me.tlbUbicazioni.Caption = "tlbUbicazioni"
        Me.tlbUbicazioni.Id = 16
        Me.tlbUbicazioni.Name = "tlbUbicazioni"
        Me.tlbUbicazioni.Visible = True
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Salva"
        Me.BarButtonItem1.Id = 17
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'pnHhst
        '
        Me.pnHhst.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.pnHhst.Appearance.Options.UseBackColor = True
        Me.pnHhst.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pnHhst.Controls.Add(Me.grDeco)
        Me.pnHhst.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnHhst.Location = New System.Drawing.Point(0, 35)
        Me.pnHhst.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat
        Me.pnHhst.LookAndFeel.UseDefaultLookAndFeel = False
        Me.pnHhst.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.pnHhst.Name = "pnHhst"
        Me.pnHhst.Size = New System.Drawing.Size(682, 468)
        '
        'grDeco
        '
        Me.grDeco.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grDeco.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.grDeco.Location = New System.Drawing.Point(0, 0)
        Me.grDeco.MainView = Me.grvDeco
        Me.grDeco.Margin = New System.Windows.Forms.Padding(3, 32, 3, 32)
        Me.grDeco.Name = "grDeco"
        Me.grDeco.NTSNoRidimensionamentoUltimaColonna = False
        Me.grDeco.Size = New System.Drawing.Size(682, 468)
        Me.grDeco.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grvDeco})
        '
        'grvDeco
        '
        Me.grvDeco.ActiveFilterEnabled = False
        Me.grvDeco.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.codditt, Me.de_contoprincipale, Me.de_descrcontoprincipale, Me.de_contosecondario, Me.de_descrcontosecondario})
        Me.grvDeco.CustomizationFormBounds = New System.Drawing.Rectangle(680, 326, 208, 170)
        Me.grvDeco.Enabled = True
        Me.grvDeco.GridControl = Me.grDeco
        Me.grvDeco.Name = "grvDeco"
        Me.grvDeco.OptionsCustomization.AllowRowSizing = True
        Me.grvDeco.OptionsFilter.AllowFilterEditor = False
        Me.grvDeco.OptionsNavigation.EnterMoveNextColumn = True
        Me.grvDeco.OptionsNavigation.UseTabKey = False
        Me.grvDeco.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.grvDeco.OptionsView.ColumnAutoWidth = False
        Me.grvDeco.OptionsView.EnableAppearanceEvenRow = True
        Me.grvDeco.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden
        Me.grvDeco.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.grvDeco.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
        Me.grvDeco.OptionsView.ShowGroupPanel = False
        '
        'codditt
        '
        Me.codditt.AppearanceCell.Options.UseBackColor = True
        Me.codditt.AppearanceCell.Options.UseTextOptions = True
        Me.codditt.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.codditt.Caption = "Codice Ditta"
        Me.codditt.Enabled = True
        Me.codditt.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.codditt.Name = "codditt"
        Me.codditt.Visible = True
        Me.codditt.VisibleIndex = 0
        '
        'de_contoprincipale
        '
        Me.de_contoprincipale.AppearanceCell.Options.UseBackColor = True
        Me.de_contoprincipale.AppearanceCell.Options.UseTextOptions = True
        Me.de_contoprincipale.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.de_contoprincipale.Caption = "Conto Principale"
        Me.de_contoprincipale.Enabled = True
        Me.de_contoprincipale.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.de_contoprincipale.Name = "de_contoprincipale"
        Me.de_contoprincipale.NTSSetparam___Tipo = NTSInformatica.NTSGridColumn.NTSSetparamTipoColonna.Numero
        Me.de_contoprincipale.Visible = True
        Me.de_contoprincipale.VisibleIndex = 1
        '
        'de_descrcontoprincipale
        '
        Me.de_descrcontoprincipale.AppearanceCell.Options.UseBackColor = True
        Me.de_descrcontoprincipale.AppearanceCell.Options.UseTextOptions = True
        Me.de_descrcontoprincipale.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.de_descrcontoprincipale.Caption = "Descrizione Conto Principale"
        Me.de_descrcontoprincipale.Enabled = True
        Me.de_descrcontoprincipale.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.de_descrcontoprincipale.Name = "de_descrcontoprincipale"
        Me.de_descrcontoprincipale.Visible = True
        Me.de_descrcontoprincipale.VisibleIndex = 2
        '
        'de_contosecondario
        '
        Me.de_contosecondario.AppearanceCell.Options.UseBackColor = True
        Me.de_contosecondario.AppearanceCell.Options.UseTextOptions = True
        Me.de_contosecondario.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.de_contosecondario.Caption = "Conto Secondario"
        Me.de_contosecondario.Enabled = True
        Me.de_contosecondario.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.de_contosecondario.Name = "de_contosecondario"
        Me.de_contosecondario.NTSSetparam___Tipo = NTSInformatica.NTSGridColumn.NTSSetparamTipoColonna.Numero
        Me.de_contosecondario.Visible = True
        Me.de_contosecondario.VisibleIndex = 3
        '
        'de_descrcontosecondario
        '
        Me.de_descrcontosecondario.AppearanceCell.Options.UseBackColor = True
        Me.de_descrcontosecondario.AppearanceCell.Options.UseTextOptions = True
        Me.de_descrcontosecondario.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        Me.de_descrcontosecondario.Caption = "Descrizione Conto Secondario"
        Me.de_descrcontosecondario.Enabled = True
        Me.de_descrcontosecondario.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.de_descrcontosecondario.Name = "de_descrcontosecondario"
        Me.de_descrcontosecondario.Visible = True
        Me.de_descrcontosecondario.VisibleIndex = 4
        '
        'FRMHHDECO
        '
        Me.ClientSize = New System.Drawing.Size(682, 503)
        Me.Controls.Add(Me.pnHhst)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.HelpButton = True
        Me.Margin = New System.Windows.Forms.Padding(3, 26, 3, 26)
        Me.Name = "FRMHHDECO"
        Me.Text = "DECODIFICA CONTO"
        CType(Me.dttSmartArt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NtsBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnHhst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnHhst.ResumeLayout(False)
        CType(Me.grDeco, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grvDeco, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#Region "Form"

    Public Overridable Sub FRM__CIVA_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            dcDeco.Dispose()
            If Not dsHhst Is Nothing Then dsHhst.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Public Overridable Sub FRM__CIVA_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Not Salva() Then e.Cancel = True
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub FRM__HHST_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Aggiunto
            'If Not oMenu.ValCodiceDb(oCleDeco.CodArt, DittaCorrente, "ARTICO", "S", oCleDeco.descrArt) Then
            '    oApp.MsgBoxErr("Codice articolo inesistente!")
            'End If
            'Predispongo i controlli
            InitControls()
            GctlSetRoules()

            '-------------------------------------------------
            'Leggo dal database i dati e collego l'NTSBinding
            grDeco.DataSource = Nothing
            If oCleDeco.Apri(DittaCorrente, dsHhst) Then
                dcDeco.DataSource = dsHhst.Tables(strNomeTabella)
                dsHhst.AcceptChanges()
                grDeco.DataSource = dcDeco
            End If

            'Setto i valori di default per le colonne che non si possono modificare
            'dsHhst.Tables("HHARTUBI").Columns("xx_descr").DefaultValue = oCleDeco.descrArt
            'dsHhst.Tables("HHARTUBI").Columns("au_codart").DefaultValue = oCleDeco.CodArt


        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub
#End Region

#Region "Griglia"

    Public Overridable Function grvDeco_NuovaRiga() As Boolean
        Try
            grvDeco.ClearSorting()

            Me.GctlApplicaDefaultValue()

            Dim currentRow As DataRow = grvDeco.NTSGetCurrentDataRow
            If currentRow Is Nothing Then Return False


            If currentRow IsNot Nothing Then
                Dim coddittValue As String = ""
                If Not IsDBNull(currentRow!am_codmarca) Then
                    coddittValue = currentRow!am_codmarca.ToString()
                End If
            End If

            grvDeco.NTSAllowInsert = True

            nuovaRiga = True

            Return True

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Function

    Public Overridable Sub grvDeco_NTSFocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles grvDeco.NTSFocusedRowChanged
        Try

            Dim testDebug As String = String.Empty

            'Dim currentRow As DataRow = grvDeco.NTSGetCurrentDataRow
            'If currentRow Is Nothing Then Return

            'Select Case nuovaRiga
            '    Case True
            '        am_codmarca.Enabled = True
            '        codditt.Enabled = True

            '        nuovaRiga = False

            '    Case False
            '        If am_codmarca.Enabled Then am_codmarca.Enabled = False
            '        If codditt.Enabled Then codditt.Enabled = False

            'End Select


        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub

    Public Overridable Sub grvDeco_NTSBeforeRowUpdate(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowAllowEventArgs) Handles grvDeco.NTSBeforeRowUpdate
        Try
            If Not Salva() Then
                'Rimango sulla riga su cui sono
                e.Allow = False
                Return
            End If

            If Not grvDeco_NuovaRiga() Then Return

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

#End Region

    'Setto le immagini della toolBar
    Public Overridable Sub InitToolBarImage()
        Try

            tlbCancella.GlyphPath = (oApp.ChildImageDir & "\delete.png")

            tlbRipristina.GlyphPath = (oApp.ChildImageDir & "\restore.png")

            tlbZoom.GlyphPath = (oApp.ChildImageDir & "\zoom.png")

            tlbStrumenti.GlyphPath = (oApp.ChildImageDir & "\options.png")

            tlbGuida.GlyphPath = (oApp.ChildImageDir & "\help.png")

            tlbEsci.GlyphPath = (oApp.ChildImageDir & "\exit.png")

        Catch ex As Exception

        End Try
    End Sub

    Public Overridable Sub InitControls()
        InitControlsBeginEndInit(Me, False)

        Try
            'Carico le immagini della Toolbar
            InitToolBarImage()

            tlbMain.NTSSetToolTip()

            'Setto il nome della griglia
            grvDeco.NTSSetParam(oMenu, "HHDECODICA")

            'Setto la colonna codditt
            codditt.NTSSetParamSTR(oMenu, "Codice Ditta", 50) 'OBBLIGATORIO
            codditt.Enabled = False
            codditt.Visible = False

            ''Setto la colonna xx_descr
            'xx_descr.NTSSetParamSTR(oMenu, "Descrizione Marca", 50) 'OBBLIGATORIO
            'xx_descr.Enabled = False

            'Setto la colonna am_codmarca
            de_contoprincipale.NTSSetParamSTR(oMenu, "Conto Principale", 50) 'OBBLIGATORIO
            'am_codmarca.Enabled = False

            'Setto la colonna am_codmarcaweb
            de_descrcontoprincipale.NTSSetParamSTR(oMenu, "Descrizione Conto Principale", 50) 'OBBLIGATORIO
            de_descrcontoprincipale.Enabled = False

            ''Setto la colonna am_descrmarcaweb
            de_contosecondario.NTSSetParamSTR(oMenu, "Conto Secondario", 50) 'OBBLIGATORIO

            ''Setto la colonna am_note
            de_descrcontosecondario.NTSSetParamSTR(oMenu, "Descrizione Conto Secondario", 50) 'OBBLIGATORIO
            de_descrcontosecondario.Enabled = False

            ''Setto la colonna xx_ubicaz
            'au_ubicaz.NTSSetParamSTR(oMenu, "Ubicaz", 50)

            'au_ubicaz.NTSSetParamZoom("ZOOMANAUBIC")

            'Chiamo lo script per inizializzare i controlli caricati con Source Extender
            NTSScriptExec("InitControls", Me, Nothing)

        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
        InitControlsBeginEndInit(Me, True)
    End Sub

    Public Overridable Function Salva() As Boolean
        Try

            'Dim currentRow As DataRow = grvDeco.NTSGetCurrentDataRow
            'If currentRow Is Nothing Then Return False

            '----------------------------------------------------------------------------------------------------------
            Me.ValidaLastControl()
            '----------------------------------------------------------------------------------------------------------
            Select Case Me.NTSChiediSalva(grvDeco, oCleDeco.RecordIsChanged)
                Case DialogResult.Yes : If Not oCleDeco.Salva(False) Then Return False
                Case DialogResult.No : oCleDeco.Ripristina(dcDeco.Position, dcDeco.Filter)
                Case DialogResult.Cancel : Return False
            End Select
            '----------------------------------------------------------------------------------------------------------

            Return True
        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        Finally
            stampaErrori()
        End Try
    End Function

    Public Overridable Sub Stampa(ByVal nDestin As Integer)
        Dim nPjob As Object
        Dim nRis As Integer = 0
        Dim strCrpe As String = ""
        Dim i As Integer
        Try

            If Not Salva() Then Return


            '--------------------------------------------------
            'Preparo il motore di stampa
            strCrpe = ""
            nPjob = oMenu.ReportPEInit(oApp.Ditta, Me, "BS--CIVA", "Reports1", " ", 0, nDestin, "BS--CIVA.RPT", False, "Codici iva", False)
            If nPjob Is Nothing Then Return

            '--------------------------------------------------
            'Lancio tutti gli eventuali reports (gestisce già il multireport)
            For i = 1 To UBound(CType(nPjob, Array), 2)
                nRis = oMenu.PESetSelectionFormula(NTSCInt(CType(nPjob, Array).GetValue(0, i)), CrpeResolveFormula(Me, CStr(CType(nPjob, Array).GetValue(2, i)), strCrpe))
                nRis = oMenu.ReportPEVai(NTSCInt(CType(nPjob, Array).GetValue(0, i)))
            Next
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub

    Public Overridable Sub stampaErrori()
        Try
            oCleDeco.listaMessaggiErrore.ForEach(Sub(Err) oApp.MsgBoxErr(Err))

            oCleDeco.listaMessaggiErrore.Clear()

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        End Try
    End Sub


#Region "Toolbar"

    Public Overridable Sub tlbCancella_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbCancella.ItemClick
        Try
            '---------------------------------------------------------------------------------------------------------
            Dim currentRow As DataRow = grvDeco.NTSGetCurrentDataRow

            If grvDeco.NTSGetCurrentDataRow Is Nothing Then Return
            '---------------------------------------------------------------------------------------------------------
            If Me.NTSChiediCancella(grvDeco) = DialogResult.Yes Then oCleDeco.Salva(True)
            '---------------------------------------------------------------------------------------------------------

        Catch ex As Exception
            CLN__STD.GestErr(ex, Me, "")
        Finally
            stampaErrori()
        End Try
    End Sub

    Public Overridable Sub tlbRipristina_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbRipristina.ItemClick
        Try
            If grvDeco.NTSRipristinaRigaCorrenteBefore(dcDeco, CType(IIf(sender Is Nothing, False, True), Boolean)) Then
                oCleDeco.Ripristina(dcDeco.Position, dcDeco.Filter)
                grvDeco.NTSRipristinaRigaCorrenteAfter()
            End If
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub


    'Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick

    '    Try
    '        Dim dsValue As New DataSet

    '        ' Apri lo Zoom per ottenere le ubicazioni
    '        If Not oCleDeco.ApriZoom(DittaCorrente, dsValue) Then Return

    '        ' Controlla che il DataSet contenga la tabella "ANAUBIC"
    '        If dsValue.Tables.Contains("ANAUBIC") AndAlso dsValue.Tables("ANAUBIC").Rows.Count > 0 Then
    '            Dim dtZoom As DataTable = dsValue.Tables("ANAUBIC")

    '            ' Crea un nuovo NTSPanel
    '            Dim ntsP As New NTSInformatica.NTSPanel()
    '            ntsP.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    '            ntsP.Dock = System.Windows.Forms.DockStyle.Fill
    '            ntsP.Location = New System.Drawing.Point(0, 0)
    '            ntsP.Name = "pnTop"
    '            ntsP.Size = New System.Drawing.Size(808, 600)

    '            ntsP.Appearance.BackColor = System.Drawing.Color.White
    '            ntsP.Appearance.Options.UseBackColor = True
    '            ntsP.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    '            ntsP.Location = New System.Drawing.Point(177, 0)
    '            ntsP.Name = "pnArti"
    '            ntsP.Size = New System.Drawing.Size(100, 100)

    '            '' Crea una ListBox per mostrare le ubicazioni
    '            'Dim lstUbicazioni As New ListBox()
    '            'lstUbicazioni.Dock = DockStyle.Top
    '            'lstUbicazioni.Height = 300

    '            '' Riempie la ListBox con i dati della colonna (sostituisci "au_ubicaz" con il nome corretto)
    '            'For Each row As DataRow In dtZoom.Rows
    '            '    lstUbicazioni.Items.Add(row("au_ubicaz").ToString())
    '            'Next

    '            '' Crea un pulsante OK
    '            'Dim btnOK As New Button()
    '            'btnOK.Text = "OK"
    '            'btnOK.Dock = DockStyle.Bottom
    '            'AddHandler btnOK.Click, Sub()
    '            '                            If lstUbicazioni.SelectedItem IsNot Nothing Then
    '            '                                Dim s As String = lstUbicazioni.SelectedItem.ToString()
    '            '                            Else
    '            '                                MessageBox.Show("Seleziona un'ubicazione.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            '                            End If
    '            '                        End Sub

    '            '' Aggiunge i controlli al pannello NTSPanel
    '            'ntsP.Controls.Add(lstUbicazioni)
    '            'ntsP.Controls.Add(btnOK)

    '            ' Aggiungi il NTSPanel al form principale (assumendo che "Me" sia il form principale)
    '            Me.Controls.Add(ntsP)

    '            ' Gestione della visualizzazione del pannello
    '            ntsP.BringToFront()
    '            ntsP.Visible = True
    '        Else
    '            MessageBox.Show("Nessuna ubicazione trovata.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        End If
    '    Catch ex As Exception
    '        ' Gestione errore generica
    '        CLN__STD.GestErr(ex, Me, "Errore durante l'apertura dello Zoom ubicazioni")
    '    End Try

    'End Sub

    'Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick

    '    Try
    '        Dim dsValue As New DataSet

    '        ' Apri lo Zoom per ottenere le ubicazioni
    '        If Not oCleDeco.ApriZoom(DittaCorrente, dsValue) Then Return

    '        ' Controlla che il DataSet contenga la tabella "ANAUBIC"
    '        If dsValue.Tables.Contains("ANAUBIC") AndAlso dsValue.Tables("ANAUBIC").Rows.Count > 0 Then
    '            Dim dtZoom As DataTable = dsValue.Tables("ANAUBIC")

    '            ' Crea un nuovo form standard
    '            Dim ntsP As NTSPanel = New NTSInformatica.NTSPanel()
    '            ntsP.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
    '            ntsP.Dock = System.Windows.Forms.DockStyle.Top
    '            ntsP.Location = New System.Drawing.Point(0, 0)
    '            ntsP.Name = "pnTop"
    '            ntsP.Size = New System.Drawing.Size(808, 108)

    '            Dim frm As New Form()
    '            frm.Text = "Seleziona Ubicazione"
    '            frm.Size = New Size(80, 600)

    '            ' Posiziona il form a destra dello schermo
    '            frm.StartPosition = FormStartPosition.Manual
    '            frm.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - frm.Width, 0)

    '            ' Crea una ListBox per mostrare le ubicazioni
    '            Dim lstUbicazioni As New ListBox()
    '            lstUbicazioni.Dock = DockStyle.Top
    '            lstUbicazioni.Height = 300

    '            ' Riempie la ListBox con i dati della colonna (sostituisci "NomeColonnaUbicazione" con il nome corretto)
    '            For Each row As DataRow In dtZoom.Rows
    '                lstUbicazioni.Items.Add(row("au_ubicaz").ToString())
    '            Next

    '            ' Crea un pulsante OK
    '            Dim btnOK As New Button()
    '            btnOK.Text = "OK"
    '            btnOK.Dock = DockStyle.Bottom
    '            AddHandler btnOK.Click, Sub()
    '                                        If lstUbicazioni.SelectedItem IsNot Nothing Then
    '                                            Dim s As String = lstUbicazioni.SelectedItem.ToString()
    '                                            frm.DialogResult = DialogResult.OK
    '                                        Else
    '                                            MessageBox.Show("Seleziona un'ubicazione.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '                                        End If
    '                                    End Sub

    '            ' Aggiunge i controlli al form
    '            frm.Controls.Add(lstUbicazioni)
    '            frm.Controls.Add(btnOK)

    '            ' Mostra il form come modale
    '            If frm.ShowDialog() = DialogResult.OK Then
    '                ' L'ubicazione è stata selezionata e salvata in txtUbicazione.Text
    '            End If
    '        Else
    '            MessageBox.Show("Nessuna ubicazione trovata.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        End If
    '    Catch ex As Exception
    '        ' Gestione errore generica
    '        CLN__STD.GestErr(ex, Me, "Errore durante l'apertura dello Zoom ubicazioni")
    '    End Try
    'End Sub

    Public Overridable Sub tlbZoom_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbZoom.ItemClick
        Try
            'If dsZoom.Tables("ANAUBCAZ").Rows.Count > 0 Then
            '    oCallParams.strOut = grvDeco.GetFocusedRowCellDisplayText("tt_ubicaz")
            'End If

            NTSCallStandardZoom()

        Catch ex As Exception
            ' Gestione errore generica
            CLN__STD.GestErr(ex, Me, "Errore durante l'apertura dello Zoom ubicazioni")
        End Try
    End Sub

    Public Overridable Sub tlbGuida_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbGuida.ItemClick
        SendKeys.Send("{F1}")
    End Sub

    Public Overridable Sub tlbEsci_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles tlbEsci.ItemClick
        Try
            If Not Salva() Then Return
            Me.Close()
        Catch ex As Exception
            '-------------------------------------------------
            CLN__STD.GestErr(ex, Me, "")
            '-------------------------------------------------
        End Try
    End Sub


    Private Sub grDeco_Click(sender As Object, e As EventArgs) Handles grDeco.Click

        Dim currentRow As DataRow = grvDeco.NTSGetCurrentDataRow

        Dim a As Integer = 0
    End Sub


#End Region

End Class

