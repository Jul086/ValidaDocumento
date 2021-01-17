Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO

Public Class ValidaDocumento
    Dim dtglobal As DataTable = New DataTable
    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
        If Len(txtCliente.Text) = 0 Then
            MsgBox("El Cliente no puede estar vacio")
            txtCliente.Focus()
            Return
        End If
        ExpedientePorAuditar()
    End Sub

    Private Sub ExpedientePorAuditar()
        Try
            Dim dt As DataTable = New DataTable

            Dim connectionString = ConfigurationManager.ConnectionStrings("Cnx").ConnectionString
            Dim queryString = "dbo.csp_AUX_ExpedientesPendientesAuditar "
            queryString += " @FINICIAL = '" & dtpFecIni.Value.ToString("yyyy-MM-dd") & "'"
            queryString += " , @FFINAL = '" & dtpFecFin.Value.ToString("yyyy-MM-dd") & "'"
            queryString += " , @CTE = '" & Trim(txtCliente.Text) & "'"

            Using connection As New SqlConnection(connectionString)
                Dim command = New SqlCommand(queryString, connection)
                connection.Open()

                dt.Load(command.ExecuteReader())
                dtglobal = dt

                dt = Nothing

                connection.Close()
            End Using

            EjSplit
            ObtieneRuta()

        Catch ex As Exception
            'Throw New GestorExcepcio(exc.Message)
            MsgBox(ex.Message, vbOKOnly, "Error")
        End Try

    End Sub

    Private Sub EjSplit()
        'Dim testString As String = "apple    pear banana  "
        Dim testString As String = "apple/pear/banana  "
        Dim testArray() As String = Split(testString, "/")
        '' testArray holds {"apple", "", "", "", "pear", "banana", "", ""}
        'Dim lastNonEmpty As Integer = -1
        'For i As Integer = 0 To testArray.Length - 1
        '    If testArray(i) <> "/" Then
        '        lastNonEmpty += 1
        '        testArray(lastNonEmpty) = testArray(i)
        '    End If
        'Next
        'ReDim Preserve testArray(lastNonEmpty)
        '' testArray now holds {"apple", "pear", "banana"}

    End Sub


    Private Sub ObtieneRuta()

        Dim MiTabla As New DataTable
        ' Create four typed columns in the DataTable.
        MiTabla.Columns.Add("Factura", GetType(String))
        MiTabla.Columns.Add("Referencia", GetType(String))
        MiTabla.Columns.Add("Rectificacion", GetType(Integer))
        MiTabla.Columns.Add("Cove Ruta", GetType(String))
        MiTabla.Columns.Add("Cove Valida", GetType(String))
        MiTabla.Columns.Add("Doda Ruta", GetType(String))
        MiTabla.Columns.Add("Doda Valida", GetType(String))
        MiTabla.Columns.Add("Pedimento Ruta", GetType(String))
        MiTabla.Columns.Add("Pedimento Valida", GetType(String))
        MiTabla.Columns.Add("HC Ruta", GetType(String))
        MiTabla.Columns.Add("HC Valida", GetType(String))
        MiTabla.Columns.Add("Cuenta de Gastos Ruta", GetType(String))
        MiTabla.Columns.Add("Cuenta de Gastos Valida", GetType(String))
        MiTabla.Columns.Add("Simplificado1 Ruta", GetType(String))
        MiTabla.Columns.Add("Simplificado1 Valida", GetType(String))
        MiTabla.Columns.Add("Simplificado2 Ruta", GetType(String))
        MiTabla.Columns.Add("Simplificado2 Valida", GetType(String))
        MiTabla.Columns.Add("Archivo Val Ruta", GetType(String))
        MiTabla.Columns.Add("Archivo Val Valida", GetType(String))
        MiTabla.Columns.Add("Banco Ruta", GetType(String))
        MiTabla.Columns.Add("Banco Valida", GetType(String))
        MiTabla.Columns.Add("FacturaXML Ruta", GetType(String))
        MiTabla.Columns.Add("FacturaXML Valida", GetType(String))
        MiTabla.Columns.Add("FacturaPDF Ruta", GetType(String))
        MiTabla.Columns.Add("FacturaPDF Valida", GetType(String))
        MiTabla.Columns.Add("Acuse Ruta", GetType(String))
        MiTabla.Columns.Add("Acuse Valida", GetType(String))

        Dim connectionString = ConfigurationManager.ConnectionStrings("Cnx").ConnectionString

        Try
            For Each row As DataRow In dtglobal.Rows
                Dim Factura As String = CStr(row("Factura"))
                Dim Referencia As String = CStr(row("Referencia"))
                Dim ReferenciaArray() As String = Split(Referencia, "/")

                Dim Rectificacion As String = CStr(row("Rectificacion"))
                Dim queryString = "dbo.sp_vista_aduasis_pu_expedienteUnico "
                Dim Parametros As String = ""

                Parametros += " @referencia = '" & Trim(ReferenciaArray(0)) & "'"
                Parametros += " , @FECHAI = NULL "
                Parametros += " , @FECHAF = NULL "
                Parametros += " , @rectificacion= '" & Trim(Rectificacion) & "'"
                queryString += Parametros

                Using connection As New SqlConnection(connectionString)
                    'Dim command = New SqlCommand(queryString, connection)
                    'connection.Open()
                    Dim dt As DataTable = New DataTable
                    Dim adapter As SqlDataAdapter = New SqlDataAdapter(queryString, connection)
                    adapter.Fill(dt)

                    'dt.Load(command.ExecuteReader())
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0).Item("Referencia").ToString() <> "" Then
                            Dim aaa = 1

                            MiTabla.Rows.Add(Factura,
                                             Referencia,
                                             Rectificacion,
                                             dt.Rows(0).Item("Cove_local").ToString(), " ",
                                             dt.Rows(0).Item("Doda_local").ToString(), " ",
                                             dt.Rows(0).Item("Pedimento_local").ToString(), " ",
                                             dt.Rows(0).Item("Hc_local").ToString(), " ",
                                             dt.Rows(0).Item("Mv_local").ToString(), " ",
                                             dt.Rows(0).Item("Simplificado1_local").ToString(), " ",
                                             dt.Rows(0).Item("Simplificado2_local").ToString(), " ",
                                             dt.Rows(0).Item("Archivo_validacion_local").ToString(), " ",
                                             dt.Rows(0).Item("Archivo_banco_local").ToString(), " ",
                                             dt.Rows(0).Item("Factura_xml_local").ToString(), " ",
                                             dt.Rows(0).Item("Factura_pdf_local").ToString(), " ",
                                             dt.Rows(0).Item("Acuse Cove_local").ToString(), " ")
                        End If
                    End If

                    dt = Nothing
                    connection.Close()
                End Using


            Next
            dgvDocumento.DataSource = MiTabla
            RevisaDocumento()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly, "Error")
        End Try


    End Sub

    Private Sub EjecutaSQL()
        Try
            'Dim name As String = ConfigurationManager.AppSettings("Cnx")
            'Dim dr As SqlDataReader
            Dim dt As DataTable = New DataTable

            Dim connectionString = ConfigurationManager.ConnectionStrings("Cnx").ConnectionString
            Dim queryString = "dbo.csp_AUX_ExpedientesPendientesAuditar "
            queryString += " @FINICIAL = '" & dtpFecIni.Value.ToString("yyyy-MM-dd") & "'"
            queryString += " , @FFINAL = '" & dtpFecFin.Value.ToString("yyyy-MM-dd") & "'"
            queryString += " , @CTE = '" & Trim(txtCliente.Text) & "'"

            Using connection As New SqlConnection(connectionString)
                Dim command = New SqlCommand(queryString, connection)
                connection.Open()

                dt.Load(command.ExecuteReader())
                dtglobal = dt
                dgvDocumento.DataSource = dt

                dt = Nothing

                'Using reader As SqlDataReader = command.ExecuteReader()
                '    While reader.Read()
                '        'Console.WriteLine(String.Format("{0}, {1}", reader(0), reader(1)))
                '        MsgBox(String.Format("{0}, {1}", reader(0), reader(1)))
                '    End While
                'End Using
                connection.Close()

            End Using



            'Dim s As String = ("SELECT * FROM Alumnes")
            'connexio = New OleDbConnection(myConnectionString)
            'myCommand = New OleDbCommand(s)
            'myCommand.Connection = connexio
            'connexio.Open()
            'Dim myReader As OleDbDataReader = myCommand.ExecuteReader()
            'While myReader.Read()
            '    Dim NOM As String = myReader("NOM")
            '    Dim COGNOM As String = myReader("COGNOM")
            'End While
        Catch exc As Exception
            'Throw New GestorExcepcio(exc.Message)
            MsgBox(exc.Message)
        End Try

    End Sub

    Private Sub RevisaDocumento()
        'MiTabla.Columns.Add("Factura", GetType(String))
        'MiTabla.Columns.Add("Referencia", GetType(String))
        'MiTabla.Columns.Add("Rectificacion", GetType(Integer))
        'MiTabla.Columns.Add("Cove Ruta", GetType(String))
        'MiTabla.Columns.Add("Cove Valida", GetType(String))
        'MiTabla.Columns.Add("Doda Ruta", GetType(String))
        'MiTabla.Columns.Add("Doda Valida", GetType(String))
        'MiTabla.Columns.Add("Pedimento Ruta", GetType(String))
        'MiTabla.Columns.Add("Pedimento Valida", GetType(String))
        'MiTabla.Columns.Add("HC Ruta", GetType(String))
        'MiTabla.Columns.Add("HC Valida", GetType(String))
        'MiTabla.Columns.Add("Cuenta de Gastos Ruta", GetType(String))
        'MiTabla.Columns.Add("Cuenta de Gastos Valida", GetType(String))
        'MiTabla.Columns.Add("Simplificado1 Ruta", GetType(String))
        'MiTabla.Columns.Add("Simplificado1 Valida", GetType(String))
        'MiTabla.Columns.Add("Simplificado2 Ruta", GetType(String))
        'MiTabla.Columns.Add("Simplificado2 Valida", GetType(String))
        'MiTabla.Columns.Add("Archivo Val Ruta", GetType(String))
        'MiTabla.Columns.Add("Archivo Val Valida", GetType(String))
        'MiTabla.Columns.Add("Banco Ruta", GetType(String))
        'MiTabla.Columns.Add("Banco Valida", GetType(String))
        'MiTabla.Columns.Add("FacturaXML Ruta", GetType(String))
        'MiTabla.Columns.Add("FacturaXML Valida", GetType(String))
        'MiTabla.Columns.Add("FacturaPDF Ruta", GetType(String))
        'MiTabla.Columns.Add("FacturaPDF Valida", GetType(String))
        'MiTabla.Columns.Add("Acuse Ruta", GetType(String))
        'MiTabla.Columns.Add("Acuse Valida", GetType(String))



        Try
            Dim bitCorrecto = True
            'Recorrer el grid
            If dgvDocumento.RowCount > 0 Then
                For i = 0 To dgvDocumento.NewRowIndex - 1
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Cove Ruta").Value, "Cove Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Doda Ruta").Value, "Doda Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Pedimento Ruta").Value, "Pedimento Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("HC Ruta").Value, "HC Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Cuenta de Gastos Ruta").Value, "Cuenta de Gastos Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Simplificado1 Ruta").Value, "Simplificado1 Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Simplificado2 Ruta").Value, "Simplificado2 Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Archivo Val Ruta").Value, "Archivo Val Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Banco Ruta").Value, "Banco Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("FacturaXML Ruta").Value, "FacturaXML Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("FacturaPDF Ruta").Value, "FacturaPDF Valida")
                    ValidaDocumento(i, dgvDocumento.Rows(i).Cells("Acuse Ruta").Value, "Acuse Valida")


                    'bitCorrecto = False
                    'Dim fileName As String = dgvDocumento.Rows(i).Cells("Cove Ruta").Value
                    'Dim fi As New IO.FileInfo(fileName)
                    'Dim exists As Boolean = fi.Exists
                    'If fi.Exists Then
                    '    Dim size As Long = fi.Length
                    '    If size > 0 Then
                    '        bitCorrecto = True
                    '    End If
                    'End If

                    'If bitCorrecto = False Then
                    '    'dgvDocumento.Rows(i).DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#640000") 'Color.Red
                    '    'dgvDocumento.Rows(i).DefaultCellStyle.ForeColor = Color.White
                    '    dgvDocumento.Rows(i).Cells("Cove Valida").Value = "-NO-"
                    'End If
                Next
            End If

        Catch exc As Exception
            'Throw New GestorExcepcio(exc.Message)
            MsgBox(exc.Message)
        End Try






        'Try
        '    Dim bitCorrecto = True
        '    'Recorrer el grid
        '    If dgvDocumento.RowCount > 0 Then
        '        For i = 0 To dgvDocumento.NewRowIndex - 1
        '            bitCorrecto = False

        '            Dim fileName As String = dgvDocumento.Rows(i).Cells(2).Value
        '            Dim fi As New IO.FileInfo(fileName)
        '            Dim exists As Boolean = fi.Exists
        '            If fi.Exists Then
        '                Dim size As Long = fi.Length
        '                If size > 0 Then
        '                    bitCorrecto = True
        '                End If
        '            End If

        '            If bitCorrecto = False Then
        '                dgvDocumento.Rows(i).DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#640000") 'Color.Red
        '                dgvDocumento.Rows(i).DefaultCellStyle.ForeColor = Color.White
        '                dgvDocumento.Rows(i).Cells(3).Value = "- NO -"
        '            End If
        '        Next
        '    End If

        'Catch exc As Exception
        '    'Throw New GestorExcepcio(exc.Message)
        '    MsgBox(exc.Message)
        'End Try

    End Sub

    Private Sub ValidaDocumento(Idx As Int32, Ruta As String, Campo As String)
        Dim bitCorrecto = False
        Dim folder As String = ""
        Dim fileName As String = Ruta
        Dim fileNameArray() As String = Split(Ruta, "/")

        If Len(Ruta) = 0 Then
            Return
        End If

        For i = 0 To fileNameArray.Count - 1
            Dim fi As New IO.FileInfo(fileNameArray(i))
            If i = 0 Then
                folder = fi.DirectoryName & "\"
            End If
            Dim archivo As String = folder + fi.Name
            'Dim exists As Boolean = fi.Exists

            Dim fi2 As New IO.FileInfo(archivo)
            If fi2.Exists Then
                Dim size As Long = fi2.Length
                If size > 0 Then
                    bitCorrecto = True
                End If
            End If

            If bitCorrecto = False Then
                'dgvDocumento.Rows(i).DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#640000") 'Color.Red
                'dgvDocumento.Rows(i).DefaultCellStyle.ForeColor = Color.White
                dgvDocumento.Rows(Idx).Cells(Campo).Value = "-NO-"
            End If

        Next



    End Sub

    Private Sub btnCopiar_Click(sender As Object, e As EventArgs) Handles btnCopiar.Click
        Try
            If dgvDocumento.Rows.Count = 0 Then Exit Sub
            dgvDocumento.SuspendLayout()
            dgvDocumento.RowHeadersVisible = False
            'If dgvDocumento.SelectedRows.Count = 0 Then dgvDocumento.SelectAll()
            dgvDocumento.SelectAll()
            Clipboard.SetDataObject(dgvDocumento.GetClipboardContent())
            dgvDocumento.ClearSelection()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            dgvDocumento.RowHeadersVisible = True
            dgvDocumento.ResumeLayout()
        End Try
    End Sub
End Class
