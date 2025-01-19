using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibraryTesis;

namespace POSG_V1.POSG
{
    public partial class POSG_DOCENTE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si Session["userId"] no es null antes de convertir a string
                if (Session["userId"] != null)
                {
                    string userId = Session["userId"].ToString();

                    // Continuar con tu lógica solo si userId no es null y no está vacío
                    if (!string.IsNullOrEmpty(userId))
                    {
                        USERINROLES ur = new USERINROLES();

                        lblUser.Text = userId;
                        var rolUser = ur.LoadUSERINROLES("xPK", "DOCENTE", lblUser.Text, "", "");

                        if (rolUser.Count == 1)
                        {
                            lblMsg.Text = "Bienvenido, rol docente";
                        }
                        else
                        {
                            Response.Redirect("~/POSG/ErrorPage.aspx");
                        }
                    }
                    else
                    {
                        // Redirigir si el userId está vacío
                        Response.Redirect("/Login.aspx");
                    }
                }
                else
                {
                    // Redirigir si Session["userId"] es null
                    Response.Redirect("/Login.aspx");
                }
            }
        }
    }
}
