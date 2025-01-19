using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace POSG_V1.Pages
{
    public partial class Notas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LlenarDropDownPeriodosAcademicos();
                LlenarDropDownNombreMaestria();
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
            string connectionString = "data source=.; database=Titulacion; integrated security=SSPI";
            string query = "SELECT DISTINCT strPeriodoAcademico_not FROM POSG_NOTAS";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlPeriodoAcademico.DataSource = reader;
                    ddlPeriodoAcademico.DataTextField = "strPeriodoAcademico_not";
                    ddlPeriodoAcademico.DataValueField = "strPeriodoAcademico_not";
                    ddlPeriodoAcademico.DataBind();

                    ddlPeriodoAcademico.Items.Insert(0, new ListItem("Seleccionar Período Académico", ""));
                    reader.Close();
                }
            }
        }

        private void LlenarDropDownNombreMaestria()
        {
            string connectionString = "data source=.; database=Titulacion; integrated security=SSPI";
            string query = "SELECT DISTINCT strNombreMaestria_ins FROM POSG_INSCRIPCION";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlNombreMaestria.DataSource = reader;
                    ddlNombreMaestria.DataTextField = "strNombreMaestria_ins";
                    ddlNombreMaestria.DataValueField = "strNombreMaestria_ins";
                    ddlNombreMaestria.DataBind();

                    ddlNombreMaestria.Items.Insert(0, new ListItem("Seleccionar Nombre de Maestría", ""));
                    reader.Close();
                }
            }
        }

        private void CargarDatos(string periodoAcademico, string nombreMaestria)
        {
            string connectionString = "data source=.; database=Titulacion; integrated security=SSPI";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetNotas1", sqlConnection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (string.IsNullOrEmpty(periodoAcademico) && string.IsNullOrEmpty(nombreMaestria))
                    {
                        cmd.Parameters.AddWithValue("@Comodin", "ALL");
                        cmd.Parameters.AddWithValue("@FILTRO1", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FILTRO2", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Comodin", "byPeriodo");
                        cmd.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                        cmd.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);
                    }

                    try
                    {
                        sqlConnection.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();
                        tablaNota.DataSource = sqlReader;
                        tablaNota.DataBind();
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