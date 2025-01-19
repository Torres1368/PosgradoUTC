<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="POSG_DetalleIns.aspx.cs" Inherits="POSG_V1.POSG.POSG_DetalleIns" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="container-fluid">
        
        <asp:Label ID="lblUser" runat="server" Visible="false" ></asp:Label>
        <asp:Panel ID="pnlInscritos" runat="server">

            <div class="alert alert-success"><i class="fa fa-2x fa-graduation-cap">&nbsp; Detalle inscritos para el proceso de titulación </i> </div>
            <asp:Label ID="lblMsg" runat="server" Visible="true" ForeColor="Black"   CssClass="form-control alert alert-info"></asp:Label>
            <asp:GridView ID="gvwMatriculados" Caption="se han encontrado:" 
                CaptionAlign="Top" Width="100%" AutoGenerateColumns="false" runat="server" OnSelectedIndexChanged="gvwMatriculados_SelectedIndexChanged"
                DataKeyNames="strid_ins,strCod_per,strcod_matric,strid_per">
                <Columns>
                    <asp:BoundField DataField="strid_ins" Visible="false" HeaderText="Codigo"></asp:BoundField>
                    <asp:BoundField DataField="strCod_per" Visible="false" HeaderText="Periodo"></asp:BoundField>
                    <asp:BoundField DataField="strcod_matric" Visible="false" HeaderText="MC"></asp:BoundField>
                    <asp:BoundField DataField="strid_per" HeaderText="Cedula"></asp:BoundField>
                    <asp:BoundField DataField="strUserLog" HeaderText="Nombres y Apellidos"></asp:BoundField>

                     <asp:TemplateField HeaderText="Enlace al documento">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkDocumento" runat="server" NavigateUrl='<%# Eval("strUrl_ins") %>' Text="Ver documento" Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Revisada">
                        <ItemTemplate>
                            <asp:Label ID="lblRevisada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitMatrizRevisada_ins")) ? "✔" : "✘" %>'
                                CssClass='<%# Convert.ToBoolean(Eval("bitMatrizRevisada_ins")) ? "true-background" : "false-background" %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Pagada">
                        <ItemTemplate>
                            <asp:Label ID="lblPagada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitPagado_ins")) ? "✔" : "✘" %>'
                                CssClass='<%# Convert.ToBoolean(Eval("bitPagado_ins")) ? "true-background" : "false-background" %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                   

                    <asp:TemplateField HeaderText="Aprobado">
                        <ItemTemplate>
                            <asp:Label ID="lblAprobada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitMatrizAprobada_ins")) ? "✔" : "✘" %>'
                                CssClass='<%# Convert.ToBoolean(Eval("bitMatrizAprobada_ins")) ? "true-background" : "false-background" %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

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
            
            <div id="divDetalles" runat="server" >
            <div class="panel panel-primary">

                <div class="panel-heading">
                    <asp:Label ID="lblCodCabecera" Visible="true" ForeColor="Black" CssClass="alert alert-info" runat="server"></asp:Label>
                    
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <i class="fa fa-list-alt">&nbsp;<label for="txtTema">Tema:</label></i>
                        <asp:TextBox ID="txtTema" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <i class="fa fa-graduation-cap">&nbsp;<label for="txtModalidad">Modalidad de al maestría:</label></i>
                            <asp:TextBox ID="txtModalidad" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <i class="fa fa-gears">&nbsp;<label for="txtMecanismo">Modalidad de titulación:</label></i>
                            <asp:TextBox ID="txtMecanismo" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <i class="fa fa-lightbulb-o">&nbsp;<label for="txtTema">Observaciones:</label></i>
                        <asp:TextBox ID="txtObservaciones" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <i class="fa fa-lightbulb-o">&nbsp;
                            <label for="txtModalidad">Sugerencias:</label></i>
                        <asp:TextBox ID="txtSugerencias" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    
                </div>

                <div class="panel-footer panel-primary">
                </div>
            </div>
            <div class="panel panel-primary">

                <div class="panel-heading">
                    <div class="alert alert-info"><i class="fa fa-2x fa-archive"></i> Objetivos</div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <%--<asp:Button ID="btnAddObj" runat="server" Text="Agregar Objetivo" CssClass="btn btn-primary" OnClientClick="$('#myModal').modal('show'); return false;" />--%>
                        <asp:Button ID="btnAddObj" runat="server" Text="Agregar Objetivo" CssClass="btn btn-primary" OnClick="btnAddObj_Click" />
                        <div>
                            <br />
                        </div>
                        
                        <asp:GridView ID="gvwObjetivos" runat="server" OnRowDataBound="gvwObjetivos_RowDataBound"
                            Caption="se han encontrado:" OnRowDeleting="gvwObjetivos_RowDeleting" CaptionAlign="Top" Width="100%" AutoGenerateColumns="false"
                            DataKeyNames="strId_insd,strId_ins,strTipo_insd,strDescTipo_insd" OnRowCommand="gvwObjetivos_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="strid_insd" Visible="false" HeaderText="Codigo"></asp:BoundField>
                                <asp:BoundField DataField="strid_ins" Visible="false" HeaderText="Periodo"></asp:BoundField>
                                <asp:BoundField DataField="strObs1_insd" Visible="true" HeaderText="TIPO DE OBJETIVO">
                                    <ItemStyle CssClass="text-justify" />
                                </asp:BoundField>
                                <asp:BoundField DataField="strDescTipo_insd" HeaderText="Descripcion">
                                    <ItemStyle CssClass="text-justify" />
                                </asp:BoundField>
                                <asp:BoundField DataField="strObsAprobado_insd" HeaderText="Observaciones">
                                    <ItemStyle CssClass="text-justify" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Aprobado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAprobada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "✔" : "✘" %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "true-background" : "false-background" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <HeaderTemplate>Editar</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnEdit" CssClass="fa fa-hand-o-up fa-2x" ForeColor="Blue" runat="server"
                                            CausesValidation="False" CommandName="SelectObjective" CommandArgument='<%# Container.DataItemIndex %>'
                                            Text=""></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="True" ForeColor="#337AB7" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <HeaderTemplate>
                                        Eliminar
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnDelete" CssClass="fas fa-trash-alt" ForeColor="Red" runat="server" CausesValidation="False" CommandName="Delete" Text=""></asp:LinkButton>
                                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtenderObj" TargetControlID="lnkBtnDelete" ConfirmText="Desea eliminar este registro?" runat="server"></asp:ConfirmButtonExtender>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="True" ForeColor="#337AB7" HorizontalAlign="Center" />
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

                <div class="panel-footer panel-primary">
                </div>
            </div>
            <div class="panel panel-primary">

                <div class="panel-heading">
                    <div class="alert alert-success"><i class="fa fa-2x fa-group"></i> Tribunal </div>
                </div>
                <div class="panel-body">
                   
                    <div class="form-group">
                        <asp:GridView ID="gvwTribunal" runat="server" Caption="se han encontrado:"
                            CaptionAlign="Top" Width="100%" AutoGenerateColumns="false"
                            DataKeyNames="strId_insd,strId_ins">
                            <Columns>
                                <asp:BoundField DataField="strid_insd" Visible="false" HeaderText="Codigo"></asp:BoundField>
                                <asp:BoundField DataField="strid_ins" Visible="false" HeaderText="Periodo"></asp:BoundField>
                                <asp:BoundField DataField="strDescTipo_insd" Visible="true" HeaderText="Tribunal"></asp:BoundField>
                                <asp:BoundField DataField="strCedTribunal_insd" HeaderText="CEDULA"></asp:BoundField>
                                <asp:BoundField DataField="strObs2_insd" HeaderText="Nombres y apellidos"></asp:BoundField>
                                  <asp:TemplateField HeaderText="Aprobada">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAprobada" runat="server" Text='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "✔" : "✘" %>'
                                            CssClass='<%# Convert.ToBoolean(Eval("bitaprobado_insd")) ? "true-background" : "false-background" %>'>
                                        </asp:Label>
                                    </ItemTemplate>
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

                <div class="panel-footer panel-primary">
                </div>
            </div>
            </div>

        </asp:Panel>
        <div class="modal fade" id="modalObjetivo" tabindex="-1" role="dialog" aria-labelledby="modalObjetivoLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalObjetivoLabel">Nuevo Objetivo</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtNuevoObjetivo">Nuevo Objetivo:</label>
                            <asp:TextBox ID="txtNuevoObjetivo" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>

                            <label for="ddlObjetivos">Tipo de objetivo:</label>
                            <asp:DropDownList ID="ddlObjetivos" CssClass="form-control" runat="server">
                                <asp:ListItem Value="GENERAL">OBJETIVO GENERAL</asp:ListItem>
                                <asp:ListItem Value="ESPECIFICO">OBJETIVO ESPECIFICO</asp:ListItem>
                            </asp:DropDownList>
                            <div><br /></div>
                            <label for="txtObs">Observaciones al objetivo:</label>
                            <asp:TextBox ID="txtObsObj" CssClass="form-control" runat="server"  Rows="5"></asp:TextBox>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGuardarObjetivo" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarObjetivo_Click" OnClientClick="return validateForm();" />

                        <%--<asp:Button ID="btnGuardarObjetivo" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarObjetivo_Click" />--%>
                       <%-- <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>--%>
                        <asp:Button ID="btnCerrarModal" runat="server" Text="Cerrar" CssClass="btn btn-secondary" OnClick="btnCerrarModal_Click" />
          
                    </div>
                </div>
            </div>
        </div>



    </div>
    <script type="text/javascript">
        function validateForm() {
            var txtNuevoObjetivo = document.getElementById('<%= txtNuevoObjetivo.ClientID %>').value;
        if (txtNuevoObjetivo.trim() === "") {
            alert("El campo 'Nuevo Objetivo' es obligatorio.");
            return false; // Evita que se envíe el formulario
        }
        return true;
    }
</script>
</asp:Content>
