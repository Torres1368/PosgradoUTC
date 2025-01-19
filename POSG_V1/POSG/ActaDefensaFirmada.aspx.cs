using ClassLibraryTesis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSG_V1.POSG
{
    public partial class ActaDefensaFirmada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si el usuario está logueado y tiene el rol adecuado
                if (Session["userId"] != null)
                {
                    string userId = Session["userId"].ToString();
                    lblUser.Text = userId; // Asigna el valor del userId al Label

                    USERINROLES ur = new USERINROLES();
                    var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", ""); // Usando lblUser.Text

                    if (rolUser.Count == 1)  // Validación de rol
                    {
                        string idActa = Request.QueryString["idActa"];
                        if (!string.IsNullOrEmpty(idActa))
                        {
                            hdnIdActa.Value = idActa;
                        }
                        else
                        {
                            Response.Redirect("POSG_ActasDefensa.aspx");
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                string idActa = hdnIdActa.Value;
                string folderPath = Server.MapPath("~/Files/ACTAS_DEFENSA/Firmadas/");
                string fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                // Crear el directorio si no existe
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Guardar el archivo
                fileUpload.SaveAs(filePath);

                // Actualizar la base de datos con la URL del archivo
                string relativePath = "/Files/ACTAS_DEFENSA/Firmadas/" + fileName;
                string connectionString = "data source=.; database=TESIS; integrated security=SSPI";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("POSG_UPDATE_ACTA_DEFENSA_FIRMADA", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@strId_act", idActa);
                        cmd.Parameters.AddWithValue("@UrlArchivo_act", relativePath);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Redirigir a la página Actas_defensa.aspx para que la tabla se actualice
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "Swal.fire({title: 'Éxito', text: 'El archivo ha sido subido y actualizado correctamente.', icon: 'success'}).then((result) => { window.location.href = 'POSG_ActasDefensa.aspx'; });", true);

            }
            else
            {
                // Mostrar SweetAlert de error
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "Swal.fire({title: 'Error', text: 'Por favor, seleccione un archivo para subir.', icon: 'error'});", true);
            }
        }
    }
}
