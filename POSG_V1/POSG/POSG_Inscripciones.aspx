    <%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_Inscripciones.aspx.cs" Inherits="POSG_V1.POSG.POSG_Inscripciones" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">

     <style>

        .modal-dialog-wide {
        max-width: 60%; /* Ajustar el porcentaje según sea necesario */
    }
         .true-background {
        background-color: green;
        color: white;
    }

    .false-background {
        background-color: red;
        color: white;
    }
        .gridview-header {
            background-color: #0e407f;
            font-weight: bold;
            color: white;
            text-align: center;
        }

        .gridview-row {
            background-color: #ffffff;
            text-align: center;
        }

        .gridview-row-alternate {
            background-color: #EAEAD3;
        }

        .gridview-selected-row {
            background-color: DodgerBlue;
            font-weight: bold;
            color: white;
        }

        .gridview-pager {
            background-color: #F7F7DE;
            color: black;
            text-align: right;
        }

        .gridview-footer {
            background-color: #CCCC99;
        }

        .gridview-sorted-ascending {
            background-color: #FBFBF2;
        }

        .gridview-sorted-descending {
            background-color: #575357;
            color: white;
        }
         .text-justify {
        text-align: justify;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
   
    <asp:UpdatePanel ID="upRegistrar" UpdateMode="Always" runat="server">
        <ContentTemplate>
           <asp:Label runat="server" ID="lblUser" Visible="false"></asp:Label>
            <br />
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm">
                        <asp:DropDownList ID="ddlSedes" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSedes_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="col-sm">
                        <asp:DropDownList ID="ddlFacultad" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFacultad_SelectedIndexChanged" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm">
                        <asp:DropDownList ID="ddlMaestria" AutoPostBack="true" OnSelectedIndexChanged="ddlMaestria_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm" hidden>
                        <asp:DropDownList ID="ddlCohorte" AutoPostBack="true" OnSelectedIndexChanged="ddlCohorte_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <asp:Label ID="lblMessage" CssClass="form-control alert alert-warning" Visible="true" runat="server"></asp:Label>
                <asp:Panel ID="pnlMatriculados" runat="server">
                     
                    <div class="alert alert-success"><b>LISTADO DE MATRICULADOS EN LA MAESTRIA DE :</b> &nbsp;<asp:Label ID="lblMaestria" runat="server" ></asp:Label> </div>
                <asp:GridView ID="gvwMatriculados"  Width="100%" 
                    DataKeyNames="strCod_matric,strCod_per,strCod_alu,strCaso_matric,strUser_log"
                     AutoGenerateColumns="false" runat="server" 
                    OnSelectedIndexChanged="gvwMatriculados_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="strCod_matric" Visible="false" HeaderText="Codigo"></asp:BoundField>
                        <asp:BoundField DataField="strCod_per" Visible="false" HeaderText="Periodo"></asp:BoundField>
                        <asp:BoundField DataField="strCod_alu" HeaderText="Cédula"></asp:BoundField>
                        <asp:BoundField DataField="strUser_log" HeaderText="Nombres y Apellidos"></asp:BoundField>
                        <asp:BoundField DataField="strObs1_matric" HeaderText="Telefono"></asp:BoundField>
                        <asp:BoundField DataField="strObs2_matric" HeaderText="Email"></asp:BoundField>
                        <asp:BoundField DataField="strObs3_matric" HeaderText="Celular"></asp:BoundField>
                        <asp:BoundField DataField="strObs4_matric" HeaderText="Dirección"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <HeaderTemplate>
                                Seleccionar
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnSelect" CssClass="fa fa-hand-o-up fa-2x" ForeColor="Orange" runat="server" CausesValidation="False" CommandName="Select" Text=""></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="False" Font-Italic="True" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#0e407f" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#ffffff" />
                    <SelectedRowStyle BackColor="DodgerBlue" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
                </asp:Panel>
            </div>


            <div class="container-fluid">
            <asp:Panel ID="pnlRegistrar" runat="server">
                
                <div class="panel panel-primary">
                    
                    <div class="panel-heading">
                        <asp:Label ID="lblCodCabecera" Visible="true" BackColor="Yellow" ForeColor="Black" runat="server"></asp:Label>
                        <div class="alert alert-info">Datos de inscripción</div>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col">
                                 <i class="fa fa-credit-card">&nbsp;<label for="txtCedula">Cédula:</label></i>
                                <asp:TextBox ID="txtCedula" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                               
                            </div>
                            <div class="col">
                               <i class="fa fa-user">&nbsp;  <label for="txtNombres">Nombres y apellidos:</label></i>
                                <asp:TextBox ID="txtNombres" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <i class="fa fa-list-alt">&nbsp;<label for="txtTema">Tema:</label></i>
                            <asp:TextBox ID="txtTema" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtTema" runat="server" ErrorMessage="Campo requerido" ControlToValidate="txtTema" CssClass="alert-danger" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>
                        <div class="row">
                            <div class="col">
                                <i class="fa fa-graduation-cap">&nbsp;<label for="txtModalidad">Modalidad de la maestría:</label></i>
                                <asp:TextBox ID="txtModalidad" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                            <div class="col">
                                <i class="fa fa-gears">&nbsp;
                                    <label for="ddlMecanismo">Mecanismo de titulación:</label></i>
                                <asp:DropDownList ID="ddlMecanismo" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="M1">PROPUESTAS METODOLÓGICAS Y TECNOLOGÍAS AVANZADAS</asp:ListItem>
                                    <asp:ListItem Value="M2">PROYECTO DE INVESTIGACIÓN</asp:ListItem>
                                    <asp:ListItem Value="M3">ARTICULO ACADÉMICO Y CIENTÍFICO</asp:ListItem>
                                     <asp:ListItem Value="M4">PROYECTO DE TITULACIÓN CON COMPONENTES DE INVESTIGACIÓN APLICADA Y/O DESARROLLO </asp:ListItem>
                                    <asp:ListItem Value="M5">ANÁLISIS DE CASOS </asp:ListItem>
                                    <asp:ListItem Value="M6">PROYECTO DE DESARROLLO</asp:ListItem>
                                    <asp:ListItem Value="M7">EMPRENDIMIENTOS</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <i class="fa fa-lightbulb-o">&nbsp;<label for="txtTema">Observaciones:</label></i>
                            <asp:TextBox ID="txtObservaciones" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtObservaciones" runat="server" ErrorMessage="Campo requerido" ControlToValidate="txtObservaciones" CssClass="alert-danger" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>
                        
                        <div class="form-group">
                           <i class="fa fa-lightbulb-o">&nbsp; <label for="txtModalidad">Sugerencias:</label></i>
                            <asp:TextBox ID="txtSugerencias" CssClass="form-control"  runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtSugerencias" runat="server" ErrorMessage="Campo requerido" ControlToValidate="txtSugerencias" CssClass="alert-danger" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                          <i class="fa fa-link">&nbsp;  <label for="txtUrl">Url del documento:</label></i>
                            <asp:TextBox ID="txtUrl" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtUrl" runat="server" ErrorMessage="Campo requerido" ControlToValidate="txtUrl" CssClass="alert-danger" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>
                    
                        <div class="row">
                            <div class="col-10">
                                <i class="fas fa-chalkboard-teacher">&nbsp; 
                                    <label for="txtUrl">Nombres y apellidos del tutor:</label></i>
                                <asp:TextBox ID="txtTutor" Enabled="false"  CssClass="form-control" runat="server"></asp:TextBox>
                                
                            </div>
                            <div class="col-2">
                                <i class="fas fa-chalkboard-teacher">&nbsp;<label for="btnAddTutor">Agregar tutor:</label></i>
                                <asp:Button ID="btnAddTutor" runat="server" Text="+" Font-Size="Large" CssClass="btn btn-success" OnClick="btnAddTutor_Click" />
                            </div>
                            <asp:RequiredFieldValidator ID="rfvtxtTutor" runat="server" ErrorMessage="Campo requerido" ControlToValidate="txtTutor" CssClass="alert-danger" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </div>

                    

                        
                    </div>

                    <div class="panel-footer panel-primary" >
                        <asp:Button ID="btnSalir" runat="server" OnClick="btnSalir_Click" CssClass="btn btn-warning" Text="Cancelar" ToolTip="Cancelar" />
                        <asp:Button ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="btn btn-primary" runat="server" Text="Guardar" ValidationGroup="Add" CausesValidation="true" ToolTip="Guardar registro" />

                    </div>
                </div>
            </asp:Panel>


                <div class="modal fade" id="modalTutor" tabindex="-1" role="dialog" aria-labelledby="modalTutorLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="modalTutorLabel">Buscar docente</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label for="txtCedulaT">Cédula:</label>
                                    <div class="row">
                                        <div class="col">
                                            <asp:TextBox ID="txtCedulaT" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-auto">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                                        </div>
                                    </div>
                                    <div>
                                        <br />
                                    </div>
                                    <label for="gvwDocente">Resultado de la búsqueda:</label>
                                    <asp:GridView ID="gvwDocente" OnSelectedIndexChanged="gvwDocente_SelectedIndexChanged"
                                        DataKeyNames="strId_per,strNomb_per,strApellidop_per,strApellidom_per"  runat="server" Width="100%" AutoGenerateColumns="false">
                                        <Columns>
                                            

                                            <asp:BoundField DataField="strId_per" HeaderText="Cédula" />
                                            <asp:BoundField DataField="strNomb_per" HeaderText="Nombres" />
                                            <asp:BoundField DataField="strApellidop_per" HeaderText="Apellido P." />
                                            <asp:BoundField DataField="strApellidom_per" HeaderText="Apellido M." />
                                            <asp:TemplateField ShowHeader="False">
                                                <HeaderTemplate>
                                                    Seleccionar
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnSelect" CssClass="fa fa-hand-o-up fa-2x" ForeColor="Orange" runat="server" ToolTip="Ver detalles de inscripción" CausesValidation="False" CommandName="Select" Text=""></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="False" Font-Italic="True" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="gridview-header" />
                                        <RowStyle CssClass="gridview-row" />
                                        <SelectedRowStyle CssClass="gridview-selected-row" />
                                        <PagerStyle CssClass="gridview-pager" />
                                        <FooterStyle CssClass="gridview-footer" />
                                        <SortedAscendingCellStyle CssClass="gridview-sorted-ascending" />
                                        <SortedDescendingCellStyle CssClass="gridview-sorted-descending" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="modal-footer">


                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </ContentTemplate>
        
    </asp:UpdatePanel>


   

    

</asp:Content>

