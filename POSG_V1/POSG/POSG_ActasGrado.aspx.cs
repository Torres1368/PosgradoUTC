    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.Office.Interop.Word;
    using Application = Microsoft.Office.Interop.Word.Application;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Core;
    using System.Web;
    using ClassLibraryTesis;

    namespace POSG_V1.POSG
    {
        public partial class POSG_ActasGrado : System.Web.UI.Page
        {
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {

                    if (Session["userId"] != null)
                    {
                        string userId = Session["userId"].ToString();
                        lblUser.Text = userId; 

                        USERINROLES ur = new USERINROLES();
                        var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", ""); 

                        if (rolUser.Count == 1) 
                        {
                            LlenarDropDownPeriodosAcademicos();
                            LlenarDropDownNombreMaestria();

                            if (Session["PeriodoAcademico"] != null)
                            {
                                ddlPeriodoAcademico.SelectedValue = Session["PeriodoAcademico"].ToString();
                            }

                            if (Session["NombreMaestria"] != null)
                            {
                                ddlNombreMaestria.SelectedValue = Session["NombreMaestria"].ToString();
                            }

                            if (Session["PresentaDocumentacion"] != null)
                            {
                                ddlPresentaDocumentacion.SelectedValue = Session["PresentaDocumentacion"].ToString();
                            }

                            if (Session["PeriodoAcademico"] != null || Session["NombreMaestria"] != null || Session["PresentaDocumentacion"] != null)
                            {
                                CargarDatos(Session["PeriodoAcademico"]?.ToString(),
                                            Session["NombreMaestria"]?.ToString(),
                                            Session["PresentaDocumentacion"] != null ? (bool?)Convert.ToBoolean(Session["PresentaDocumentacion"]) : null);
                            }


                            if (Session["DownloadCompleted"] == null)
                            {
                                Session["DownloadCompleted"] = false;
                            }
                        }
                        else
                        {
                            Response.Redirect("~/POSG/ErrorPage.aspx");  
                        }
                    }
                    else
                    {

                        Response.Redirect("/Login.aspx");
                    }
                }
            }

            protected void btnFiltrar_Click(object sender, EventArgs e)
            {
                string periodoAcademico = ddlPeriodoAcademico.SelectedValue;
                string nombreMaestria = ddlNombreMaestria.SelectedValue;
                bool? presentaDocumentacion = string.IsNullOrEmpty(ddlPresentaDocumentacion.SelectedValue) ? (bool?)null : Convert.ToBoolean(ddlPresentaDocumentacion.SelectedValue);

            
                Session["PeriodoAcademico"] = periodoAcademico;
                Session["NombreMaestria"] = nombreMaestria;
                Session["PresentaDocumentacion"] = presentaDocumentacion?.ToString();

                if (string.IsNullOrEmpty(periodoAcademico) || string.IsNullOrEmpty(nombreMaestria))
                {
                    return;
                }

                CargarDatos(periodoAcademico, nombreMaestria, presentaDocumentacion);
            }

            private void LlenarDropDownPeriodosAcademicos()
            {
                try
                {
                    string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
                    string query = "SELECT DISTINCT strNombre_per FROM SIG_PERIODOS";

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            sqlConnection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            ddlPeriodoAcademico.DataSource = reader;
                            ddlPeriodoAcademico.DataTextField = "strNombre_per";
                            ddlPeriodoAcademico.DataValueField = "strNombre_per";
                            ddlPeriodoAcademico.DataBind();

                            ddlPeriodoAcademico.Items.Insert(0, new ListItem("Seleccionar Período Académico", ""));
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error al llenar el dropdown de periodos académicos: " + ex.Message);
                }
            }

            private void LlenarDropDownNombreMaestria()
            {
                try
                {
                    string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
                    string query = "SELECT DISTINCT strNombre_Car FROM UB_CARRERAS";

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                        {
                            sqlConnection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            ddlNombreMaestria.DataSource = reader;
                            ddlNombreMaestria.DataTextField = "strNombre_Car";
                            ddlNombreMaestria.DataValueField = "strNombre_Car";
                            ddlNombreMaestria.DataBind();

                            ddlNombreMaestria.Items.Insert(0, new ListItem("Seleccionar Nombre de Maestría", ""));
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error al llenar el dropdown de nombres de maestría: " + ex.Message);
                }
            }

            private void CargarDatos(string periodoAcademico, string nombreMaestria, bool? presentaDocumentacion)
            {
                try
                {
                    string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("POSG_GetActas", sqlConnection))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Comodin", "byPeriodo");
                            cmd.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                            cmd.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);
                            cmd.Parameters.AddWithValue("@FILTRO3", !presentaDocumentacion.HasValue ? DBNull.Value : (object)presentaDocumentacion.Value);

                            sqlConnection.Open();
                            SqlDataReader sqlReader = cmd.ExecuteReader();

                            tablaActa.DataSource = null;
                            tablaActa.DataBind();

                            if (sqlReader.HasRows)
                            {
                                tablaActa.DataSource = sqlReader;
                            }
                            tablaActa.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error al cargar los datos: " + ex.Message);
                }
            }

            protected void btnGenerarCertificado_Click(object sender, EventArgs e)
            {
                try
                {
                    Session["DownloadCompleted"] = false;
                    string fechaSeleccionada = Request.Form["fechaSeleccionada"];
                    DateTime fecha;

                    if (!DateTime.TryParse(fechaSeleccionada, out fecha))
                    {
                        lblError.Text = "Por favor seleccione una fecha válida.";
                        return;
                    }

                    DateTime fechaActual = DateTime.Now;
                    if (fecha < fechaActual.AddDays(-3) || fecha > fechaActual.AddDays(3))
                    {
                        lblError.Text = "La fecha debe estar dentro de los 3 días anteriores o posteriores a la fecha actual.";
                        return;
                    }

                    List<StudentData> estudiantes = ObtenerDatosFiltrados();
                    string pdfDirectory = Server.MapPath("~/Files/ACTAS_GRADO/Sin_firmar");
                    string zipFilePath = Server.MapPath("~/Files/ACTAS_GRADO/Sin_firmar/Actas_grado.zip");

                    if (!Directory.Exists(pdfDirectory))
                    {
                        Directory.CreateDirectory(pdfDirectory);
                    }

                    List<string> generatedFiles = new List<string>();

                    Application wordApp = new Application();

                    foreach (var estudiante in estudiantes)
                    {
                        string pdfPath = GenerarCertificado(estudiante, pdfDirectory, fecha, wordApp);
                        generatedFiles.Add(pdfPath);
                    }

                    wordApp.Quit();

                    CrearArchivoZip(generatedFiles, zipFilePath);

                    // Actualizar la tabla después de generar los certificados
                    CargarDatos(ddlPeriodoAcademico.SelectedValue, ddlNombreMaestria.SelectedValue, string.IsNullOrEmpty(ddlPresentaDocumentacion.SelectedValue) ? (bool?)null : Convert.ToBoolean(ddlPresentaDocumentacion.SelectedValue));

                    string iframeDownloadScript = "document.getElementById('downloadIframe').src = '/Files/ACTAS_GRADO/Sin_firmar/Actas_grado.zip';";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "DownloadFile", iframeDownloadScript, true);

                    Session["DownloadCompleted"] = true;
                }
                catch (Exception ex)
                {
                    LogError("Error al generar los certificados: " + ex.Message);
                    string script = $"Swal.fire('Error', 'Ocurrió un error al generar los certificados: {ex.Message}', 'error');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseAlertWithError", script, true);
                }
            }

        private List<StudentData> ObtenerDatosFiltrados()
        {
            List<StudentData> estudiantes = new List<StudentData>();
            try
            {
                string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
                string periodoAcademico = ddlPeriodoAcademico.SelectedValue;
                string nombreMaestria = ddlNombreMaestria.SelectedValue;
                bool? presentaDocumentacion = string.IsNullOrEmpty(ddlPresentaDocumentacion.SelectedValue) ? (bool?)null : Convert.ToBoolean(ddlPresentaDocumentacion.SelectedValue);

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("POSG_GetActas", sqlConnection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Comodin", "byPeriodo");
                        cmd.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                        cmd.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);
                        cmd.Parameters.AddWithValue("@FILTRO3", !presentaDocumentacion.HasValue ? DBNull.Value : (object)presentaDocumentacion.Value);

                        sqlConnection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            decimal totalActa = Convert.ToDecimal(reader["decTotalActa_not"]);
                            if (totalActa >= 7)
                            {
                                StudentData estudiante = new StudentData
                                {
                                    Id = reader["strId_act"].ToString(),
                                    NombreCompleto = $"{reader["strApellidop_per"]} {reader["strApellidom_per"]} {reader["strNomb_per"]}",
                                    NombreMaestria = reader["strNombre_Car"].ToString(),
                                    PeriodoAcademico = reader["strNombre_per"].ToString(),
                                    Cedula = reader["strId_per"].ToString(),
                                    Modalidad = reader["strModalidad_ins"].ToString(),
                                    Total_acta = totalActa.ToString()
                                };
                                estudiantes.Add(estudiante);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Error al obtener los datos filtrados: " + ex.Message);
            }

            return estudiantes;
        }


        private string EliminarPalabraMaestria1(string nombreMaestria)//esto es para eliminar la palabra maestria ya que en la parte de magister en actas de grado no debe ir esa palabra
            {
                // Verifica si el nombre de la maestría empieza con "MAESTRÍA"
                int index = nombreMaestria.IndexOf("MAESTRÍA", StringComparison.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    // Elimina "MAESTRÍA" de la cadena
                    return nombreMaestria.Substring(index + "MAESTRÍA".Length).Trim();
                }

                return nombreMaestria;
            }

            private string EliminarPalabraMaestria(string nombreMaestria)//esto es para eliminar la palabra maestria ya que en la parte de magister en actas de grado no debe ir esa palabra
            {
                string palabraEliminar = "MAESTRÍA EN";
                int index = nombreMaestria.IndexOf(palabraEliminar, StringComparison.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    // Eliminamos "MAESTRÍA EN" y cualquier espacio que le siga
                    string resultado = nombreMaestria.Substring(index + palabraEliminar.Length).Trim();
                    return resultado;
                }

                return nombreMaestria;
            }



            private string GenerarCertificado(StudentData estudiante, string pdfDirectory, DateTime fecha, Application wordApp)
            {
                string pdfPath = string.Empty;
                Document wordDoc = null;

                try
                {
                    string templatePath = Server.MapPath("~/templates/Modelo_actas/ACTA_DE_GRADO_PLANTILLA.docx");
                    pdfPath = Path.Combine(pdfDirectory, $"ACTA_GRADO_{estudiante.NombreCompleto}_{estudiante.Cedula}.pdf");

                    wordDoc = wordApp.Documents.Open(templatePath);

                    ReplaceText(wordDoc, "NOMBRES_COMPLETOS", estudiante.NombreCompleto);

                    // Eliminar "MAESTRÍA" de la cadena antes de reemplazar en el documento
                    string nombreMaestriaSinMaestria = EliminarPalabraMaestria(estudiante.NombreMaestria);
                    ReplaceText(wordDoc, "NOMBRE_MAESTRIA", nombreMaestriaSinMaestria);


                    ReplaceText(wordDoc, "NUM_CEDULA", estudiante.Cedula);
                    ReplaceText(wordDoc, "TOTAL_ACTA", estudiante.Total_acta);
                    ReplaceText(wordDoc, "TIPO_MODALIDAD", estudiante.Modalidad);
                    ReplaceText(wordDoc, "Total_en_letras", NumeroALetras(Convert.ToDecimal(estudiante.Total_acta)));
                    ReplaceText(wordDoc, "ESCALA_CALIFICACION", ObtenerEscalaCalificacion(Convert.ToDecimal(estudiante.Total_acta)));
                    string fechaFormateada = FormatearFecha(fecha);
                    ReplaceText(wordDoc, "FECHA_ACTA", fechaFormateada);
                    string fechaEnPalabras = FormatearFechaEnPalabrasSinPuntos(fecha);
                    ReplaceText(wordDoc, "FECHA_EN_PALABRAS", fechaEnPalabras);



                    wordDoc.ExportAsFixedFormat(pdfPath, WdExportFormat.wdExportFormatPDF);

                    GuardarRutaArchivo(estudiante.Id, pdfPath);
                }
                catch (Exception ex)
                {
                    LogError("Error al generar el certificado para " + estudiante.NombreCompleto + ": " + ex.Message);
                }
                finally
                {
                    if (wordDoc != null)
                    {
                        wordDoc.Close(false);
                    }
                }

                return pdfPath;
            }

            private void ReplaceText(Document doc, string findText, string replaceText)
            {
                foreach (Range range in doc.StoryRanges)
                {
                    Find find = range.Find;
                    find.Text = findText;
                    find.Replacement.Text = replaceText;
                    find.Execute(Replace: WdReplace.wdReplaceAll);
                }
            }

            private void CrearArchivoZip(List<string> filePaths, string zipFilePath)
            {
                try
                {
                    using (FileStream fsOut = File.Create(zipFilePath))
                    using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
                    {
                        zipStream.SetLevel(6); // Nivel de compresión ajustado para un mejor balance entre velocidad y tamaño

                        foreach (string filePath in filePaths)
                        {
                            if (File.Exists(filePath))
                            {
                                FileInfo fi = new FileInfo(filePath);

                                string entryName = ZipEntry.CleanName(fi.Name);
                                ZipEntry newEntry = new ZipEntry(entryName)
                                {
                                    DateTime = fi.LastWriteTime,
                                    Size = fi.Length
                                };

                                zipStream.PutNextEntry(newEntry);

                                byte[] buffer = new byte[8192];
                                using (FileStream streamReader = File.OpenRead(filePath))
                                {
                                    int sourceBytes;
                                    while ((sourceBytes = streamReader.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        zipStream.Write(buffer, 0, sourceBytes);
                                    }
                                }
                                zipStream.CloseEntry();
                            }
                            else
                            {
                                LogError("File not found: " + filePath);
                            }
                        }

                        zipStream.IsStreamOwner = true;
                        zipStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error al crear el archivo ZIP: " + ex.Message);
                }
            }

            private void GuardarRutaArchivo(string studentId, string filePath)
            {
                try
                {
                    string relativePath = filePath.Replace(Server.MapPath("~/"), "/").Replace("\\", "/");
                    string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("POSG_UPDATE_URL_ACTAS_SIN_FIRMAR", connection))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@strId_act", studentId);
                            cmd.Parameters.AddWithValue("@UrlArchivo_act", relativePath);

                            connection.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error al guardar la ruta del archivo: " + ex.Message);
                }
            }

            private void LogError(string message)
            {
                // Aquí podrías guardar el mensaje en un archivo de log, en una base de datos o mostrarlo en pantalla.
                System.Diagnostics.Debug.WriteLine(message);
                // También puedes usar:
                // File.AppendAllText(Server.MapPath("~/Logs/errors.log"), message + Environment.NewLine);
            }

            public class StudentData
            {
                public string Id { get; set; }
                public string NombreCompleto { get; set; }
                public string NombreMaestria { get; set; }
                public string PeriodoAcademico { get; set; }
                public string Cedula { get; set; }
                public string Modalidad { get; set; }
                public string Total_acta { get; set; }
            }

            protected void btnSubirActaFirmada_Click(object sender, EventArgs e)
            {
                Button btn = (Button)sender;
                string idActa = btn.CommandArgument;
                Response.Redirect("ActaGradoFirmada.aspx?idActa=" + idActa);
            }

            public string NumeroALetras(decimal value)
            {
                string[] unitsMap = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez" };
                string[] tensMap = { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
                string[] teensMap = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };

                if (value < 0)
                    return "menos " + NumeroALetras(Math.Abs(value));

                int entero = (int)value;
                int decimales = (int)((value - entero) * 100);

                string letrasEntero = NumeroALetrasEntero(entero);
                string letrasDecimales = NumeroALetrasEntero(decimales);

                if (decimales == 0)
                {
                    return $"{letrasEntero} puntos";
                }

                return $"{letrasEntero} puntos con {letrasDecimales} centésimas";
            }

            private string NumeroALetrasEntero(int value)
            {
                string[] unitsMap = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez" };
                string[] tensMap = { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
                string[] teensMap = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };

                if (value < 11)
                    return unitsMap[value];
                if (value < 20)
                    return teensMap[value - 10];
                if (value < 100)
                    return tensMap[value / 10] + ((value % 10 > 0) ? " y " + unitsMap[value % 10] : "");

                return "";
            }

            public string ObtenerEscalaCalificacion(decimal value)
            {
                if (value >= 9.00m && value <= 10.00m)
                    return "EXCELENTE";
                if (value >= 8.00m && value <= 8.99m)
                    return "MUY BUENO";
                if (value >= 7.50m && value <= 7.99m)
                    return "BUENO";
                if (value >= 7.00m && value <= 7.49m)
                    return "REGULAR";

                return "DEFICIENTE";
            }

            private string FormatearFecha(DateTime fecha)
            {
                string dia = fecha.Day.ToString("00");
                string mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                string año = fecha.Year.ToString();

                return $"{dia} de {mes} de {año}";
            }



            //para convertir la fecha en palabras con diferente gramatica 


            private string FormatearFechaEnPalabrasSinPuntos(DateTime fecha)
            {
                string dia = NumeroALetrasSinPuntos(fecha.Day);
                string mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                string año = NumeroALetrasSinPuntos(fecha.Year);

                return $"a los {dia} días del mes de {mes} del año {año}";
            }

            private string NumeroALetrasSinPuntos(int numero)
            {
                string[] unidades = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
                string[] especiales = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
                string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };

                if (numero < 10)
                    return unidades[numero];
                else if (numero < 20)
                    return especiales[numero - 10];
                else if (numero < 100)
                    return decenas[numero / 10] + ((numero % 10 > 0) ? " y " + unidades[numero % 10] : "");
                else if (numero < 1000)
                    return (numero / 100 == 1 ? "cien" : unidades[numero / 100] + "cientos") + ((numero % 100 > 0) ? " " + NumeroALetrasSinPuntos(numero % 100) : "");
                else if (numero < 10000)
                    return unidades[numero / 1000] + " mil " + ((numero % 1000 > 0) ? NumeroALetrasSinPuntos(numero % 1000) : "");

                return numero.ToString();
            }



        }
    }
