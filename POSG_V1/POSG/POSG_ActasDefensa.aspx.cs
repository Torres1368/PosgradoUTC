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
using WordTable = Microsoft.Office.Interop.Word.Table;
using System.Runtime.InteropServices;
using ClassLibraryTesis;

namespace POSG_V1.POSG
{
    public partial class POSG_ActasDefensa : System.Web.UI.Page
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
                    var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", ""); // Usando lblUser.Text para la validación

                    if (rolUser.Count == 1)  // Validación de rol
                    {
                        LlenarDropDownPeriodosAcademicos();
                        LlenarDropDownNombreMaestria();
                        LlenarDropDownNombreMecanismo();

                        // Configuración de filtros
                        if (Session["PeriodoAcademico"] != null)
                            ddlPeriodoAcademico.SelectedValue = Session["PeriodoAcademico"].ToString();

                        if (Session["NombreMaestria"] != null)
                            ddlNombreMaestria.SelectedValue = Session["NombreMaestria"].ToString();

                        if (Session["NombreMecanismo"] != null)
                            ddlNombreMecanismo.SelectedValue = Session["NombreMecanismo"].ToString();

                        if (Session["PresentaDocumentacion"] != null)
                            ddlPresentaDocumentacion.SelectedValue = Session["PresentaDocumentacion"].ToString();

                        if (Session["PeriodoAcademico"] != null || Session["NombreMaestria"] != null || Session["NombreMecanismo"] != null || Session["PresentaDocumentacion"] != null)
                        {
                            CargarDatos(Session["PeriodoAcademico"]?.ToString(),
                                        Session["NombreMaestria"]?.ToString(),
                                        Session["NombreMecanismo"]?.ToString(),
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
            string nombreMecanismo = ddlNombreMecanismo.SelectedValue;
            bool? presentaDocumentacion = string.IsNullOrEmpty(ddlPresentaDocumentacion.SelectedValue) ? (bool?)null : Convert.ToBoolean(ddlPresentaDocumentacion.SelectedValue);

            Session["PeriodoAcademico"] = periodoAcademico;
            Session["NombreMaestria"] = nombreMaestria;
            Session["NombreMecanismo"] = nombreMecanismo;
            Session["PresentaDocumentacion"] = presentaDocumentacion?.ToString();

            if (string.IsNullOrEmpty(periodoAcademico) || string.IsNullOrEmpty(nombreMaestria))
            {
                return;
            }

            CargarDatos(periodoAcademico, nombreMaestria, string.IsNullOrEmpty(nombreMecanismo) ? null : nombreMecanismo, presentaDocumentacion);
        }

        private void LlenarDropDownPeriodosAcademicos()
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
            string query = "SELECT DISTINCT strNombre_per FROM SIG_PERIODOS"; // Consulta actualizada

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlPeriodoAcademico.DataSource = reader;
                    ddlPeriodoAcademico.DataTextField = "strNombre_per";  // Campo actualizado
                    ddlPeriodoAcademico.DataValueField = "strNombre_per";  // Campo actualizado
                    ddlPeriodoAcademico.DataBind();

                    ddlPeriodoAcademico.Items.Insert(0, new ListItem("Seleccionar Período Académico", ""));
                    reader.Close();
                }
            }
        }

        private void LlenarDropDownNombreMaestria()
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

        private void LlenarDropDownNombreMecanismo()
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
            string query = "SELECT DISTINCT strMecanismo_ins FROM POSG_INSCRIPCION";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlNombreMecanismo.DataSource = reader;
                    ddlNombreMecanismo.DataTextField = "strMecanismo_ins";
                    ddlNombreMecanismo.DataValueField = "strMecanismo_ins";
                    ddlNombreMecanismo.DataBind();

                    ddlNombreMecanismo.Items.Insert(0, new ListItem("Selecciona el tipo", ""));
                    reader.Close();
                }
            }
        }

        private void CargarDatos(string periodoAcademico, string nombreMaestria, string nombreMecanismo, bool? presentaDocumentacion)
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetActasDefensa", sqlConnection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Comodin", "byPeriodo");
                    cmd.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                    cmd.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);
                    cmd.Parameters.AddWithValue("@FILTRO3", !presentaDocumentacion.HasValue ? DBNull.Value : (object)presentaDocumentacion.Value);
                    cmd.Parameters.AddWithValue("@FILTRO4", string.IsNullOrEmpty(nombreMecanismo) ? DBNull.Value : (object)nombreMecanismo);

                    try
                    {
                        sqlConnection.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();

                        tablaActa.DataSource = null;
                        tablaActa.DataBind();

                        if (sqlReader.HasRows)
                        {
                            Console.WriteLine("Datos encontrados.");
                            tablaActa.DataSource = sqlReader;
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron datos.");
                        }
                        tablaActa.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al cargar los datos: " + ex.Message);
                    }
                    finally
                    {
                        if (sqlConnection.State == System.Data.ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }
                    }
                }
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
                string pdfDirectory = Server.MapPath("~/Files/ACTAS_DEFENSA/Sin_firmar/");
                string zipFilePath = Server.MapPath("~/Files/ACTAS_DEFENSA/Sin_firmar/Actas_defensa.zip");

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

                // Actualizar la tabla después de generar las actas
                CargarDatos(ddlPeriodoAcademico.SelectedValue, ddlNombreMaestria.SelectedValue, ddlNombreMecanismo.SelectedValue, string.IsNullOrEmpty(ddlPresentaDocumentacion.SelectedValue) ? (bool?)null : Convert.ToBoolean(ddlPresentaDocumentacion.SelectedValue));

                string iframeDownloadScript = "document.getElementById('downloadIframe').src = '/Files/ACTAS_DEFENSA/Sin_firmar/Actas_defensa.zip';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "DownloadFile", iframeDownloadScript, true);

                Session["DownloadCompleted"] = true;
            }
            catch (Exception ex)
            {
                string script = $"Swal.fire('Error', 'Ocurrió un error al generar las Actas de defensa: {ex.Message}', 'error');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseAlertWithError", script, true);
            }
        }


        private List<StudentData> ObtenerDatosFiltrados()
        {
            List<StudentData> estudiantes = new List<StudentData>();
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
            string periodoAcademico = ddlPeriodoAcademico.SelectedValue;
            string nombreMaestria = ddlNombreMaestria.SelectedValue;
            string nombreMecanismo = ddlNombreMecanismo.SelectedValue;
            bool? presentaDocumentacion = string.IsNullOrEmpty(ddlPresentaDocumentacion.SelectedValue) ? (bool?)null : Convert.ToBoolean(ddlPresentaDocumentacion.SelectedValue);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetActasDefensa", sqlConnection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Comodin", "byPeriodo");
                    cmd.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                    cmd.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);
                    cmd.Parameters.AddWithValue("@FILTRO3", !presentaDocumentacion.HasValue ? DBNull.Value : (object)presentaDocumentacion.Value);
                    cmd.Parameters.AddWithValue("@FILTRO4", string.IsNullOrEmpty(nombreMecanismo) ? DBNull.Value : (object)nombreMecanismo);

                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        StudentData estudiante = new StudentData
                        {
                            Id = reader["strId_act"].ToString(),
                            NombreCompleto = $"{reader["strApellidop_per"]} {reader["strApellidom_per"]} {reader["strNomb_per"]}",
                            NombreMaestria = reader["strNombre_Car"].ToString(),
                            PeriodoAcademico = reader["strNombre_per"].ToString(),
                            Cedula = reader["strId_per"].ToString(),
                            Modalidad = reader["strModalidad_ins"].ToString(),
                            Total_acta = reader["decTotalActa_not"].ToString(),
                            Mecanismo = reader["strMecanismo_ins"].ToString().Trim(),
                            Tema = reader["strTema_ins"].ToString(),
                            InicialesJ1 = reader["InicialesJurado1"].ToString(),
                            InicialesJ2 = reader["InicialesJurado2"].ToString(),
                            InicialesJ3 = reader["InicialesJurado3"].ToString(),
                            InicialesTutor = reader["InicialesTutor"].ToString(),
                            NombreJurado1 = reader["Jurado1"].ToString(),
                            NombreJurado2 = reader["Jurado2"].ToString(),
                            NombreJurado3 = reader["Jurado3"].ToString(),
                            CedulaJurado1 = reader["CedJurado1"].ToString(),
                            CedulaJurado2 = reader["CedJurado2"].ToString(),
                            CedulaJurado3 = reader["CedJurado3"].ToString(),
                            Promedio_curricular = reader["decPromedioCurricular_not"].ToString(),
                            Tutor = reader["Tutor"].ToString()

                        };

                        estudiantes.Add(estudiante);
                    }
                }
            }

            return estudiantes;
        }


        private string GenerarCertificado(StudentData estudiante, string pdfDirectory, DateTime fecha, Application wordApp)
        {
            string templatePath;

            if (string.IsNullOrWhiteSpace(estudiante.Mecanismo))
            {
                throw new Exception("Mecanismo es nulo o vacío");
            }

            // condicion si es articulo cientifico o diferente de este
            switch (estudiante.Mecanismo.ToUpper())
            {
                case "M3":
                    templatePath = Server.MapPath("~/templates/Modelo_actas_defensa/articulo_cientifico.docx");
                    break;
                default: // Cualquier otro valor diferente de M3
                    templatePath = Server.MapPath("~/templates/Modelo_actas_defensa/informe_investigacion.docx");
                    break;
            }

            string pdfPath = Path.Combine(pdfDirectory, $"ACTA_DEFENSA_{estudiante.NombreCompleto}_{estudiante.Cedula}.pdf");

            Document wordDoc = wordApp.Documents.Open(templatePath);

            try
            {
                ReplaceText(wordDoc, "NOMBRE_ESTUDIANTE", estudiante.NombreCompleto);
                ReplaceText(wordDoc, "NOMBRE_MAESTRIA", estudiante.NombreMaestria);
                ReplaceText(wordDoc, "NUM_CEDULA", estudiante.Cedula);
                ReplaceText(wordDoc, "TOTAL_ACTA", estudiante.Total_acta);
                ReplaceText(wordDoc, "TIPO_MODALIDAD", estudiante.Modalidad);
                ReplaceText(wordDoc, "TOTAL_EN_LETRAS", NumeroALetras(Convert.ToDecimal(estudiante.Total_acta)));
                ReplaceText(wordDoc, "ESCALA_CALIFICACION", ObtenerEscalaCalificacion(Convert.ToDecimal(estudiante.Total_acta)));
                ReplaceText(wordDoc, "TEMA_MAESTRIA", estudiante.Tema);


                ReplaceText(wordDoc, "INICIALES_J1", estudiante.InicialesJ1);
                ReplaceText(wordDoc, "INICIALES_J2", estudiante.InicialesJ2);
                ReplaceText(wordDoc, "INICIALES_J3", estudiante.InicialesJ3);
                ReplaceText(wordDoc, "INICIALES_TUTOR", estudiante.InicialesTutor);


                ReplaceText(wordDoc, "NOMBRE_JURADO1", estudiante.NombreJurado1);
                ReplaceText(wordDoc, "NOMBRE_JURADO2", estudiante.NombreJurado2);
                ReplaceText(wordDoc, "NOMBRE_JURADO3", estudiante.NombreJurado3);
                ReplaceText(wordDoc, "CEDULA_JURADO1", estudiante.CedulaJurado1);
                ReplaceText(wordDoc, "CEDULA_JURADO2", estudiante.CedulaJurado2);
                ReplaceText(wordDoc, "CEDULA_JURADO3", estudiante.CedulaJurado3);
                ReplaceText(wordDoc, "NOMBRE_TUTOR", estudiante.Tutor);
                string fechaFormateada = FormatearFecha(fecha);
                ReplaceText(wordDoc, "FECHA_ACTA", fechaFormateada);
                string fechaEnPalabras = FormatearFechaEnPalabras(fecha);
                ReplaceText(wordDoc, "fecha_en_palabras ", fechaEnPalabras);
                ColocarNotaEnColumnaCorrespondiente(wordDoc, estudiante.Promedio_curricular);

                wordDoc.ExportAsFixedFormat(pdfPath, WdExportFormat.wdExportFormatPDF);

                GuardarRutaArchivo(estudiante.Id, pdfPath);
            }
            finally
            {
                wordDoc.Close(false);
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


        private void ColocarNotaEnColumnaCorrespondiente(Document doc, string promedioCurricular)
        {

            decimal nota;

            // Convertir el string a decimal y validar
            if (decimal.TryParse(promedioCurricular, out nota))
            {
                // Determinar el marcador basado en la nota
                string marcador = DeterminarMarcador(nota);

                // Verificar si el marcador existe
                bool marcadorExiste = false;
                foreach (Bookmark bookmark in doc.Bookmarks)
                {
                    if (bookmark.Name == marcador)
                    {
                        marcadorExiste = true;
                        break;
                    }
                }

                if (marcadorExiste)
                {
                    var bookmark = doc.Bookmarks[marcador];
                    var range = bookmark.Range;

                    // Reemplazar el texto en el marcador
                    range.Text = nota.ToString("0.00");

                    // Actualizar el marcador después de cambiar el texto
                    doc.Bookmarks.Add(marcador, range);
                }
                else
                {
                    throw new ArgumentException($"El marcador {marcador} no existe en el documento.");
                }
            }
            else
            {
                throw new ArgumentException("El promedio curricular no es un valor numérico válido.");
            }
        }

        private string DeterminarMarcador(decimal nota)
        {
            if (nota >= 9.00m)
                return "NOTA_EXCELENTE1";
            else if (nota >= 8.00m)
                return "NOTA_MUY_BUENA1";
            else if (nota >= 7.50m)
                return "NOTA_BUENA1";
            else if (nota >= 7.00m)
                return "NOTA_REGULAR1";
            else
                return "NOTA_DEFICIENTE1";
        }



        private void CrearArchivoZip(List<string> filePaths, string zipFilePath)
        {
            using (FileStream fsOut = File.Create(zipFilePath))
            using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
            {
                zipStream.SetLevel(9); 

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

                        byte[] buffer = new byte[16384];
                        using (FileStream streamReader = File.OpenRead(filePath))
                        {
                            StreamUtils.Copy(streamReader, zipStream, buffer);
                        }
                        zipStream.CloseEntry();
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {filePath}");
                    }
                }

                zipStream.IsStreamOwner = true;
                zipStream.Close();
            }
        }

        private void GuardarRutaArchivo(string studentId, string filePath)
        {
            string relativePath = filePath.Replace(Server.MapPath("~/"), "/").Replace("\\", "/");
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_UPDATE_ACTAS_DEFENSA_SIN_FIRMAR", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@strId_act", studentId);
                    cmd.Parameters.AddWithValue("@UrlArchivo_act", relativePath);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
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
            public string Promedio_curricular { get; set; }
            public string Mecanismo { get; set; }
            public string Tema { get; set; }
            public string NombreJurado1 { get; set; }
            public string NombreJurado2 { get; set; }
            public string NombreJurado3 { get; set; }
            public string CedulaJurado1 { get; set; }
            public string CedulaJurado2 { get; set; }
            public string CedulaJurado3 { get; set; }
            public string Tutor { get; set; }
            public string InicialesJ1 { get; set; }
            public string InicialesJ2 { get; set; }
            public string InicialesJ3 { get; set; }
            public string InicialesTutor { get; set; }
        }

        protected void btnSubirActaFirmada_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string idActa = btn.CommandArgument;
            Response.Redirect("ActaDefensaFirmada.aspx?idActa=" + idActa);
        }

        public string NumeroALetras(decimal value)
        {
            string[] unitsMap = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez" };
            string[] tensMap = { "cero", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
            string[] teensMap = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };

            if (value < 0)
                return "menos " + NumeroALetras(Math.Abs(value));
            if (value == 0)
                return "cero";
            if (value < 11)
                return unitsMap[(int)value];
            if (value < 20)
                return teensMap[(int)value - 10];
            if (value < 100)
                return tensMap[(int)(value / 10)] + ((value % 10 > 0) ? " y " + NumeroALetras(value % 10) : "");

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

            return "SIN ESCALA DEFINIDA";
        }

        private string FormatearFecha(DateTime fecha)
        {
            string dia = fecha.Day.ToString("00");
            string mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
            string año = fecha.Year.ToString();

            return $"{dia} de {mes} de {año}";
        }

        //fecha en palabras 
        private string FormatearFechaEnPalabras(DateTime fecha)
        {
            string dia = NumeroALetras(fecha.Day);
            string mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
            string año = NumeroALetras(fecha.Year);

            return $"a los {dia} días del mes de {mes} del año {año}";
        }

    }
}
