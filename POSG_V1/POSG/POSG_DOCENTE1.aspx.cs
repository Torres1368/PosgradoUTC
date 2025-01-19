using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibraryTesis;
namespace POSG_V1.POSG
{
    public partial class POSG_DOCENTE1 : System.Web.UI.Page
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
                    var rolUser = ur.LoadUSERINROLES("xPK", "DOCENTE", lblUser.Text, "", "");

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

            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();

            var listIns = ins.LoadPOSG_INSCRIPCION("xDocente", cedula, "", "", "");
            if (listIns.Count > 0)
            {

                gvwMatriculados.DataSource = listIns;
                gvwMatriculados.DataBind();
                lblCodCabecera.Text = "TUTOR: " + listIns[0].strobs3_ins;
                gvwMatriculados.Caption = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros";
            }
            else
            {
                gvwMatriculados.DataSource = null;
                gvwMatriculados.DataBind();

                lblMsg.Text = "Estimado/a usuario no tiene elementos para revisar";
                gvwMatriculados.Caption = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros";
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
            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();
            var listIns = ins.LoadPOSG_INSCRIPCION("xPK", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "", "", "");
            if (listIns.Count == 1)
            {
                txtTema.Text = listIns[0].strtema_ins;
                txtMecanismo.Text = listIns[0].strmecanismo_ins;
                txtModalidad.Text = listIns[0].strmodalidad_ins;
                txtObservaciones.Text = listIns[0].strobservaciones_ins;
                txtSugerencias.Text = listIns[0].strsugerencias_ins;
                var listTrib = insd.LoadPOSG_INSCRIPCIOND("xInscripcionT", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "TRIBUNAL", "", "");
                if (listTrib.Count >= 1)
                {
                    gvwTribunal.DataSource = listTrib;
                    gvwTribunal.DataBind();
                    gvwTribunal.Caption = "Se han encontrado: " + gvwTribunal.Rows.Count.ToString() + " registros";
                }
                else
                {
                    gvwTribunal.DataSource = null;
                    gvwTribunal.DataBind();
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
                    gvwObjetivos.DataSource = null;
                    gvwObjetivos.DataBind();


                    lblMsg.Text = "No se han encontardo datos agregados, por favor ingrese información...";
                }
                gvwObjetivos.SelectedIndex = -1;
                gvwTribunal.SelectedIndex = -1;
            }
            else
            {
                lblMsg.Text = "No se han encontrado datos...";
            }
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
                    listInsd[0].strobsaprobado_insd = txtObsObj.Text.ToUpper();
                    listInsd[0].bitaprobado_insd = chkValidar.Checked;
                    listInsd[0].strcedaprobado_insd = lblUser.Text;
                    listInsd[0].dtfechaaprobado_insd = DateTime.Now;
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
                    // LinkButton lnkBtnDelete = (LinkButton)e.Row.FindControl("lnkBtnDelete");
                    LinkButton lnkBtnEdit = (LinkButton)e.Row.FindControl("lnkBtnEdit");
                    if (lnkBtnEdit != null)
                    {
                        lnkBtnEdit.Visible = false;
                        // lnkBtnDelete.Visible = false;
                    }
                }
            }
        }


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
                    chkValidar.Checked = listInsD[0].bitaprobado_insd;
                    txtNuevoObjetivo.Enabled = false;
                    ddlObjetivos.Visible = false;
                    txtObsObj.Enabled = true;

                    //lblMsg.Text = "Los datos se guardaron correctamente.";
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



        protected void btnValidarObj_Click(object sender, EventArgs e)
        {
            txtCedulaT.Enabled = true;
            txtCedulaT.Text = string.Empty;
            gvwDocente.DataSource = null;
            gvwDocente.DataBind();
            gvwDocente.SelectedIndex = -1;
            // Mostrar el modal utilizando JavaScript
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTribunal').modal('show');", true);
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{

        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTribunal').modal('show');", true);
        //}



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            POSG_PERSONA per = new POSG_PERSONA();
            lblMsgT.Text = string.Empty;
            var listPer = per.LoadPOSG_PERSONA("xPK", txtCedulaT.Text.Trim(), "", "", "");
            if (listPer.Count == 1)
            {

                if (listPer[0].strid_per != lblUser.Text)
                {

                    gvwDocente.DataSource = listPer;
                    gvwDocente.DataBind();
                }
                else
                {

                    gvwDocente.DataSource = null;
                    gvwDocente.DataBind();
                    lblMsgT.Text = "El tutor no puede ser parte del tribunal, por favor ingrese un número de cédula diferente";
                }


            }
            else
            {
                gvwDocente.DataSource = null;
                gvwDocente.DataBind();
                lblMsgT.Text = "No existen registros...";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTribunal').modal('show');", true);
        }

        protected void gvwDocente_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCedulaT.Enabled = false;
            string nombreCompleto = $"{gvwDocente.SelectedDataKey["strNomb_per"].ToString()} {gvwDocente.SelectedDataKey["strApellidop_per"].ToString()} {gvwDocente.SelectedDataKey["strApellidom_per"].ToString()}";
            txtTribunal.Text = nombreCompleto;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTribunal').modal('show');", true);
        }




        private void AddUpdateTri()
        {
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();
            var detalleIns = insd.LoadPOSG_INSCRIPCIOND("xInscripcionT", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "TRIBUNAL", "", "");
            //var maxObj = detalleIns.Any() ? detalleIns.Max(detalleIns1 => detalleIns1.decobs1_insd) : 0;
            if (gvwTribunal.SelectedIndex == -1) //ADD
            {

                insd.strid_ins = gvwMatriculados.SelectedDataKey["strid_ins"].ToString();
                insd.strid_insd = string.Concat(gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "_", txtCedulaT.Text);
                insd.strtipo_insd = "TRIBUNAL";
                insd.strdesctipo_insd = "TRIBUNAL";
                insd.strcedtribunal_insd = txtCedulaT.Text;
                insd.bitaprobado_insd = chkValidarTribunal.Checked;
                insd.strcedaprobado_insd = lblUser.Text;
                insd.dtfechaaprobado_insd = DateTime.Now;
                insd.strobsaprobado_insd = string.Empty;
                insd.struserlog = lblUser.Text;  //reemplazar por User.Identity.Name
                insd.dtfechalog = DateTime.Now;
                insd.strobs1_insd = "TRIBUNAL";
                insd.strobs2_insd = string.Empty;
                insd.strobs3_insd = string.Empty;
                insd.decobs1_insd = -1;
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
                var listInsd = insd.LoadPOSG_INSCRIPCIOND("xPK", gvwTribunal.SelectedDataKey["strId_insd"].ToString(), "", "", "");
                if (listInsd.Count == 1)
                {
                    //listInsd[0].strid_insd = string.Concat(gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "_", txtCedulaT.Text);
                    listInsd[0].bitaprobado_insd = chkValidarTribunal.Checked;
                    listInsd[0].strcedaprobado_insd = lblUser.Text;
                    listInsd[0].dtfechaaprobado_insd = DateTime.Now;
                    //listInsd[0].strcedtribunal_insd = txtCedulaT.Text;
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

        }

        protected void btnAddT_Click(object sender, EventArgs e)
        {
            AddUpdateTri();
        }

        protected void gvwTribunal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectTribunal")
            {
                txtCedulaT.Enabled = false;
                gvwDocente.SelectedIndex = 0;

                // Obtén el índice de la fila seleccionada
                int index = Convert.ToInt32(e.CommandArgument);

                // Establece el SelectedIndex del GridView
                gvwTribunal.SelectedIndex = index;
                // Obtén la fila seleccionada
                GridViewRow selectedRow = gvwTribunal.Rows[index];


                // Asume que gvwObjetivos.SelectedDataKey["strid_insd"] tiene el valor correcto
                string strid_insd = gvwTribunal.DataKeys[index].Values["strId_insd"].ToString();
                string strid_ins = gvwTribunal.DataKeys[index].Values["strId_ins"].ToString();
                //string strtipo_insd = gvwTribunal.DataKeys[index].Values["strDescTipo_insd"].ToString();
                POSG_PERSONA per = new POSG_PERSONA();

                POSG_INSCRIPCIOND insD = new POSG_INSCRIPCIOND();
                var listInsD = insD.LoadPOSG_INSCRIPCIOND("xPK", strid_insd, "", "", "");

                if (listInsD != null && listInsD.Count > 0)
                {
                    var tribunal = per.LoadPOSG_PERSONA("xPK", listInsD[0].strcedtribunal_insd, "", "", "");
                    txtCedulaT.Text = listInsD[0].strcedtribunal_insd;
                    txtTribunal.Text = tribunal[0].strnomb_per + " " + tribunal[0].strapellidop_per + " " + tribunal[0].strapellidom_per;
                    chkValidarTribunal.Checked = listInsD[0].bitaprobado_insd;

                    lblMsg.Text = "datos encontrados correctamente.";
                }
                else
                {
                    lblMsg.Text = "No se han encontrado datos asociados";
                }

                // Muestra el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTribunal').modal('show');", true);
            }
        }

        protected void btnCloseTrib_Click(object sender, EventArgs e)
        {
            gvwTribunal.SelectedIndex = -1;
        }

        protected void gvwTribunal_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Obtén el DataKey del GridView para la fila seleccionada
            string strid_insd = gvwTribunal.DataKeys[e.RowIndex].Value.ToString();

            // Lógica para eliminar el registro
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();

            insd.DelPOSG_INSCRIPCIOND("xPK", strid_insd, "", "", "");

            if (insd.resultado)
            {
                lblMsg.Text = "El registro ha sido eliminado del sistema.";
            }
            else
            {
                lblMsg.Text = "Hubo un problema al eliminar el registro: " + insd.msg;
            }

            // Recargar datos del GridView
            GetDetalles();

            // Cancelar la eliminación predeterminada del GridView
            e.Cancel = true;
            gvwTribunal.SelectedIndex = -1;
        }
    }
}