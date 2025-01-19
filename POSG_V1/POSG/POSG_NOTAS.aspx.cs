using System;
using System.Data;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using POSG_V1.DataAccess;
using System.IO;
using System.Web;
using ClassLibraryTesis;

namespace POSG_V1.POSG
{
    public partial class POSG_NOTAS : System.Web.UI.Page
    {
        private NotasClass _notasDataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] != null)  
                {
                    string userId = Session["userId"].ToString();

                    USERINROLES ur = new USERINROLES();
                    lblUser.Text = userId;  

                    var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", "");

                    if (rolUser.Count == 1)  
                    {
                        _notasDataAccess = new NotasClass();
                        LlenarDropDownPeriodosAcademicos();
                        LlenarDropDownNombreMaestria();
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
            if (string.IsNullOrEmpty(periodoAcademico) || string.IsNullOrEmpty(nombreMaestria))
            {
                return;
            }
            CargarDatos(periodoAcademico, nombreMaestria);
        }

        private void LlenarDropDownPeriodosAcademicos()
        {
            if (_notasDataAccess == null)
            {
                _notasDataAccess = new NotasClass();
            }

            DataTable dt = _notasDataAccess.ObtenerPeriodosAcademicos();
            ddlPeriodoAcademico.DataSource = dt;
            ddlPeriodoAcademico.DataTextField = "strNombre_per";  
            ddlPeriodoAcademico.DataValueField = "strNombre_per";  
            ddlPeriodoAcademico.DataBind();
            ddlPeriodoAcademico.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Período Académico", ""));
        }

        private void LlenarDropDownNombreMaestria()
        {
            if (_notasDataAccess == null)
            {
                _notasDataAccess = new NotasClass();
            }

            DataTable dt = _notasDataAccess.ObtenerNombreMaestria();
            ddlNombreMaestria.DataSource = dt;
            ddlNombreMaestria.DataTextField = "strNombre_Car";  // Campo actualizado
            ddlNombreMaestria.DataValueField = "strNombre_Car";  // Campo actualizado
            ddlNombreMaestria.DataBind();
            ddlNombreMaestria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccionar Nombre de Maestría", ""));
        }

        private void CargarDatos(string periodoAcademico, string nombreMaestria)
        {
            if (_notasDataAccess == null)
            {
                _notasDataAccess = new NotasClass();
            }

            DataTable dt = _notasDataAccess.ObtenerNotas(periodoAcademico, nombreMaestria);
            tablaNota.DataSource = dt;
            tablaNota.DataBind();
        }

        public DataTable dtAlumno(string periodoAcademico, string nombreMaestria)
        {
            if (_notasDataAccess == null)
            {
                _notasDataAccess = new NotasClass();
            }

            return _notasDataAccess.ObtenerNotas(periodoAcademico, nombreMaestria);
        }

        protected void btnGenerarPDF_Click(object sender, EventArgs e)
        {
            string periodoAcademico = ddlPeriodoAcademico.SelectedValue;
            string nombreMaestria = ddlNombreMaestria.SelectedValue;

            if (string.IsNullOrEmpty(periodoAcademico) || string.IsNullOrEmpty(nombreMaestria))
            {
                string errorMessage = "Por favor seleccione ";
                if (string.IsNullOrEmpty(periodoAcademico) && string.IsNullOrEmpty(nombreMaestria))
                {
                    errorMessage += "un periodo académico y un nombre de maestría antes de generar el PDF.";
                }
                else if (string.IsNullOrEmpty(periodoAcademico))
                {
                    errorMessage += "un periodo académico antes de generar el PDF.";
                }
                else
                {
                    errorMessage += "un nombre de maestría antes de generar el PDF.";
                }

                string script = $@"<script>
                            Swal.fire({{
                                title: '¡Error!',
                                text: '{errorMessage}',
                                icon: 'error'
                            }});
                         </script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "NoFiltros", script, false);
                return;
            }

            DataTable dt = dtAlumno(periodoAcademico, nombreMaestria);

            if (dt.Rows.Count > 0)
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);

                document.Open();

                string imagePath = Server.MapPath("~/Images/logo/logotipo_UTC1.png");
                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
                    logo.ScaleToFit(105f, 105f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    document.Add(logo);
                }
                else
                {
                    document.Add(new Paragraph("Imagen no encontrada."));
                }

                Font fontTitle = FontFactory.GetFont(FontFactory.COURIER_BOLD, 20);
                Font fontHeader = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                Font font9 = FontFactory.GetFont(FontFactory.TIMES, 11);
                Font tablaTitulo = FontFactory.GetFont(FontFactory.TIMES_BOLD, 11);
                Font estilo1 = FontFactory.GetFont(FontFactory.TIMES_BOLD, 13);

                document.Add(new Paragraph("Universidad Técnica de Cotopaxi ", fontTitle) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10f });
                document.Add(new Paragraph($"Nombre de maestría: {nombreMaestria}", fontHeader));
                document.Add(new Paragraph($"Periodo Académico: {periodoAcademico}", fontHeader) { SpacingAfter = 2f });
                document.Add(new Paragraph($"REPORTE DE NOTAS ", estilo1) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Chunk("\n"));

                PdfPTable table = new PdfPTable(7);
                float[] widths = new float[] { 3f, 4f, 3f, 3f, 3f, 3f, 3f };
                table.SetWidths(widths);
                table.WidthPercentage = 100;

                BaseColor headerBackgroundColor = new BaseColor(148, 176, 248);

                table.AddCell(new PdfPCell(new Phrase("Identificación", tablaTitulo)) { BackgroundColor = headerBackgroundColor });
                table.AddCell(new PdfPCell(new Phrase("Nombres", tablaTitulo)) { BackgroundColor = headerBackgroundColor });
                table.AddCell(new PdfPCell(new Phrase("Apellido Paterno", tablaTitulo)) { BackgroundColor = headerBackgroundColor });
                table.AddCell(new PdfPCell(new Phrase("Apellido Materno", tablaTitulo)) { BackgroundColor = headerBackgroundColor });
                table.AddCell(new PdfPCell(new Phrase("Promedio Curricular", tablaTitulo)) { BackgroundColor = headerBackgroundColor });
                table.AddCell(new PdfPCell(new Phrase("Promedio Defensa", tablaTitulo)) { BackgroundColor = headerBackgroundColor });
                table.AddCell(new PdfPCell(new Phrase("Nota final Acta", tablaTitulo)) { BackgroundColor = headerBackgroundColor });

                foreach (DataRow r in dt.Rows)
                {
                    table.AddCell(new Phrase(r["strId_per"].ToString(), font9));
                    table.AddCell(new Phrase(r["strNomb_per"].ToString(), font9));
                    table.AddCell(new Phrase(r["strApellidop_per"].ToString(), font9));
                    table.AddCell(new Phrase(r["strApellidom_per"].ToString(), font9));
                    table.AddCell(new Phrase(r["decPromedioCurricular_not"].ToString(), font9));
                    table.AddCell(new Phrase(r["decPromedioDefensa_not"].ToString(), font9));
                    table.AddCell(new Phrase(r["decTotalActa_not"].ToString(), font9));
                }

                document.Add(table);
                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Calificaciones_de_estudiantes.pdf");
                HttpContext.Current.Response.Write(document);
                Response.Flush();
                Response.End();
            }
            else
            {
                string script = @"<script>
                            Swal.fire({
                                title: '¡Error!',
                                text: 'No se encontraron datos, use los filtros antes de generar el PDF.',
                                icon: 'error'
                            });
                         </script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "NoDatos", script, false);
            }
        }
    }
}
