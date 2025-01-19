using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using ClassLibraryTesis;
using System.Data;
using POSG_V1.ConexionDb;

namespace POSG_V1.POSG
{
    public partial class POSG_Inscripciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {
                    string userId = Session["userId"].ToString();

                    USERINROLES ur = new USERINROLES();

                    lblUser.Text = userId;
                    var rolUser = ur.LoadUSERINROLES("xPK", "SEACADEMICO", lblUser.Text, "", "");

                    if (rolUser.Count == 1)
                    {
                        GetSedes();
                        GetFacultades();
                        GetCarreras();
                        GetPeriodos();
                        pnlRegistrar.Visible = false;
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
            lblMessage.Text = ddlSedes.SelectedValue;

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
                lblMessage.Text = per.msg.ToString();
            }
        }

        private void GetMatriculados()
        {
            AC_MATRICULAC mc = new AC_MATRICULAC();
            var listMc = mc.LoadAC_MATRICULAC("xPeriodo", ddlCohorte.SelectedValue, "", "", "");
            if (listMc.Count >= 1)
            {
                gvwMatriculados.DataSource = listMc;
                gvwMatriculados.DataBind();
            }
            else
            {
                gvwMatriculados.DataSource = null;
                gvwMatriculados.DataBind();
            }
            gvwMatriculados.SelectedIndex = -1;
            gvwMatriculados.Caption = "Se han encontrado: " + gvwMatriculados.Rows.Count + " registros";
            pnlRegistrar.Visible = false;
            pnlMatriculados.Visible = true;
            lblMaestria.Text = ddlMaestria.SelectedItem.ToString();
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
        }

        protected void ddlCohorte_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetMatriculados();
        }

        protected void gvwMatriculados_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlRegistrar.Visible = true;
            pnlMatriculados.Visible = false;
            txtCedula.Text = gvwMatriculados.SelectedDataKey["strCod_alu"].ToString();
            txtNombres.Text = gvwMatriculados.SelectedDataKey["strUser_log"].ToString();
            txtModalidad.Text = gvwMatriculados.SelectedDataKey["strCaso_matric"].ToString();
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            pnlRegistrar.Visible = false;
            pnlMatriculados.Visible = true;
            gvwMatriculados.SelectedIndex = -1;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveRegistro();
        }

        private void SaveRegistro()
        {
            POSG_INSCRIPCION ins = new POSG_INSCRIPCION();
            var listIns = ins.LoadPOSG_INSCRIPCION("xPK", gvwMatriculados.SelectedDataKey["strCod_matric"].ToString(), "", "", "");

            if (listIns.Count == 0) // no existe el registro y se hará un add
            {
                ins.strid_ins = gvwMatriculados.SelectedDataKey["strCod_matric"].ToString();
                ins.strcod_per = gvwMatriculados.SelectedDataKey["strCod_per"].ToString();
                ins.strcod_matric = gvwMatriculados.SelectedDataKey["strCod_matric"].ToString();
                ins.strid_per = gvwMatriculados.SelectedDataKey["strCod_alu"].ToString();
                ins.strtema_ins = txtTema.Text;
                ins.strmodalidad_ins = txtModalidad.Text;
                ins.strmecanismo_ins = ddlMecanismo.SelectedValue;
                ins.dtfechacreacion_ins = DateTime.Now;
                ins.bitmatrizaprobada_ins = false;
                ins.dtfechamatrizaprobada_ins = DateTime.Now;
                ins.strcedapruebamatriz_ins1 = string.Empty;
                ins.bitmatrizrevisada_ins = false;
                ins.dtfechamatrizrevisada_ins = DateTime.Now;
                ins.strcedrevisamatriz_ins = string.Empty;
                ins.bitpagado_ins = false;
                ins.dtfechapago_ins = DateTime.Now;
                ins.strcedregistrapago_ins = string.Empty;
                ins.strobservaciones_ins = txtObservaciones.Text;
                ins.strsugerencias_ins = txtSugerencias.Text;
                ins.strurl_ins = txtUrl.Text;
                ins.bitaprobadoperfil_ins = false;
                ins.strperiodoacademico_ins = "";
                ins.struserlog = User.Identity.Name;
                ins.dtfechalog = DateTime.Now;
                ins.strobs1_ins = "";
                ins.strobs2_ins = "";
                ins.strobs3_ins = "";
                ins.strvacio1_ins = "";
                ins.strvacio2_ins = "";
                ins.decobs1_ins = -1;
                ins.bitobs1_ins = false;
                ins.bitobs2_ins = false;
                ins.strid_per2 = gvwDocente.SelectedDataKey["strId_per"].ToString();
                ins.dtobs1_ins = DateTime.Now;
                ins.dtobs2_ins = DateTime.Now;
                ins.strcoid_per = string.Empty;

                ins.AddPOSG_INSCRIPCION(ins);

                if (ins.resultado)
                {
                    GenerarRelacionNotasYActas(ins.strid_ins); // Llamada para generar relaciones en POSG_NOTAS y POSG_ACTAS
                    lblMessage.Text = ins.msg;
                    Limpiar();
                }
                else
                {
                    lblMessage.Text = ins.msg;
                }
                GetMatriculados();
            }
            else if (listIns.Count == 1) // ya existe el registro y no se realizará ninguna acción
            {
                lblMessage.Text = "El registro ya existe, no se realizó ningún cambio";
            }
        }


        private void GenerarRelacionNotasYActas(string strid_ins)
        {
            ConexionDB conexionDb = new ConexionDB();
            using (SqlConnection conn = conexionDb.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ADD_PK_POSG_Notas_POSG_Actas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@strId_ins", strid_ins);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "Registro generado correctamente.";
                    }
                    catch (SqlException sqlEx)
                    {
                        lblMessage.Text = $"Error SQL al generar registro TB_ACT_NOT: {sqlEx.Message}";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = $"Error general al generar registro TB_ACT_NOT: {ex.Message}";
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }



        private void Limpiar()
        {
            txtModalidad.Text = string.Empty;
            txtTema.Text = string.Empty;
            txtSugerencias.Text = string.Empty;
            txtUrl.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            pnlMatriculados.Visible = true;
            pnlRegistrar.Visible = false;
            gvwMatriculados.SelectedIndex = -1;
        }

        protected void btnAddTutor_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTutor').modal('show');", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            POSG_PERSONA per = new POSG_PERSONA();
            var listPer = per.LoadPOSG_PERSONA("xPK", txtCedulaT.Text.Trim(), "", "", "");
            if (listPer.Count == 1)
            {
                gvwDocente.DataSource = listPer;
                gvwDocente.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalTutor').modal('show');", true);
        }

        protected void gvwDocente_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreCompleto = $"{gvwDocente.SelectedDataKey["strNomb_per"].ToString()} {gvwDocente.SelectedDataKey["strApellidop_per"].ToString()} {gvwDocente.SelectedDataKey["strApellidom_per"].ToString()}";
            txtTutor.Text = nombreCompleto;
        }
    }
}
