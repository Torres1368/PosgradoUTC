using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibraryTesis;
namespace POSG_V1.POSG
{
    public partial class POSG_DetalleIns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {
                    USERINROLES ur = new USERINROLES();
                    divDetalles.Visible = false;
                    lblUser.Text = Session["UserId"].ToString();
                    var rolUser = ur.LoadUSERINROLES("xPK", "ESTUDIANTE", lblUser.Text, "", "");

                    if (rolUser.Count == 1)
                    {
                        GetInscritos(lblUser.Text);
                    }
                    else
                    {
                        Response.Redirect("~/POSG/ErrorPage.aspx");
                    }

                    gvwMatriculados.SelectedIndex = -1;
                }
                else
                {
                    // Si la sesión es nula, redirige al usuario a la página de inicio de sesión
                    Response.Redirect("/Login.aspx");
                }



            }
        }

        private void GetInscritos(string cedula)
        {
            AC_MATRICULAC mc = new AC_MATRICULAC();
            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
            var listMc = mc.LoadAC_MATRICULAC("xCedula", cedula, "", "", "");
            if (listMc.Count == 1)
            {

                var listIns = ins.LoadPOSG_INSCRIPCION("xPK", listMc[0].strcod_matric, "", "", "");
                if (listIns.Count == 1)
                {

                    gvwMatriculados.DataSource = listIns;
                    gvwMatriculados.DataBind();
                    txtTema.Text = listIns[0].strtema_ins;
                    txtMecanismo.Text = listIns[0].strmecanismo_ins;
                    txtModalidad.Text = listIns[0].strmodalidad_ins;
                    txtObservaciones.Text = listIns[0].strobservaciones_ins;
                    txtSugerencias.Text = listIns[0].strsugerencias_ins;
                    lblCodCabecera.Text = "TUTOR: " + listIns[0].strobs1_ins;
                    gvwMatriculados.Caption = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros";
                }
                else
                {
                    gvwMatriculados.DataSource = null;
                    gvwMatriculados.DataBind();

                    lblMsg.Text = "Estimado/a usuario usted aún no se ha registrado para el proceso, acerquese a secretaria de posgrado";
                    gvwMatriculados.Caption = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros";
                }

            }
            else
            {
                divDetalles.Visible = false;
                lblMsg.Text = "No existe matricula para el periodo vigente";
            }



        }

        protected void gvwMatriculados_SelectedIndexChanged(object sender, EventArgs e)
        {
            divDetalles.Visible = true;
            //lblMsg.Text = gvwMatriculados.SelectedDataKey["strid_ins"].ToString();

            GetDetalles();
        }

        private void GetDetalles()
        {
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();
            var listTrib = insd.LoadPOSG_INSCRIPCIOND("xInscripcionT", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "TRIBUNAL", "", "");
            if (listTrib.Count >= 1)
            {
                gvwTribunal.DataSource = listTrib;
                gvwTribunal.DataBind();
                gvwTribunal.Caption = "Se han encontrado: " + gvwTribunal.Rows.Count.ToString() + " registros";
            }
            else
            {
                lblMsg.Text = "Aún no se registrado miembros para el tribunal";
            }
            var listObj = insd.LoadPOSG_INSCRIPCIOND("xInscripcionO", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "OBJETIVO", "", "");
            if (listObj.Count >= 1)
            {
                gvwObjetivos.DataSource = listObj;
                gvwObjetivos.DataBind();
                gvwObjetivos.Caption = "Se han encontrado: " + gvwObjetivos.Rows.Count.ToString() + " registros";
            }
            else
            {
                lblMsg.Text = "No tiene objetivos agregados, por favor ingrese datos...";
            }
            gvwObjetivos.SelectedIndex = -1;
            gvwTribunal.SelectedIndex = -1;
        }

        protected void btnGuardarObjetivo_Click(object sender, EventArgs e)
        {
            AddUpdateObj();
        }

        private void AddUpdateObj()
        {
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();
            var detalleIns = insd.LoadPOSG_INSCRIPCIOND("xInscripcionOG", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "OBJETIVO", "ESPECIFICO", "");
            var maxObj = detalleIns.Any() ? detalleIns.Max(detalleIns1 => detalleIns1.decobs1_insd) : 0;
            if (gvwObjetivos.SelectedIndex == -1) //ADD
            {
                var tipoObj = "";
                if (ddlObjetivos.SelectedIndex == 0)
                {
                    tipoObj = "OBJGEN";
                    insd.strid_insd = string.Concat(gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "_", tipoObj);
                    insd.strobs1_insd = "GENERAL";
                }
                else if (ddlObjetivos.SelectedIndex == 1)
                {
                    tipoObj = "OBJESP";

                    insd.strid_insd = string.Concat(gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "_", tipoObj, (maxObj + 1).ToString("F0"));
                    insd.strobs1_insd = "ESPECIFICO";
                }
                insd.strid_ins = gvwMatriculados.SelectedDataKey["strid_ins"].ToString();

                //insd.strid_insd = string.Concat( gvwMatriculados.SelectedDataKey["strid_ins"].ToString(),"_",tipoObj,detalleIns.Count+1);
                insd.strtipo_insd = "OBJETIVO";
                string textoObjetivo = txtNuevoObjetivo.Text;
                // Eliminar saltos de línea
                textoObjetivo = textoObjetivo.Replace("\r", "").Replace("\n", "");
                insd.strdesctipo_insd = textoObjetivo;
                insd.strcedtribunal_insd = "NO APLICA";
                insd.bitaprobado_insd = false;
                insd.strcedaprobado_insd = "NO APLICA";
                insd.dtfechaaprobado_insd = DateTime.Now;
                insd.strobsaprobado_insd = string.Empty;
                insd.struserlog = lblUser.Text;  //reemplazar por User.Identity.Name
                insd.dtfechalog = DateTime.Now;
                insd.strobs2_insd = string.Empty;
                insd.strobs3_insd = string.Empty;
                insd.decobs1_insd = maxObj + 1;
                insd.decobs2_insd = -1;
                insd.bitobs1_insd = false;
                insd.bitobs2_insd = false;
                insd.dtobs1_insd = DateTime.Now;
                insd.dtobs2_insd = DateTime.Now;
                insd.AddPOSG_INSCRIPCIOND(insd);
                if (insd.resultado)
                {
                    lblMsg.Text = insd.msg;

                }
                else
                {
                    lblMsg.Text = insd.msg;
                }

            }
            else //UPDATE
            {
                var listInsd = insd.LoadPOSG_INSCRIPCIOND("xPK", gvwObjetivos.SelectedDataKey["strId_insd"].ToString(), "", "", "");
                if (listInsd.Count == 1)
                {
                    string textoObjetivo = txtNuevoObjetivo.Text;
                    // Eliminar saltos de línea
                    textoObjetivo = textoObjetivo.Replace("\r", "").Replace("\n", "");
                    listInsd[0].strdesctipo_insd = textoObjetivo;
                    listInsd[0].strobsaprobado_insd = txtObsObj.Text;
                    listInsd[0].struserlog = lblUser.Text;
                    listInsd[0].dtfechalog = DateTime.Now;
                    insd.UpdatePOSG_INSCRIPCIOND(listInsd[0]);

                    if (insd.resultado)
                    {
                        lblMsg.Text = insd.msg;
                    }
                    else
                    {
                        lblMsg.Text = insd.msg;
                    }

                }
            }
            GetDetalles();
            txtNuevoObjetivo.Text = string.Empty;
            txtObsObj.Text = string.Empty;
        }

        protected void gvwObjetivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var aprobado = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "bitaprobado_insd"));


                if (aprobado == true)
                {
                    LinkButton lnkBtnDelete = (LinkButton)e.Row.FindControl("lnkBtnDelete");
                    LinkButton lnkBtnEdit = (LinkButton)e.Row.FindControl("lnkBtnEdit");
                    if (lnkBtnEdit != null && lnkBtnDelete != null)
                    {
                        lnkBtnEdit.Visible = false;
                        lnkBtnDelete.Visible = false;
                    }
                }
            }
        }
        //protected void ShowToastMessage(string message, string title = "Notificación")
        //{
        //    string script = $@"
        //<script type='text/javascript'>
        //    $(document).ready(function() {{
        //        var toast = $('#globalToast');
        //        toast.find('.toast-body').text('{message}');
        //        toast.find('.toast-header strong').text('{title}');
        //        toast.toast('show');
        //    }});
        //</script>";
        //    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
        //}

        protected void gvwObjetivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectObjective")
            {
                // Obtén el índice de la fila seleccionada
                int index = Convert.ToInt32(e.CommandArgument);

                // Establece el SelectedIndex del GridView
                gvwObjetivos.SelectedIndex = index;
                // Obtén la fila seleccionada
                GridViewRow selectedRow = gvwObjetivos.Rows[index];


                // Asume que gvwObjetivos.SelectedDataKey["strid_insd"] tiene el valor correcto
                string strid_insd = gvwObjetivos.DataKeys[index].Values["strId_insd"].ToString();
                string strid_ins = gvwObjetivos.DataKeys[index].Values["strId_ins"].ToString();
                string strtipo_insd = gvwObjetivos.DataKeys[index].Values["strDescTipo_insd"].ToString();

                POSG_INSCRIPCIOND insD = new POSG_INSCRIPCIOND();
                var listInsD = insD.LoadPOSG_INSCRIPCIOND("xPK", strid_insd, "", "", "");

                if (listInsD != null && listInsD.Count > 0)
                {
                    txtNuevoObjetivo.Text = listInsD[0].strdesctipo_insd;
                    txtObsObj.Text = listInsD[0].strobsaprobado_insd;
                    ddlObjetivos.Enabled = false;
                    txtObsObj.Enabled = false;

                    lblMsg.Text = "Los datos se guardaron correctamente.";
                }
                else
                {
                    lblMsg.Text = "No se han encontrado datos asociados";
                }

                // Muestra el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalObjetivo').modal('show');", true);
            }
        }

        protected void gvwObjetivos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            // Obtén el DataKey del GridView para la fila seleccionada
            string strid_insd = gvwObjetivos.DataKeys[e.RowIndex].Value.ToString();

            // Lógica para eliminar el registro
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();

            insd.DelPOSG_INSCRIPCIOND("xPK", strid_insd, "", "", "");

            if (insd.resultado)
            {
                lblMsg.Text = "El registro ha sido eliminado correctamente.";
            }
            else
            {
                lblMsg.Text = "Hubo un problema al eliminar el registro: " + insd.msg;
            }

            // Recargar datos del GridView
            GetDetalles();

            // Cancelar la eliminación predeterminada del GridView
            e.Cancel = true;
            gvwObjetivos.SelectedIndex = -1;
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            // Limpiar el TextBox de objetivos
            txtNuevoObjetivo.Text = string.Empty;
            txtObsObj.Text = string.Empty;
            gvwObjetivos.SelectedIndex = -1;
            // Ocultar el modal utilizando JavaScript
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideModal", "$('#modalObjetivo').modal('hide');", true);
        }

        protected void btnAddObj_Click(object sender, EventArgs e)
        {
            // Habilitar el DropDownList
            ddlObjetivos.Enabled = true;
            txtObsObj.Enabled = false;
            // Mostrar el modal utilizando JavaScript
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalObjetivo').modal('show');", true);
        }

    }
}