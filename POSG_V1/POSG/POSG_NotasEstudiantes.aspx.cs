using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using ClassLibraryTesis;

namespace POSG_V1.POSG
{
    public partial class POSG_NotasEstudiantes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {

                    lblUser.Text = Session["userId"].ToString();


                    USERINROLES ur = new USERINROLES();
                    var rolUser = ur.LoadUSERINROLES("xPK", "ESTUDIANTE", lblUser.Text, "", "");

                    if (rolUser.Count == 1)  
                    {

                        CargarPeriodosAcademicos(lblUser.Text);
                        CargarMaestrias(lblUser.Text);
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

        private void CargarNotasConFiltro(string studentId, string periodoAcademico, string nombreMaestria)
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetNotasxEstudiante", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", string.IsNullOrEmpty(studentId) ? (object)DBNull.Value : studentId);
                    cmd.Parameters.AddWithValue("@PeriodoAcademico", string.IsNullOrEmpty(periodoAcademico) ? (object)DBNull.Value : periodoAcademico);
                    cmd.Parameters.AddWithValue("@NombreMaestria", string.IsNullOrEmpty(nombreMaestria) ? (object)DBNull.Value : nombreMaestria);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        // Eliminar duplicados si es necesario
                        DataView view = new DataView(dt);
                        DataTable distinctValues = view.ToTable(true, "strId_not", "strNomb_per", "strApellidop_per", "strApellidom_per", "strNombre_Car", "strId_per", "strCod_per", "decPromedioCurricular_not", "decPromedioDefensa_not", "decTotalActa_not", "strNombre_per", "strUrlActasFirmadas_act", "strId_ins");

                        tablaNota.DataSource = distinctValues;
                        tablaNota.DataBind();
                    }
                }
            }
        }

        private void CargarPeriodosAcademicos(string studentId)
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetNotasxEstudiante", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@PeriodoAcademico", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreMaestria", DBNull.Value);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlPeriodoAcademico.DataSource = reader;
                        ddlPeriodoAcademico.DataTextField = "strNombre_per";
                        ddlPeriodoAcademico.DataValueField = "strNombre_per";
                        ddlPeriodoAcademico.DataBind();

                        ddlPeriodoAcademico.Items.Insert(0, new ListItem("Selecciona el Período Académico", ""));
                    }
                }
            }
        }

        private void CargarMaestrias(string studentId)
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_GetNotasxEstudiante", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@PeriodoAcademico", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreMaestria", DBNull.Value);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlNombreMaestria.DataSource = reader;
                        ddlNombreMaestria.DataTextField = "strNombre_Car";
                        ddlNombreMaestria.DataValueField = "strNombre_Car";
                        ddlNombreMaestria.DataBind();

                        ddlNombreMaestria.Items.Insert(0, new ListItem("Selecciona la maestría", ""));
                    }
                }
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            string periodoAcademico = ddlPeriodoAcademico.SelectedValue;
            string nombreMaestria = ddlNombreMaestria.SelectedValue;
            string studentId = lblUser.Text;

            CargarNotasConFiltro(studentId, periodoAcademico, nombreMaestria);
        }
    }
}
