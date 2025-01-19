using System;
using System.Data.SqlClient;
using System.Web.UI;
using ClassLibraryTesis;
namespace POSG_V1
{
    public partial class login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                POSG_PERSONA per = new POSG_PERSONA();
                var listPer = per.LoadPOSG_PERSONA("xPK", txtusuario.Text, "", "", "");


                if (listPer.Count == 1)
                {

                    string userId = listPer[0].strid_per;

                    if (txtpassword.Text == userId)
                    {

                        Session["userId"] = userId;

                        // Guardar la URL de la imagen en la sesión
                        Session["userImage"] = listPer[0].strimg_per;

                        Session["userName"] = listPer[0].strnomb_per;
                        Session["userLastName"] = listPer[0].strapellidop_per;

                        Response.Redirect("/POSG/Menu.aspx");
                    }
                    else
                    {
                        lblError.Text = "Credenciales incorrectas";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Credenciales incorrectas";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

    }
}