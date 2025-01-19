using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;
using ClassLibraryTesis;
using System;

namespace POSG_V1.POSG
{
    public partial class POSG_NotasDefensa : System.Web.UI.Page
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
                            CargarDatos(Session["PeriodoAcademico"]?.ToString(), Session["NombreMaestria"]?.ToString(), Session["PresentaDocumentacion"] != null ? (bool?)Convert.ToBoolean(Session["PresentaDocumentacion"]) : null);
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

            // Guardar los filtros en la sesión
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

        private void CargarDatos(string periodoAcademico, string nombreMaestria, bool? presentaDocumentacion)
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetActas", sqlConnection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (string.IsNullOrEmpty(periodoAcademico) && string.IsNullOrEmpty(nombreMaestria) && !presentaDocumentacion.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@Comodin", "ALL");
                        cmd.Parameters.AddWithValue("@FILTRO1", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FILTRO2", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FILTRO3", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Comodin", "byPeriodo");
                        cmd.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                        cmd.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);
                        cmd.Parameters.AddWithValue("@FILTRO3", !presentaDocumentacion.HasValue ? DBNull.Value : (object)presentaDocumentacion.Value);
                    }

                    try
                    {
                        sqlConnection.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();

                        tablaActa.DataSource = null;
                        tablaActa.DataBind();

                        tablaActa.DataSource = sqlReader;
                        tablaActa.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
    }
}