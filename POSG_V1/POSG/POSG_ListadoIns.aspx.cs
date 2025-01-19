using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibraryTesis;
using Microsoft.Reporting.WebForms;

namespace POSG_V1.POSG
{
    public partial class POSG_ListadoIns : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Session["userId"] != null)

                {

                    USERINROLES ur = new USERINROLES();
                    string userId = Session["userId"].ToString();
                    lblUser.Text = Session["UserId"].ToString();
                    var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", "");

                    if (rolUser.Count == 1)
                    {
                        GetSedes();
                        GetFacultades();
                        GetCarreras();
                        GetPeriodos();
                        //pnlRegistrar.Visible = false;
                        divDetalle.Visible = false;
                        //btnAddTribunal.Enabled = false;
                        gvwMatriculados.SelectedIndex = -1;

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
        private void GetSedes()
        {
            UB_SEDES sedes = new UB_SEDES();
            var listsedes = sedes.LoadUB_SEDES("ALL", "", "", "", "");
            if (listsedes.Count >= 1)
            {
                ddlSedes.DataSource = listsedes;
                ddlSedes.DataTextField = "strNombre_Sede";
                ddlSedes.DataValueField = "strCod_Sede";
                ddlSedes.DataBind();
            }
        }

        private void GetFacultades()
        {
            UB_FACULTADES fac = new UB_FACULTADES();

            //lblMsg.Text = ddlSedes.SelectedValue;

            var listFac = fac.LoadUB_FACULTADES("xSede", ddlSedes.SelectedValue, "", "", "");



            if (listFac.Count >= 1)
            {
                ddlFacultad.DataSource = listFac;
                ddlFacultad.DataTextField = "strNombre_Fac";
                ddlFacultad.DataValueField = "strCod_Fac";
                ddlFacultad.DataBind();
            }
        }

        private void GetCarreras()
        {
            UB_CARRERAS car = new UB_CARRERAS();


            var listCar = car.LoadUB_CARRERAS("xSedeFac", ddlSedes.SelectedValue, ddlFacultad.SelectedValue, "", "");
            if (listCar.Count >= 1)
            {
                ddlMaestria.DataSource = listCar;
                ddlMaestria.DataTextField = "strNombre_Car";
                ddlMaestria.DataValueField = "strCod_Car";
                ddlMaestria.DataBind();
                lblMaestria.Text = ddlMaestria.SelectedItem.ToString();
            }
        }
        private void GetPeriodos()
        {
            SIG_PERIODOS per = new SIG_PERIODOS();
            var listPer = per.LoadSIG_PERIODOS("xSedeFacCar", ddlSedes.SelectedValue, ddlFacultad.SelectedValue, ddlMaestria.SelectedValue, "");
            if (listPer.Count >= 1)
            {
                ddlCohorte.DataSource = listPer;
                ddlCohorte.DataTextField = "strNombre_Per";
                ddlCohorte.DataValueField = "strCod_Per";
                ddlCohorte.DataBind();

            }
            else
            {
                lblMsg.Text = per.msg.ToString();
            }
        }

        protected void ddlSedes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFacultades();
            GetCarreras();
            GetPeriodos();
        }

        protected void ddlFacultad_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCarreras();
            GetPeriodos();
            GetMatriculados();
        }

        protected void ddlMaestria_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPeriodos();
            GetMatriculados();
            gvwMatriculados.SelectedIndex = -1;

        }

        protected void ddlCohorte_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetMatriculados();
        }

        private void GetMatriculados()
        {
            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
            var listIns = ins.LoadPOSG_INSCRIPCION("xPer", ddlCohorte.SelectedValue, "", "", "");
            if (listIns.Count >= 1)
            {
                gvwMatriculados.DataSource = listIns;
                gvwMatriculados.DataBind();
                divDetalle.Visible = true;
            }
            else
            {
                gvwMatriculados.DataSource = null;
                gvwMatriculados.DataBind();
                divDetalle.Visible = false;
                //btnAddTribunal.Enabled = false;
            }
            gvwMatriculados.SelectedIndex = -1;
            gvwMatriculados.Caption = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros";
            //lblMessage.Text = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros"; 

            lblMaestria.Text = ddlMaestria.SelectedItem.ToString();

        }

        protected void gvwMatriculados_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDetalles();
            // btnAddTribunal.Enabled = true;
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            // Muestra el modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalReport').modal('show');", true);
            rvwRpt.LocalReport.Refresh();
        }
        private void GetDetalles()
        {
            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
            POSG_INSCRIPCIOND insd = new POSG_INSCRIPCIOND();
            var listIns = ins.LoadPOSG_INSCRIPCION("xPK", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "", "", "");
            if (listIns.Count == 1)
            {

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

        protected void gvwMatriculados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AprobarAll")
            {
                // Obtén el índice de la fila seleccionada
                int index = Convert.ToInt32(e.CommandArgument);

                // Establece el SelectedIndex del GridView
                gvwMatriculados.SelectedIndex = index;
                // Obtén la fila seleccionada
                GridViewRow selectedRow = gvwMatriculados.Rows[index];


                // Asume que gvwObjetivos.SelectedDataKey["strid_insd"] tiene el valor correcto
                // string strid_insd = gvwMatriculados.DataKeys[index].Values["strId_insd"].ToString();
                string strid_ins = gvwMatriculados.SelectedDataKey["strid_ins"].ToString();
                //string strtipo_insd = gvwObjetivos.DataKeys[index].Values["strDescTipo_insd"].ToString();

                POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
                var listIns = ins.LoadPOSG_INSCRIPCION("xPK", strid_ins, "", "", "");

                if (listIns != null && listIns.Count > 0)
                {
                    chkAprobada.Checked = listIns[0].bitmatrizaprobada_ins;
                    chkRevisado.Checked = listIns[0].bitmatrizrevisada_ins;
                    chkPagado.Checked = listIns[0].bitpagado_ins;

                }
                else
                {
                    lblMsg.Text = "No se han encontrado datos asociados";
                }

                // Muestra el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalValidar').modal('show');", true);
            }
        }


        private void UpdateMatriz()
        {
            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
            var listIns = ins.LoadPOSG_INSCRIPCION("xPK", gvwMatriculados.SelectedDataKey["strid_ins"].ToString(), "", "", "");
            if (listIns.Count == 1)
            {

                listIns[0].bitpagado_ins = chkPagado.Checked;
                listIns[0].strcedregistrapago_ins = lblUser.Text;
                listIns[0].dtfechapago_ins = DateTime.Now;

                listIns[0].bitmatrizrevisada_ins = chkRevisado.Checked;
                listIns[0].strcedregistrapago_ins = lblUser.Text;
                listIns[0].dtfechapago_ins = DateTime.Now;

                listIns[0].bitmatrizaprobada_ins = chkAprobada.Checked;
                listIns[0].strcedregistrapago_ins = lblUser.Text;
                listIns[0].dtfechapago_ins = DateTime.Now;

                listIns[0].struserlog = lblUser.Text;
                listIns[0].dtfechalog = DateTime.Now;
                ins.UpdatePOSG_INSCRIPCION(listIns[0]);

                if (ins.resultado)
                {
                    lblMsg.Text = ins.msg;
                }
                else
                {
                    lblMsg.Text = ins.msg;
                }
                gvwMatriculados.SelectedIndex = -1;
                GetMatriculados();
            }
        }

        protected void btnAddT_Click(object sender, EventArgs e)
        {
            UpdateMatriz();
        }

        protected void btnCloseTrib_Click(object sender, EventArgs e)
        {
            gvwMatriculados.SelectedIndex = -1;

        }
    }
}