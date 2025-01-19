using ClassLibraryTesis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSG_V1.POSG
{
    public partial class NotasJurados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si el usuario está logueado y tiene el rol adecuado
                if (Session["userId"] != null)
                {
                    string userId = Session["userId"].ToString();
                    lblUser.Text = userId; // Asignando el valor del userId al Label

                    USERINROLES ur = new USERINROLES();
                    var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", ""); // Usando lblUser.Text para la validación

                    if (rolUser.Count == 1)  // Validación de rol
                    {
                        string idActa = Request.QueryString["idActa"];
                        if (!string.IsNullOrEmpty(idActa))
                        {
                            hdnIdActa.Value = idActa;
                            CargarNotas(idActa);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/POSG/ErrorPage.aspx");  // Redirige si no tiene el rol adecuado
                    }
                }
                else
                {
                    // Si la sesión es nula, redirige al usuario a la página de inicio de sesión
                    Response.Redirect("/Login.aspx");
                }
            }
        }

        private void CargarNotas(string idActa)
        {
            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_Get_NotaJurado", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idActa", idActa);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblJurado1.InnerText = "Nota Jurado 1: " + reader["Jurado1"].ToString();
                            txtNotaJurado1.Text = reader["Nota1"].ToString();
                            lblJurado2.InnerText = "Nota Jurado 2: " + reader["Jurado2"].ToString();
                            txtNotaJurado2.Text = reader["Nota2"].ToString();
                            lblJurado3.InnerText = "Nota Jurado 3: " + reader["Jurado3"].ToString();
                            txtNotaJurado3.Text = reader["Nota3"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            string idActa = hdnIdActa.Value;
            decimal notaJurado1;
            decimal notaJurado2;
            decimal notaJurado3;

            // Validar la conversión de los textos a decimales
            if (!decimal.TryParse(txtNotaJurado1.Text, out notaJurado1) ||
                !decimal.TryParse(txtNotaJurado2.Text, out notaJurado2) ||
                !decimal.TryParse(txtNotaJurado3.Text, out notaJurado3))
            {
                // Manejar el caso donde la conversión falle
                lblError.Text = "Por favor ingrese notas válidas.";
                return;
            }

            string connectionString = "data source=.; database=TESIS; integrated security=SSPI";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("POSG_UPDATE_NOTA_Jurado", sqlConnection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idActa", idActa);
                    cmd.Parameters.AddWithValue("@notaJurado1", notaJurado1);
                    cmd.Parameters.AddWithValue("@notaJurado2", notaJurado2);
                    cmd.Parameters.AddWithValue("@notaJurado3", notaJurado3);

                    try
                    {
                        sqlConnection.Open();
                        cmd.ExecuteNonQuery();
                        // Mostrar SweetAlert
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "Swal.fire({title: 'Éxito', text: 'Notas actualizadas correctamente.', icon: 'success'}).then((result) => { window.location.href = 'POSG_NotasDefensa.aspx'; });", true);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error: " + ex.Message;
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
